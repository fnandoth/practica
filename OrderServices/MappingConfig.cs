using System;
using AutoMapper;
using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;


namespace OrderServices;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Order, OrderDTOs>().ReverseMap();

        CreateMap<OrderProduct, OrderProductDTO>().ReverseMap();

        CreateMap<Order, OrderResponseDTO>();

        CreateMap<OrderProduct, ProductResponseDTO>();
    }
}
