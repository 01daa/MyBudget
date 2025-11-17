using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyBudgetMvc.Controllers
{
    public class FinanceController : Controller
    {
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("IsLoggedIn") == "true";
        }

        private IActionResult RequireLogin(string actionName)
        {
            var returnUrl = Url.Action(actionName, "Finance");
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [HttpGet]
        public IActionResult Budget()
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Budget));

            return View();
        }

        [HttpPost]
        public IActionResult Budget(decimal income, decimal expenses)
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Budget));

            ViewBag.Income = income;
            ViewBag.Expenses = expenses;
            ViewBag.Balance = income - expenses;

            return View();
        }

        [HttpGet]
        public IActionResult Goal()
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Goal));

            return View();
        }

        [HttpPost]
        public IActionResult Goal(decimal currentAmount, decimal targetAmount)
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Goal));

            ViewBag.CurrentAmount = currentAmount;
            ViewBag.TargetAmount = targetAmount;

            if (targetAmount > 0)
            {
                var progress = (currentAmount / targetAmount) * 100m;
                if (progress < 0) progress = 0;
                if (progress > 100) progress = 100;
                ViewBag.Progress = decimal.Round(progress, 2);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Report()
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Report));

            return View();
        }

        [HttpPost]
        public IActionResult Report(decimal food, decimal transport, decimal fun)
        {
            if (!IsLoggedIn())
                return RequireLogin(nameof(Report));

            var total = food + transport + fun;

            ViewBag.Food = food;
            ViewBag.Transport = transport;
            ViewBag.Fun = fun;
            ViewBag.Total = total;

            return View();
        }
    }
}
