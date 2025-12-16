using OnlineShopZitro.Application.DTOs;
using OnlineShopZitro.Application.Interfaces;
using OnlineShopZitro.Domain.Entities;
using OnlineShopZitro.Domain.Interfaces;

namespace OnlineShopZitro.Application.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductRepository _productRepository;

    public BasketService(
        IBasketRepository basketRepository,
        IProductRepository productRepository)
    {
        _basketRepository = basketRepository;
        _productRepository = productRepository;
    }

    public async Task<ApiResponse<BasketDto>> AddToBasketAsync(AddToBasketRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            return ApiResponse<BasketDto>.ErrorResponse("محصول یافت نشد");

        if (!product.CanBePurchased())
            return ApiResponse<BasketDto>.ErrorResponse("محصول در دسترس نیست");


        if (await _basketRepository.IsProductLockedAsync(request.ProductId))
        {
            var lockOwner = await _basketRepository.GetProductLockOwnerAsync(request.ProductId);
            if (lockOwner != request.UserId)
                return ApiResponse<BasketDto>.ErrorResponse("محصول در حال حاضر توسط کاربر دیگری رزرو شده است");
        }

        var lockSuccess = await _basketRepository.LockProductAsync(
            request.ProductId,
            request.UserId,
            TimeSpan.FromMinutes(10));

        if (!lockSuccess)
            return ApiResponse<BasketDto>.ErrorResponse("خطا در قفل کردن محصول");

        var basket = await _basketRepository.GetAsync(request.UserId);
        if (basket == null)
        {
            basket = new Basket(request.UserId);
        }

        var basketItem = new BasketItem(
            product.Id,
            product.Name,
            product.Price);

        basket.AddItem(basketItem);

        await _basketRepository.SaveAsync(basket);

        product.MarkAsLocked();
        await _productRepository.UpdateAsync(product);

        return ApiResponse<BasketDto>.SuccessResponse(
            BasketDto.FromEntity(basket),
            "محصول با موفقیت به سبد خرید اضافه شد و برای 10 دقیقه قفل گردید");
    }

    public async Task<ApiResponse<BasketDto>> GetBasketAsync(string userId)
    {
        var basket = await _basketRepository.GetAsync(userId);

        if (basket == null || basket.IsEmpty())
            return ApiResponse<BasketDto>.ErrorResponse("سبد خرید خالی است");

        return ApiResponse<BasketDto>.SuccessResponse(BasketDto.FromEntity(basket));
    }
}