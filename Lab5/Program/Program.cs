using Backups;
using Backups.Extra;

namespace Program;

public class Program
{
    public static void Main()
    {
        Repository repository = new InMemoryRepository();
        IStorageAlgorithm algorithm = new SplitStorage();
        BackupObject backupObject1 = new FileBackupObject("/root/dotnet/Zip2/A.txt");
        BackupObject backupObject2 = new FileBackupObject("/root/dotnet/Zip2/B.txt");
        BackupObject backupObject3 = new FileBackupObject("/root/dotnet/Zip2/B.txt");
        IRestorePointCleanMethod clean = new RestorePointClean();
        IRestorePointLimit countLimit = new RestorePointCountLimit(10);
        ILogging logging = new FileLogging("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab5/Backups.Extra/f.txt");
        string serializeFilePath = "/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab5/Backups.Extra/SerializeFile.txt";
        var jsonFileSerialization = new JsonFileSerialization(serializeFilePath);
        var backupTask = new BackupTaskExtra("Task", repository, algorithm, countLimit, clean, logging);
        backupTask.AddObject(backupObject1);
        backupTask.AddObject(backupObject2);
        backupTask.AddObject(backupObject3);
        backupTask.Execute();
        jsonFileSerialization.Serialize(backupTask);
        BackupTaskSerializableInformation newBackupTask = jsonFileSerialization.Deserialize();
    }
}