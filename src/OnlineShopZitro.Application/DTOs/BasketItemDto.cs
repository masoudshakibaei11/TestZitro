using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Application.DTOs;

public class BasketItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }

    public static BasketItemDto FromEntity(BasketItem item)
    {
        return new BasketItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Price = item.Price,
            TotalPrice = item.GetTotalPrice()
        };
    }
}