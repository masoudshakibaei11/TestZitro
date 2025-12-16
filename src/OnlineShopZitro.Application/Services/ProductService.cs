using OnlineShopZitro.Application.DTOs;
using OnlineShopZitro.Application.Interfaces;
using OnlineShopZitro.Domain.Interfaces;

namespace OnlineShopZitro.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(ProductDto.FromEntity).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(string productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product != null ? ProductDto.FromEntity(product) : null;
    }
}