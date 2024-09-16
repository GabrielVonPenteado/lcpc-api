using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                                 .AsNoTracking()
                                 .Where(u => u.DeletedAt == null) // Soft delete condition
                                 .ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null); // Soft delete condition
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.SetDeletedAt(); // Soft delete
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
