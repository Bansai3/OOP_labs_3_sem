namespace ShopExceptions;

[Serializable]
public class ShopNullReferenceException : Exception
{
    public ShopNullReferenceException(){}
    public ShopNullReferenceException(string message): base(message) {}
    public ShopNullReferenceException(string message, Exception inner) : base(message, inner) {}
}