namespace IsuExtraException;

[Serializable]
public class OgnpGroupNullReferenceException : Exception
{
    public OgnpGroupNullReferenceException() {}
    public OgnpGroupNullReferenceException(string message) : base(message) {}
    public OgnpGroupNullReferenceException(string message, Exception inner) : base(message, inner){}
}