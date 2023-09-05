namespace Application.Exceptions;
[Serializable]
public class SameUsersException : Exception
{
    public SameUsersException() { }

    public SameUsersException(string message)
        : base(message) { }

    public SameUsersException(string message, Exception inner)
        : base(message, inner) { }
}