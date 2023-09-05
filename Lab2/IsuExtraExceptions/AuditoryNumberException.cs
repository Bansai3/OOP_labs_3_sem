namespace IsuExtraException;

[Serializable]
public class AuditoryNumberException : Exception
{
    public AuditoryNumberException() {}
    public AuditoryNumberException(string message) : base(message) {}
    public AuditoryNumberException(string message, Exception inner) : base(message, inner){}
}