using BankProject.Models;

namespace BankProject.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllByUserIdAsync(int userId);
        Task<Contact> GetByIdAsync(int id);
        Task AddAsync(Contact contact);
        Task DeleteAsync(Contact contact);
        Task<bool> ExistsByUserAndCardAsync(int userId, string cardNumber);
        Task SaveChangesAsync();
    }
}
