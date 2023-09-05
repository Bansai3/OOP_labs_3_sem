namespace CustomException;

[Serializable]
public class TooManyStudentsInGroupException : Exception
{
    public TooManyStudentsInGroupException() {}
    public TooManyStudentsInGroupException(string message) : base(message) {}
    public TooManyStudentsInGroupException(string message, Exception inner) : base(message, inner){}
}