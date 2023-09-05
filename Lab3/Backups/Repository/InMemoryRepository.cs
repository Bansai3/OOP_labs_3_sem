using System.Collections.ObjectModel;

namespace Backups;

public class InMemoryRepository : Repository
{
    private List<BackupObject> _objects;
    private List<Storage> _storages;

    public InMemoryRepository()
    {
        _objects = new List<BackupObject>();
        _storages = new List<Storage>();
    }

    public ReadOnlyCollection<Storage> Storages => new (_storages);
    public ReadOnlyCollection<BackupObject> BackupObjects => new (_objects);

    public override void AddBackupObject(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("Backup object can not be null!");
        _objects.Add(backupObject);
        BackupObjectsCount++;
    }

    public override void AddStorage(Storage storage)
    {
        if (!CheckStorage(storage)) throw new Exception("Storage can not be null!");
        _storages.Add(storage);
        StoragesCount += storage.StorageCount;
    }
}