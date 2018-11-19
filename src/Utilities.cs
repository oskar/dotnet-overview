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
    public static string FormatProjects(IEnumerable<Project> projects, bool showPath = false)
    {
      if (!projects.Any())
        return string.Empty;

      var rows = new List<string[]>();
      rows.Add(new string[] { "Project", "Target framework", "New csproj format" });
      rows.AddRange(projects.Select(p => new string[] { showPath ? p.Path : p.Name, p.TargetFramework, FormatBoolean(p.NewCsProjFormat) }));

      return FormatRows(rows.ToArray());
    }

    public static string FormatBoolean(bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "-";

    public static string FormatRows(string[][] rows, string separator = " ")
    {
      if (!rows.Any())
        return string.Empty;

      var formatString = string.Join(separator, Enumerable
                                                  .Range(0, rows.First().Length)
                                                  .Select(i => $"{{{i},-{rows.Max(r => r[i]?.Length ?? 0)}}}"));

      return string.Join(Environment.NewLine, rows.Select(row => string.Format(formatString, row)));
    }
  }
}
