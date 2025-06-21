using MarketSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MarketSpot.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPurchasedProductService _purchasedproductService;
        private readonly ILogger<AccountController> _logger; 


        public AccountController(IPurchasedProductService purchasedproductService, ILogger<AccountController> logger)
        {
            _purchasedproductService = purchasedproductService;
            _logger = logger;
        }


        public async Task<IActionResult> Downloads()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Попытка доступа к Downloads без авторизации");
                return Challenge();
            }

            _logger.LogInformation("Пользователь {UserId} запросил список купленных товаров", userId);

            var purchasedProducts = await _purchasedproductService.GetPurchasedProductsByUserAsync(userId);

            return View(purchasedProducts);
        }

        public IActionResult Signup()
        {
            _logger.LogInformation("Переход к странице регистрации");
            return View();
        }

        public IActionResult AuthorSignup()
        {
            _logger.LogInformation("Переход к странице регистрации автора");
            return View();
        }

        public IActionResult Login()
        {
            _logger.LogInformation("Переход к странице входа");
            return View();
        }

        public IActionResult MyItems()
        {
            _logger.LogInformation("Переход к странице MyItems");
            return View();
        }

        public IActionResult Settings()
        {
            _logger.LogInformation("Переход к странице настроек");
            return View();
        }

        public IActionResult AddItem()
        {
            _logger.LogInformation("Переход к странице добавления товара");
            return View();
        }
    }
}
