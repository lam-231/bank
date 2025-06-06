using BankProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.Controllers
{
    public class MobileController : Controller
    {
        private readonly IUserService _userService;
        private const string SessionKeyUserId = "_UserId";

        public MobileController(IUserService userService)
        {
            _userService = userService;
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

            return View(user);
        }
    }
}
