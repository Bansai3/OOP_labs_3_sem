using System.IO.Compression;
using Backups;
using Backups.FileDirectoryHandlers;

namespace Program;

public class MyProgram
{
    static void Main()
    {
        Repository repository = new FileSystemRepository("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab3/Backups/Zip1");
        IStorageAlgorithm algorithm = new SingleStorage();
        BackupObject backupObject1 = new FileBackupObject("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab3/Backups/Zip2/A.txt");
        BackupObject backupObject2 = new FileBackupObject("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab3/Backups/Zip2/B.txt");
        BackupObject backupObject3 = new DirectoryBackupObject("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab3/Backups/Zip2/Catalog");
        BackupObject backupObject4 = new DirectoryBackupObject("/Users/Vadim/Desktop/ITMO/Labs_3_sem/Bansai3/Lab3/Backups/Zip2/Catalog2");
        var backupTask = new BackupTask("Task", repository, algorithm);
        backupTask.AddObject(backupObject3);
        backupTask.AddObject(backupObject4);
        backupTask.AddObject(backupObject1);
        backupTask.AddObject(backupObject2);
        backupTask.Execute();
    }
}