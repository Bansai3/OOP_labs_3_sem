namespace Application.Exceptions;
[Serializable]
public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException() { }

    public EmployeeNotFoundException(string message)
        : base(message) { }

    public EmployeeNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}