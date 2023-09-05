namespace ShopExceptions;

[Serializable]
public class ProductNotInShopException : Exception
{
    public ProductNotInShopException(){}
    public ProductNotInShopException(string message): base(message) {}
    public ProductNotInShopException(string message, Exception inner) : base(message, inner) {}
}