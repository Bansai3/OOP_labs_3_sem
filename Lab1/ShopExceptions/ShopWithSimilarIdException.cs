namespace ShopExceptions;

[Serializable]
public class ShopWithSimilarIdException : Exception
{
    public ShopWithSimilarIdException() {}
    public ShopWithSimilarIdException(string message) : base(message) {}
    public ShopWithSimilarIdException(string message, Exception inner) : base(message, inner){}
}