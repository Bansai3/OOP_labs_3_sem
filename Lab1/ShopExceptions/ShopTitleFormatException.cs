namespace ShopExceptions;

[Serializable]
public class ShopTitleFormatException : Exception
{
    public ShopTitleFormatException(){}
    public ShopTitleFormatException(string message): base(message) {}
    public ShopTitleFormatException(string message, Exception inner) : base(message, inner) {}
}