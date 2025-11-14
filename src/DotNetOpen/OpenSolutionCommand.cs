using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DotNetOpen;

public sealed class OpenSolutionCommand(IAnsiConsole ansiConsole) : Command<OpenSolutionCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to search. Defaults to current directory.")]
        [CommandArgument(0, "[path]")]
        public string Path { get; init; } = "";

        [Description("Open first solution if multiple are found.")]
        [CommandOption("-f|--first")]
        public bool First { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        // Calculate absolute path from supplied path and default
        // to current directory if no path is specified.
        var searchPath = string.IsNullOrEmpty(settings.Path)
          ? Directory.GetCurrentDirectory()
          : Path.GetFullPath(settings.Path);

        if (!Directory.Exists(searchPath))
        {
            ansiConsole.MarkupLine($"Path does not exist: [green]{searchPath}[/].");
            return 1;
        }

        var files = Directory.EnumerateFiles(searchPath, "*.sln", new EnumerationOptions
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true
        }).ToArray();

        if (files.Length == 0)
        {
            ansiConsole.WriteLine("No solution found in path.");
            return 0;
        }

        if (files.Length == 1)
        {
            OpenFile(files[0]);
            return 0;
        }

        ansiConsole.MarkupLine($"Found [green]{files.Length}[/] solutions in [green]{searchPath}[/].");

        if (settings.First)
        {
            OpenFile(files[0]);
            return 0;
        }

        var selectionPrompt = new SelectionPrompt<string>
        {
            Title = "Select which to open:",
            PageSize = 15,
            SearchEnabled = true,
            Converter = filePath => Path.GetRelativePath(searchPath, filePath)
        };
        selectionPrompt.AddChoices(files);
        var selectedSolution = ansiConsole.Prompt(selectionPrompt);
        OpenFile(selectedSolution);

        return 0;
    }

    private void OpenFile(string filePath)
    {
        ansiConsole.MarkupLine($"Opening [green]{filePath}[/].");
        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}
