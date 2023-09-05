namespace IsuExtraException;

[Serializable]
public class OgnpNullReferenceException : Exception
{
    public OgnpNullReferenceException() {}
    public OgnpNullReferenceException(string message) : base(message) {}
    public OgnpNullReferenceException(string message, Exception inner) : base(message, inner){}
}