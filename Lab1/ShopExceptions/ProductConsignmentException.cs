namespace ShopExceptions;

[Serializable]
public class ProductConsignmentException : Exception
{
    public ProductConsignmentException(){}
    public ProductConsignmentException(string message): base(message) {}
    public ProductConsignmentException(string message, Exception inner) : base(message, inner) {}
}