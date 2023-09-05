using Aspose.Zip;

namespace Backups.FileDirectoryHandlers;

public interface IZipHandler
{
    void AddZipBackupObject(string zipArchivePath);
    void CreateZipBackupObject(string path, string title);
}