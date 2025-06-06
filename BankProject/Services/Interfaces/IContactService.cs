using BankProject.Models;
using BankProject.ViewModels;

namespace BankProject.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllAsync(int userId);
        Task<(bool Success, string Error)> CreateAsync(int userId, ContactCreateViewModel model);
        Task DeleteAsync(int contactId);
    }
}
