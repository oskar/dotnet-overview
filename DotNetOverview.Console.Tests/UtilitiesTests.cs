using System;
using System.Collections.Generic;
using DotNetOverview.Library;
using Xunit;

namespace DotNetOverview.Console.Tests
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
      Utilities.FormatProjects(new List<Project>());
    }
  }
}
