using System.IO.Compression;
using System.Text;
using Aspose.Zip;
using Aspose.Zip.Saving;
using Backups.FileDirectoryHandlers;

namespace Backups;

public class DirectoryBackupObject : BackupObject
{
    public DirectoryBackupObject(string path)
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
        string[] directories = zipArchivePath.Split("/");
        Array.Clear(directories, directories.Length - 1, 1);
        string directoryPath = string.Join('/', directories);
        string zipPath = directoryPath + Title + ".zip";
        ZipFile.CreateFromDirectory(Path, zipPath);
        string zipTitle = Title + ".zip";
        archive.CreateEntryFromFile(zipPath, zipTitle);
        File.Delete(zipPath);
    }

    public override void CreateZipBackupObject(string path, string title)
    {
        if (!CheckDirPath(path)) throw new Exception("Invalid directory path!");
        if (!CheckTitle(title)) throw new Exception("Invalid zip title!");
        string zipFilePath = path + "/" + title;
        ZipFile.CreateFromDirectory(Path, zipFilePath);
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