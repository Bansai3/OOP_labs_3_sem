namespace CustomException;

[Serializable]
public class GroupNameFormatException : Exception
{
    public GroupNameFormatException() {}
    public GroupNameFormatException(string message) : base(message) {}
    public GroupNameFormatException(string message, Exception inner) : base(message, inner){}
}