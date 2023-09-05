using System.Text.Json.Serialization;

namespace Backups.Extra;

public class BackupTaskSerializableInformation
{
    public string? Title { get; }
    public List<BackupObjectSerializableInformation>? Objects { get; set; }

    public List<RestorePointSerializableInformation>? RestorePoints { get; set; }
}