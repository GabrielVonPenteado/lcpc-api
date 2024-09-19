using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Enums;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IItemOrderRepository _itemOrderRepository;

        public OrderService(IOrderRepository orderRepository, ApplicationDbContext context, IClientRepository clientRepository, IItemOrderRepository itemOrderRepository)
        {
            _orderRepository = orderRepository;
            _context = context;
            _clientRepository = clientRepository;
            _itemOrderRepository = itemOrderRepository;
        }

        public async Task<ServiceResult<IEnumerable<Order>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<Order>>
            {
                Success = true,
                Data = orders
            };
        }

        public async Task<ServiceResult<Order>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<Order>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                result.Success = false;
                result.Message = $"Order with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = order;
            return result;
        }

        public async Task<ServiceResult> AddAsync(Order order)
        {
            var result = new ServiceResult();

            if (order == null)
            {
                result.Success = false;
                result.Message = "Order cannot be null.";
                return result;
            }

            if (order.TotalValue <= 0 || order.NInstallments <= 0)
            {
                result.Success = false;
                result.Message = "TotalValue and NInstallments must be greater than zero.";
                return result;
            }

            await _orderRepository.AddAsync(order);
            result.Success = true;
            result.Message = "Order added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(OrderUpdateDto orderDto)
        {
            var result = new ServiceResult();

            var existingOrder = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (existingOrder == null)
            {
                result.Success = false;
                result.Message = "Order not found.";
                return result;
            }

            if (orderDto.TotalValue <= 0 || orderDto.NInstallments <= 0)
            {
                result.Success = false;
                result.Message = "TotalValue and NInstallments must be greater than zero.";
                return result;
            }

            existingOrder.Description = orderDto.Description;
            existingOrder.TotalValue = orderDto.TotalValue;
            existingOrder.ShippingDate = orderDto.ShippingDate;
            existingOrder.DeliveryDate = orderDto.DeliveryDate;
            existingOrder.ExpectedDeliveryDate = orderDto.ExpectedDeliveryDate;
            existingOrder.Status = orderDto.Status;
            existingOrder.NInstallments = orderDto.NInstallments;
            existingOrder.FkUserId = orderDto.FkUserId;
            existingOrder.FkClientId = orderDto.FkClientId;

            await _orderRepository.UpdateAsync(existingOrder);
            result.Success = true;
            result.Message = "Order updated successfully.";
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var result = new ServiceResult();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                result.Success = false;
                result.Message = $"Order with ID {id} not found.";
                return result;
            }

            await _orderRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "Order deleted successfully.";
            return result;
        }

    public async Task<ServiceResult<List<OrderReportDto>>> GetOrdersReportAsync(DateTime startDate, DateTime endDate, OrderStatus? status = null)
    {
        var orders = await _orderRepository.GetAllAsync();

        var filteredOrders = orders.Where(o => o.CreationDate >= startDate && o.CreationDate <= endDate);

        if (status.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.Status == status.Value);
        }

        var report = new List<OrderReportDto>();

        foreach (var order in filteredOrders)
        {
            var client = await _clientRepository.GetByIdAsync(order.FkClientId);
            var items = await _itemOrderRepository.GetByOrderIdAsync(order.Id);
            var productNames = items.Select(i => i.Product.Name).ToList();

            report.Add(new OrderReportDto
            {
                OrderId = order.Id,
                ClientName = client.Name,
                OrderDate = order.CreationDate,
                TotalValue = order.TotalValue,
                Status = order.Status.ToString(),
                Products = productNames
            });
        }

        return new ServiceResult<List<OrderReportDto>> { Data = report, Success = true };
        }

        public async Task<ServiceResult<IEnumerable<OrderBillingReportDto>>> GenerateOrderBillingReportAsync(DateTime? startDate, DateTime? endDate, Guid? clientId)
        {

            var orders = await _orderRepository.GetOrdersWithClientByDateRangeAsync(startDate, endDate, clientId);

            if (orders == null || !orders.Any())
            {
                return new ServiceResult<IEnumerable<OrderBillingReportDto>>
                {
                    Success = false,
                    Message = "Nenhum pedido encontrado para os critérios selecionados."
                };
            }

            var report = orders
                .GroupBy(o => o.Client.Id)  
                .Select(group => new OrderBillingReportDto
                {
                    ClientName = group.First().Client.Name,  
                    TotalOrders = group.Count(),  
                    TotalOrderValue = group.Sum(o => o.TotalValue),  
                    AverageOrderValue = group.Average(o => o.TotalValue)  
                })
                .ToList();

            return new ServiceResult<IEnumerable<OrderBillingReportDto>>
            {
                Success = true,
                Data = report
            };
        }
        public async Task<ServiceResult<IEnumerable<ProductSalesReportDto>>> GenerateTopSoldProductsReportAsync(DateTime? startDate, DateTime? endDate, ProductTypeEnum? productType)
        {
            var products = await _orderRepository.GetTopSoldProductsAsync(startDate, endDate, productType);

            if (products == null || !products.Any())
            {
                return new ServiceResult<IEnumerable<ProductSalesReportDto>>
                {
                    Success = false,
                    Message = "Nenhum produto encontrado para os critérios selecionados."
                };
            }

            return new ServiceResult<IEnumerable<ProductSalesReportDto>>
            {
                Success = true,
                Data = products
            };
        }
    }
}