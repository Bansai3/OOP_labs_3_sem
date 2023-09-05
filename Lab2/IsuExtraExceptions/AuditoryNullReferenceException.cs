namespace IsuExtraException;

[Serializable]
public class AuditoryNullReferenceException : Exception
{
    public AuditoryNullReferenceException() {}
    public AuditoryNullReferenceException(string message) : base(message) {}
    public AuditoryNullReferenceException(string message, Exception inner) : base(message, inner){}
}