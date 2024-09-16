using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<ServiceResult<IEnumerable<State>>> GetAllAsync()
        {
            var states = await _stateRepository.GetAllAsync();
            return new ServiceResult<IEnumerable<State>>
            {
                Success = true,
                Data = states
            };
        }

        public async Task<ServiceResult<State>> GetByUFAsync(string uf)
        {
            var result = new ServiceResult<State>();

            if (string.IsNullOrEmpty(uf) || uf.Length != 2)
            {
                result.Success = false;
                result.Message = "Invalid UF.";
                return result;
            }

            var state = await _stateRepository.GetByUFAsync(uf);
            if (state == null)
            {
                result.Success = false;
                result.Message = $"State with UF {uf} not found.";
                return result;
            }

            result.Success = true;
            result.Data = state;
            return result;
        }

        public async Task<ServiceResult> AddAsync(State state)
        {
            var result = new ServiceResult();

            if (state == null)
            {
                result.Success = false;
                result.Message = "State cannot be null.";
                return result;
            }

            if (await _stateRepository.GetByUFAsync(state.UF) != null)
            {
                result.Success = false;
                result.Message = "State with the same UF already exists.";
                return result;
            }

            await _stateRepository.AddAsync(state);
            result.Success = true;
            result.Message = "State added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(string uf, StateUpdateDto stateDto)
        {
            var result = new ServiceResult();

            var existingState = await _stateRepository.GetByUFAsync(uf);
            if (existingState == null)
            {
                result.Success = false;
                result.Message = "State not found.";
                return result;
            }

            existingState.Name = stateDto.Name;

            await _stateRepository.UpdateAsync(existingState);
            result.Success = true;
            result.Message = "State updated successfully.";
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(string uf)
        {
            var result = new ServiceResult();

            if (string.IsNullOrEmpty(uf) || uf.Length != 2)
            {
                result.Success = false;
                result.Message = "Invalid UF.";
                return result;
            }

            var state = await _stateRepository.GetByUFAsync(uf);
            if (state == null)
            {
                result.Success = false;
                result.Message = $"State with UF {uf} not found.";
                return result;
            }

            await _stateRepository.DeleteAsync(uf);
            result.Success = true;
            result.Message = "State deleted successfully.";
            return result;
        }
    }
}
