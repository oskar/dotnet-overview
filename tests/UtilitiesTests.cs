using System;
using System.Collections.Generic;
using Xunit;

namespace DotNetOverview.Tests
{
  public class UtilitiesTests
  {
    [Fact]
    public void FormatBoolean_returns_correct_string()
    {
      Assert.Equal("-", Utilities.FormatBoolean(null));
      Assert.Equal("Yes", Utilities.FormatBoolean(true));
      Assert.Equal("No", Utilities.FormatBoolean(false));
    }

    [Fact]
    public void FormatProjects_throws_on_missing_argument()
    {
      Assert.Throws<ArgumentNullException>(() => Utilities.FormatProjects(null));
    }

    [Fact]
    public void FormatProjects_returns_nothing_on_empty_project_list()
    {
      Assert.Equal("", Utilities.FormatProjects(new List<Project>()));
    }

    [Fact]
    public void FormatProjects_uses_space_as_separator()
    {
      // Arrange
      var expected = "Project Target framework New csproj format" + Environment.NewLine +
                     "proj    dotnetcore       Yes              ";
      var projects = new List<Project>
      {
        new Project { Name = "proj", TargetFramework = "dotnetcore", NewCsProjFormat = true }
      };

      // Act and assert
      Assert.Equal(expected, Utilities.FormatProjects(projects));
    }

    [Fact]
    public void FormatProjects_uses_path_instead_of_name_if_specified()
    {
      // Arrange
      var expected = "Project             Target framework New csproj format" + Environment.NewLine +
                     "path/to/file.csproj something        -                ";
      var projects = new List<Project>
      {
        new Project { Path = "path/to/file.csproj", TargetFramework = "something" }
      };

      // Act and assert
      Assert.Equal(expected, Utilities.FormatProjects(projects, true));
    }

    [Fact]
    public void FormatRows_throws_on_missing_argument()
      => Assert.Throws<ArgumentNullException>(() => Utilities.FormatRows(null));

    [Fact]
    public void FormatRows_returns_nothing_on_empty_rows()
      => Assert.Equal("", Utilities.FormatRows(new string[0][]));

    [Fact]
    public void FormatRows_uses_specified_separator()
      => Assert.Equal("1*2*3", Utilities.FormatRows(new string[][] { new [] { "1", "2", "3" } }, "*"));

    [Fact]
    public void FormatRows_pads_with_spaces()
    {
      var rows = new string[][]
      {
        new [] { "first", "second", "third" },
        new [] { "1", "2", "3" }
      };

      Assert.Equal("first second third" + Environment.NewLine +
                   "1     2      3    ", Utilities.FormatRows(rows));
    }
  }
}
