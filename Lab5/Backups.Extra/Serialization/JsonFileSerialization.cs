using System.Text;
using System.Text.Json;

namespace Backups.Extra;

public class JsonFileSerialization : ISerialization
{
    public JsonFileSerialization(string filePath)
    {
        if (CheckFilePath(filePath) == false)
            throw new ArgumentException("Invalid file path!");
        FilePath = filePath;
    }

    public string FilePath { get; private set; }

    public void Serialize(BackupTaskExtra backupTaskExtra)
    {
        var format = new JsonSerializerOptions { WriteIndented = true };
        string jsonInfo = JsonSerializer.Serialize(backupTaskExtra, format);
        File.WriteAllText(FilePath, jsonInfo);
    }

    public BackupTaskSerializableInformation Deserialize()
    {
        string jsonFileInfo = File.ReadAllText(FilePath);
        var format = new JsonSerializerOptions { WriteIndented = true };
        BackupTaskSerializableInformation? backupTaskExtra = JsonSerializer.Deserialize<BackupTaskSerializableInformation>(jsonFileInfo, format);
        if (backupTaskExtra == null)
            throw new InvalidOperationException("Deserialization failed!");
        return backupTaskExtra;
    }

    public void ChangeFilePath(string newFilePath)
    {
        if (CheckFilePath(newFilePath) == false)
            throw new ArgumentException("Invalid file path!");
        FilePath = newFilePath;
    }

    private bool CheckFilePath(string filePath) => string.IsNullOrEmpty(filePath) == false;
}