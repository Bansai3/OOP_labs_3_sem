namespace IsuExtraException;

[Serializable]
public class LectorNameException : Exception
{
    public LectorNameException() {}
    public LectorNameException(string message) : base(message) {}
    public LectorNameException(string message, Exception inner) : base(message, inner){}
}