using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Enums;
using MyProject.Models;

namespace MyProject.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
    Task<List<OrderReportDto>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate, OrderStatus? status = null);
    Task<IEnumerable<Order>> GetOrdersWithClientByDateRangeAsync(DateTime? startDate, DateTime? endDate, Guid? clientId);
    Task<IEnumerable<ProductSalesReportDto>> GetTopSoldProductsAsync(DateTime? startDate, DateTime? endDate, ProductTypeEnum? productType);
}

