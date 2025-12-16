using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineShopZitro.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OnlineShopZitro.Infrastructure.RabbitMQ;

public class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMQPublisher(
        IOptions<RabbitMQSettings> settings,
        ILogger<RabbitMQPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    private void EnsureConnection()
    {
        if (_connection != null && _connection.IsOpen)
            return;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _logger.LogInformation("RabbitMQ connection established");
    }

    public Task PublishPaymentRequestAsync(string userId, decimal amount)
    {
        EnsureConnection();

        var message = new PaymentMessage
        {
            UserId = userId,
            Amount = amount,
            Timestamp = DateTime.UtcNow
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = _channel!.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: "",
            routingKey: _settings.QueueName,
            basicProperties: properties,
            body: body);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
