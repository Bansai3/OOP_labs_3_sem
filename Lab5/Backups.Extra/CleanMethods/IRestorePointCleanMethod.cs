namespace Backups.Extra;

public interface IRestorePointCleanMethod
{
    void Clean(List<RestorePoint> restorePoints, List<RestorePoint> restorePointsToDelete);
}