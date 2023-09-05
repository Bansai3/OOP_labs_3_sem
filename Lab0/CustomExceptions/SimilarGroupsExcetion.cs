namespace CustomException;

[Serializable]
public class SimilarGroupsException : Exception
{
    public SimilarGroupsException() { }
    public SimilarGroupsException(string message) : base(message) { }
    public SimilarGroupsException(string message, Exception inner) : base(message, inner) { }
}