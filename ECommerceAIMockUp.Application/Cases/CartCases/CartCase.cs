using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.DTOs.Cart;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;


namespace ECommerceAIMockUp.Application.Cases.CartCases
{
    public class CartCase
    {
        private readonly IOrderRepository _orderRepository;
        //private readonly IConfiguration _configuration;


        public CartCase(IOrderRepository orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            //_configuration = configuration;
        }

        public async Task<Order> GetOrCreateCartAsync(string userId)
        {
            var cart = await _orderRepository.GetCartAsync(userId);
            if (cart != null)
                return cart;

            cart = new Order
            {
                AppUserId = userId,
                Status = OrderStatus.CartItem,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            await _orderRepository.CreateAsync(cart);
            await _orderRepository.SaveChangesAsync();
            return cart;
        }

        public async Task<List<OrderItemDto>> GetCartItemsAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return cart.OrderItems.Select(item => new OrderItemDto
            {
                ProductDetailsId = item.ProductDetailsId,
                DesignDetailsId = item.DesignDetailsId,
                Quantity = item.Quantity,
                ProductName = item.ProductDetails?.Product.Name ?? string.Empty,
            }).ToList();
        }

        public async Task IncreaseQuantityAsync(string userId, int productDetailsId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.OrderItems.FirstOrDefault(i => i.ProductDetailsId == productDetailsId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.OrderItems.Add(new OrderItem
                {
                    ProductDetailsId = productDetailsId,
                    Quantity = 1
                });
            }
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(string userId, int productDetailsId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.OrderItems.FirstOrDefault(i => i.ProductDetailsId == productDetailsId);
            if (item == null) return;

            if (item.Quantity <= 1)
            {
                cart.OrderItems.Remove(item);
            }
            else
            {
                item.Quantity--;
            }

            await _orderRepository.SaveChangesAsync();
        }

        public async Task<string> SubmitOrderAsync(string userId)
        {
            var cart = await _orderRepository.GetCartAsync(userId);
            if (cart == null || cart.OrderItems.Count == 0)
                throw new Exception("Cart is empty");

            var domain = "http://localhost:4200";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "/order-success?orderId={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/cart",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in cart.OrderItems)
            {
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.ProductDetails.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductDetails.Product.Name
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(lineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            cart.Status = OrderStatus.Pending;
            cart.PaymentStatus = PaymentStatus.Pending;
            cart.OrderDate = DateTime.UtcNow;
            cart.OrderTotal = cart.OrderItems.Sum(i => i.Quantity * i.ProductDetails.Product.Price);
            cart.SessionId = session.Id;

            await _orderRepository.SaveChangesAsync();

            return session.Url;
        }

        public async Task ConfirmOrderAsync(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            var order = await _orderRepository.GetBySessionIdAsync(sessionId);

            if (order == null)
                throw new Exception("Order not found");

            if (session.PaymentStatus == "paid")
            {
                order.PaymentStatus = PaymentStatus.Approved;
                order.Status = OrderStatus.Approved;
                order.PaymentDate = DateTime.UtcNow;
                order.PaymentIntentId = session.PaymentIntentId;
                await _orderRepository.SaveChangesAsync();
            }
        }

        public async Task AddToCartAsync(string userId, int productDetailsId, int designDetailsId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var existing = cart.OrderItems.FirstOrDefault(i =>
                i.ProductDetailsId == productDetailsId &&
                i.DesignDetailsId == designDetailsId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.OrderItems.Add(new OrderItem
                {
                    ProductDetailsId = productDetailsId,
                    DesignDetailsId = designDetailsId,
                    Quantity = quantity
                });
            }

            await _orderRepository.SaveChangesAsync();
        }
    }
}
