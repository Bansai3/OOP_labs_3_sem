namespace Backups.Extra;

public class RestorePointCountLimit : IRestorePointLimit
{
    public const int MinimalLimitNumber = 1;

    public RestorePointCountLimit(int limitNumber)
    {
        if (CheckLimitNumber(limitNumber) == false)
            throw new ArgumentException("Invalid limit number!");
        LimitNumber = limitNumber;
    }

    public int LimitNumber { get; private set; }

    public List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints)
    {
        if (CheckRestorePoints(restorePoints) == false)
            throw new ArgumentException("Invalid restore points!");
        var extraRestorePoints = new List<RestorePoint>();
        if (restorePoints.Count > LimitNumber)
        {
            int extraRestorePointsCount = restorePoints.Count - LimitNumber;
            for (int i = 0; i < extraRestorePointsCount; i++)
            {
                extraRestorePoints.Add(restorePoints[i]);
            }
        }

        return extraRestorePoints;
    }

    public void ChangeLimitNumber(int newLimitNumber)
    {
        if (CheckLimitNumber(newLimitNumber) == false)
            throw new ArgumentException("Invalid limit number!");
        LimitNumber = newLimitNumber;
    }

    private bool CheckLimitNumber(int limitNumber) => limitNumber >= MinimalLimitNumber;
    private bool CheckRestorePoints(List<RestorePoint>? restorePointsExtra) => restorePointsExtra != null;
}