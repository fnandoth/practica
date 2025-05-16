using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserServices.Aplication.DTOs;
using UserServices.Domain.Entities;

namespace UserServices.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO> GetUserByIdAsync(string Id);
        Task<UserResponseDTO> CreateUserAsync(UserDTO user);
        Task<bool> UpdateUserAsync(string name, UserDTO user, string password);
        Task<bool> DeleteUserAsync(string name, string password);
        Task<UserResponseDTO> GetUserByEmailAsync(string email);
        Task<UserResponseDTO> GetUserByNameAsync(string name);
        Task<bool> VerifyPasswordAsync(string name, string password);
        Task<int> SaveChangesAsync();
    }
}