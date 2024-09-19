using MyProject.Enums;
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
        Task<ServiceResult<List<OrderReportDto>>> GetOrdersReportAsync(DateTime startDate, DateTime endDate, OrderStatus? status = null);
        Task<ServiceResult<IEnumerable<OrderBillingReportDto>>> GenerateOrderBillingReportAsync(DateTime? startDate, DateTime? endDate, Guid? clientId);
        Task<ServiceResult<IEnumerable<ProductSalesReportDto>>> GenerateTopSoldProductsReportAsync(DateTime? startDate, DateTime? endDate, ProductTypeEnum? productType);
    }
}
