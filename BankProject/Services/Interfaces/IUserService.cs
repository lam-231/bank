using BankProject.Models;
using BankProject.ViewModels;

namespace BankProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string[] Errors)> RegisterAsync(RegisterViewModel model);
        Task<User> AuthenticateAsync(LoginViewModel model);
        Task<User> GetByIdAsync(int id);

        Task<User> GetByEmailAsync(string email);
    }
}
