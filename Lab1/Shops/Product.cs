using ShopExceptions;
namespace Shops;

public class Product
{
    public const double MinPrice = 0;
    public Product(string productName, double? price = null)
    {
        if (!CheckProductName(productName)) throw new ProductNameFormatException("Invalid product name!");
        Name = productName;
        if (!CheckPrice(price)) throw new NegativeMoneyException("Negative price!");
        Price = price;
    }

    public string Name { get; private set; }
    public double? Price { get; private set; }

    public void ChangeProductName(string newName)
    {
        if (!CheckProductName(newName)) throw new ProductNameFormatException("Invalid product name!");
        Name = newName;
    }

    public void ChangeProductPrice(double? newPrice)
    {
        if (!CheckPrice(newPrice)) throw new NegativeMoneyException("Negative price!");
        Price = newPrice;
    }

    private bool CheckProductName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        string[] fullName = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullName.All(str => str.All(char.IsLetter));
    }

    private bool CheckPrice(double? price) => price is null or >= MinPrice;
}