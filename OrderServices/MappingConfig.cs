using System;
using AutoMapper;
using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;


namespace OrderServices;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Order, OrderDTOs>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        CreateMap<OrderDTOs, Order>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());
        CreateMap<OrderProduct, OrderProductDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        CreateMap<OrderProductDTO, OrderProduct>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        CreateMap<Order, OrderResponseDTO>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        CreateMap<OrderProduct, ProductResponseDTO>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
