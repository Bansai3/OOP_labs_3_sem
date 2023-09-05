namespace DataAccess.Models;

public abstract class Employee
{
    public Employee(string name, Guid id)
    {
        Name = name;
        Id = id;
        HaveAccess = false;
    }
    
    protected Employee() {}
    public string Name { get; set; }
    public Guid Id { get; set; }
    public bool HaveAccess { get; set; }
}