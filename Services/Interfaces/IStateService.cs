using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IStateService : IAppService
    {
        Task<ServiceResult<IEnumerable<State>>> GetAllAsync();
        Task<ServiceResult<State>> GetByUFAsync(string uf);
        Task<ServiceResult> AddAsync(State state);
        Task<ServiceResult> UpdateAsync(string uf, StateUpdateDto stateDto);
        Task<ServiceResult> DeleteAsync(string uf);
    }
}
