using Microsoft.EntityFrameworkCore;
using MyProject.Enums;
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
                                 .Where(o => o.DeletedAt == null)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithClientByDateRangeAsync(DateTime? startDate, DateTime? endDate, Guid? clientId)
        {
            var query = _context.Orders
                .Include(o => o.Client)
                .Where(o => o.DeletedAt == null && o.Client != null);

            if (startDate.HasValue)
                query = query.Where(o => o.CreationDate >= startDate.Value.ToUniversalTime());

            if (endDate.HasValue)
                query = query.Where(o => o.CreationDate <= endDate.Value.ToUniversalTime());

            if (clientId.HasValue)
                query = query.Where(o => o.FkClientId == clientId.Value);

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(o => o.Id == id && o.DeletedAt == null);
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
                order.SetDeletedAt();
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderReportDto>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate, OrderStatus? status = null)
        {
            var query = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.ItensOrder)
                .ThenInclude(io => io.Product)
                .Where(o => o.CreationDate >= startDate && o.CreationDate <= endDate);

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status);
            }

            var orders = await query
                .Select(o => new OrderReportDto
                {
                    OrderId = o.Id,
                    ClientName = o.Client.Name,
                    OrderDate = o.CreationDate,
                    TotalValue = o.TotalValue,
                    Status = o.Status.ToString(),
                    Products = o.ItensOrder.Select(io => io.Product.Name).ToList()
                })
                .ToListAsync();

            return orders;
        }
        public async Task<IEnumerable<ProductSalesReportDto>> GetTopSoldProductsAsync(DateTime? startDate, DateTime? endDate, ProductTypeEnum? productType)
        {
            var query = _context.ItensOrder
                .Include(io => io.Product)
                .Include(io => io.Order)
                .Where(io => io.Order.DeletedAt == null);

            if (startDate.HasValue)
            {
                query = query.Where(io => io.Order.CreationDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(io => io.Order.CreationDate <= endDate);
            }

            if (productType.HasValue)
            {
                query = query.Where(io => io.Product.ProductType == productType);
            }

            var result = await query
                .GroupBy(io => new { io.Product.Name, io.Product.ProductType })
                .Select(g => new ProductSalesReportDto
                {
                    ProductName = g.Key.Name,
                    ProductType = g.Key.ProductType.ToString(),
                    QuantitySold = g.Sum(io => io.Quantity),
                    TotalSalesValue = g.Sum(io => io.Quantity * io.ItemValue),
                    AverageSalesValue = g.Average(io => io.ItemValue)
                })
                .OrderByDescending(r => r.QuantitySold)
                .ToListAsync();

            return result;
        }
    }
}
