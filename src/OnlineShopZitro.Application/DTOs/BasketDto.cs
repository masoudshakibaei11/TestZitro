using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Application.DTOs;

public class BasketDto
{
    public string UserId { get; set; } = string.Empty;
    public List<BasketItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }

    public static BasketDto FromEntity(Basket basket)
    {
        return new BasketDto
        {
            UserId = basket.UserId,
            Items = basket.Items.Select(BasketItemDto.FromEntity).ToList(),
            TotalAmount = basket.GetTotalAmount()
        };
    }
}