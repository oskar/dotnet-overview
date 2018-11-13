using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetOverview
{
  public class Utilities
  {
    public static string FormatProjects(IEnumerable<Project> projects)
    {
      if(!projects.Any())
        return string.Empty;

      var nameLabel = "Project";
      var targetFrameworkLabel = "Target framework";
      var newCsProjFormatLabel = "New csproj format";

      var maxLengthName = Math.Max(projects.Max(p => p.Name?.Length ?? 0), nameLabel.Length);
      var maxLengthTargetFramework = Math.Max(projects.Max(p => p.TargetFramework?.Length ?? 0), targetFrameworkLabel.Length);

      var formatString = $"{{0,-{maxLengthName}}} {{1,-{maxLengthTargetFramework}}} {{2,-4}}";
      System.Console.WriteLine(formatString, nameLabel, targetFrameworkLabel, newCsProjFormatLabel);

      var sb = new StringBuilder();

      foreach(var project in projects)
      {
        sb.AppendLine(string.Format(formatString,
          project.Name,
          project.TargetFramework,
          FormatBoolean(project.NewCsProjFormat)));
      }

      return sb.ToString();
    }

    public static string FormatBoolean(bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "-";
  }
}
