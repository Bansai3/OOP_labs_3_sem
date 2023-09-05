namespace DataAccess.Models;

public class Email : MessageSource
{
    public Email(Guid id) : base(id, "Email")
    { }
    
    protected Email(){}
}