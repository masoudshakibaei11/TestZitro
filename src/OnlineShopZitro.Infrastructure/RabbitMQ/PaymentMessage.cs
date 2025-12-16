namespace OnlineShopZitro.Infrastructure.RabbitMQ;

public class PaymentMessage
{
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}