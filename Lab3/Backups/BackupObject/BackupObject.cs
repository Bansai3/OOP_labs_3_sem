using Aspose.Zip;
using Backups.FileDirectoryHandlers;

namespace Backups;

public abstract class BackupObject : IZipHandler
{
    public abstract string Path { get; }
    public abstract string Title { get; }

    public abstract void AddZipBackupObject(string zipArchivePath);

    public abstract void CreateZipBackupObject(string path, string title);
    protected string GetTitleFromPath(string path) => path.Split('/')[^1];
}