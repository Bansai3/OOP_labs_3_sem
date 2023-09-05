namespace Backups.Extra;

public interface IRestorePointLimit
{
    List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints);
}