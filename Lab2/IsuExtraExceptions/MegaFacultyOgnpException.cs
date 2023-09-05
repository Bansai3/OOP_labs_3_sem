namespace IsuExtraException;

[Serializable]
public class MegaFacultyOgnpException : Exception
{
    public MegaFacultyOgnpException() {}
    public MegaFacultyOgnpException(string message) : base(message) {}
    public MegaFacultyOgnpException(string message, Exception inner) : base(message, inner){}
}