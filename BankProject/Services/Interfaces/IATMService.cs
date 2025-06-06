namespace BankProject.Services.Interfaces
{
    public interface IATMService
    {
        Task<decimal> GetTotalCashAsync();                                     
        Task AddCashAsync(decimal amount);                                      
        Task<bool> WithdrawCashAsync(decimal amount);
    }
}
