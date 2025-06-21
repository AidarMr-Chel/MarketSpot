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
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            IProductService productService,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("Users")]
        public IActionResult Users()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpPost("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Попытка удаления несуществующего пользователя с Id {UserId}", id);
                return NotFound();
            }

            _logger.LogInformation("Удаление пользователя с Id {UserId}", id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Ошибка при удалении пользователя {UserId}: {Errors}", id, string.Join(", ", result.Errors));
                return BadRequest(result.Errors);
            }

            return RedirectToAction("Users");
        }

        [HttpGet("UserProducts/{userId}")]
        public async Task<IActionResult> UserProducts(string userId)
        {
            var products = await _productService.GetProductsByAuthorAsync(userId);
            ViewBag.UserId = userId;
            return View(products);
        }

        [HttpPost("DeleteUserProduct/{productId}")]
        public async Task<IActionResult> DeleteUserProduct(Guid productId)
        {
            try
            {
                _logger.LogInformation("Удаление продукта с Id {ProductId} администратором", productId);
                await _productService.DeleteAsync(productId);
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении продукта с Id {ProductId}", productId);
                return BadRequest("Ошибка при удалении продукта");
            }
        }
    }
}
