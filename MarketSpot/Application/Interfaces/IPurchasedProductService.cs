using MarketSpot.Core.Entities;
namespace MarketSpot.Application.Interfaces
{
    public interface IPurchasedProductService
    {
        Task AddPurchasedProductAsync(string userId, Guid productId);
        Task<IEnumerable<PurchasedProduct>> GetPurchasedProductsByUserAsync(string userId);
    }
}
