using BankProject.Models;
using BankProject.ViewModels;
using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICardRepository _cardRepo;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepo, ICardRepository cardRepo)
        {
            _userRepo = userRepo;
            _cardRepo = cardRepo;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<(bool Success, string[] Errors)> RegisterAsync(RegisterViewModel model)
        {
            var errors = new List<string>();

            var existingUser = await _userRepo.GetByEmailAsync(model.Email);
            if (existingUser != null)
            {
                errors.Add("Користувач з таким email уже зареєстрований.");
                return (false, errors.ToArray());
            }

            var user = new User
            {
                Name = model.Name.Trim(),
                Surname = model.Surname.Trim(),
                Phone = model.Phone.Trim(),
                Email = model.Email.Trim(),
                Address = model.Address.Trim(),
                Age = model.Age
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            string cardNumber;
            do
            {
                cardNumber = GenerateCardNumber(); 
            } while (await _cardRepo.CardNumberExistsAsync(cardNumber));

            var cvv = GenerateCVV(); 

            var card = new Card
            {
                CardNumber = cardNumber,
                CVV = cvv,
                Balance = 0m,
                User = user
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync(); 

            card.UserId = user.Id;
            await _cardRepo.AddAsync(card);
            await _cardRepo.SaveChangesAsync();

            return (true, Array.Empty<string>());
        }

        public async Task<User> AuthenticateAsync(LoginViewModel model)
        {
            var user = await _userRepo.GetByEmailAsync(model.Email);
            if (user == null) return null;

            if (user.Card.CardNumber != model.CardNumber || user.Card.CVV != model.CVV)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Success)
                return user;

            return null;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepo.GetByEmailAsync(email);
        }

        private string GenerateCardNumber()
        {
            var rnd = new Random();
            string number = "";
            for (int i = 0; i < 12; i++)
                number += rnd.Next(0, 10).ToString();
            return number;
        }

        private string GenerateCVV()
        {
            var rnd = new Random();
            int cvv = rnd.Next(100, 1000); 
            return cvv.ToString();
        }
    }
}
