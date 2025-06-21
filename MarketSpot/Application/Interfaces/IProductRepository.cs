using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketSpot.Application.DTOs;
using MarketSpot.Core.Entities;

namespace MarketSpot.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        IQueryable<Product> Query();
        Task<List<Product>> GetByAuthorAsync(string authorId);
        Task DeleteAsync(Product product);

    }

}
