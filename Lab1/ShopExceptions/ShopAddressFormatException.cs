namespace ShopExceptions;

[Serializable]
public class ShopAddressFormatException : Exception
{
    public ShopAddressFormatException(){}
    public ShopAddressFormatException(string message): base(message) {}
    public ShopAddressFormatException(string message, Exception inner) : base(message, inner) {}
}