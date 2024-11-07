using Elderforge.Core.Extensions;
using Elderforge.Core.Server.Types;


namespace Elderforge.Core.Server.Data;

public class DirectoriesConfig
{
    private readonly string _rootDirectory;

    public DirectoriesConfig(string rootDirectory)
    {
        _rootDirectory = rootDirectory;

        Init();
    }


    public string this[DirectoryType directoryType] => GetPath(directoryType);


    public string GetPath(DirectoryType directoryType)
    {
        return Path.Combine(_rootDirectory, directoryType.ToString().ToSnakeCase());
    }

    private void Init()
    {
        if (!Directory.Exists(_rootDirectory))
        {
            Directory.CreateDirectory(_rootDirectory);
        }

        var directoryTypes = Enum.GetValues<DirectoryType>().ToList();

        directoryTypes.Remove(DirectoryType.Root);


        foreach (var directory in directoryTypes)
        {
            var path = GetPath(directory);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}