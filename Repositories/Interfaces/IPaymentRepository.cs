using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment> GetByIdAsync(Guid id); 
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Guid id);
}

