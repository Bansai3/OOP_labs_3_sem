namespace DataAccess.Models;

public class Supervisor : Employee
{
    public Supervisor(string name, Guid id) : base(name, id)
    {
        Subordinates = new List<Worker>();
    }
    
    protected Supervisor() {}
    public virtual ICollection<Worker> Subordinates { get; set; }
}