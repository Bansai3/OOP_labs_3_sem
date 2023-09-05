namespace Backups;

public abstract class Repository
{
    public int BackupObjectsCount { get; protected set; }
    public int StoragesCount { get; protected set; }
    public abstract void AddBackupObject(BackupObject backupObject);
    public abstract void AddStorage(Storage storage);

    protected bool CheckBackupObject(BackupObject? backupObject) => backupObject is not null;

    protected bool CheckStorage(Storage? storage) => storage is not null;
}