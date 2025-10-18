using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DotNetOpen;

internal sealed class OpenSolutionCommand : Command<OpenSolutionCommand.Settings>
{
  public sealed class Settings : CommandSettings
  {
    [Description("Path to search. Defaults to current directory.")]
    [CommandArgument(0, "[searchPath]")]
    public string? SearchPath { get; init; }

    [Description("Open first solution if multiple are found.")]
    [CommandOption("-f|--first")]
    public bool First { get; init; }
  }

  public override int Execute(CommandContext context, Settings settings)
  {
    // Calculate absolute path from supplied path and default
    // to current directory if no path is specified.
    var searchPath = string.IsNullOrEmpty(settings.SearchPath)
      ? settings.SearchPath ?? Directory.GetCurrentDirectory()
      : Path.GetFullPath(settings.SearchPath);

    var files = Directory.EnumerateFiles(searchPath, "*.sln", new EnumerationOptions
    {
      IgnoreInaccessible = true,
      RecurseSubdirectories = true
    }).ToArray();

    if (files.Length == 0)
    {
      AnsiConsole.MarkupLine("[red]No solution found in path.[/]");
      return 1;
    }

    if (files.Length == 1)
    {
      OpenFile(files[0]);
      return 0;
    }

    AnsiConsole.MarkupLine($"Found [green]{files.Length}[/] solutions in [green]{searchPath}[/].");

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
      Converter = path => Path.GetRelativePath(searchPath, path)
    };
    selectionPrompt.AddChoices(files);
    var selectedSolution = AnsiConsole.Prompt(selectionPrompt);
    OpenFile(selectedSolution);

    return 0;
  }

  private static void OpenFile(string filePath)
  {
    AnsiConsole.MarkupLine($"Opening [green]{filePath}[/].");
    Process.Start(new ProcessStartInfo
    {
      FileName = filePath,
      UseShellExecute = true
    });
  }
}
