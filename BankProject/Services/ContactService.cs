using BankProject.Models;
using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;
using BankProject.ViewModels;

namespace BankProject.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepo;
        private readonly ICardRepository _cardRepo;     

        public ContactService(IContactRepository contactRepo, ICardRepository cardRepo)
        {
            _contactRepo = contactRepo;
            _cardRepo = cardRepo;
        }

        public async Task<List<Contact>> GetAllAsync(int userId)
        {
            return await _contactRepo.GetAllByUserIdAsync(userId);
        }

        public async Task<(bool Success, string Error)> CreateAsync(int userId, ContactCreateViewModel model)
        {
            var cardExists = await _cardRepo.CardNumberExistsAsync(model.ContactCardNumber);
            if (!cardExists)
            {
                return (false, "Картка з таким номером не знайдена.");
            }

            var alreadyExists = await _contactRepo.ExistsByUserAndCardAsync(userId, model.ContactCardNumber);
            if (alreadyExists)
            {
                return (false, "Цей контакт уже доданий.");
            }

            var contact = new Contact
            {
                UserId = userId,
                ContactName = model.ContactName.Trim(),
                ContactCardNumber = model.ContactCardNumber.Trim()
            };

            await _contactRepo.AddAsync(contact);
            await _contactRepo.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteAsync(int contactId)
        {
            var contact = await _contactRepo.GetByIdAsync(contactId);
            if (contact == null)
                return;

            await _contactRepo.DeleteAsync(contact);
            await _contactRepo.SaveChangesAsync();
        }
    }
}
