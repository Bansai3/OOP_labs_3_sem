namespace DataAccess.Models;

public class Phone : MessageSource
{
    public Phone(Guid id) : base(id, "Phone")
    {}
    
    protected Phone() {}
}