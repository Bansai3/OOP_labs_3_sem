namespace Shops;

public interface IShop
{
    double Buy(Сustomer customer, params KeyValuePair<Product, int>[] products);
    void AddProducts(Dictionary<Product, int> products);
    void AddProduct(Product product, int number);
    void ChangeProductPrice(Product product, double price);
    Product? FindProduct(string productName);
    Product GetProduct(string productName);
    bool ContainsProduct(Product product);
    int GetProductCount(string product);
}