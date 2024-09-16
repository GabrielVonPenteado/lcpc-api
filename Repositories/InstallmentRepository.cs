using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class InstallmentRepository : IInstallmentRepository
    {
        private readonly ApplicationDbContext _context;

        public InstallmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Installment>> GetAllAsync()
        {
            return await _context.Installments
                                 .AsNoTracking()
                                 .Where(i => i.DeletedAt == null)
                                 .ToListAsync();
        }

        public async Task<Installment> GetByIdAsync(Guid id)
        {
            return await _context.Installments
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);
        }

        public async Task AddAsync(Installment installment)
        {
            await _context.Installments.AddAsync(installment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Installment installment)
        {
            _context.Installments.Update(installment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var installment = await _context.Installments.FindAsync(id);
            if (installment != null)
            {
                installment.SetDeletedAt();
                _context.Installments.Update(installment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
