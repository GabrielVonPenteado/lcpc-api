using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IItemOrderRepository
{
    Task<IEnumerable<ItemOrder>> GetAllAsync();
    Task<ItemOrder> GetByIdAsync(Guid id);
    Task AddAsync(ItemOrder itemOrder);
    Task UpdateAsync(ItemOrder itemOrder);
    Task DeleteAsync(Guid id);
}
