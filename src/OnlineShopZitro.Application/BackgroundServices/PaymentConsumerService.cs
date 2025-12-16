using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineShopZitro.Application.Interfaces;
using OnlineShopZitro.Infrastructure.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OnlineShopZitro.Application.BackgroundServices;

public class PaymentConsumerService : BackgroundService
{
    private readonly ILogger<PaymentConsumerService> _logger;
    private readonly RabbitMQSettings _settings;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IModel? _channel;

    public PaymentConsumerService(
        ILogger<PaymentConsumerService> logger,
        IOptions<RabbitMQSettings> settings,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _settings = settings.Value;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Payment Consumer Service شروع به کار کرد");

        InitializeRabbitMQ();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentMessage = JsonSerializer.Deserialize<PaymentMessage>(message);

                if (paymentMessage != null)
                {
                    _logger.LogInformation(
                        $"پیام پرداخت دریافت شد: کاربر={paymentMessage.UserId}, مبلغ={paymentMessage.Amount}");

                    using var scope = _serviceProvider.CreateScope();
                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                    await paymentService.ProcessPaymentAsync(
                        paymentMessage.UserId,
                        paymentMessage.Amount);

                    _channel!.BasicAck(ea.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در پردازش پیام پرداخت");
                _channel!.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel!.BasicConsume(
            queue: _settings.QueueName,
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation($"شروع به گوش دادن به صف {_settings.QueueName}");

        return Task.CompletedTask;
    }

    private void InitializeRabbitMQ()
    {
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

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        base.Dispose();
    }
}