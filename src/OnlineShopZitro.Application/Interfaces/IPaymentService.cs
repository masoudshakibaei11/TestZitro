using OnlineShopZitro.Application.DTOs;

namespace OnlineShopZitro.Application.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<string>> StartPaymentAsync(StartPaymentRequest request);
    Task ProcessPaymentAsync(string userId, decimal amount);
}