using System.IO;
using Xunit;

namespace DotNetOverview.Tests;

public class SolutionParserIntegrationTests
{
  [Fact]
  public void Parse_real_solution_file_returns_expected_projects()
  {
    // Arrange
    var solutionPath = Path.Combine("..", "..", "..", "..", "..", "DotNetOverview.sln");
    var parser = new SolutionParser();

    // Act
    var projectPaths = parser.Parse(solutionPath);

    // Assert
    Assert.Equal(2, projectPaths.Count);
    Assert.Contains("DotNetOverview.csproj", projectPaths[0]);
    Assert.Contains("DotNetOverview.Tests.csproj", projectPaths[1]);
    Assert.All(projectPaths, p => File.Exists(p));
  }
}
