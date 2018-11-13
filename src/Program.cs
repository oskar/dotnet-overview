using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOverview
{
  class Program
  {
    static void Main(string[] args)
    {
      var fileList = new DirectoryInfo(".").GetFiles("*.csproj", SearchOption.AllDirectories);

      if (!fileList.Any())
      {
        System.Console.WriteLine("No csproj files found in current directory");
        return;
      }
      
      var parser = new ProjectParser();
      var projects = fileList.Select(f => parser.Parse(f.FullName));
      System.Console.WriteLine(Utilities.FormatProjects(projects));
    }
  }
}
