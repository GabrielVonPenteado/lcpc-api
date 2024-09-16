using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class ItemOrderRepository : IItemOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemOrder>> GetAllAsync()
        {
            return await _context.ItensOrder
                                 .AsNoTracking()
                                 .Where(i => i.DeletedAt == null) 
                                 .ToListAsync();
        }

        public async Task<ItemOrder> GetByIdAsync(Guid id)
        {
            return await _context.ItensOrder
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);
        }

        public async Task AddAsync(ItemOrder itemOrder)
        {
            await _context.ItensOrder.AddAsync(itemOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemOrder itemOrder)
        {
            _context.ItensOrder.Update(itemOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var itemOrder = await _context.ItensOrder.FindAsync(id);
            if (itemOrder != null)
            {
                itemOrder.SetDeletedAt();
                _context.ItensOrder.Update(itemOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
