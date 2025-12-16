
namespace OnlineShopZitro.Infrastructure.DTOs;

public class BasketDto
{
    public required string UserId { get; set; }
    public List<BasketItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; private set; }

}