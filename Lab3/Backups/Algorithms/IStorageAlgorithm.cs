namespace Backups;

public interface IStorageAlgorithm
{
    Storage GetComposedObject(RestorePoint restorePoint);
}