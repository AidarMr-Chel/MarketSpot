using MarketSpot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketSpot.Application.Interfaces;
using MarketSpot.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;


namespace Infrastructure.Identity
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public IQueryable<Product> Query()
        {
            return _context.Products.AsQueryable();
        }
        public async Task<List<Product>> GetByAuthorAsync(string authorId)
        {
            return await _context.Products
                .Where(p => p.AuthorId == authorId)
                .ToListAsync();
        }
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

    }

}
