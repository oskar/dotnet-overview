using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DotNetOverview;

public class SolutionParser
{
  /// <summary>
  /// Parses a solution file and returns a list of project file paths referenced in the solution.
  /// </summary>
  /// <param name="solutionFilePath">The path to the solution file (.sln)</param>
  /// <returns>A list of project file paths (.csproj, .vbproj, .fsproj, etc.)</returns>
#pragma warning disable CA1822
  public List<string> Parse(string solutionFilePath)
#pragma warning restore CA1822
  {
    if (string.IsNullOrEmpty(solutionFilePath))
      throw new ArgumentNullException(nameof(solutionFilePath));

    if (!File.Exists(solutionFilePath))
      throw new ArgumentException($"Solution file does not exist ({solutionFilePath})", nameof(solutionFilePath));

    var projectPaths = new List<string>();
    var solutionDirectory = Path.GetDirectoryName(solutionFilePath) ??
                            throw new ArgumentException("Cannot get directory name of solution file");

    // Regular expression to match project lines in solution file
    // Format: Project("{PROJECT-TYPE-GUID}") = "ProjectName", "RelativePath", "{PROJECT-GUID}"
    var projectLinePattern = @"^Project\(""[^""]+\""\)\s*=\s*""[^""]+"",\s*""([^""]+)"",\s*""[^""]+""";
    var regex = new Regex(projectLinePattern, RegexOptions.Multiline);

    var solutionContent = File.ReadAllText(solutionFilePath);
    var matches = regex.Matches(solutionContent);

    foreach (Match match in matches)
    {
      if (match.Success && match.Groups.Count > 1)
      {
        var relativePath = match.Groups[1].Value;

        // Convert backslashes to platform-specific path separator
        relativePath = relativePath.Replace('\\', Path.DirectorySeparatorChar);

        // Only include project files (not solution folders or other items)
        if (IsProjectFile(relativePath))
        {
          var fullPath = Path.GetFullPath(Path.Combine(solutionDirectory, relativePath));

          projectPaths.Add(fullPath);
        }
      }
    }

    return projectPaths;
  }

  /// <summary>
  /// Determines if the given path is a project file based on its extension.
  /// </summary>
  /// <param name="filePath">The file path to check</param>
  /// <returns>True if the file is a project file, false otherwise</returns>
  private static bool IsProjectFile(string filePath)
  {
    if (string.IsNullOrEmpty(filePath))
      return false;

    var extension = Path.GetExtension(filePath).ToLowerInvariant();

    // Common .NET project file extensions
    return extension == ".csproj" ||
           extension == ".vbproj" ||
           extension == ".fsproj" ||
           extension == ".vcxproj" ||
           extension == ".proj";
  }
}
