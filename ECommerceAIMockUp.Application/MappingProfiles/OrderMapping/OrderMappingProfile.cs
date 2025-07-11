using AutoMapper;
using ECommerceAIMockUp.Application.DTOs.Order;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.MappingProfiles.OrderMapping;
public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderReadDto>();
        CreateMap<OrderItem, OrderItemReadDto>();
    }
}
