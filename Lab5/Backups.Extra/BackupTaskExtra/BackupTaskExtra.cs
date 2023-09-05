using System.Collections.ObjectModel;

namespace Backups.Extra;

public class BackupTaskExtra : BackupTask, IRecovery
{
    private Backup _backup;
    private IRestorePointLimit _limit;
    private IRestorePointCleanMethod _cleanMethod;
    private LoggingProxy _logging;
    private IStorageAlgorithm _algorithm;
    private Repository _repository;
    public BackupTaskExtra(
        string title,
        Repository repository,
        IStorageAlgorithm storageAlgorithm,
        IRestorePointLimit limit,
        IRestorePointCleanMethod cleanMethod,
        ILogging logging,
        bool timeCodePrefix = false)
        : base(title, repository, storageAlgorithm)
    {
        if (CheckLimit(limit) == false)
            throw new ArgumentException("Invalid limit!");
        if (CheckRestorePointCleanMethod(cleanMethod) == false)
            throw new ArgumentException("Invalid Clean method!");
        if (CheckLogging(logging) == false)
            throw new ArgumentException("Invalid logging!");
        _limit = limit;
        _cleanMethod = cleanMethod;
        _logging = new LoggingProxy(logging, timeCodePrefix);
        _algorithm = storageAlgorithm;
        _backup = new Backup();
        _repository = repository;
    }

    public ReadOnlyCollection<RestorePoint> RestorePoints => new (_backup.GetRestorePoints());

    public new RestorePoint Execute()
    {
        var newRestorePoint = new RestorePoint(Objects.ToList());
        _logging.Log("-------------------------");
        _logging.Log("New restore point created");
        _logging.Log(newRestorePoint.ShowRestorePointInfo());
        _backup.AddRestorePoint(newRestorePoint);
        _logging.Log("New restore point added");
        _logging.Log(_backup.ShowBackupInfo());
        Storage storage = _algorithm.GetComposedObject(newRestorePoint);
        _logging.Log("Storage created");
        _logging.Log(storage.ShowStorageInfo());
        _repository.AddStorage(storage);
        _logging.Log("Storage added to repository");
        _logging.Log("---------------------------");
        return newRestorePoint;
    }

    public void CleanRestorePoints()
    {
        List<RestorePoint> restorePoints = _backup.GetRestorePoints();
        List<RestorePoint> restorePointsToDelete = GetPointsToDelete();
        _cleanMethod.Clean(restorePoints, restorePointsToDelete);
    }

    public void RecoverToOriginalLocation(RestorePoint restorePoint)
    {
        if (CheckRestorePoint(restorePoint) == false)
            throw new ArgumentException("Invalid restore point!");
        ReadOnlyCollection<BackupObject> backupObjects = restorePoint.Objects;
        foreach (BackupObject backupObject in backupObjects)
        {
            var fileInfo = new FileInfo(backupObject.Path);
            fileInfo.CopyTo(backupObject.Path);
        }
    }

    public new int GetRestorePointsAmount()
    {
        return _backup.GetRestorePoints().Count;
    }

    public void RecoverToDifferentLocation(RestorePoint restorePoint, Repository repository)
    {
        if (CheckRestorePoint(restorePoint) == false)
            throw new ArgumentException("Invalid restore point!");
        if (CheckRepository(repository) == false)
            throw new ArgumentException("Invalid repository!");
        ReadOnlyCollection<BackupObject> backupObjects = restorePoint.Objects;
        foreach (BackupObject backupObject in backupObjects)
        {
            repository.AddBackupObject(backupObject);
        }
    }

    private List<RestorePoint> GetPointsToDelete()
    {
        List<RestorePoint> restorePoints = _backup.GetRestorePoints();
        return _limit.GetRestorePointsToDelete(restorePoints);
    }

    private bool CheckLimit(IRestorePointLimit? limit) => limit != null;

    private bool CheckRestorePointCleanMethod(IRestorePointCleanMethod? cleanMethod) => cleanMethod != null;

    private bool CheckLogging(ILogging? logging) => logging != null;

    private bool CheckRestorePoint(RestorePoint? restorePoint) => restorePoint != null;
    private bool CheckRepository(Repository? repository) => repository != null;
}