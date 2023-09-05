using System.Collections.ObjectModel;

namespace Backups;

public class Backup
{
    private List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public List<RestorePoint> GetRestorePoints() => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (!CheckRestorePoint(restorePoint)) throw new Exception("Restore point can not be null!");
        _restorePoints.Add(restorePoint);
    }

    private bool CheckRestorePoint(RestorePoint? restorePoint) => restorePoint is not null;
}