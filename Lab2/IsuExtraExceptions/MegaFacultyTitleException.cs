namespace IsuExtraException;

[Serializable]
public class MegaFacultyTitleException : Exception
{
    public MegaFacultyTitleException() {}
    public MegaFacultyTitleException(string message) : base(message) {}
    public MegaFacultyTitleException(string message, Exception inner) : base(message, inner){}
}