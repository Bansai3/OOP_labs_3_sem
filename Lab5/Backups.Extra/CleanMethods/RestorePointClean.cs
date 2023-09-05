namespace Backups.Extra;

public class RestorePointClean : IRestorePointCleanMethod
{
    public void Clean(List<RestorePoint> restorePoints, List<RestorePoint> restorePointsToDelete)
    {
        if (CheckRestorePointsExtra(restorePoints) == false ||
            CheckRestorePointsExtra(restorePointsToDelete) == false)
            throw new ArgumentException("Invalid restore points extra!");
        foreach (RestorePoint restorePointExtra in restorePointsToDelete)
        {
            restorePoints.Remove(restorePointExtra);
        }
    }

    private bool CheckRestorePointsExtra(List<RestorePoint>? restorePoints) => restorePoints != null;
}