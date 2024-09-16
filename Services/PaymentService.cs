using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<ServiceResult<IEnumerable<Payment>>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<Payment>>
            {
                Success = true,
                Data = payments
            };
        }

        public async Task<ServiceResult<Payment>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<Payment>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                result.Success = false;
                result.Message = $"Payment with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = payment;
            return result;
        }

        public async Task<ServiceResult> AddAsync(Payment payment)
        {
            var result = new ServiceResult();

            if (payment == null)
            {
                result.Success = false;
                result.Message = "Payment cannot be null.";
                return result;
            }

            if (payment.Value <= 0)
            {
                result.Success = false;
                result.Message = "Payment value must be greater than zero.";
                return result;
            }

            await _paymentRepository.AddAsync(payment);
            result.Success = true;
            result.Message = "Payment added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(PaymentUpdateDto paymentDto)
        {
            var result = new ServiceResult();

            var existingPayment = await _paymentRepository.GetByIdAsync(paymentDto.Id);
            if (existingPayment == null)
            {
                result.Success = false;
                result.Message = "Payment not found.";
                return result;
            }

            if (paymentDto.Value <= 0)
            {
                result.Success = false;
                result.Message = "Payment value must be greater than zero.";
                return result;
            }

            existingPayment.DataPayment = paymentDto.DataPayment;
            existingPayment.Value = paymentDto.Value;
            existingPayment.PaymentType = paymentDto.PaymentType;
            existingPayment.ReceivementType = paymentDto.ReceivementType;
            existingPayment.FkInstallmentId = paymentDto.FkInstallmentId;
            existingPayment.FkUserId = paymentDto.FkUserId;

            await _paymentRepository.UpdateAsync(existingPayment);
            result.Success = true;
            result.Message = "Payment updated successfully.";
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

            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                result.Success = false;
                result.Message = $"Payment with ID {id} not found.";
                return result;
            }

            await _paymentRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "Payment deleted successfully.";
            return result;
        }
    }
}
