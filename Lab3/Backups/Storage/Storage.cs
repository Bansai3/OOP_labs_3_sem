namespace Backups;

public abstract class Storage
{
    public int StorageCount { get; protected set; }
    public virtual void AddStorage(Storage storage) { }
    public virtual void CreateCompressedFile(string path) { }
}