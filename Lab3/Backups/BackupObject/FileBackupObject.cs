using System.IO.Compression;
using System.Text;
using Aspose.Zip;
using Aspose.Zip.Saving;
using Backups.FileDirectoryHandlers;

namespace Backups;

public class FileBackupObject : BackupObject
{
    public FileBackupObject(string path)
    {
        Path = path;
        Title = GetTitleFromPath(path);
    }

    public override string Path { get; }
    public override string Title { get; }

    public override void AddZipBackupObject(string zipArchivePath)
    {
        using ZipArchive archive = File.Exists(zipArchivePath)
            ? ZipFile.Open(zipArchivePath, ZipArchiveMode.Update)
            : ZipFile.Open(zipArchivePath, ZipArchiveMode.Create);
        archive.CreateEntryFromFile(Path, Title);
    }

    public override void CreateZipBackupObject(string path, string title)
    {
        if (!CheckDirPath(path)) throw new Exception("Invalid directory path!");
        if (!CheckTitle(title)) throw new Exception("Invalid zip title!");
        string newZipFile = path + "/" + title;
        using ZipArchive archive = ZipFile.Open(newZipFile, ZipArchiveMode.Create);
        archive.CreateEntryFromFile(Path, Title);
    }

    private bool CheckDirPath(string dirPath) => Directory.Exists(dirPath);
    private bool CheckTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return false;
        if (title.Length < 5)
            return false;
        return title[^4..] == ".zip";
    }
}