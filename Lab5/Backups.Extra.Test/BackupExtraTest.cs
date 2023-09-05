using Xunit;

namespace Backups.Extra.Test;

public class BackupExtraTest
{
    private Repository _repository = new InMemoryRepository();
    private IStorageAlgorithm _algorithm = new SplitStorage();
    private BackupObject _backupObject1 = new FileBackupObject("/root/dotnet/Files/A.txt");
    private BackupObject _backupObject2 = new FileBackupObject("/root/dotnet/Files/B.txt");
    private BackupObject _backupObject3 = new FileBackupObject("/root/dotnet/Files/C.txt");
    private IRestorePointLimit _countLimit = new RestorePointCountLimit(2);
    private IRestorePointLimit _timeLimit = new RestorePointDateLimit(new TimeSpan(0, 4, 0, 0));
    private IRestorePointLimit _hybridAllLimit =
        new RestorePointHybridLimit(new TimeSpan(0, 4, 0, 0), 2, new Requirements(0));
    private IRestorePointLimit _hybridOneAndMoreLimit =
        new RestorePointHybridLimit(new TimeSpan(0, 4, 0, 0), 2, new Requirements(1));
    private IRestorePointCleanMethod _clean = new RestorePointClean();
    private IRestorePointCleanMethod _merge = new RestorePointMerge();
    private ILogging _logging = new ConsoleLogging();

    [Fact]
    public void RestorePointsCountAfterCountLimitClean()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _countLimit, _clean, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        backupTask.Execute();
        backupTask.AddObject(_backupObject1);
        backupTask.Execute();
        backupTask.AddObject(_backupObject3);
        backupTask.Execute();
        backupTask.CleanRestorePoints();
        Assert.Equal(2, backupTask.GetRestorePointsAmount());
    }

    [Fact]
    public void LastPointBackupObjectsCountAfterCountLimitMerge()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _countLimit, _merge, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        backupTask.CleanRestorePoints();
        Assert.Equal(3, lastRestorePoint.Objects.Count);
    }

    [Fact]
    public void RestorePointsCountAfterTimeLimitClean()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _timeLimit, _clean, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint restorePoint4 = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(creationTime);
        restorePoint2.ChangeTimeOfCreation(creationTime);
        restorePoint3.ChangeTimeOfCreation(creationTime);
        restorePoint4.ChangeTimeOfCreation(creationTime);
        backupTask.CleanRestorePoints();
        Assert.Equal(0, backupTask.GetRestorePointsAmount());
    }

    [Fact]
    public void LastRestorePointBackupObjectsCountAfterTimeLimitMerge()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _timeLimit, _merge, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(creationTime);
        restorePoint2.ChangeTimeOfCreation(creationTime);
        restorePoint3.ChangeTimeOfCreation(creationTime);
        lastRestorePoint.ChangeTimeOfCreation(DateTime.Now);
        backupTask.CleanRestorePoints();
        Assert.Equal(3, lastRestorePoint.Objects.Count);
    }

    [Fact]
    public void RestorePointCountAfterHybridAllLimitClean()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _hybridAllLimit, _clean, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(creationTime);
        restorePoint2.ChangeTimeOfCreation(creationTime);
        restorePoint3.ChangeTimeOfCreation(DateTime.Now);
        lastRestorePoint.ChangeTimeOfCreation(DateTime.Now);
        backupTask.CleanRestorePoints();
        Assert.Equal(2, backupTask.GetRestorePointsAmount());
    }

    [Fact]
    public void LastRestorePointBackupObjectsCountAfterHybridAllLimitMerge()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _hybridAllLimit, _merge, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(creationTime);
        restorePoint2.ChangeTimeOfCreation(creationTime);
        restorePoint3.ChangeTimeOfCreation(DateTime.Now);
        lastRestorePoint.ChangeTimeOfCreation(DateTime.Now);
        backupTask.CleanRestorePoints();
        Assert.Equal(3, lastRestorePoint.Objects.Count);
    }

    [Fact]
    public void RestorePointCountAfterHybridOneAndMoreLimitClean()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _hybridOneAndMoreLimit, _clean, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(creationTime);
        restorePoint2.ChangeTimeOfCreation(DateTime.Now);
        restorePoint3.ChangeTimeOfCreation(DateTime.Now);
        lastRestorePoint.ChangeTimeOfCreation(DateTime.Now);
        backupTask.CleanRestorePoints();
        Assert.Equal(2, backupTask.GetRestorePointsAmount());
    }

    [Fact]
    public void LastRestorePointBackupObjectsCountAfterHybridOneAndMoreLimitMerge()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _hybridOneAndMoreLimit, _merge, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint1 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject1);
        RestorePoint restorePoint2 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject2);
        RestorePoint restorePoint3 = backupTask.Execute();
        backupTask.DeleteObject(_backupObject3);
        RestorePoint lastRestorePoint = backupTask.Execute();
        var creationTime = new DateTime(2020, 1, 1);
        restorePoint1.ChangeTimeOfCreation(DateTime.Now);
        restorePoint2.ChangeTimeOfCreation(creationTime);
        restorePoint3.ChangeTimeOfCreation(DateTime.Now);
        lastRestorePoint.ChangeTimeOfCreation(DateTime.Now);
        backupTask.CleanRestorePoints();
        Assert.Equal(3, lastRestorePoint.Objects.Count);
    }

    [Fact]
    public void BackupObjectsCopyToDifferentLocation()
    {
        var backupTask = new BackupTaskExtra("Task", _repository, _algorithm, _hybridOneAndMoreLimit, _merge, _logging);
        backupTask.AddObject(_backupObject1);
        backupTask.AddObject(_backupObject2);
        backupTask.AddObject(_backupObject3);
        RestorePoint restorePoint = backupTask.Execute();
        Repository repository = new InMemoryRepository();
        backupTask.RecoverToDifferentLocation(restorePoint, repository);
        Assert.Equal(3, repository.BackupObjectsCount);
    }
}