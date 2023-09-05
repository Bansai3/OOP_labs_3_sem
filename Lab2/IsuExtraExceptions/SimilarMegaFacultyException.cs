namespace IsuExtraException;

[Serializable]
public class SimilarMegaFacultyException : Exception
{
    public SimilarMegaFacultyException() {}
    public SimilarMegaFacultyException(string message) : base(message) {}
    public SimilarMegaFacultyException(string message, Exception inner) : base(message, inner){}
}