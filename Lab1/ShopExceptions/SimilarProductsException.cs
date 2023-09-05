namespace ShopExceptions;

[Serializable]
public class SimilarProductsException: Exception
{
    public SimilarProductsException(){}
    public SimilarProductsException(string message): base(message) {}
    public SimilarProductsException(string message, Exception inner) : base(message, inner) {}
}