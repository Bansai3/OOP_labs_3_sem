namespace IsuExtraException;

[Serializable]
public class FlowNullReferenceException : Exception
{
    public FlowNullReferenceException() {}
    public FlowNullReferenceException(string message) : base(message) {}
    public FlowNullReferenceException(string message, Exception inner) : base(message, inner){}
}