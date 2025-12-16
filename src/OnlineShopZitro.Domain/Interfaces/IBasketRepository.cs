using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Domain.Interfaces;

public interface IBasketRepository
{
    Task<Basket?> GetAsync(string userId);
    Task SaveAsync(Basket basket);
    Task DeleteAsync(string userId);
    Task<bool> LockProductAsync(string productId, string userId, TimeSpan expiration);
    Task<bool> IsProductLockedAsync(string productId);
    Task UnlockProductAsync(string productId);
    Task<string?> GetProductLockOwnerAsync(string productId);
}
