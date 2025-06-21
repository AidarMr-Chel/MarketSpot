using MarketSpot.Application.DTOs;
using MarketSpot.Application.Interfaces;
using MarketSpot.Core.Entities;
using MarketSpot.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;


public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly string _uploadRootPath;

    public ProductService(IProductRepository productRepository, string uploadRootPath)
    {
        _productRepository = productRepository;
        _uploadRootPath = uploadRootPath;
    }

    public async Task AddProductAsync(AddProductModel model, string authorId)
    {
        Console.WriteLine("Сохраняем продукт:");
        Console.WriteLine($"Title: {model.Title}");
        Console.WriteLine($"AuthorId: {authorId}");

        Directory.CreateDirectory(_uploadRootPath);

        string productFileName = $"{Guid.NewGuid()}_{model.ProductFile.FileName}";
        string productPath = Path.Combine(_uploadRootPath, productFileName);
        using (var stream = new FileStream(productPath, FileMode.Create))
        {
            await model.ProductFile.CopyToAsync(stream);
        }

        string imageFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
        string imagePath = Path.Combine(_uploadRootPath, imageFileName);

        Console.WriteLine($"ImagePath: /uploads/{imagePath}");

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await model.ImageFile.CopyToAsync(stream);
        }

        var product = new Product
        {
            Title = model.Title,
            Subtitle = model.Subtitle,
            Category = model.Category,
            ShortDescription = model.ShortDescription,
            FullDescription = model.FullDescription,
            Price = model.Price,
            DiscountPrice = model.DiscountPrice,
            AuthorId = authorId,
            ProductFilePath = $"/uploads/{productFileName}",
            ImageFilePath = $"/uploads/{imageFileName}"
        };
        Console.WriteLine(product.Id);
        await _productRepository.AddAsync(product);
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<PaginatedList<ProductDto>> SearchProductsAsync(string? title, string? category, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
    {
        var query = _productRepository.Query(); 

        if (!string.IsNullOrEmpty(title))
            query = query.Where(p => p.Title.Contains(title));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageFilePath = p.ImageFilePath,
                Category = p.Category
            })
            .ToListAsync();

        return new PaginatedList<ProductDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<List<ProductDto>> GetProductsByAuthorAsync(string authorId)
    {
        var products = await _productRepository.GetByAuthorAsync(authorId);

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Title = p.Title,
            Subtitle = p.Subtitle,
            Price = p.Price,
            DiscountPrice = p.DiscountPrice,
            ImageFilePath = p.ImageFilePath,
        }).ToList();
    }
    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return;

        if (File.Exists(product.ImageFilePath))
            File.Delete(product.ImageFilePath);
        if (File.Exists(product.ProductFilePath))
            File.Delete(product.ProductFilePath);

        await _productRepository.DeleteAsync(product);
    }



}
