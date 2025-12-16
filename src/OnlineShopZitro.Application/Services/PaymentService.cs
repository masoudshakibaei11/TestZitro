using OnlineShopZitro.Application.DTOs;
using OnlineShopZitro.Application.Interfaces;
using OnlineShopZitro.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace OnlineShopZitro.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IBasketRepository basketRepository,
        IProductRepository productRepository,
        IMessagePublisher messagePublisher,
        ILogger<PaymentService> logger)
    {
        _basketRepository = basketRepository;
        _productRepository = productRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task<ApiResponse<string>> StartPaymentAsync(StartPaymentRequest request)
    {
        var basket = await _basketRepository.GetAsync(request.UserId);

        if (basket == null || basket.IsEmpty())
            return ApiResponse<string>.ErrorResponse("سبد خرید خالی است");

        var totalAmount = basket.GetTotalAmount();

        await _messagePublisher.PublishPaymentRequestAsync(request.UserId, totalAmount);

        return ApiResponse<string>.SuccessResponse(
            $"مبلغ: {totalAmount:N0} ریال",
            "درخواست پرداخت با موفقیت ارسال شد و در حال پردازش است");
    }

    public async Task ProcessPaymentAsync(string userId, decimal amount)
    {
        _logger.LogInformation($"Start Payment {userId} با مبلغ {amount}");

        var basket = await _basketRepository.GetAsync(userId);
        if (basket == null)
        {
            _logger.LogWarning($"Basket Not Found {userId} ");
            return;
        }


        await Task.Delay(2000);
        var isSuccessful = new Random().NextDouble() > 0.2;

        if (isSuccessful)
        {
            _logger.LogInformation($"Payment Success {userId}");


            foreach (var item in basket.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.MarkAsSold();
                    await _productRepository.UpdateAsync(product);
                    await _basketRepository.UnlockProductAsync(item.ProductId);
                }
            }
        }
        else
        {
            _logger.LogWarning($"Payment Not Success {userId}");


            foreach (var item in basket.Items)
            {
                await _basketRepository.UnlockProductAsync(item.ProductId);

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.MarkAsAvailable();
                    await _productRepository.UpdateAsync(product);
                }
            }

        }

        await _basketRepository.DeleteAsync(userId);
    }
}