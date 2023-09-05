namespace Application.Exceptions;
[Serializable]
public class MessageSourceNotFoundException : Exception
{
    public MessageSourceNotFoundException() { }

    public MessageSourceNotFoundException(string message)
        : base(message) { }

    public MessageSourceNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}