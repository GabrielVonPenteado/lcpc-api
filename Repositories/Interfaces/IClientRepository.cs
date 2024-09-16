using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client> GetByIdAsync(Guid id); 
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Guid id); 
}
