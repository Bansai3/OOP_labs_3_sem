using System.Collections.ObjectModel;

namespace Backups;

public class SingleStorage : IStorageAlgorithm
{
    public Storage GetComposedObject(RestorePoint restorePoint)
    {
        var multipleBackupObjectStorage = new MultipleBackupObjectStorage();
        ReadOnlyCollection<BackupObject> backupObjects = restorePoint.Objects;
        foreach (BackupObject backupObject in backupObjects)
        {
            multipleBackupObjectStorage.AddBackupObject(backupObject);
        }

        return multipleBackupObjectStorage;
    }
}