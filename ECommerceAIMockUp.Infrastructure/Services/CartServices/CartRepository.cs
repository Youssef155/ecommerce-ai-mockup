using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Shopping_Cart;
using ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;
using ECommerceAIMockUp.Domain;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Infrastructure.Services;

public class CartRepository(IBaseRepository<Order> orderRepository, IBaseRepository<OrderItem> orderItemRepo, IMapper mapper) : ICartRepository
{
    private async Task<Order?> GetCartAsync (string userId)
    {
        //var cart = orderRepository.GetAllQueryable(tracking: true)
        //    .Include(o => o.OrderItems)
        //        .ThenInclude(i => i.DesignDetails)
        //    .Include(o => o.OrderItems)
        //        .ThenInclude(i => i.ProductDetails)
        //    .FirstOrDefault(order => order.AppUserId == userId && order.Status == OrderStatus.CartItem);

        var cart = await orderRepository.GetAllQueryable(tracking: true)
    .Where(o => o.AppUserId == userId && o.Status == OrderStatus.CartItem)
    .Include(o=>o.OrderItems)
        .ThenInclude(i=>i.ProductDetails)
           .ThenInclude(p=>p.Product)
    .Include(o => o.OrderItems)
        .ThenInclude(i => i.DesignDetails)
    .FirstOrDefaultAsync();


        //if (cart == null) //the user has no cart yet
        //{
        //    throw new Exception("You Have no cart, start adding items in order to display them in the cart");
        //}
        return cart;
    } //private method to use a repeated logic
        
    public async Task<IEnumerable<OrderItemDTO>> GetAllItems(string UserId)
    {
        var cart = await GetCartAsync (UserId);
        if (cart == null)
            throw new Exception("Your Cart is empty, Start adding items to display them in the cart");
        cart.OrderTotal = cart.OrderItems.Sum(i => i.Quantity * i.ProductDetails.Product.Price);

        var itemsDto = cart.OrderItems.Select(i => new OrderItemDTO
        {
            DesignDetailsId = i.DesignDetailsId,
            ProductDetailsId = i.ProductDetailsId,
            Quantity = i.Quantity,
            OrderId = i.OrderId,
            OrderTotal = cart.OrderTotal,
            UnitPrice = i.ProductDetails.Product.Price,
            LineTotal = i.Quantity * i.ProductDetails.Product.Price
        }).ToList();

        return itemsDto;
        //var items = await orderItemRepo.GetAllAsync(tracking: true, o => o.ProductDetails, o => o.DesignDetails);

        //if (items == null)
        //    throw new Exception("The Cart is Empty");

    }

    public async Task<OrderItem> GetItemById(string UserId, int itemId)
    {
        var cart = await GetCartAsync(UserId);
        if (cart == null)
            throw new Exception("Start adding items to display them in the cart");

        var item = await orderItemRepo.GetByIdAsunc(itemId,tracking: true, o=>o.DesignDetails, o=>o.ProductDetails);
        if (item == null)
            throw new Exception("Item not found");
        

        return item;
    }

    public async Task AddItem(OrderDTO item,string UserId)
    {
        var cart = await GetCartAsync(UserId);
        if(cart == null)
        {
            cart = new Order
            {
                Status = OrderStatus.CartItem,
                OrderItems = new List<OrderItem>(),
                OrderDate = DateTime.Now,
                AppUserId = UserId,
                PhoneNumber = item.PhoneNumber,
                ShippingAddress = new Address()
                {
                    City = item.City,
                    Governorate = item.Governorate,
                    Street = item.Street,
                    Zip = item.Zip
                }
            };
            await orderRepository.CreateAsync(cart);

        }

        cart.OrderDate = DateTime.Now;
        cart.AppUserId = UserId;

        var orderitem = new OrderItem
        {
            DesignDetailsId = item.DesignDetailsId,
            ProductDetailsId = item.ProductDetailsId,
            OrderId = cart.Id,
        };
        cart.OrderItems.Add(orderitem);
    }

    public async Task Remove(string userId, OrderItem orderItem)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null)
            throw new Exception("Your cart is empty");

        var item = cart.OrderItems.FirstOrDefault(o => o.Id == orderItem.Id);
        //var item = await GetItemById(userId, orderItem.Id);
        if (item == null)
            throw new Exception("Item was not found");

        await orderItemRepo.DeleteAsync(item.Id);
        cart.OrderTotal = cart.OrderItems
            .Sum(i => i.Quantity * i.ProductDetails.Product.Price);

    }

    public async Task RemoveRange(string userId, List<OrderItem> items)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null)
            throw new Exception("Your cart is empty");

        var Orderitems = cart.OrderItems;

        foreach (var item in items)
        {
            Orderitems.Remove(item);
            await orderItemRepo.DeleteAsync(item.Id);
        }
        cart.OrderTotal = cart.OrderItems
            .Sum(i => i.Quantity * i.ProductDetails.Product.Price);

    }

    public async Task UpdateQuantity(string userId, int ItemId, int quantity)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null)
            throw new Exception("User has no cart");
        var item= await GetItemById(userId, ItemId);
        item.Quantity = quantity;
        cart.OrderDate = DateTime.Now;
        cart.OrderTotal = cart.OrderItems
            .Sum(i => i.Quantity * i.ProductDetails.Product.Price);

        //save changes
    }

    public async Task AddOrderItem(OrderItemDTO item)
    {
        var orderItem = new OrderItem()
        {
            DesignDetailsId = item.DesignDetailsId,
            Quantity = item.Quantity,
            ProductDetailsId = item.ProductDetailsId,
            OrderId = item.OrderId,
        };

        await orderItemRepo.CreateAsync(orderItem);
    }
}
