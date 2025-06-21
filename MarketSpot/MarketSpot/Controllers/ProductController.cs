using MarketSpot.Application.Interfaces;
using MarketSpot.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketSpot.Web.Controllers
{
    [Authorize(Roles = "Author")]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            UserManager<ApplicationUser> userManager,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("MyItems")]
        public async Task<IActionResult> MyItems()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Попытка доступа к MyItems без аутентификации");
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("Загрузка товаров автора с Id {UserId}", user.Id);

            var products = await _productService.GetProductsByAuthorAsync(user.Id);

            _logger.LogInformation("Найдено {Count} товаров для автора {UserId}", products?.Count ?? 0, user.Id);

            return View("~/Views/Account/MyItems.cshtml", products);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Запрос на удаление продукта с Id {ProductId}", id);

                await _productService.DeleteAsync(id);

                _logger.LogInformation("Продукт с Id {ProductId} успешно удален", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении продукта с Id {ProductId}", id);
            }

            return RedirectToAction("MyItems");
        }
    }
}
