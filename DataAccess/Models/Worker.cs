namespace DataAccess.Models;

public class Worker : Employee
{
    public Worker(string name, Guid id, Supervisor supervisor) : base(name, id)
    {
        Supervisor = supervisor;
        MessageSources = new List<MessageSource>();
    }
    
    protected Worker() {}
    public virtual Supervisor Supervisor { get; set; }
    public virtual ICollection<MessageSource> MessageSources { get; set; }
}