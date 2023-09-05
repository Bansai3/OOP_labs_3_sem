using System.Collections.ObjectModel;

namespace Backups;

public class MultipleStorage : Storage
{
    private List<Storage> _storages;

    public MultipleStorage()
    {
        _storages = new List<Storage>();
        StorageCount = 0;
    }

    public ReadOnlyCollection<Storage> Storages => new ReadOnlyCollection<Storage>(_storages);

    public override void AddStorage(Storage storage)
    {
        if (!CheckStorage(storage)) throw new Exception("Storage can not be null!");
        _storages.Add(storage);
        StorageCount++;
    }

    public override void CreateCompressedFile(string path)
    {
        foreach (Storage storage in _storages)
        {
            storage.CreateCompressedFile(path);
        }
    }

    private bool CheckStorage(Storage? storage) => storage is not null;
}