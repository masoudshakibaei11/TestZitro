
using OnlineShopZitro.Application.DTOs;

namespace OnlineShopZitro.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(string productId);
}
