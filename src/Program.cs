using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace DotNetOverview
{
  public class Program
  {
    private readonly IConsole _console;

    private static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

    [Argument(0, Description = "Path to search. Defaults to current working directory")]
    public string Path { get; set; }

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

    public Program(IConsole console)
    {
      _console = console;
    }

    public void OnExecute()
    {
      if (Version)
      {
        var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        var version = attribute?.InformationalVersion.Split('+')[0] ?? "Unknown";
        _console.WriteLine(version);
        return;
      }

      if (string.IsNullOrEmpty(Path))
      {
        // Default to current working directory
        Path = ".";
      }

      if (!Directory.Exists(Path))
      {
        _console.WriteLine("Path does not exist: " + Path);
        return;
      }

      var files = new DirectoryInfo(Path).GetFiles("*.csproj", SearchOption.AllDirectories);

      if (files.Length == 0)
      {
        _console.WriteLine("No csproj files found in path.");
        return;
      }

      var basePath = AbsolutePaths ? null : Path;
      var parser = new ProjectParser(basePath);
      var projects = files
        .Select(f => f.FullName)
        .OrderBy(f => f)
        .Select(parser.Parse);

      if (Json)
      {
        var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(projects, jsonOptions);
        _console.WriteLine(json);
      }
      else
      {
        _console.WriteLine(Utilities.FormatProjects(projects, ShowPaths));
      }

      if (Count)
      {
        _console.WriteLine($"Found {files.Length} project(s).");
      }
    }
  }
}
