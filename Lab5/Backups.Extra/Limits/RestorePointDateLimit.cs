namespace Backups.Extra;

public class RestorePointDateLimit : IRestorePointLimit
{
    public const int MinimalStorageTimeInHours = 3;
    public const int MaximalStorageTimeInHours = 120;
    private TimeSpan _storageTime;

    public RestorePointDateLimit(TimeSpan storageTime)
    {
        if (CheckStorageTime(storageTime) == false)
            throw new ArgumentException("Invalid storage time!");
        _storageTime = storageTime;
    }

    public List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints)
    {
        if (CheckRestorePoints(restorePoints) == false)
            throw new ArgumentException("Invalid restore points!");
        return restorePoints.Where(restorePointExtra => DateTime.Now - restorePointExtra.GetTimeOfCreation() > _storageTime).ToList();
    }

    private bool CheckStorageTime(TimeSpan storageTime) => storageTime.TotalHours is >= MinimalStorageTimeInHours and <= MaximalStorageTimeInHours;
    private bool CheckRestorePoints(List<RestorePoint>? restorePointsExtra) => restorePointsExtra != null;
}