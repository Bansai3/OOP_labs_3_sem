using System.Collections.ObjectModel;

namespace Backups;

public class SplitStorage : IStorageAlgorithm
{
    public Storage GetComposedObject(RestorePoint restorePoint)
    {
        var multipleStorage = new MultipleStorage();
        ReadOnlyCollection<BackupObject> backupObjects = restorePoint.Objects;
        foreach (BackupObject backupObject in backupObjects)
        {
            multipleStorage.AddStorage(new SingleBackupObjectStorage(backupObject));
        }

        return multipleStorage;
    }
}