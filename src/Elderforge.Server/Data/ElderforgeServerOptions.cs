using CommandLine;

namespace Elderforge.Server.Data;

public class ElderforgeServerOptions
{
    [Option('r', "root-directory", Required = false, HelpText = "The root directory of the server.")]
    public string RootDirectory { get; set; }

    [Option('p', "port", Required = false, HelpText = "The port the server will listen on.")]
    public int Port { get; set; } = 5000;


    [Option('d', "debug", Required = true, HelpText = "Whether the server should run in debug mode.")]
    public bool IsDebug { get; set; }

}

