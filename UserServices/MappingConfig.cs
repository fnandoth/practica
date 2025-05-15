using System;
using AutoMapper;
namespace UserServices;
using UserServices.Aplication.DTOs;
using UserServices.Domain.Entities;
public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, UserResponseDTO>().ReverseMap();
    }

}
