using BankProject.Services.Interfaces;
using BankProject.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace BankProject.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private const string SessionKeyUserId = "_UserId";

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var contacts = await _contactService.GetAllAsync(userId.Value);
            return View(contacts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _contactService.CreateAsync(userId.Value, model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            await _contactService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
