using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync();
    Task<City> GetByIdAsync(Guid id); 
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(Guid id); 
}
