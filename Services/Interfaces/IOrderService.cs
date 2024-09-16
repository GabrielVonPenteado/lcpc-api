using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IOrderService : IAppService
    {
        Task<ServiceResult<IEnumerable<Order>>> GetAllAsync();
        Task<ServiceResult<Order>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(Order order);
        Task<ServiceResult> UpdateAsync(OrderUpdateDto orderDto);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<IEnumerable<OrderReportDto>> GetOrderReportAsync(DateTime startDate, DateTime endDate, string status);
    }
}
