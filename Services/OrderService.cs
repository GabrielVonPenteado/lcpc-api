using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
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
            existingOrder.State = orderDto.State;
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

        public async Task<IEnumerable<OrderReportDto>> GetOrderReportAsync(DateTime startDate, DateTime endDate, string status)
        {
            return await _orderRepository.GetOrderReportAsync(startDate, endDate, status);
        }
    }
}
