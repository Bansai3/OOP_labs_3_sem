namespace CustomException;

[Serializable]
public class AddStudentWhoIsInAnotherGroupException : Exception
{
    public AddStudentWhoIsInAnotherGroupException() {}
    public AddStudentWhoIsInAnotherGroupException(string message) : base(message) {}
    public AddStudentWhoIsInAnotherGroupException(string message, Exception inner) : base(message, inner){}
}