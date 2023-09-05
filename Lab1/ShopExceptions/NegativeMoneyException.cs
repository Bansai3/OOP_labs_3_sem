namespace ShopExceptions;

[Serializable]
public class NegativeMoneyException : Exception
{
    public NegativeMoneyException(){}
    public NegativeMoneyException(string message) : base(message){}
    public NegativeMoneyException(string message, Exception inner) : base(message, inner){}
}