using BankProject.Services.Interfaces;
using BankProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankProject.Controllers
{
    public class MobileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IContactService _contactService;
        private readonly ICardService _cardService;
        private const string SessionKeyUserId = "_UserId";

        public MobileController(
            IUserService userService,
            IContactService contactService,
            ICardService cardService)
        {
            _userService = userService;
            _contactService = contactService;
            _cardService = cardService;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
            {
                HttpContext.Session.Remove(SessionKeyUserId);
                return RedirectToAction("Login", "Account");
            }

            var contacts = await _contactService.GetAllAsync(userId.Value);
            ViewData["Contacts"] = contacts;
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Transfer()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var contacts = await _contactService.GetAllAsync(userId.Value);
            var vm = new TransferViewModel
            {
                AvailableContacts = contacts
                    .Select(c => new SelectListItem
                    {
                        Value = c.ContactCardNumber,                  
                        Text = $"{c.ContactName} ({c.ContactCardNumber})"
                    })
                    .ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var contacts = await _contactService.GetAllAsync(userId.Value);
            model.AvailableContacts = contacts
                .Select(c => new SelectListItem
                {
                    Value = c.ContactCardNumber,
                    Text = $"{c.ContactName} ({c.ContactCardNumber})"
                })
                .ToList();

            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _cardService.TransferAsync(userId.Value, model.Amount, model.DestinationCardNumber);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            TempData["SuccessMessage"] = "Переказ виконано успішно.";
            return RedirectToAction("Profile");
        }
    }
}
