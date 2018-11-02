using System;
using Xunit;

namespace DotNetOverview.Library.Tests
{
  public class ProjectParserTests
  {
    [Fact]
    public void Parse_throws_exception_on_missing_argument()
    {
      var parser = new ProjectParser();

      Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
      Assert.Throws<ArgumentNullException>(() => parser.Parse(""));
    }

    [Fact]
    public void Parse_throws_exception_on_missing_file()
    {
      var parser = new ProjectParser();

      Assert.Throws<ArgumentException>(() => parser.Parse("thisfiledoesnotexist.csproj"));
    }
  }
}
