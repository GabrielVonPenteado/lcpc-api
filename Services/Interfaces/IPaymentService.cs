using MyProject.Controllers;
using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IPaymentService : IAppService
    {
        Task<ServiceResult<IEnumerable<Payment>>> GetAllAsync();
        Task<ServiceResult<Payment>> GetByIdAsync(Guid id);
        Task<ServiceResult> AddAsync(Payment payment);
        Task<ServiceResult> UpdateAsync(PaymentUpdateDto paymentDto);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
