using BankProject.Models;
using BankProject.ViewModels;
using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BankProject.Controllers
{
    public class ATMController : Controller
    {
        private const string SessionKeyATMUserId = "_ATMUserId";
        private readonly ICardRepository _cardRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IATMService _atmService;

        public ATMController(
            ICardRepository cardRepo,
            IPasswordHasher<User> passwordHasher,
            IATMService atmService)
        {
            _cardRepo = cardRepo;
            _passwordHasher = passwordHasher;
            _atmService = atmService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32(SessionKeyATMUserId) != null)
                return RedirectToAction("Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 12)
            {
                ModelState.AddModelError(string.Empty, "Введіть дійсний 12-значний номер картки.");
                return View();
            }

            var card = await _cardRepo.GetByCardNumberAsync(cardNumber);
            if (card == null)
            {
                ModelState.AddModelError(string.Empty, "Картку не знайдено.");
                return View();
            }

            HttpContext.Session.SetInt32(SessionKeyATMUserId, card.UserId);
            return RedirectToAction("Authenticate");
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authenticate(string cvv, string password)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            var userCard = await _cardRepo.GetByUserIdAsync(userId.Value);
            if (userCard == null)
            {
                HttpContext.Session.Remove(SessionKeyATMUserId);
                return RedirectToAction("Login");
            }

            var user = userCard.User;
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Невірний пароль.");
                return View();
            }

            return RedirectToAction("Home");
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            var userCard = await _cardRepo.GetByUserIdAsync(userId.Value);
            if (userCard == null)
            {
                HttpContext.Session.Remove(SessionKeyATMUserId);
                return RedirectToAction("Login");
            }

            var atmCash = await _atmService.GetTotalCashAsync();
            ViewData["ATMCash"] = atmCash;

            return View(userCard);
        }

        [HttpGet]
        public IActionResult Deposit()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(ATMDepositViewModel model)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Amount > 5000m)
            {
                ModelState.AddModelError(string.Empty, "Максимальний депозит за раз – 5000.");
                return View(model);
            }

            var userCard = await _cardRepo.GetByUserIdAsync(userId.Value);
            if (userCard == null)
            {
                ModelState.AddModelError(string.Empty, "Помилка: картку користувача не знайдено.");
                return View(model);
            }

            userCard.Balance += model.Amount;
            await _cardRepo.UpdateAsync(userCard);
            await _cardRepo.SaveChangesAsync();

            await _atmService.AddCashAsync(model.Amount);

            TempData["SuccessMessage"] = $"Успішно покладено {model.Amount:C} на рахунок.";
            return RedirectToAction("Home");
        }

        [HttpGet]
        public async Task<IActionResult> Withdraw()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            var atmCash = await _atmService.GetTotalCashAsync();
            ViewData["ATMCash"] = atmCash;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(ATMWithdrawViewModel model)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyATMUserId);
            if (userId == null)
                return RedirectToAction("Login");

            var atmCash = await _atmService.GetTotalCashAsync();
            ViewData["ATMCash"] = atmCash;

            if (!ModelState.IsValid)
                return View(model);

            if (model.Amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Сума має бути більшою за 0.");
                return View(model);
            }

            if (atmCash < model.Amount)
            {
                ModelState.AddModelError(string.Empty, "Немає достатньо готівки в банкоматі.");
                return View(model);
            }

            var userCard = await _cardRepo.GetByUserIdAsync(userId.Value);
            if (userCard == null)
            {
                ModelState.AddModelError(string.Empty, "Помилка: картку користувача не знайдено.");
                return View(model);
            }

            if (userCard.Balance < model.Amount)
            {
                ModelState.AddModelError(string.Empty, "Недостатньо коштів на вашому рахунку.");
                return View(model);
            }

            userCard.Balance -= model.Amount;
            await _cardRepo.UpdateAsync(userCard);

            var success = await _atmService.WithdrawCashAsync(model.Amount);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Помилка при знятті коштів з банкомату.");
                return View(model);
            }

            await _cardRepo.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Успішно знято {model.Amount:C} із рахунку.";
            return RedirectToAction("Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyATMUserId);
            return RedirectToAction("Login");
        }
    }
}
