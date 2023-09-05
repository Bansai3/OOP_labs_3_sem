namespace IsuExtraException;

[Serializable]
public class MegaFacultyNullReferenceException : Exception
{
    public MegaFacultyNullReferenceException() {}
    public MegaFacultyNullReferenceException(string message) : base(message) {}
    public MegaFacultyNullReferenceException(string message, Exception inner) : base(message, inner){}
}