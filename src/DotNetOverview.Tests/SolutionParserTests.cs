using System;
using System.IO;
using Xunit;

namespace DotNetOverview.Tests;

public sealed class SolutionParserTests : IDisposable
{
  private readonly string _tempSolutionFile;
  private readonly SolutionParser _parser;

  public SolutionParserTests()
  {
    var tempFileName = Path.ChangeExtension(Path.GetRandomFileName(), ".sln");
    _tempSolutionFile = Path.Combine(Path.GetTempPath(), tempFileName);
    Assert.False(File.Exists(_tempSolutionFile),
      $"Test prerequisite failed: File '{_tempSolutionFile}' should not exist");

    _parser = new SolutionParser();
  }

  public void Dispose()
  {
    // Teardown
    if (File.Exists(_tempSolutionFile))
      File.Delete(_tempSolutionFile);
  }

  [Fact]
  public void Parse_throws_on_null() =>
    Assert.Throws<ArgumentNullException>(() => new SolutionParser().Parse(null));

  [Fact]
  public void Parse_throws_on_empty_argument() =>
    Assert.Throws<ArgumentNullException>(() => new SolutionParser().Parse(string.Empty));

  [Fact]
  public void Parse_throws_on_non_existent_file() =>
    Assert.Throws<ArgumentException>(() => new SolutionParser().Parse("thisfiledoesnotexist.sln"));

  [Fact]
  public void Parse_returns_project_paths()
  {
    // Arrange
    File.WriteAllText(_tempSolutionFile, @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.26124.0
MinimumVisualStudioVersion = 15.0.26124.0
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""DotNetOverview"", ""src\DotNetOverview\DotNetOverview.csproj"", ""{GUID1}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""DotNetOverview.Tests"", ""src\DotNetOverview.Tests\DotNetOverview.Tests.csproj"", ""{GUID2}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
EndGlobal");

    // Act
    var projectPaths = _parser.Parse(_tempSolutionFile);

    // Assert
    Assert.Equal(2, projectPaths.Count);
    Assert.Contains(projectPaths, p => p.Contains("DotNetOverview.csproj"));
    Assert.Contains(projectPaths, p => p.Contains("DotNetOverview.Tests.csproj"));
  }

  [Fact]
  public void Parse_filters_out_solution_items()
  {
    // Arrange
    File.WriteAllText(_tempSolutionFile, @"
Microsoft Visual Studio Solution File, Format Version 12.00
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""TestProject"", ""src\TestProject\TestProject.csproj"", ""{GUID1}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Solution Items"", ""Solution Items"", ""{GUID2}""
	ProjectSection(SolutionItems) = preProject
		README.md = README.md
	EndProjectSection
EndProject");

    // Act
    var projectPaths = _parser.Parse(_tempSolutionFile);

    // Assert
    Assert.Single(projectPaths);
    Assert.Contains(projectPaths, p => p.Contains("TestProject.csproj"));
  }
}
