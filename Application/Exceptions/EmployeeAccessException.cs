namespace Application.Exceptions;
[Serializable]
public class EmployeeAccessException : Exception
{
    public EmployeeAccessException() { }

    public EmployeeAccessException(string message)
        : base(message) { }

    public EmployeeAccessException(string message, Exception inner)
        : base(message, inner) { }
}