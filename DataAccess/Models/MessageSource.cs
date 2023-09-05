namespace DataAccess.Models;

public class MessageSource
{
    public MessageSource(Guid id, string title)
    {
        Id = id;
        Title = title;
        Messages = new List<Message>();
        Workers = new List<Worker>();
    }
    
    protected MessageSource() {}
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<Worker> Workers { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
}