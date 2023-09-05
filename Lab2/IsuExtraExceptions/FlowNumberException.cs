namespace IsuExtraException;

[Serializable]
public class FlowNumberException : Exception
{
    public FlowNumberException() {}
    public FlowNumberException(string message) : base(message) {}
    public FlowNumberException(string message, Exception inner) : base(message, inner){}
}