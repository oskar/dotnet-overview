using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using McMaster.Extensions.CommandLineUtils;
using Spectre.Console;

namespace DotNetOverview;

public class OverviewCommand(IAnsiConsole ansiConsole)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    [Argument(0, Description = "Path to search. Defaults to current working directory")]
    public string? Path { get; set; }

    [Option(Description = "Print version of this tool and exit")]
    public bool Version { get; set; }

    [Option(Description = "Show project file paths instead of name", ShortName = "p")]
    public bool ShowPaths { get; set; }

    [Option(Description = "Show absolute paths instead of relative")]
    public bool AbsolutePaths { get; set; }

    [Option(Description = "Show number of projects found")]
    public bool Count { get; set; }

    [Option(Description = "Format the result as JSON")]
    public bool Json { get; set; }

    public void OnExecute()
    {
        if (Version)
        {
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var version = attribute?.InformationalVersion.Split('+')[0] ?? "Unknown";
            ansiConsole.WriteLine(version);
            return;
        }

        if (string.IsNullOrEmpty(Path))
        {
            // Default to current working directory
            Path = ".";
        }

        if (!Directory.Exists(Path))
        {
            ansiConsole.MarkupLine($"Path does not exist: [green]{Path}[/].");
            return;
        }

        var files = Directory.EnumerateFiles(Path, "*.csproj", new EnumerationOptions
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true
        }).ToArray();

        if (files.Length == 0)
        {
            ansiConsole.WriteLine("No csproj files found in path.");
            return;
        }

        var basePath = AbsolutePaths ? null : Path;
        var parser = new ProjectParser(basePath);
        var projects = files
            .OrderBy(f => f)
            .Select(parser.Parse)
            .ToList();

        if (Json)
        {
            var json = JsonSerializer.Serialize(projects, JsonOptions);
            ansiConsole.WriteLine(json);
        }
        else
        {
            ansiConsole.Write(Utilities.FormatProjects(projects, ShowPaths));
        }

        if (Count)
        {
            ansiConsole.MarkupLine($"Found [green]{files.Length}[/] project(s).");
        }
    }
}
