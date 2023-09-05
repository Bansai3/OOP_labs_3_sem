namespace IsuExtraException;

[Serializable]
public class LectorNullReferenceException : Exception
{
    public LectorNullReferenceException() {}
    public LectorNullReferenceException(string message) : base(message) {}
    public LectorNullReferenceException(string message, Exception inner) : base(message, inner){}
}