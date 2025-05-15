using System;
using AutoMapper;
using ProductServices.Aplication.DTOs;
using ProductServices.Domain.Entities;
namespace ProductServices;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Product, ProductResponseDTO>().ReverseMap();
        CreateMap<ProductDTO, Product>().ReverseMap();
        CreateMap<ProductDTO, ProductResponseDTO>().ReverseMap();

    }

}
