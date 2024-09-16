using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Controllers;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class ItemOrderService : IItemOrderService
    {
        private readonly IItemOrderRepository _itemOrderRepository;

        public ItemOrderService(IItemOrderRepository itemOrderRepository)
        {
            _itemOrderRepository = itemOrderRepository;
        }

        public async Task<ServiceResult<IEnumerable<ItemOrder>>> GetAllAsync()
        {
            var itemOrders = await _itemOrderRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<ItemOrder>>
            {
                Success = true,
                Data = itemOrders
            };
        }

        public async Task<ServiceResult<ItemOrder>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<ItemOrder>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var itemOrder = await _itemOrderRepository.GetByIdAsync(id);
            if (itemOrder == null)
            {
                result.Success = false;
                result.Message = $"Item Order with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = itemOrder;
            return result;
        }

        public async Task<ServiceResult> AddAsync(ItemOrder itemOrder)
        {
            var result = new ServiceResult();

            if (itemOrder == null)
            {
                result.Success = false;
                result.Message = "Item Order cannot be null.";
                return result;
            }

            if (itemOrder.Quantity <= 0 || itemOrder.ItemValue <= 0)
            {
                result.Success = false;
                result.Message = "Quantity and ItemValue must be greater than zero.";
                return result;
            }

            await _itemOrderRepository.AddAsync(itemOrder);
            result.Success = true;
            result.Message = "Item Order added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(ItemOrderUpdateDto itemOrderDto)
        {
            var result = new ServiceResult();

            if (itemOrderDto == null)
            {
                result.Success = false;
                result.Message = "Item Order DTO cannot be null.";
                return result;
            }

            var existingItemOrder = await _itemOrderRepository.GetByIdAsync(itemOrderDto.Id);
            if (existingItemOrder == null)
            {
                result.Success = false;
                result.Message = "Item Order not found.";
                return result;
            }

            if (itemOrderDto.Quantity <= 0 || itemOrderDto.ItemValue <= 0)
            {
                result.Success = false;
                result.Message = "Quantity and ItemValue must be greater than zero.";
                return result;
            }

            existingItemOrder.FkProductId = itemOrderDto.FkProductId;
            existingItemOrder.FkOrderId = itemOrderDto.FkOrderId;
            existingItemOrder.Quantity = itemOrderDto.Quantity;
            existingItemOrder.ItemValue = itemOrderDto.ItemValue;

            await _itemOrderRepository.UpdateAsync(existingItemOrder);
            result.Success = true;
            result.Message = "Item Order updated successfully.";
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

            var itemOrder = await _itemOrderRepository.GetByIdAsync(id);
            if (itemOrder == null)
            {
                result.Success = false;
                result.Message = $"Item Order with ID {id} not found.";
                return result;
            }

            await _itemOrderRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "Item Order deleted successfully.";
            return result;
        }
    }
}
