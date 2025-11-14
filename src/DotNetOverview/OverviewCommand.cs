using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DotNetOverview;

public sealed class OverviewCommand(IAnsiConsole ansiConsole) : Command<OverviewCommand.Settings>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public sealed class Settings : CommandSettings
    {
        [Description("Path to search. Defaults to current directory.")]
        [CommandArgument(0, "[path]")]
        public string? Path { get; set; }

        [Description("Print version of this tool and exit")]
        [CommandOption("-v|--version")]
        public bool Version { get; set; }

        [Description("Show project file paths instead of name")]
        [CommandOption("-p|--show-paths")]
        public bool ShowPaths { get; set; }

        [Description("Show absolute paths instead of relative")]
        [CommandOption("-a|--absolute-paths")]
        public bool AbsolutePaths { get; set; }

        [Description("Show number of projects found")]
        [CommandOption("-c|--count")]
        public bool Count { get; set; }

        [Description("Format the result as JSON")]
        [CommandOption("-j|--json")]
        public bool Json { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.Version)
        {
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var version = attribute?.InformationalVersion.Split('+')[0] ?? "Unknown";
            ansiConsole.WriteLine(version);
            return 0;
        }

        if (string.IsNullOrEmpty(settings.Path))
        {
            // Default to current working directory
            settings.Path = ".";
        }

        if (!Directory.Exists(settings.Path))
        {
            ansiConsole.MarkupLine($"Path does not exist: [green]{settings.Path}[/].");
            return 0;
        }

        var files = Directory.EnumerateFiles(settings.Path, "*.csproj", new EnumerationOptions
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true
        }).ToArray();

        if (files.Length == 0)
        {
            ansiConsole.WriteLine("No csproj files found in path.");
            return 0;
        }

        var basePath = settings.AbsolutePaths ? null : settings.Path;
        var parser = new ProjectParser(basePath);
        var projects = files
            .OrderBy(f => f)
            .Select(parser.Parse)
            .ToList();

        if (settings.Json)
        {
            var json = JsonSerializer.Serialize(projects, JsonOptions);
            ansiConsole.WriteLine(json);
        }
        else
        {
            ansiConsole.Write(Utilities.FormatProjects(projects, settings.ShowPaths));
        }

        if (settings.Count)
        {
            ansiConsole.MarkupLine($"Found [green]{files.Length}[/] project(s).");
        }

        return 0;
    }
}
