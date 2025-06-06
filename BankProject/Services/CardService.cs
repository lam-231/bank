using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;

namespace BankProject.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepo;
        private readonly IUserService _userService;

        public CardService(ICardRepository cardRepo, IUserService userService)
        {
            _cardRepo = cardRepo;
            _userService = userService;
        }

        public async Task<(bool Success, string Error)> TransferAsync(int userId, decimal amount, string destinationCardNumber)
        {
            var sourceCard = await _cardRepo.GetByUserIdAsync(userId);
            if (sourceCard == null)
                return (false, "Джерелo переказу не знайдено.");

            if (amount <= 0)
                return (false, "Сума має бути більшою за нуль.");

            if (sourceCard.Balance < amount)
                return (false, "Недостатньо коштів на вашому рахунку.");

            var destCard = await _cardRepo.GetByCardNumberAsync(destinationCardNumber);
            if (destCard == null)
                return (false, "Картку отримувача не знайдено.");

            if (destCard.CardNumber == sourceCard.CardNumber)
                return (false, "Ви не можете переказувати кошти на власну картку.");

            sourceCard.Balance -= amount;
            destCard.Balance += amount;

            try
            {
                await _cardRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (false, $"Не вдалося виконати переказ: {ex.Message}");
            }

            return (true, null);
        }
    }
}
