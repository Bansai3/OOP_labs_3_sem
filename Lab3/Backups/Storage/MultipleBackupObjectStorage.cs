using System.Collections.ObjectModel;
using System.IO.Compression;

namespace Backups;

public class MultipleBackupObjectStorage : Storage
{
    private List<BackupObject> _objects;

    public MultipleBackupObjectStorage()
    {
        _objects = new List<BackupObject>();
        StorageCount = 1;
    }

    public MultipleBackupObjectStorage(List<BackupObject> objects)
    {
        if (!CheckObjects(_objects)) throw new Exception("Objects can not be null!");
        _objects = new List<BackupObject>(objects);
    }

    public ReadOnlyCollection<BackupObject> Objects => new (_objects);

    public void AddBackupObject(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("Backup object can not be null!");
        _objects.Add(backupObject);
    }

    public override void CreateCompressedFile(string path)
    {
        string zipFilePath = path + "/res.zip";
        foreach (BackupObject backupObject in _objects)
        {
            backupObject.AddZipBackupObject(zipFilePath);
        }
    }

    private bool CheckObjects(List<BackupObject>? objects) => objects is not null;

    private bool CheckBackupObject(BackupObject? backupObject) => backupObject is not null;
}