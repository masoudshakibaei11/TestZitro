namespace OnlineShopZitro.Domain.Entities;

public class Basket
{
    public string UserId { get; private set; }
    public List<BasketItem> Items { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Basket(string userId, List<BasketItem> items, DateTime createdAt)
    {
        UserId = userId;
        Items = items;
        CreatedAt = createdAt;
    }

    public Basket(string userId)
    {
        UserId = userId;
        Items = new List<BasketItem>();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(BasketItem item)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        Items.Add(item);

    }

    public void RemoveItem(string productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public decimal GetTotalAmount()
    {
        return Items.Sum(i => i.GetTotalPrice());
    }

    public bool IsEmpty() => !Items.Any();
}

