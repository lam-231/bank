using BankProject.Data;
using BankProject.Models;
using BankProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _ctx;

        public CardRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> CardNumberExistsAsync(string cardNumber)
        {
            return await _ctx.Cards.AnyAsync(c => c.CardNumber == cardNumber);
        }

        public async Task AddAsync(Card card)
        {
            await _ctx.Cards.AddAsync(card);
        }

        public async Task<Card> GetByCardNumberAsync(string cardNumber)
        {
            return await _ctx.Cards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }

        public async Task<Card> GetByUserIdAsync(int userId)
        {
            return await _ctx.Cards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
