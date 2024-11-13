using CommandLine;
using Elderforge.Core.Server.Types;

namespace Elderforge.Server.Data;

public class ElderforgeServerOptions
{
    [Option('r', "root-directory", Required = false, HelpText = "The root directory of the server.")]
    public string RootDirectory { get; set; }

    [Option('p', "port", Required = false, HelpText = "The port the server will listen on.")]
    public int Port { get; set; } = 5000;


    [Option('d', "debug", Default = true, Required = false, HelpText = "Whether the server should run in debug mode.")]
    public bool IsDebug { get; set; }


    [Option(
        't',
        "database-type",
        Default = DatabaseType.LiteDb,
        Required = false,
        HelpText = "The type of database to use."
    )]
    public DatabaseType DatabaseType { get; set; }


    [Option(
        'f',
        "database-file-name",
        Default = "elderforge.db",
        Required = false,
        HelpText = "The name of the database file."
    )]
    public string DatabaseFileName { get; set; }
}
