using System.Collections.ObjectModel;

namespace Backups.Extra;

public class RestorePointMerge : IRestorePointCleanMethod
{
    public void Clean(List<RestorePoint> restorePoints, List<RestorePoint> restorePointsToDelete)
    {
        if (CheckRestorePointsExtra(restorePoints) == false ||
            CheckRestorePointsExtra(restorePointsToDelete) == false)
            throw new ArgumentException("Invalid restore points extra!");
        RestorePoint newestRestorePoint = restorePoints[^1];
        ReadOnlyCollection<BackupObject> newestRestorePointBackupObjects = newestRestorePoint.Objects;
        foreach (RestorePoint restorePointExtra in restorePointsToDelete)
        {
            ReadOnlyCollection<BackupObject> backupObjects = restorePointExtra.Objects;
            foreach (BackupObject backupObject in backupObjects)
            {
                if (newestRestorePointBackupObjects.Contains(backupObject) == false)
                    newestRestorePoint.AddBackupObject(backupObject);
            }

            restorePoints.Remove(restorePointExtra);
        }
    }

    private bool CheckRestorePointsExtra(List<RestorePoint>? restorePointsExtra) => restorePointsExtra != null;
}