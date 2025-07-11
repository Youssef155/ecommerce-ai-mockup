using AutoMapper;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Order;
using ECommerceAIMockUp.Application.Services.Interfaces;

namespace ECommerceAIMockUp.Application.Services.Implementations;
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepo.GetOrdersWithDetailsAsync(false);
        return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
    }

    public async Task<OrderReadDto?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepo.GetOrderWithDetailsByIdAsync(id, false);
        return order == null ? null : _mapper.Map<OrderReadDto>(order);
    }

    public async Task<OrderReadDto?> UpdateOrderStatusAsync(OrderUpdateStatusDto dto)
    {
        var order = await _orderRepo.GetByIdAsunc(dto.Id, true);
        if (order == null)
            return null;

        order.Status = dto.Status;
        _orderRepo.Update(order);
        await _orderRepo.SaveAsync();

        return _mapper.Map<OrderReadDto>(order);
    }

    public async Task<OrderReadDto?> UpdatePaymentStatusAsync(OrderUpdatePaymentDto dto)
    {
        var order = await _orderRepo.GetByIdAsunc(dto.Id, true);
        if (order == null)
            return null;

        order.PaymentStatus = dto.PaymentStatus;
        if (dto.PaymentDate.HasValue)
            order.PaymentDate = dto.PaymentDate;

        _orderRepo.Update(order);
        await _orderRepo.SaveAsync();

        return _mapper.Map<OrderReadDto>(order);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsunc(id, false);
        if (order == null)
            return false;

        await _orderRepo.DeleteAsync(id);
        await _orderRepo.SaveAsync();
        return true;
    }
}
