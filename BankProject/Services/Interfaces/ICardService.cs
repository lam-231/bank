namespace BankProject.Services.Interfaces
{
    public interface ICardService
    {
        Task<(bool Success, string Error)> TransferAsync(int userId, decimal amount, string destinationCardNumber);
    }
}
