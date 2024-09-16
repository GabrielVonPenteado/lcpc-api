using MyProject.Models;

namespace MyProject.Services.Interfaces;

public interface IItemOrderService
{
    Task<ServiceResult<IEnumerable<ItemOrder>>> GetAllAsync();
    Task<ServiceResult<ItemOrder>> GetByIdAsync(Guid id);
    Task<ServiceResult> AddAsync(ItemOrder itemOrder);
    Task<ServiceResult> UpdateAsync(ItemOrderUpdateDto itemOrderDto);
        Task<ServiceResult> DeleteAsync(Guid id);
}
