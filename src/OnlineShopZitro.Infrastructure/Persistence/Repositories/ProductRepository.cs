using OnlineShopZitro.Domain.Entities;
using OnlineShopZitro.Domain.Interfaces;

namespace OnlineShopZitro.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    public Task<Product?> GetByIdAsync(string productId)
    {
        var product = InMemoryProductStore.GetProductById(productId);
        return Task.FromResult(product);
    }

    public Task<List<Product>> GetAllAsync()
    {
        var products = InMemoryProductStore.GetProducts();
        return Task.FromResult(products);
    }

    public Task UpdateAsync(Product product)
    {
        return Task.CompletedTask;
    }
}