namespace Backups.Extra;

public interface IRecovery
{
    void RecoverToOriginalLocation(RestorePoint restorePoint);
    void RecoverToDifferentLocation(RestorePoint restorePoint, Repository repository);
}