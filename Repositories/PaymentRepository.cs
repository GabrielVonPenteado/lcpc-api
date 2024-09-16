using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                                 .AsNoTracking()
                                 .Where(p => p.DeletedAt == null) // Soft delete condition
                                 .ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await _context.Payments
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null); // Soft delete condition
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                payment.SetDeletedAt(); // Soft delete
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
