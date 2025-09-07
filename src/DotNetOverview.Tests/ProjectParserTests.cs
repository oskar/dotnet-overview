using System;
using Xunit;

namespace DotNetOverview.Tests
{
  public class ProjectParserTests
  {
    [Fact]
    public void Parse_throws_on_null() =>
      Assert.Throws<ArgumentNullException>(() => new ProjectParser().Parse(null));

    [Fact]
    public void Parse_throws_on_empty_argument() =>
      Assert.Throws<ArgumentNullException>(() => new ProjectParser().Parse(""));

    [Fact]
    public void Parse_throws_on_missing_file() =>
      Assert.Throws<ArgumentException>(() => new ProjectParser().Parse("thisfiledoesnotexist.csproj"));
  }
}
