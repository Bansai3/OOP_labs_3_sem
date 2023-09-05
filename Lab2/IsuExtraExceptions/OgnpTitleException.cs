namespace IsuExtraException;

[Serializable]
public class OgnpTitleException : Exception
{
    public OgnpTitleException() {}
    public OgnpTitleException(string message) : base(message) {}
    public OgnpTitleException(string message, Exception inner) : base(message, inner){}
}