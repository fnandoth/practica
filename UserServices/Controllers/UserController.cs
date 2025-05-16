using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserServices.Aplication.DTOs;
using UserServices.Domain.Entities;
using UserServices.Domain.Interfaces;

namespace UserServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUser(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new ArgumentException($"El ID {guidId} no es un GUID válido");
            }
            var user = await _userRepository.GetUserByIdAsync(guidId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByName(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser(UserDTO user)
        {
            await _userRepository.CreateUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), user);
        }

        [HttpPut("update/{name}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(string name, UserDTO user)
        {
            var existingUser = await _userRepository.GetUserByNameAsync(name);
            if (existingUser == null)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(name, user);
            await _userRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{name}")]
        public async Task<ActionResult> DeleteUser(string name, [FromQuery] string password)
        {
            await _userRepository.DeleteUserAsync(name, password);
            await _userRepository.SaveChangesAsync();
            return NoContent();
        }

        
    }
}