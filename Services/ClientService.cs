using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Services
{
    public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<ServiceResult<Client>> GetByIdAsync(Guid id)
    {
        var result = new MyProject.Services.Interfaces.ServiceResult<Client>();

        if (id == Guid.Empty)
        {
            result.Success = false;
            result.Message = "ID cannot be empty.";
            return result;
        }

        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
        {
            result.Success = false;
            result.Message = $"Client with ID {id} not found.";
            return result;
        }

        result.Success = true;
        result.Data = client;
        return result;
    }

    public async Task<ServiceResult> AddAsync(Client client)
    {
        var result = new ServiceResult();

        if (client == null)
        {
            result.Success = false;
            result.Message = "Client cannot be null.";
            return result;
        }

        if (string.IsNullOrWhiteSpace(client.Name))
        {
            result.Success = false;
            result.Message = "Client name is required.";
            return result;
        }

        await _clientRepository.AddAsync(client);
        result.Success = true;
        result.Message = "Client added successfully.";
        return result;
    }

    public async Task<ServiceResult> UpdateAsync(Client client)
    {
        var result = new ServiceResult();

        if (client == null)
        {
            result.Success = false;
            result.Message = "Client cannot be null.";
            return result;
        }

        if (client.Id == Guid.Empty)
        {
            result.Success = false;
            result.Message = "Client ID is required for update.";
            return result;
        }

        var existingClient = await _clientRepository.GetByIdAsync(client.Id);
        if (existingClient == null)
        {
            result.Success = false;
            result.Message = $"Client with ID {client.Id} not found.";
            return result;
        }

        existingClient.Name = client.Name;
        existingClient.Streetplace = client.Streetplace;
        existingClient.Neighborhood = client.Neighborhood;
        existingClient.Number = client.Number;
        existingClient.Complement = client.Complement;
        existingClient.Phone = client.Phone;
        existingClient.Email = client.Email;
        existingClient.CNPJ = client.CNPJ;
        existingClient.FkCityId = client.FkCityId;

        await _clientRepository.UpdateAsync(existingClient);
        result.Success = true;
        result.Message = "Client updated successfully.";
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

        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
        {
            result.Success = false;
            result.Message = $"Client with ID {id} not found.";
            return result;
        }

        await _clientRepository.DeleteAsync(id);
        result.Success = true;
        result.Message = "Client deleted successfully.";
        return result;
    }
}

}
