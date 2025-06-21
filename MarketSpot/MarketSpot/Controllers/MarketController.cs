using MarketSpot.Application.DTOs;
using MarketSpot.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace MarketSpot.Controllers
{
    public class MarketController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MarketController> _logger;

        public MarketController(IProductService productService, IHttpContextAccessor httpContextAccessor, ILogger<MarketController> logger)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("/product/{id}")]
        public async Task<IActionResult> Product(Guid id)
        {
            _logger.LogInformation("Запрошен продукт с Id {ProductId}", id);
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Продукт с Id {ProductId} не найден", id);
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Author")]
        [HttpPost("/product/add")]
        [RequestSizeLimit(50_000_000)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddProduct([FromForm] AddProductModel model)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogWarning("Попытка добавления продукта без идентификатора пользователя");
                return Unauthorized("User ID not found.");
            }

            _logger.LogInformation("Пользователь {UserId} добавляет продукт с заголовком: {Title}", userId, model.Title);

            try
            {
                await _productService.AddProductAsync(model, userId);
                _logger.LogInformation("Продукт успешно добавлен пользователем {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении продукта пользователем {UserId}", userId);
                return BadRequest("Error adding product.");
            }

            return Ok("Product uploaded successfully");
        }

        [HttpGet]
        public async Task<IActionResult> SearchJson(string? title, string? category, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 10)
        {
            _logger.LogInformation("Поиск продуктов с параметрами: Title={Title}, Category={Category}, MinPrice={MinPrice}, MaxPrice={MaxPrice}, Page={Page}, PageSize={PageSize}",
                title, category, minPrice, maxPrice, page, pageSize);

            var result = await _productService.SearchProductsAsync(title, category, minPrice, maxPrice, page, pageSize)
                ?? new PaginatedList<ProductDto>
                {
                    Items = new List<ProductDto>(),
                    TotalCount = 0,
                    Page = 1,
                    PageSize = pageSize
                };

            return Json(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Cart(Guid productId)
        {
            _logger.LogInformation("Пользователь зашел в корзину для продукта с Id {ProductId}", productId);
            ViewData["ProductId"] = productId;
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
    }
}
