using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Application.DTOs;

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;

    public static ProductDto FromEntity(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status.ToString()
        };
    }
}