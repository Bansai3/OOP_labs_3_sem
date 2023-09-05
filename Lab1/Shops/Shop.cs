using System.Collections.ObjectModel;
using ShopExceptions;

namespace Shops;

internal enum ShopAddressParameters
{
    City,
    Street,
    HouseNumber,
}

public class Shop : IShop
{
    public const int MinId = 1;
    public const double MinPrice = 0;
    public const int MinCount = 0;
    public const int AddressParametersCount = 3;

    private List<Product> _products;
    private List<int> _productCount;

    public Shop(int id, string title, string address)
    {
        if (!CheckId(id)) throw new NegativeIdException("Negative id!");
        Id = id;
        if (!CheckTitle(title)) throw new ShopTitleFormatException("Invalid shop title!");
        Title = title;
        if (!CheckAddress(address)) throw new ShopAddressFormatException("Invalid shop address!");
        Address = address;
        _products = new List<Product>();
        _productCount = new List<int>();
    }

    public int Id { get; }
    public string Title { get; private set; }
    public string Address { get; private set; }
    public ReadOnlyCollection<Product> Products => _products.AsReadOnly();

    public double Buy(Сustomer customer, params KeyValuePair<Product, int>[] products)
    {
        if (!CheckProductsPurchase(customer, products))
            throw new ProductConsignmentException("There is no such product consignment in stock or customer can not afford it!");
        double purchaseSum = 0;
        foreach (KeyValuePair<Product, int> product in products)
        {
            Product pr = GetProduct(product.Key.Name);
            purchaseSum += product.Value * pr.Price.GetValueOrDefault();
            _productCount[_products.IndexOf(pr)] -= product.Value;
        }

        customer.SubtractMoney(purchaseSum);
        return purchaseSum;
    }

    public void AddProducts(Dictionary<Product, int> products)
    {
        if (!CheckConsignmentOfProducts(products))
            throw new ProductConsignmentException("Incorrect products consignment!");
        foreach (KeyValuePair<Product, int> product in products)
        {
            Product? pr = FindProduct(product.Key.Name);
            if (pr is null)
            {
                _products.Add(new Product(product.Key.Name, product.Key.Price));
                _productCount.Add(product.Value);
            }
            else
            {
                if (product.Key.Price.HasValue) pr.ChangeProductPrice(product.Key.Price);
                _productCount[_products.IndexOf(pr)] += product.Value;
            }
        }
    }

    public void AddProduct(Product product, int number)
    {
        if (!CheckAddedProduct(product, number)) throw new ProductConsignmentException("Product is not specified or product number is negative!");
        Product? pr = FindProduct(product.Name);
        if (pr is null)
        {
            _products.Add(new Product(product.Name, product.Price));
            _productCount.Add(number);
        }
        else
        {
            _productCount[_products.IndexOf(pr)] += number;
        }
    }

    public void ChangeProductPrice(Product product, double price)
    {
        Product? prod = _products.SingleOrDefault(pr => pr.Name == product.Name);
        if (prod is null) throw new ProductNotInShopException("There is no such product!");
        prod.ChangeProductPrice(price);
    }

    public Product? FindProduct(string productName)
    {
        return _products.SingleOrDefault(product => product.Name == productName);
    }

    public Product GetProduct(string productName)
    {
        Product? product = _products.SingleOrDefault(product => product.Name == productName);
        if (product is null) throw new ProductNotInShopException($"There is no product {productName} in the shop!");
        return product;
    }

    public bool ContainsProduct(Product product)
    {
        Product? pr = _products.SingleOrDefault(prod => prod.Name == product.Name);
        return pr is not null;
    }

    public int GetProductCount(string product)
    {
        Product? productToFind = FindProduct(product);
        return productToFind is null ? 0 : _productCount[_products.IndexOf(productToFind)];
    }

    public void ChangeTitle(string title)
    {
        if (!CheckTitle(title)) throw new ShopTitleFormatException("Invalid shop title!");
        Title = title;
    }

    public void ChangeAddress(string address)
    {
        if (!CheckAddress(address)) throw new ShopAddressFormatException("Invalid shop address!");
        Address = address;
    }

    public double GetCostOfProductConsignment(params KeyValuePair<Product, int>[] products)
    {
        double cost = 0;
        foreach (KeyValuePair<Product, int> product in products)
        {
            Product? pr = FindProduct(product.Key.Name);
            if (pr is null) return -1;
            if (_productCount[_products.IndexOf(pr)] < product.Value) return -1;
            if (!pr.Price.HasValue) return -1;
            cost += product.Value * pr.Price.Value;
        }

        return cost;
    }

    private bool CheckId(int id) => id >= MinId;

    private bool CheckTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return false;
        string[] fullTitle = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullTitle.All(str => str.All(char.IsLetter));
    }

    private bool CheckAddress(string address)
    {
        if (string.IsNullOrEmpty(address)) return false;
        string[] fullAddress = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fullAddress.Length != AddressParametersCount) return false;
        if (!CheckAddressParameter(fullAddress[0], ShopAddressParameters.City)) return false;
        if (!CheckAddressParameter(fullAddress[1], ShopAddressParameters.Street)) return false;
        if (!CheckAddressParameter(fullAddress[2], ShopAddressParameters.HouseNumber)) return false;
        return true;
    }

    private bool CheckProductsPurchase(Сustomer customer, params KeyValuePair<Product, int>[] products)
    {
        double purchaseSum = 0;
        foreach (KeyValuePair<Product, int> product in products)
        {
            Product? pr = FindProduct(product.Key.Name);
            if (pr is null) return false;
            if (_productCount[_products.IndexOf(pr)] < product.Value) return false;
            if (!pr.Price.HasValue) return false;
            purchaseSum += product.Value * pr.Price.Value;
        }

        return purchaseSum <= customer.Money;
    }

    private bool CheckConsignmentOfProducts(Dictionary<Product, int> products)
    {
        return !products.Any(product => product.Key.Price < MinPrice || product.Value < MinCount);
    }

    private bool CheckAddedProduct(Product product, int number) => !(product is null || number < 0);

    private bool CheckAddressParameter(string parameter, ShopAddressParameters number)
    {
        switch (number)
        {
            case ShopAddressParameters.City:
            case ShopAddressParameters.Street:
                return parameter.All(symbol => char.IsLetter(symbol) || symbol.Equals('-'));
            case ShopAddressParameters.HouseNumber:
                return parameter.All(char.IsNumber);
            default:
                return false;
        }
    }
}