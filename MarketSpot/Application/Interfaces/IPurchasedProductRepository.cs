using MarketSpot.Core.Entities;
namespace MarketSpot.Application.Interfaces
{
    public interface IPurchasedProductRepository
    {
        Task AddAsync(PurchasedProduct purchasedProduct);
        Task<IEnumerable<PurchasedProduct>> GetByUserIdAsync(string userId);
    }
}