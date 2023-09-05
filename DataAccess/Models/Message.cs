namespace DataAccess.Models;

public enum State
{
    New,
    Received,
    Processed,
}
public class Message
{
    public Message(Guid id, string messageText, MessageSource messageSource, State state)
    {
        MessageText = messageText;
        MessageSource = messageSource;
        State = state;
        Id = id;
        Date = DateTime.Now;
    }
    
    protected Message() {}
    public string MessageText { get; set; }
    public virtual MessageSource MessageSource { get; set; }
    public State State { get; set; }
    public Guid Id { get; set; }
    public virtual DateTime Date { get; set; }
}