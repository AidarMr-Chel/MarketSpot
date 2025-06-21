using MarketSpot.Application.DTOs;
using MarketSpot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSpot.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductModel model, string authorId);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<PaginatedList<ProductDto>> SearchProductsAsync(string? title, string? category, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
        Task<List<ProductDto>> GetProductsByAuthorAsync(string authorId);
        Task DeleteAsync(Guid id);





    }

}
