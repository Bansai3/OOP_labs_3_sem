namespace DataAccess.Models;

public class Messenger : MessageSource
{
    public Messenger(Guid id):base(id, "Messenger")
    {}
    
    protected Messenger(){}
}