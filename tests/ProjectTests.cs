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
    public void Constructor_creates_object_without_default_value_for_SdkFormat() =>
      Assert.Null(new Project().SdkFormat);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_TargetFramework() =>
      Assert.Null(new Project().TargetFramework);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_TreatWarningsAsErrors() =>
      Assert.Null(new Project().TreatWarningsAsErrors);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_OutputType() =>
      Assert.Null(new Project().OutputType);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_Authors() =>
      Assert.Null(new Project().Authors);

    [Fact]
    public void Constructor_creates_object_without_default_value_for_Version() =>
      Assert.Null(new Project().Version);
  }
}
