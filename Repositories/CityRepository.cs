using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;

namespace MyProject.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context.Citys
            .Where(c => c.DeletedAt == null)  // Exclui cidades "deletadas" (soft delete)
            .ToListAsync();
    }

    public async Task<City> GetByIdAsync(Guid id)
    {
        return await _context.Citys
            .Where(c => c.Id == id && c.DeletedAt == null)  // Exclui cidades "deletadas"
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(City city)
    {
        await _context.Citys.AddAsync(city);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(City city)
    {
        _context.Citys.Update(city);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var city = await _context.Citys.FindAsync(id);
        if (city != null)
        {
            city.SetDeletedAt();
            _context.Citys.Update(city);
            await _context.SaveChangesAsync();
        }
    }
}
