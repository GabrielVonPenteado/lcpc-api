using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories.Interfaces
{
    public interface IInstallmentRepository
    {
        Task<IEnumerable<Installment>> GetAllAsync();
        Task<Installment> GetByIdAsync(Guid id);
        Task AddAsync(Installment installment);
        Task UpdateAsync(Installment installment);
        Task DeleteAsync(Guid id);
    }
}
