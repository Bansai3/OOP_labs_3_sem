namespace ShopExceptions;

[Serializable]
public class ProductNameFormatException : Exception
{
    public ProductNameFormatException() { }
    public ProductNameFormatException(string message) : base(message) { }
    public ProductNameFormatException(string message, Exception inner) : base(message, inner) { }
}