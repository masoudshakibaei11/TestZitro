using System.Text.Json;
using StackExchange.Redis;
using OnlineShopZitro.Domain.Entities;
using OnlineShopZitro.Domain.Interfaces;
using OnlineShopZitro.Infrastructure.Redis;
using Microsoft.Extensions.Logging;
using OnlineShopZitro.Infrastructure.DTOs;

namespace OnlineShopZitro.Infrastructure.Persistence.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _redis;
    private const string BasketKeyPrefix = "basket:";
    private const string LockKeyPrefix = "lock:product:";

    public BasketRepository()
    {
        _redis = RedisConnectionFactory.GetDatabase();
    }

    public async Task<Basket?> GetAsync(string userId)
    {
        var key = GetBasketKey(userId);
        var value = await _redis.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        var dto = JsonSerializer.Deserialize<BasketDto>(value!);
        if (dto == null)
            return null;

        var basket = new Basket(dto.UserId, new List<BasketItem>(), dto.CreatedAt);

        foreach (var itemDto in dto.Items)
        {
            var item = new BasketItem(itemDto.ProductId, itemDto.ProductName, itemDto.Price);
            basket.AddItem(item);
        }

        return basket;
    }

    public async Task SaveAsync(Basket basket)
    {
        var key = GetBasketKey(basket.UserId);
        var value = JsonSerializer.Serialize(basket);

        await _redis.StringSetAsync(key, value, TimeSpan.FromMinutes(10));
    }

    public async Task DeleteAsync(string userId)
    {
        var key = GetBasketKey(userId);
        await _redis.KeyDeleteAsync(key);
    }

    public async Task<bool> LockProductAsync(string productId, string userId, TimeSpan expiration)
    {
        var key = GetLockKey(productId);
        return await _redis.StringSetAsync(key, userId, expiration, When.NotExists);
    }

    public async Task<bool> IsProductLockedAsync(string productId)
    {
        var key = GetLockKey(productId);
        return await _redis.KeyExistsAsync(key);
    }

    public async Task UnlockProductAsync(string productId)
    {
        var key = GetLockKey(productId);
        await _redis.KeyDeleteAsync(key);
    }

    public async Task<string?> GetProductLockOwnerAsync(string productId)
    {
        var key = GetLockKey(productId);
        var owner = await _redis.StringGetAsync(key);
        return owner.IsNullOrEmpty ? null : owner.ToString();
    }

    private static string GetBasketKey(string userId) => $"{BasketKeyPrefix}{userId}";
    private static string GetLockKey(string productId) => $"{LockKeyPrefix}{productId}";

}