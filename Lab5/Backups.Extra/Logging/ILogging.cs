namespace Backups.Extra;

public interface ILogging
{
    void Log(string message);
    void TimeCodeLog(string message);
}