using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                                 .AsNoTracking()
                                 .Where(o => o.DeletedAt == null) // Soft delete condition
                                 .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(o => o.Id == id && o.DeletedAt == null); // Soft delete condition
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.SetDeletedAt(); // Soft delete
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderReportDto>> GetOrderReportAsync(DateTime startDate, DateTime endDate, string status)
        {
            var query = _context.Orders
                .Where(order => order.CreationDate >= startDate && order.CreationDate <= endDate);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(order => order.State == status);
            }

            var report = await query
                .Select(order => new OrderReportDto
                {
                    OrderId = order.Id,
                    ClientName = order.Client.Name,
                    OrderDate = order.CreationDate,
                    TotalValue = order.TotalValue,
                    Status = order.State,
                    Products = order.ItensOrder.Select(item => item.Product.Name).ToList()
                })
                .ToListAsync();

            return report;
        }
    }
}
