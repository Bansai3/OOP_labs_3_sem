using Xunit;

namespace Shops.Test;
using Pair = KeyValuePair<Product, int>;

public class ShopsTest
{
    private Dictionary<Product, int> _products = new Dictionary<Product, int>();
    private ShopManager _shopManager = new ShopManager();

    [Theory]
    [InlineData(100, 20, 4, 3)]
    public void BuyProductsConsignmentFromShop(double moneyBefore, double productPrice, int productCount, int productToBuyCount)
    {
        var customer = new Сustomer("Vlad Gumbatov Vladimirovich", moneyBefore);
        Shop shop = _shopManager.CreateShop(23, "Magnet", "Moscow SaintStreet 5");
        Product product = _shopManager.RegisterProduct("orange", productPrice);
        InitializeProducts(new Pair(product, productCount));
        shop.AddProducts(_products);
        shop.Buy(customer, new Pair(product, productToBuyCount));
        Assert.Equal(moneyBefore - (productPrice * productToBuyCount), customer.Money);
        Assert.Equal(productCount - productToBuyCount, shop.GetProductCount("orange"));
    }

    [Theory]
    [InlineData(20, 30)]
    public void ChangeProductPrice(double priceBefore, double newPrice)
    {
        Shop shop = _shopManager.CreateShop(23, "Pyaterochka", "Moscow Grecheskiy 10");
        Product product = _shopManager.RegisterProduct("watermelon", priceBefore);
        InitializeProducts(new Pair(product, 10));
        shop.AddProducts(_products);
        shop.ChangeProductPrice(product, newPrice);
        Assert.Equal(newPrice, shop.GetProduct("watermelon").Price);
    }

    [Fact]
    public void GetShopWithTheMinProductConsignmentPrice()
    {
        Shop shop1 = _shopManager.CreateShop(23, "Pyaterochka", "Moscow Grecheskiy 10");
        Shop shop2 = _shopManager.CreateShop(24, "Magnet", "Saint-Petersburg SaintStreet 5");
        Product product1 = _shopManager.RegisterProduct("melon", 40);
        Product product2 = _shopManager.RegisterProduct("watermelon", 20);
        Product product3 = _shopManager.RegisterProduct("lemon", 15);
        InitializeProducts(new Pair(product1, 10), new Pair(product2, 20), new Pair(product3, 30));
        shop1.AddProducts(_products);
        product1.ChangeProductPrice(50);
        shop2.AddProducts(_products);
        Shop resultShop = _shopManager.GetShopWithTheMinProductConsignmentPrice(
            new Pair(new Product("melon"), 1),
            new Pair(new Product("lemon"), 10));
        Assert.Equal(shop1, resultShop);
    }

    private void InitializeProducts(params Pair[] products)
    {
        foreach (Pair product in products)
        {
            _products.Add(product.Key, product.Value);
        }
    }
}