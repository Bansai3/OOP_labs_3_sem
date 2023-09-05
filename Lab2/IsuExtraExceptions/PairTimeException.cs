namespace IsuExtraException;

[Serializable]
public class PairTimeException : Exception
{
    public PairTimeException() {}
    public PairTimeException(string message) : base(message) {}
    public PairTimeException(string message, Exception inner) : base(message, inner){}
}