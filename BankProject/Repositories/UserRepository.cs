using BankProject.Data;
using BankProject.Models;
using BankProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _ctx;

        public UserRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _ctx.Users
                .Include(u => u.Card)
                .ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _ctx.Users
                .Include(u => u.Card)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _ctx.Users
                .Include(u => u.Card)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _ctx.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
