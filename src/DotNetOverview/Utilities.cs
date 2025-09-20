using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DotNetOverview;

public static class Utilities
{
  private static readonly string[] Headers = ["Project", "Target framework", "SDK format"];

  public static string FormatProjects(ICollection<Project> projects, bool showPath = false)
  {
    if (projects.Count == 0)
      return string.Empty;

    var rows = new List<string[]> { Headers };
    rows.AddRange(projects.Select(p => new[]
      { showPath ? p.Path : p.Name, p.TargetFramework, FormatBoolean(p.SdkFormat) }));

    return FormatRows(rows.ToArray());
  }

  public static string FormatBoolean(bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "-";

  public static string FormatRows(string[][] rows, string separator = " ")
  {
    if (rows.Length == 0)
      return string.Empty;

    var formatString = string.Join(separator, Enumerable
      .Range(0, rows.First().Length)
      .Select(i => $"{{{i},-{rows.Max(r => r[i]?.Length ?? 0)}}}"));

    return string.Join(Environment.NewLine, rows.Select(row => string.Format(CultureInfo.InvariantCulture, formatString, row)));
  }
}
