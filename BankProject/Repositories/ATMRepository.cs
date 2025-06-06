using BankProject.Data;
using BankProject.Models;
using BankProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Repositories
{
    public class ATMRepository : IATMRepository
    {
        private readonly ApplicationDbContext _ctx;

        public ATMRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ATM> GetAsync()
        {
            return await _ctx.ATMs.FirstOrDefaultAsync();
        }

        public async Task AddAsync(ATM atm)
        {
            await _ctx.ATMs.AddAsync(atm);
        }

        public async Task UpdateAsync(ATM atm)
        {
            _ctx.ATMs.Update(atm);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
