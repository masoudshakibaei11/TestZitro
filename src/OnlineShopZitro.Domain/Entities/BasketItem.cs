namespace OnlineShopZitro.Domain.Entities;

public class BasketItem
{
    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal Price { get; private set; }

    public BasketItem(string productId, string productName, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");
    }

    public decimal GetTotalPrice() => Price;
}