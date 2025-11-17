using Microsoft.AspNetCore.Mvc;
using MyBudgetMvc.Models;
using MyBudgetMvc.Services;
using Microsoft.AspNetCore.Http;

namespace MyBudgetMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserStore _userStore;
        private readonly IConfiguration _configuration;

        public AccountController(UserStore userStore, IConfiguration configuration)
        {
            _userStore = userStore;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_userStore.IsUserNameTaken(model.UserName))
            {
                ModelState.AddModelError(nameof(model.UserName), "Користувач з таким логіном вже існує.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _userStore.SaveUser(model);
            return RedirectToAction("Profile", new { userName = model.UserName });
        }

        [HttpGet]
        public IActionResult Profile(string userName)
        {
            var user = _userStore.GetUser(userName);
            if (user == null)
            {
                ViewBag.UserName = userName;
                return View("ProfileNotFound");
            }

            return View(user);
        }

        public IActionResult Login(string? returnUrl = null)
        {
            var domain = _configuration.GetValue<string>("Auth0:Domain");
            var clientId = _configuration.GetValue<string>("Auth0:ClientId");
            var redirectUri = Url.Action("AuthCallback", "Account", null, Request.Scheme);

            var url =
                $"https://{domain}/authorize" +
                $"?client_id={Uri.EscapeDataString(clientId!)}" +
                $"&response_type=code" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri!)}" +
                $"&scope=openid%20profile%20email";

            if (!string.IsNullOrEmpty(returnUrl))
            {
                url += $"&state={Uri.EscapeDataString(returnUrl)}";
            }

            return Redirect(url);
        }

        public IActionResult AuthCallback(string? code, string? state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("IsLoggedIn", "true");

            var returnUrl = string.IsNullOrEmpty(state) ? Url.Action("Index", "Home") : state;
            return Redirect(returnUrl!);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsLoggedIn");
            return RedirectToAction("Index", "Home");
        }
    }
}
