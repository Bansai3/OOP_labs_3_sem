namespace DataAccess.Models;

public class Account
{
    public Account(string login, string password, Guid id, Employee employee)
    {
        Login = login;
        Password = password;
        Id = id;
        Employee = employee;
    }
    
    protected Account() {}
    public string Login { get; set; }
    public string Password { get; set; }
    public Guid Id { get; set; }
    public virtual Employee Employee { get; set; }
}