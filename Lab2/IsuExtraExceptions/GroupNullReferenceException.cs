namespace IsuExtraException;

[Serializable]
public class GroupNullReferenceException : Exception
{
    public GroupNullReferenceException() {}
    public GroupNullReferenceException(string message) : base(message) {}
    public GroupNullReferenceException(string message, Exception inner) : base(message, inner){}
}