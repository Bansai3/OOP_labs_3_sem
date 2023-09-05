namespace Application.Exceptions;
[Serializable]
public class EmployeeTypeException : Exception
{
    public EmployeeTypeException() { }

    public EmployeeTypeException(string message)
        : base(message) { }

    public EmployeeTypeException(string message, Exception inner)
        : base(message, inner) { }
}