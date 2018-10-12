namespace DotNetOverview.Library
{
  public class Project
  {
    public string Name { get; set; }
    public string Path { get; set; }
    public bool? NewCsProjFormat { get; set; }

    public string TargetFramework { get; set; }
    public bool? TreatWarningsAsErrors { get; set; }
  }
}
