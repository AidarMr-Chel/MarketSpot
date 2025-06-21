using MarketSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MarketSpot.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPurchasedProductService _purchasedProductService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPurchasedProductService purchasedProductService, ILogger<PaymentController> logger)
    {
        _purchasedProductService = purchasedProductService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] PaymentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            _logger.LogWarning("Пользователь не авторизован при попытке оплаты продукта {ProductId}", request.ProductId);
            return Unauthorized();
        }

        try
        {
            await _purchasedProductService.AddPurchasedProductAsync(userId, request.ProductId);
            _logger.LogInformation("Платеж успешно обработан для пользователя {UserId} и продукта {ProductId}", userId, request.ProductId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке платежа для пользователя {UserId} и продукта {ProductId}", userId, request.ProductId);
            return StatusCode(500, "Ошибка сервера при обработке платежа");
        }
    }
}
