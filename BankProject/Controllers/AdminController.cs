using BankProject.Repositories.Interfaces;
using BankProject.Services.Interfaces;
using BankProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IATMService _atmService;

        public AdminController(IUserRepository userRepo, IATMService atmService)
        {
            _userRepo = userRepo;
            _atmService = atmService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allUsers = await _userRepo.GetAllAsync();

            var atmCash = await _atmService.GetTotalCashAsync();

            var vm = new AdminIndexViewModel
            {
                AllUsers = allUsers,
                ATMCash = atmCash
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCash(AdminAddCashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userRepo.GetAllAsync();
                var cash = await _atmService.GetTotalCashAsync();
                ViewData["AllUsers"] = users;
                ViewData["ATMCash"] = cash;
                return View("Index", new AdminIndexViewModel
                {
                    AllUsers = users,
                    ATMCash = cash
                });
            }

            await _atmService.AddCashAsync(model.Amount);

            TempData["SuccessMessage"] = $"Успішно додано {model.Amount:C} до банкомату.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
