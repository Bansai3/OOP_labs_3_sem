namespace ShopExceptions;

[Serializable]
public class CustomerNameFormatException : Exception
{
    public CustomerNameFormatException(){}
    public CustomerNameFormatException(string message): base(message) {}
    public CustomerNameFormatException(string message, Exception inner) : base(message, inner) {}
}