namespace OnlineShopZitro.Application.DTOs;

public class AddToBasketRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
}
