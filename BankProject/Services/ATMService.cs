using BankProject.Models;
using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;

namespace BankProject.Services
{
    public class ATMService : IATMService
    {
        private readonly IATMRepository _atmRepo;

        public ATMService(IATMRepository atmRepo)
        {
            _atmRepo = atmRepo;
        }

        public async Task<decimal> GetTotalCashAsync()
        {
            var atm = await _atmRepo.GetAsync();
            if (atm == null)
                return 0m;
            return atm.TotalCash;
        }

        public async Task AddCashAsync(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума має бути більшою за 0", nameof(amount));

            var atm = await _atmRepo.GetAsync();
            if (atm == null)
            {
                atm = new ATM { TotalCash = amount };
                await _atmRepo.AddAsync(atm);
            }
            else
            {
                atm.TotalCash += amount;
                await _atmRepo.UpdateAsync(atm);
            }

            await _atmRepo.SaveChangesAsync();
        }

        public async Task<bool> WithdrawCashAsync(decimal amount)
        {
            if (amount <= 0)
                return false;

            var atm = await _atmRepo.GetAsync();
            if (atm == null || atm.TotalCash < amount)
                return false;

            atm.TotalCash -= amount;
            await _atmRepo.UpdateAsync(atm);
            await _atmRepo.SaveChangesAsync();
            return true;
        }
    }
}
