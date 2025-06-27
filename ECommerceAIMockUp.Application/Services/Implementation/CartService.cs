
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;
using ECommerceAIMockUp.Domain;
using ECommerceAIMockUp.Domain.Entities;
using ECommerceAIMockUp.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Services.Implementation;

public class CartService(IBaseRepository<Order> orderRepository, IBaseRepository<OrderItem> orderItemRepo) : ICartService
{
    private async Task<Order?> GetOrCreateCartAsync (string userId)
    {
        var cart = (await orderRepository.GetAllAsync(tracking: true, o => o.OrderItems,
            o => o.OrderItems.Select(i=>i.DesignDetails),
            o => o.OrderItems.Select(i=>i.ProductDetails)))
           .FirstOrDefault(order => order.AppUserId == userId && order.Status == OrderStatus.CartItem);

        //if (cart == null) //the user has no cart yet
        //{
        //    throw new Exception("You Have no cart, start adding items in order to display them in the cart");
        //}
        return cart;
    } //private method to use a repeated logic
        
    public async Task<IEnumerable<OrderItem>> GetAllItems(string UserId)
    {
        var cart = await GetOrCreateCartAsync (UserId);
        if (cart == null)
            throw new Exception("Your Cart is empty, Start adding items to display them in the cart");

        var items =  cart.OrderItems.ToList();

        //if (items == null)
        //    throw new Exception("The Cart is Empty");

        return items;
    }

    public async Task<OrderItem> GetItemById(string UserId, int itemId)
    {
        var cart = await GetOrCreateCartAsync(UserId);
        if (cart == null)
            throw new Exception("Start adding items to display them in the cart");

        var item = await orderItemRepo.GetByIdAsunc(itemId,tracking: true, o=>o.DesignDetails, o=>o.ProductDetails);
        if (item == null)
            throw new Exception("Item not found");
        

        return item;
    }

    public async Task AddItem(OrderItem item,string UserId)
    {
        var cart = await GetOrCreateCartAsync(UserId);
        if(cart == null)
        {
            cart = new Order
            {
                Status = OrderStatus.CartItem,
                OrderItems = new List<OrderItem>(),
                OrderDate = DateTime.Now,
                AppUserId = UserId
            };
            cart.OrderItems.Add(item);
        }
        cart.OrderDate = DateTime.Now;
        cart.AppUserId = UserId;
        cart.OrderItems.Add(item);
    }

    public async Task Remove(string userId, OrderItem orderItem)
    {
        var cart = await GetOrCreateCartAsync(userId);
        if (cart == null)
            throw new Exception("Your cart is empty");

        var item = cart.OrderItems.FirstOrDefault(o => o.Id == orderItem.Id);
        //var item = await GetItemById(userId, orderItem.Id);
        if (item == null)
            throw new Exception("Item was not found");

        await orderItemRepo.DeleteAsync(item.Id);
    }

    public async Task RemoveRange(string userId, List<OrderItem> items)
    {
        var cart = await GetOrCreateCartAsync(userId);
        if (cart == null)
            throw new Exception("Your cart is empty");

        var Orderitems = cart.OrderItems;

        foreach (var item in items)
        {

        }
    }
}
