using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOverview;

public class SolutionScanner
{
  private readonly string _directoryPath;

  public SolutionScanner(string directoryPath)
  {
    _directoryPath = directoryPath;
  }

  public List<string> Scan()
  {
    return Directory.EnumerateFiles(_directoryPath, "*.sln", new EnumerationOptions
    {
      IgnoreInaccessible = true,
      RecurseSubdirectories = true
    }).ToList();
  }
}
