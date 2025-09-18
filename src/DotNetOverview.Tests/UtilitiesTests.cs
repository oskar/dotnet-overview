using System;
using System.Collections.Generic;
using Xunit;

namespace DotNetOverview.Tests;

public class UtilitiesTests
{
  [Fact]
  public void FormatBoolean_return_dash_on_null() =>
    Assert.Equal("-", Utilities.FormatBoolean(null));

  [Fact]
  public void FormatBoolean_return_yes_on_true() =>
    Assert.Equal("Yes", Utilities.FormatBoolean(true));

  [Fact]
  public void FormatBoolean_returns_no_on_false() =>
    Assert.Equal("No", Utilities.FormatBoolean(false));

  [Fact]
  public void FormatProjects_throws_on_missing_argument() =>
    Assert.Throws<NullReferenceException>(() => Utilities.FormatProjects(null));

  [Fact]
  public void FormatProjects_returns_nothing_on_empty_project_list() =>
    Assert.Equal("", Utilities.FormatProjects(new List<Project>()));

  [Fact]
  public void FormatProjects_uses_space_as_separator()
  {
    // Arrange
    var expected = "Project Target framework SDK format" + Environment.NewLine +
                   "proj    dotnetcore       Yes       ";
    var projects = new List<Project>
    {
      new() { Name = "proj", TargetFramework = "dotnetcore", SdkFormat = true }
    };

    // Act and assert
    Assert.Equal(expected, Utilities.FormatProjects(projects));
  }

  [Fact]
  public void FormatProjects_uses_path_instead_of_name_if_specified()
  {
    // Arrange
    var expected = "Project             Target framework SDK format" + Environment.NewLine +
                   "path/to/file.csproj something        -         ";
    var projects = new List<Project>
    {
      new() { Path = "path/to/file.csproj", TargetFramework = "something" }
    };

    // Act and assert
    Assert.Equal(expected, Utilities.FormatProjects(projects, true));
  }

  [Fact]
  public void FormatRows_throws_on_missing_argument() =>
    Assert.Throws<NullReferenceException>(() => Utilities.FormatRows(null));

  [Fact]
  public void FormatRows_returns_nothing_on_empty_rows() =>
    Assert.Equal("", Utilities.FormatRows([]));

  [Fact]
  public void FormatRows_uses_specified_separator() =>
    Assert.Equal("1*2*3", Utilities.FormatRows([["1", "2", "3"]], "*"));

  [Fact]
  public void FormatRows_pads_with_spaces()
  {
    var rows = new[]
    {
      new [] { "first", "second", "third" },
      new [] { "1", "2", "3" }
    };

    Assert.Equal("first second third" + Environment.NewLine +
                 "1     2      3    ", Utilities.FormatRows(rows));
  }
}
