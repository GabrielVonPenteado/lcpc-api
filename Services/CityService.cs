using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<ServiceResult<City>> GetByIdAsync(Guid id)
        {
            var result = new ServiceResult<City>();

            if (id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "ID cannot be empty.";
                return result;
            }

            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                result.Success = false;
                result.Message = $"City with ID {id} not found.";
                return result;
            }

            result.Success = true;
            result.Data = city;
            return result;
        }

        public async Task<ServiceResult> AddAsync(City city)
        {
            var result = new ServiceResult();

            if (city == null)
            {
                result.Success = false;
                result.Message = "City cannot be null.";
                return result;
            }

            // Validações
            if (string.IsNullOrWhiteSpace(city.Name))
            {
                result.Success = false;
                result.Message = "City name is required.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(city.StateUF) || city.StateUF.Length != 2)
            {
                result.Success = false;
                result.Message = "City must be associated with a valid StateUF (2 characters).";
                return result;
            }

            await _cityRepository.AddAsync(city);
            result.Success = true;
            result.Message = "City added successfully.";
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(City city)
        {
            var result = new ServiceResult();

            if (city == null)
            {
                result.Success = false;
                result.Message = "City cannot be null.";
                return result;
            }

            if (city.Id == Guid.Empty)
            {
                result.Success = false;
                result.Message = "City ID is required for update.";
                return result;
            }

            var existingCity = await _cityRepository.GetByIdAsync(city.Id);
            if (existingCity == null)
            {
                result.Success = false;
                result.Message = $"City with ID {city.Id} not found.";
                return result;
            }

            // Validações
            if (string.IsNullOrWhiteSpace(city.Name))
            {
                result.Success = false;
                result.Message = "City name is required.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(city.StateUF) || city.StateUF.Length != 2)
            {
                result.Success = false;
                result.Message = "City must be associated with a valid StateUF (2 characters).";
                return result;
            }

            await _cityRepository.UpdateAsync(city);
            result.Success = true;
            result.Message = "City updated successfully.";
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

            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                result.Success = false;
                result.Message = $"City with ID {id} not found.";
                return result;
            }

            await _cityRepository.DeleteAsync(id);
            result.Success = true;
            result.Message = "City deleted successfully.";
            return result;
        }
    }
}
