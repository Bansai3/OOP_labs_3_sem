namespace Backups.Extra;

public class FileLogging : ILogging
{
    private string _filePath;

    public FileLogging(string filePath)
    {
        if (CHeckFilePath(filePath) == false)
            throw new ArgumentException("File path does not exist!");
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, message + "\n");
    }

    public void TimeCodeLog(string message)
    {
        DateTime time = DateTime.Now;
        File.AppendAllText(_filePath, $"{time.Year}:{time.Month}:{time.Day}:{time.Hour}:{time.Minute}:{time.Second}\n{message}\n");
    }

    private bool CHeckFilePath(string filePath) => File.Exists(filePath);
}