using System.Collections.ObjectModel;

namespace Backups.Extra;

public static class BackupExtraExtensions
{
    public static string ShowRestorePointInfo(this RestorePoint restorePoint)
    {
        DateTime time = restorePoint.GetTimeOfCreation();
        string info = $"Restore point:\n" +
                      $"Creation time: {time.Year}:{time.Month}:{time.Day}:{time.Hour}:{time.Minute}:{time.Second}\n" +
                      $"Backup objects that restore point contains:\n";
        int count = 1;
        foreach (BackupObject backupObject in restorePoint.Objects)
        {
            info += $"{count++}. ";
            info += backupObject.ShowBackupObjectInfo();
        }

        return info;
    }

    public static string ShowBackupObjectInfo(this BackupObject backupObject)
    {
        string info = $"BackupObject:\n" +
                     $"Title: {backupObject.Title}\n" +
                     $"Path: {backupObject.Path}\n";
        return info;
    }

    public static string ShowBackupInfo(this Backup backup)
    {
        List<RestorePoint> restorePoints = backup.GetRestorePoints();
        string info = "Backup:\n" +
                      "Restore points that backup contains:\n";
        int count = 1;
        foreach (RestorePoint restorePoint in restorePoints)
        {
            info += $"{count++}. ";
            info += restorePoint.ShowRestorePointInfo();
        }

        return info;
    }

    public static string ShowStorageInfo(this Storage storage)
    {
        string info = string.Empty;
        if (storage is MultipleBackupObjectStorage multipleBackupObjectStorage)
        {
            info += multipleBackupObjectStorage.ShowMultipleBackupObjectStorageInfo();
        }
        else if (storage is MultipleStorage multipleStorage)
        {
            info += multipleStorage.ShowMultipleStorageInfo();
        }
        else if (storage is SingleBackupObjectStorage singleStorage)
        {
            info += singleStorage.ShowSingleBackupObjectStorageInfo();
        }

        return info;
    }

    public static string ShowMultipleBackupObjectStorageInfo(
        this MultipleBackupObjectStorage multipleBackupObjectStorage)
    {
        ReadOnlyCollection<BackupObject> backupObjects = multipleBackupObjectStorage.Objects;
        int count = 1;
        string info = "MultipleBackupObjectStorage:\n" +
                      "BackupObjects that multiple backup object storage contains:\n";
        foreach (BackupObject backupObject in backupObjects)
        {
            info += $"{count++}. ";
            info += backupObject.ShowBackupObjectInfo();
        }

        return info;
    }

    public static string ShowMultipleStorageInfo(
        this MultipleStorage multipleStorage)
    {
        ReadOnlyCollection<Storage> storages = multipleStorage.Storages;
        int count = 1;
        string info = "MultipleStorage:\n" +
                      "Storages that multiple storage contains:\n";
        foreach (Storage storage in storages)
        {
            info += $"{count++}. ";
            info += storage.ShowStorageInfo();
        }

        return info;
    }

    public static string ShowSingleBackupObjectStorageInfo(
        this SingleBackupObjectStorage singleBackupObjectStorage)
    {
        string info = $"SingleBackupObjectStorage:\n" +
                      $"Title: {singleBackupObjectStorage.GetBackupObjectTitle()}\n" +
                      $"Path: {singleBackupObjectStorage.GetBackupObjectPath()}\n";
        return info;
    }
}