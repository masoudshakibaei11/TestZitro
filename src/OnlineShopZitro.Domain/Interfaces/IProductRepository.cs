using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string productId);
    Task<List<Product>> GetAllAsync();
    Task UpdateAsync(Product product);
}
