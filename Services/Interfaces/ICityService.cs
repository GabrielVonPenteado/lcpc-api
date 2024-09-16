using MyProject.Models;

namespace MyProject.Services.Interfaces
{
    public interface ICityService : IAppService
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<ServiceResult<City>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(City city);
        Task<ServiceResult> UpdateAsync(City city);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
