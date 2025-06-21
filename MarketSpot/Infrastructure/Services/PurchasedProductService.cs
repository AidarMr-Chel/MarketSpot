using MarketSpot.Application.DTOs;
using MarketSpot.Application.Interfaces;
using MarketSpot.Core.Entities;
using MarketSpot.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
public class PurchasedProductService : IPurchasedProductService
{
    private readonly IPurchasedProductRepository _purchasedProductRepository;

    public PurchasedProductService(IPurchasedProductRepository purchasedProductRepository)
    {
        _purchasedProductRepository = purchasedProductRepository;
    }

    public async Task AddPurchasedProductAsync(string userId, Guid productId)
    {
        var purchasedProduct = new PurchasedProduct
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = productId,
            PurchasedAt = DateTime.UtcNow
        };

        await _purchasedProductRepository.AddAsync(purchasedProduct);
    }

    public async Task<IEnumerable<PurchasedProduct>> GetPurchasedProductsByUserAsync(string userId)
    {
        return await _purchasedProductRepository.GetByUserIdAsync(userId);
    }
}