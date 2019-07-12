using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DotNetOverview
{
  public class ProjectParser
  {
    private string basePath_;

    public ProjectParser(string basePath = null)
    {
      basePath_ = basePath;
    }

    private readonly XNamespace _msbuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

    public Project Parse(string projectFilePath)
    {
      if (string.IsNullOrEmpty(projectFilePath))
        throw new ArgumentNullException(nameof(projectFilePath));

      if (!File.Exists(projectFilePath))
        throw new ArgumentException(string.Format("Project file does not exist ({0})", projectFilePath), "projectFilePath");

      var project = new Project();
      project.Path = string.IsNullOrEmpty(basePath_) ? projectFilePath : Path.GetRelativePath(basePath_, projectFilePath);
      project.Name = Path.GetFileNameWithoutExtension(projectFilePath);

      var xmlDoc = XDocument.Load(projectFilePath);
      if (xmlDoc == null)
        return project;

      var sdkFormat = IsSdkFormat(xmlDoc);
      project.SdkFormat = sdkFormat;

      if (sdkFormat)
      {
        project.TargetFramework =
          GetPropertyValue(xmlDoc, "TargetFramework") ??
          GetPropertyValue(xmlDoc, "TargetFrameworks");
      }
      else
      {
        project.TargetFramework = GetPropertyValue(xmlDoc, "TargetFrameworkVersion");
      }

      project.OutputType = GetPropertyValue(xmlDoc, "OutputType");
      project.Authors = GetPropertyValue(xmlDoc, "Authors");

      project.Version = GetPropertyValue(xmlDoc, "Version");
      if (string.IsNullOrWhiteSpace(project.Version))
      {
        var prefix = GetPropertyValue(xmlDoc, "VersionPrefix");
        var suffix = GetPropertyValue(xmlDoc, "VersionSuffix");

        project.Version = string.IsNullOrWhiteSpace(suffix) ? prefix : $"{prefix}-{suffix}";
      }

      return project;
    }

    private bool IsSdkFormat(XDocument document) =>
      !string.IsNullOrEmpty(document.Element("Project")?.Attribute("Sdk")?.Value);

    private string GetPropertyValue(XDocument document, string property)
    {
      var value = document.Element(_msbuildNamespace + "Project")
          ?.Elements(_msbuildNamespace + "PropertyGroup")
          ?.Elements(_msbuildNamespace + property)
          ?.Select(v => v.Value)
          ?.FirstOrDefault();

      if (!string.IsNullOrEmpty(value))
        return value;

      return document.Element("Project")
        ?.Elements("PropertyGroup")
        ?.Elements(property)
        ?.Select(v => v.Value)
        ?.FirstOrDefault();
    }
  }
}
