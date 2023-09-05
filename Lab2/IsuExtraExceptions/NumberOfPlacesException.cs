namespace IsuExtraException;

[Serializable]
public class NumberOfPlacesException : Exception
{
    public NumberOfPlacesException() {}
    public NumberOfPlacesException(string message) : base(message) {}
    public NumberOfPlacesException(string message, Exception inner) : base(message, inner){}
}