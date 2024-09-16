using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IStateRepository
{
    Task<IEnumerable<State>> GetAllAsync();
    Task<State> GetByUFAsync(string uf);
    Task AddAsync(State state);
    Task UpdateAsync(State state);
    Task DeleteAsync(string uf);
}
