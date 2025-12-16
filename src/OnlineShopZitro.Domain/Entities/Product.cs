namespace OnlineShopZitro.Domain.Entities;

public class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public ProductStatus Status { get; private set; }

    public Product(string id, string name, decimal price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        Status = ProductStatus.Available;
    }

    public bool CanBePurchased() => Status == ProductStatus.Available;

    public void MarkAsLocked()
    {
        if (!CanBePurchased())
            throw new InvalidOperationException("Product cannot be locked");
        Status = ProductStatus.Locked;
    }

    public void MarkAsSold()
    {
        Status = ProductStatus.Sold;
    }

    public void MarkAsAvailable()
    {
        Status = ProductStatus.Available;
    }
}

