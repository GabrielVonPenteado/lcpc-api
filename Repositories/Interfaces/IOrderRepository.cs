using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<OrderReportDto>> GetOrderReportAsync(DateTime startDate, DateTime endDate, string status);
}

