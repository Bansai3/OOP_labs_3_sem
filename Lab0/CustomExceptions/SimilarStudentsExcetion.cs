namespace CustomException;

[Serializable]
public class SimilarStudentsException : Exception
{
    public SimilarStudentsException() {}
    public SimilarStudentsException(string message) : base(message) {}
    public SimilarStudentsException(string message, Exception inner) : base(message, inner){}
}