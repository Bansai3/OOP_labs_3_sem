namespace CustomException;

[Serializable]
public class CourseNumberException : Exception
{
    public CourseNumberException() {}
    public CourseNumberException(string message) : base(message) {}
    public CourseNumberException(string message, Exception inner) : base(message, inner){}
}