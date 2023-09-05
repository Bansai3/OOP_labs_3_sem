using Xunit;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void ExecuteBackupTask()
    {
        Repository repository = new InMemoryRepository();
        IStorageAlgorithm algorithm = new SplitStorage();
        BackupObject backupObject1 = new FileBackupObject("/root/dotnet/Zip2/A.txt");
        BackupObject backupObject2 = new FileBackupObject("/root/dotnet/Zip2/B.txt");
        var backupTask = new BackupTask("Task", repository, algorithm);
        backupTask.AddObject(backupObject1);
        backupTask.AddObject(backupObject2);
        backupTask.Execute();
        backupTask.DeleteObject(backupObject1);
        backupTask.Execute();
        Assert.Equal(2, backupTask.GetRestorePointsAmount());
        Assert.Equal(3, repository.StoragesCount);
    }
}