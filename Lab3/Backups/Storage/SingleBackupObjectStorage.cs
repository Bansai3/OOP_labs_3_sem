namespace Backups;

public class SingleBackupObjectStorage : Storage
{
    private BackupObject _backupObject;

    public SingleBackupObjectStorage(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("Backup object can not be null!");
        _backupObject = backupObject;
        StorageCount = 1;
    }

    public override void CreateCompressedFile(string path)
    {
        string zipFileTitle = _backupObject.Title + ".zip";
        _backupObject.CreateZipBackupObject(path, zipFileTitle);
    }

    public string GetBackupObjectTitle()
    {
        return _backupObject.Title;
    }

    public string GetBackupObjectPath()
    {
        return _backupObject.Path;
    }

    private bool CheckBackupObject(BackupObject? backupObject) => backupObject is not null;
}