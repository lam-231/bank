using BankProject.Models;

namespace BankProject.Repositories.Interfaces
{
    public interface IATMRepository
    {
        Task<ATM> GetAsync();                                    
        Task AddAsync(ATM atm);                                  
        Task UpdateAsync(ATM atm);                               
        Task SaveChangesAsync();
    }
}
