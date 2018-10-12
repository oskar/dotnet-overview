using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetOverview.Library;

namespace DotNetOverview.Console
{
  class Program
  {
    static void Main(string[] args)
    {
      var fileList = new DirectoryInfo(".").GetFiles("*.csproj", SearchOption.AllDirectories);
      var parser = new ProjectParser();

      var projects = fileList.Select(f => parser.Parse(f.FullName));

      PrintStatistics(projects);
    }

    static void PrintStatistics(IEnumerable<Project> projects)
    {
      var nameLabel = "Project";
      var targetFrameworkLabel = "Target framework";
      var newCsProjFormatLabel = "New csproj format";

      var maxLengthName = Math.Max(projects.Max(p => p.Name?.Length ?? 0), nameLabel.Length);
      var maxLengthTargetFramework = Math.Max(projects.Max(p => p.TargetFramework?.Length ?? 0), targetFrameworkLabel.Length);

      var formatString = $"{{0,-{maxLengthName}}} {{1,-{maxLengthTargetFramework}}} {{2,-4}}";
      System.Console.WriteLine(formatString, nameLabel, targetFrameworkLabel, newCsProjFormatLabel);

      foreach(var project in projects)
      {
        System.Console.WriteLine(
          formatString,
          project.Name,
          project.TargetFramework,
          Format(project.NewCsProjFormat));
      }
    }

    static string Format(bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "-";
  }
}
