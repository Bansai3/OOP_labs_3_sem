namespace Backups.Extra;

public class LoggingProxy
{
    private ILogging _logging;
    private bool _timeCodePrefix;

    public LoggingProxy(ILogging logging, bool timeCodePrefix)
    {
        if (CheckLogging(logging) == false)
            throw new ArgumentException("Invalid logging object!");
        _logging = logging;
        _timeCodePrefix = timeCodePrefix;
    }

    public void Log(string message)
    {
        if (_timeCodePrefix == true)
            _logging.TimeCodeLog(message);
        else _logging.Log(message);
    }

    public void ChangeTimeCodePrefix(bool timeCodePrefix)
    {
        _timeCodePrefix = timeCodePrefix;
    }

    private bool CheckLogging(ILogging? logging) => logging != null;
}