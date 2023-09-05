namespace IsuExtraException;

[Serializable]
public class OgnpCountException : Exception
{
    public OgnpCountException() {}
    public OgnpCountException(string message) : base(message) {}
    public OgnpCountException(string message, Exception inner) : base(message, inner){}
}