namespace IsuExtraException;

[Serializable]
public class PairNullReferenceException : Exception
{
    public PairNullReferenceException() {}
    public PairNullReferenceException(string message) : base(message) {}
    public PairNullReferenceException(string message, Exception inner) : base(message, inner){}
}