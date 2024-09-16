using MyProject.Controllers;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services
{
    public class InstallmentService : IInstallmentService
    {
        private readonly IInstallmentRepository _installmentRepository;

        public InstallmentService(IInstallmentRepository installmentRepository)
        {
            _installmentRepository = installmentRepository;
        }

        public async Task<ServiceResult<IEnumerable<Installment>>> GetAllAsync()
        {
            var installments = await _installmentRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<Installment>>
            {
                Success = true,
                Data = installments
            };
        }

        public async Task<ServiceResult<Installment>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<Installment>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var installment = await _installmentRepository.GetByIdAsync(id);
            if (installment == null)
            {
                result.Success = false;
                result.Message = $"Installment with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = installment;
            return result;
        }

        public async Task<ServiceResult> AddAsync(Installment installment)
        {
            var result = new ServiceResult();

            if (installment == null)
            {
                result.Success = false;
                result.Message = "Installment cannot be null.";
                return result;
            }

            await _installmentRepository.AddAsync(installment);
            result.Success = true;
            result.Message = "Installment added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(InstallmentUpdateDto installmentDto)
        {
            var result = new ServiceResult();

            var existingInstallment = await _installmentRepository.GetByIdAsync(installmentDto.Id);
            if (existingInstallment == null)
            {
                result.Success = false;
                result.Message = "Installment not found.";
                return result;
            }

            existingInstallment.ExpirationDate = installmentDto.ExpirationDate;
            existingInstallment.Value = installmentDto.Value;
            existingInstallment.Situation = installmentDto.Situation;
            existingInstallment.FkOrderId = installmentDto.FkOrderId;

            await _installmentRepository.UpdateAsync(existingInstallment);
            result.Success = true;
            result.Message = "Installment updated successfully.";
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

            var installment = await _installmentRepository.GetByIdAsync(id);
            if (installment == null)
            {
                result.Success = false;
                result.Message = $"Installment with ID {id} not found.";
                return result;
            }

            await _installmentRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "Installment deleted successfully.";
            return result;
        }
    }
}
