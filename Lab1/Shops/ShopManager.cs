using ShopExceptions;
namespace Shops;

public class ShopManager : IShopManager
{
    private List<Shop> _shops;
    private List<Product> _products;

    public ShopManager()
    {
        _shops = new List<Shop>();
        _products = new List<Product>();
    }

    public Shop CreateShop(int id, string title, string address)
    {
        if (CheckSimilarShops(id))
            throw new ShopWithSimilarIdException($"Shop with id : {id} is already in base!");
        var newShop = new Shop(id, title, address);
        _shops.Add(newShop);
        return newShop;
    }

    public Product RegisterProduct(string product, double? price = null)
    {
        if (CheckSimilarProducts(product))
            throw new SimilarProductsException($"Product {product} is already in base!");
        var newProduct = new Product(product, price);
        _products.Add(newProduct);
        return newProduct;
    }

    public Shop? FindShop(Shop shop) => _shops.SingleOrDefault(sh => sh == shop);

    public Shop GetShop(Shop shop)
    {
        Shop? s = _shops.SingleOrDefault(sh => sh == shop);
        if (s is null) throw new ShopNullReferenceException("There is no such shop!");
        return s;
    }

    public Shop GetShopWithTheMinProductConsignmentPrice(params KeyValuePair<Product, int>[] products)
    {
        Shop? resultShop = null;
        double sumOfPrCons = double.MaxValue;
        foreach (Shop shop in _shops)
        {
            double tempSum;
            if (!((tempSum = shop.GetCostOfProductConsignment(products)) > 0) || !(tempSum < sumOfPrCons)) continue;
            resultShop = shop;
            sumOfPrCons = tempSum;
        }

        if (resultShop is null) throw new ShopNullReferenceException("There is no any shop with such product consignment!");
        return resultShop;
    }

    public Product? FindProduct(string product) => _products.SingleOrDefault(pr => pr.Name == product);

    public Product GetProduct(string product)
    {
        Product? prod = _products.SingleOrDefault(pr => pr.Name == product);
        if (prod is null) throw new ProductNotInShopException($"There is no product {product} in the shop!");
        return prod;
    }

    private bool CheckSimilarShops(int id) => _shops.Any(sh => sh.Id == id);

    private bool CheckSimilarProducts(string product) => _products.Any(pr => pr.Name == product);
}