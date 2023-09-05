namespace IsuExtraException;

[Serializable]
public class PairIntersectionException : Exception
{
    public PairIntersectionException() {}
    public PairIntersectionException(string message) : base(message) {}
    public PairIntersectionException(string message, Exception inner) : base(message, inner){}
}