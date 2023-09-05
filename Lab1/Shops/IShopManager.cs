namespace Shops;

public interface IShopManager
{
    Shop CreateShop(int id, string title, string address);
    Product RegisterProduct(string product, double? price);
    Shop? FindShop(Shop shop);
    Shop GetShop(Shop shop);
    Shop GetShopWithTheMinProductConsignmentPrice(params KeyValuePair<Product, int>[] products);
    Product? FindProduct(string product);
    Product GetProduct(string product);
}