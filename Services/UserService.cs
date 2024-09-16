using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<IEnumerable<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<User>>
            {
                Success = true,
                Data = users
            };
        }

        public async Task<ServiceResult<User>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<User>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                result.Success = false;
                result.Message = $"User with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = user;
            return result;
        }

        public async Task<ServiceResult> AddAsync(User user)
        {
            var result = new ServiceResult();

            if (user == null)
            {
                result.Success = false;
                result.Message = "User cannot be null.";
                return result;
            }

            if (await _userRepository.GetByIdAsync(user.Id) != null)
            {
                result.Success = false;
                result.Message = "User with the same ID already exists.";
                return result;
            }

            await _userRepository.AddAsync(user);
            result.Success = true;
            result.Message = "User added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UserUpdateDto userDto)
        {
            var result = new ServiceResult();

            var existingUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (existingUser == null)
            {
                result.Success = false;
                result.Message = "User not found.";
                return result;
            }

            existingUser.Username = userDto.Username;
            existingUser.Password = userDto.Password;
            existingUser.Email = userDto.Email;

            await _userRepository.UpdateAsync(existingUser);
            result.Success = true;
            result.Message = "User updated successfully.";
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var result = new ServiceResult();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                result.Success = false;
                result.Message = $"User with ID {id} not found.";
                return result;
            }

            await _userRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "User deleted successfully.";
            return result;
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return user;
        }
    }
}
