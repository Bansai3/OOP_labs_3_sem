namespace IsuExtraException;

[Serializable]
public class StudentNullReferenceException : Exception
{
    public StudentNullReferenceException() {}
    public StudentNullReferenceException(string message) : base(message) {}
    public StudentNullReferenceException(string message, Exception inner) : base(message, inner){}
}