namespace Backups.Extra;

public class RestorePointHybridLimit : IRestorePointLimit
{
    private TimeSpan _storageTime;
    private int _limitNumber;
    private Requirements _requirements;

    public RestorePointHybridLimit(TimeSpan storageTime, int limitNumber, Requirements requirements)
    {
        if (CheckStorageTime(storageTime) == false)
            throw new ArgumentException("Invalid storage time!");
        if (CheckLimitNumber(limitNumber) == false)
            throw new ArgumentException("Invalid limit number!");
        if (CheckRequirementsType(requirements) == false)
            throw new ArgumentException("Invalid requirements type!");
        _limitNumber = limitNumber;
        _storageTime = storageTime;
        _requirements = requirements;
        _requirements = requirements;
    }

    public List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints)
    {
        if (CheckRestorePoints(restorePoints) == false)
            throw new ArgumentException("Invalid restore points!");
        var restorePointCountLimit = new RestorePointCountLimit(_limitNumber);
        var restorePointDateLimit = new RestorePointDateLimit(_storageTime);
        List<RestorePoint> extraCountRestorePoints = restorePointCountLimit.GetRestorePointsToDelete(restorePoints);
        List<RestorePoint> extraDateRestorePoints = restorePointDateLimit.GetRestorePointsToDelete(restorePoints);
        return _requirements.RequirementsType == Requirements.All ?
            GetRestorePointExtrasWithAllRequirements(extraCountRestorePoints, extraDateRestorePoints) :
            GetRestorePointExtrasWithOneAndMoreRequirements(extraCountRestorePoints, extraDateRestorePoints);
    }

    public void ChangeRequirements(Requirements newRequirements)
    {
        if (CheckRequirementsType(newRequirements) == false)
            throw new ArgumentException("Invalid requirements type!");
        _requirements = newRequirements;
    }

    private List<RestorePoint> GetRestorePointExtrasWithAllRequirements(List<RestorePoint> extraCountRestorePoints, List<RestorePoint> extraDateRestorePoints)
    {
        if (CheckRestorePoints(extraCountRestorePoints) == false || CheckRestorePoints(extraDateRestorePoints) == false)
            throw new ArgumentException("Invalid restore points!");
        return extraCountRestorePoints.Where(extraDateRestorePoints.Contains).ToList();
    }

    private List<RestorePoint> GetRestorePointExtrasWithOneAndMoreRequirements(List<RestorePoint> extraCountRestorePoints, List<RestorePoint> extraDateRestorePoints)
    {
        if (CheckRestorePoints(extraCountRestorePoints) == false || CheckRestorePoints(extraDateRestorePoints) == false)
            throw new ArgumentException("Invalid restore points!");
        return extraCountRestorePoints.Concat(extraDateRestorePoints.Where(extraRestorePoint => extraCountRestorePoints.Contains(extraRestorePoint) == false).ToList()).ToList();
    }

    private bool CheckStorageTime(TimeSpan storageTime) => storageTime.TotalHours is >= RestorePointDateLimit.MinimalStorageTimeInHours
        and <= RestorePointDateLimit.MaximalStorageTimeInHours;
    private bool CheckLimitNumber(int limitNumber) => limitNumber >= RestorePointCountLimit.MinimalLimitNumber;
    private bool CheckRestorePoints(List<RestorePoint>? restorePointsExtra) => restorePointsExtra != null;

    private bool CheckRequirementsType(Requirements? requirements) => requirements != null;
}