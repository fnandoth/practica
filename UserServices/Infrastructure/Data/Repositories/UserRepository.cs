using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserServices.Domain.Interfaces;
using AutoMapper;
using UserServices.Aplication.DTOs;
using UserServices.Domain.Entities;
namespace UserServices.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        private readonly IMapper _mapper;
        public UserRepository(UserContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");
            
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserDTO user)
        {
            var existingUser = await GetUserByNameAsync(user.Name);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"Ya existe un usuario con el nombre {user.Name}");
            }
               
            var existingEmail = await GetUserByEmailAsync(user.Email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {user.Email}");
            }
            var newUser = _mapper.Map<User>(user);
            await _context.Users.AddAsync(newUser);
            await SaveChangesAsync();
            return _mapper.Map<UserResponseDTO>(newUser);
        }
        public async Task<bool> UpdateUserAsync(string name, UserDTO user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == name)
                ?? throw new KeyNotFoundException($"Usuario con {name} no encontrado");
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            _context.Users.Update(existingUser);
            await SaveChangesAsync();
            return true;
        }
        
        
        public async Task<bool> DeleteUserAsync(string name, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name && u.Password == password) 
                ?? throw new KeyNotFoundException($"Usuario con {name} no encontrado");
            if (user.Password != password)
            {
                throw new InvalidOperationException($"Contraseña incorrecta para el usuario {name}");
            }
            _context.Users.Remove(user);
            await SaveChangesAsync();
            return true;
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null!;
            }
            return _mapper.Map<UserResponseDTO>(user);
        }
        public async Task<UserResponseDTO> GetUserByNameAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                return null!;
            }
            return _mapper.Map<UserResponseDTO>(user);
        }
        public async Task<bool> VerifyPasswordAsync(string name, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name && u.Password == password);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuario con {name} no encontrado");
            }
            return true;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}