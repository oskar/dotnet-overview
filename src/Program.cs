using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;

namespace DotNetOverview
{
  class Program
  {
    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    [Argument(0, Description = "Path to search. Defaults to current working directory")]
    public string Path { get; private set; }

    [Option(Description = "Print version of this tool and exit")]
    public bool Version { get; }

    [Option(Description = "Show project file paths instead of name", ShortName = "p")]
    public bool ShowPaths { get; }

    [Option(Description = "Show absolute paths instead of relative")]
    public bool AbsolutePaths { get; }

    [Option(Description = "Show number of projects found")]
    public bool Count { get; }

    [Option(Description = "Format the result as JSON")]
    public bool Json { get; }

    private void OnExecute()
    {
      if (Version)
      {
        var attribute = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        Console.WriteLine($"{attribute.InformationalVersion}");
        return;
      }

      if (string.IsNullOrEmpty(Path))
      {
        // Default to current working directory
        Path = ".";
      }

      if (!Directory.Exists(Path))
      {
        Console.WriteLine("Path does not exist: " + Path);
        return;
      }

      var files = new DirectoryInfo(Path).GetFiles("*.csproj", SearchOption.AllDirectories);

      if (files.Length == 0)
      {
        Console.WriteLine("No csproj files found in path.");
        return;
      }

      if (files.Length > 100 &&
          !Prompt.GetYesNo("Found more than 100 csproj files. Do you want to proceed?", true))
      {
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
        Console.WriteLine(JsonConvert.SerializeObject(projects, Formatting.Indented));
      }
      else
      {
        Console.WriteLine(Utilities.FormatProjects(projects, ShowPaths));
      }

      if (Count)
      {
        Console.WriteLine($"Found {files.Length} project(s).");
      }
    }
  }
}
