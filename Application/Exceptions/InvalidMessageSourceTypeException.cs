namespace Application.Exceptions;
[Serializable]
public class InvalidMessageSourceTypeException : Exception
{
    public InvalidMessageSourceTypeException() { }

    public InvalidMessageSourceTypeException(string message)
        : base(message) { }

    public InvalidMessageSourceTypeException(string message, Exception inner)
        : base(message, inner) { }
}