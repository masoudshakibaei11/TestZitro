using OnlineShopZitro.Domain.Entities;

namespace OnlineShopZitro.Infrastructure.Persistence;

public static class InMemoryProductStore
{
    private static readonly List<Product> _products = new()
    {
        new Product("1", "لپ‌تاپ Dell XPS 15", 45000000, 5),
        new Product("2", "گوشی Samsung Galaxy S24", 35000000, 10),
        new Product("3", "تبلت iPad Pro", 40000000, 8),
        new Product("4", "هدفون Sony WH-1000XM5", 12000000, 15),
        new Product("5", "ساعت هوشمند Apple Watch Series 9", 18000000, 12)
    };

    public static List<Product> GetProducts() => _products;

    public static Product? GetProductById(string id) => _products.FirstOrDefault(p => p.Id == id);
}