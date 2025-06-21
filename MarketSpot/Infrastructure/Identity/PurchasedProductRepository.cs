using MarketSpot.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using MarketSpot.Core.Entities;
using MarketSpot.Application.Interfaces;
namespace Infrastructure.Identity
{
    public class PurchasedProductRepository : IPurchasedProductRepository
    {
        private readonly AppDbContext _context;

        public PurchasedProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PurchasedProduct purchasedProduct)
        {
            await _context.PurchasedProducts.AddAsync(purchasedProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PurchasedProduct>> GetByUserIdAsync(string userId)
        {
            return await _context.PurchasedProducts
                .Include(pp => pp.Product)
                .Where(pp => pp.UserId == userId)
                .ToListAsync();
        }
    }
}