using BankProject.Data;
using BankProject.Models;
using BankProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BankProject.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _ctx;

        public ContactRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Contact>> GetAllByUserIdAsync(int userId)
        {
            return await _ctx.Contacts
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await _ctx.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Contact contact)
        {
            await _ctx.Contacts.AddAsync(contact);
        }

        public async Task DeleteAsync(Contact contact)
        {
            _ctx.Contacts.Remove(contact);
        }

        public async Task<bool> ExistsByUserAndCardAsync(int userId, string cardNumber)
        {
            return await _ctx.Contacts
                .AnyAsync(c => c.UserId == userId && c.ContactCardNumber == cardNumber);
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
