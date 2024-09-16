using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IProductService : IAppService
    {
        Task<ServiceResult<IEnumerable<Product>>> GetAllAsync();
        Task<ServiceResult<Product>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(Product product);
        Task<ServiceResult> UpdateAsync(ProductUpdateDto productDto);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
