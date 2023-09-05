namespace DataAccess.Models;

public class Report
{
    public Report(Guid id, string text, Supervisor supervisor, int timePeriodInDays, DateTime creationTime)
    {
        Text = text;
        Supervisor = supervisor;
        TimePeriodInDays = timePeriodInDays;
        CreationTime = creationTime;
    }
    
    protected Report() {}
    public string Text { get; set; }
    public virtual Supervisor Supervisor { get; set; }
    public Guid Id { get; set; }
    public virtual int TimePeriodInDays { get; set; }
    public virtual DateTime CreationTime { get; set; }
}