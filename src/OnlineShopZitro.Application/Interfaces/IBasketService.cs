using OnlineShopZitro.Application.DTOs;

namespace OnlineShopZitro.Application.Interfaces;

public interface IBasketService
{
    Task<ApiResponse<BasketDto>> AddToBasketAsync(AddToBasketRequest request);
    Task<ApiResponse<BasketDto>> GetBasketAsync(string userId);
}