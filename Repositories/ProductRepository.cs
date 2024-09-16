using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                                 .AsNoTracking()
                                 .Where(p => p.DeletedAt == null) // Soft delete condition
                                 .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null); // Soft delete condition
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.SetDeletedAt(); // Soft delete
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
