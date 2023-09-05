using System.Collections.ObjectModel;

namespace Backups;

public class BackupTask
{
    private Backup _backup;
    private List<BackupObject> _objects;
    private Repository _repository;
    private IStorageAlgorithm _algorithm;

    public BackupTask(string title, Repository repository, IStorageAlgorithm storageAlgorithm)
    {
        if (!CheckTitle(title)) throw new Exception("Invalid title!");
        Title = title;
        if (!CheckRepository(repository)) throw new Exception("Repository can not be null!");
        _repository = repository;
        if (!CheckAlgorithm(storageAlgorithm)) throw new Exception("Algorithm can not be null!");
        _algorithm = storageAlgorithm;
        _backup = new Backup();
        _objects = new List<BackupObject>();
    }

    public ReadOnlyCollection<BackupObject> Objects => new (_objects);
    public string Title { get; private set; }

    public RestorePoint Execute()
    {
        var newRestorePoint = new RestorePoint(_objects);
        _backup.AddRestorePoint(newRestorePoint);
        Storage storage = _algorithm.GetComposedObject(newRestorePoint);
        _repository.AddStorage(storage);
        return newRestorePoint;
    }

    public void AddObject(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("BackupObject can not be equal null!");
        _objects.Add(backupObject);
    }

    public void DeleteObject(BackupObject backupObject)
    {
        if (!CheckBackupObject(backupObject)) throw new Exception("BackupObject can not be equal null!");
        _objects.Remove(backupObject);
    }

    public int GetRestorePointsAmount()
    {
        return _backup.GetRestorePoints().Count;
    }

    public void ChangeTitle(string newTitle)
    {
        if (!CheckTitle(newTitle))
            throw new Exception("Invalid title!");
        Title = newTitle;
    }

    private bool CheckTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return false;
        string[] words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.All(word => word.All(symbol => char.IsLetter(symbol) || char.IsDigit(symbol)));
    }

    private bool CheckRepository(Repository? repository) => repository is not null;

    private bool CheckAlgorithm(IStorageAlgorithm? algorithm) => algorithm is not null;

    private bool CheckBackupObject(BackupObject? backupObject) => backupObject is not null;
}