using System.Collections.ObjectModel;

namespace Backups;

public class RestorePoint
{
    private List<BackupObject> _objects;
    private DateTime _timeOfCreation;

    public RestorePoint()
    {
        _objects = new List<BackupObject>();
        _timeOfCreation = DateTime.Now;
    }

    public RestorePoint(List<BackupObject> objects)
    {
        if (!CheckObjects(objects)) throw new Exception("Objects can not be null!");
        _objects = new List<BackupObject>(objects);
        _timeOfCreation = DateTime.Now;
    }

    public ReadOnlyCollection<BackupObject> Objects => new (_objects);

    public void AddBackupObject(BackupObject backupObject)
    {
        if (CheckBackupObject(backupObject) == false)
            throw new ArgumentException("Invalid backup object!");
        _objects.Add(backupObject);
    }

    public void ChangeTimeOfCreation(DateTime newTime)
    {
        _timeOfCreation = newTime;
    }

    public DateTime GetTimeOfCreation() => _timeOfCreation;
    private bool CheckObjects(List<BackupObject>? objects) => objects is not null;

    private bool CheckBackupObject(BackupObject? backupObject) => backupObject != null;
}