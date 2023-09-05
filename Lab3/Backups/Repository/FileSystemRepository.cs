using System.Collections.ObjectModel;

namespace Backups;

public class FileSystemRepository : Repository
{
    private List<BackupObject> _objects;
    private List<Storage> _storages;

    public FileSystemRepository(string path)
    {
        if (!CheckPath(path)) throw new Exception("Invalid path");
        Path = path;
        _objects = new List<BackupObject>();
        _storages = new List<Storage>();
    }

    public string Path { get; private set; }
    public ReadOnlyCollection<Storage> Storages => new (_storages);
    public ReadOnlyCollection<BackupObject> BackupObjects => new (_objects);

    public override void AddBackupObject(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("Backup object can not be null!");
        var fileInf = new FileInfo(backupObject.Path);
        fileInf.CopyTo(Path);
        _objects.Add(backupObject);
        BackupObjectsCount++;
    }

    public override void AddStorage(Storage storage)
    {
        if (!CheckStorage(storage)) throw new Exception("Storage can not be null!");
        storage.CreateCompressedFile(Path);
        _storages.Add(storage);
        StoragesCount += storage.StorageCount;
    }

    private bool CheckPath(string path) => Directory.Exists(path);
}