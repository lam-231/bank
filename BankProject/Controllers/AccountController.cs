using BankProject.Services.Interfaces;
using BankProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private const string SessionKeyUserId = "_UserId";

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32(SessionKeyUserId) != null)
            {
                return RedirectToAction("Profile", "Mobile");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, errors) = await _userService.RegisterAsync(model);
            if (!success)
            {
                foreach (var err in errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(model);
            }

            var registeredUser = await _userService.GetByEmailAsync(model.Email);
            if (registeredUser == null)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error: user was not found after registration.");
                return View(model);
            }

            HttpContext.Session.SetInt32(SessionKeyUserId, registeredUser.Id);


            return RedirectToAction("Profile", "Mobile");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32(SessionKeyUserId) != null)
            {
                return RedirectToAction("Profile", "Mobile");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateAsync(model);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Невірні дані для входу.");
                return View(model);
            }

            HttpContext.Session.SetInt32(SessionKeyUserId, user.Id);
            return RedirectToAction("Profile", "Mobile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyUserId);
            return RedirectToAction("Login");
        }
    }
}
