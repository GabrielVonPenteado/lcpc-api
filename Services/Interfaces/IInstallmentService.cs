using MyProject.Controllers;
using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IInstallmentService : IAppService
    {
        Task<ServiceResult<IEnumerable<Installment>>> GetAllAsync();
        Task<ServiceResult<Installment>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(Installment installment);
        Task<ServiceResult> UpdateAsync(InstallmentUpdateDto installmentDto);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
