namespace Backups.Extra;

public class ConsoleLogging : ILogging
{
    public void Log(string message)
    {
        if (CheckMessage(message) == false)
            throw new ArgumentException("Invalid message!");
        Console.WriteLine(message);
    }

    public void TimeCodeLog(string message)
    {
        DateTime time = DateTime.Now;
        Console.WriteLine($"{time.Year}:{time.Month}:{time.Day}:{time.Hour}:{time.Minute}:{time.Second}\n{message}");
    }

    private bool CheckMessage(string? message) => message != null;
}