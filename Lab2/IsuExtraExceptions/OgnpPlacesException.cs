namespace IsuExtraException;

[Serializable]
public class OgnpPlacesException : Exception
{
    public OgnpPlacesException() {}
    public OgnpPlacesException(string message) : base(message) {}
    public OgnpPlacesException(string message, Exception inner) : base(message, inner){}
}