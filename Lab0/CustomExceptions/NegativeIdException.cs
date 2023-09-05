namespace CustomException;

[Serializable]
public class NegativeIdException : Exception
{
    public NegativeIdException() { }
    public NegativeIdException(string message) : base(message) { }
    public NegativeIdException(string message, Exception inner) : base(message, inner) { }
}
