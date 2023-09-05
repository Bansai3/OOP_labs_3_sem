namespace Backups.Extra;

public interface ISerialization
{
    void Serialize(BackupTaskExtra backupTaskExtra);
    BackupTaskSerializableInformation Deserialize();
}