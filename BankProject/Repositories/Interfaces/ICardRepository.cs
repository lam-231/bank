using BankProject.Models;

namespace BankProject.Repositories.Interfaces
{
    public interface ICardRepository
    {
        Task<bool> CardNumberExistsAsync(string cardNumber);
        Task AddAsync(Card card);

        Task<Card> GetByCardNumberAsync(string cardNumber);
        Task<Card> GetByUserIdAsync(int userId);

        Task SaveChangesAsync();
    }
}
