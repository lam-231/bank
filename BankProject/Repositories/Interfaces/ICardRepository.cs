using BankProject.Models;

namespace BankProject.Repositories.Interfaces
{
    public interface ICardRepository
    {
        Task<bool> CardNumberExistsAsync(string cardNumber);
        Task AddAsync(Card card);
        Task SaveChangesAsync();
    }
}
