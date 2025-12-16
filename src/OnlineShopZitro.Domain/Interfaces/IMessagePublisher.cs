namespace OnlineShopZitro.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishPaymentRequestAsync(string userId, decimal amount);
}