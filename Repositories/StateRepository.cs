using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;

        public StateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            return await _context.States
                                 .AsNoTracking()
                                 .Where(s => s.DeletedAt == null) // Soft delete condition
                                 .ToListAsync();
        }

        public async Task<State> GetByUFAsync(string uf)
        {
            return await _context.States
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(s => s.UF == uf && s.DeletedAt == null); // Soft delete condition
        }

        public async Task AddAsync(State state)
        {
            await _context.States.AddAsync(state);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(State state)
        {
            _context.States.Update(state);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string uf)
        {
            var state = await _context.States.FindAsync(uf);
            if (state != null)
            {
                state.SetDeletedAt(); // Soft delete
                _context.States.Update(state);
                await _context.SaveChangesAsync();
            }
        }
    }
}
