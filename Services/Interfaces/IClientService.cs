using System;
using MyProject.Models;

namespace MyProject.Services.Interfaces
{
    public interface IClientService : IAppService
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<ServiceResult<Client>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(Client client);
        Task<ServiceResult> UpdateAsync(Client client);
        Task<ServiceResult> DeleteAsync(Guid id);
    }

}
