using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IUserService : IAppService
    {
        Task<ServiceResult<IEnumerable<User>>> GetAllAsync();
        Task<ServiceResult<User>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(User user);
        Task<ServiceResult> UpdateAsync(UserUpdateDto userDto);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<User> ValidateUserAsync(string username, string password);
    }
}
