using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _context.Clients
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                client.SetDeletedAt();
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
