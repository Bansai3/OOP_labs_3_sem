namespace IsuExtraException;

[Serializable]
public class SimilarOgnpException : Exception
{
    public SimilarOgnpException() {}
    public SimilarOgnpException(string message) : base(message) {}
    public SimilarOgnpException(string message, Exception inner) : base(message, inner){}
}