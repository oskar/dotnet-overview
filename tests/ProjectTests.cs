using System;
using Xunit;

namespace DotNetOverview.Tests
{
  public class ProjectTests
  {
    [Fact]
    public void Constructor_creates_object_without_name() =>
      Assert.Null(new Project().Name);

    [Fact]
    public void Constructor_creates_object_without_path() =>
      Assert.Null(new Project().Path);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_NewCsProjFormat() =>
      Assert.Null(new Project().NewCsProjFormat);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_TargetFramework() =>
      Assert.Null(new Project().TargetFramework);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_TreatWarningsAsErrors() =>
      Assert.Null(new Project().TreatWarningsAsErrors);
  }
}
