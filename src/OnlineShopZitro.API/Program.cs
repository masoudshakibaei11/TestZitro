using OnlineShopZitro.Application.BackgroundServices;
using OnlineShopZitro.Application.Interfaces;
using OnlineShopZitro.Application.Services;
using OnlineShopZitro.Domain.Interfaces;
using OnlineShopZitro.Infrastructure.Persistence.Repositories;
using OnlineShopZitro.Infrastructure.RabbitMQ;
using OnlineShopZitro.Infrastructure.Redis;

var builder = WebApplication.CreateBuilder(args);

// تنظیمات
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Redis Configuration
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
RedisConnectionFactory.Initialize(redisConnectionString ?? "localhost:6379,abortConnect=false,defaultDatabase=0");

// RabbitMQ Configuration
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Register Application Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Register Domain Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Register Infrastructure Services
builder.Services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

// Register Background Services
builder.Services.AddHostedService<PaymentConsumerService>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("---------------App Start-------------");

app.Run();