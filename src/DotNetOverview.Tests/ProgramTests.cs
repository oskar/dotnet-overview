using System;
using System.IO;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;
using Xunit;

namespace DotNetOverview.Tests;

public class ProgramTests
{
  [Fact]
  public void Prints_version_and_exits_when_Version_set()
  {
    // Arrange
    var semVerPattern = @"^\d+\.\d+\.\d+$";

    var console = CreateMockConsole();
    var program = new Program(console);
    program.Version = true;

    // Act
    program.OnExecute();

    // Assert
    console.Out.Received(1).WriteLine(Arg.Is<string>(s => Regex.IsMatch(s, semVerPattern)));
  }

  [Fact]
  public void Checks_for_invalid_path()
  {
    // Arrange
    var console = CreateMockConsole();
    var program = new Program(console);
    program.Path = "apaththatdoesnotexist";
    Assert.False(Directory.Exists(program.Path), $"Test prerequisite failed: Path '{program.Path}' should not exist");

    // Act
    program.OnExecute();

    // Assert
    console.Out.Received(1).WriteLine(Arg.Is<string>(s => s.StartsWith("Path does not exist")));
  }

  [Fact]
  public void Stops_if_no_csproj_files_found()
  {
    // Arrange
    var console = CreateMockConsole();
    var program = new Program(console);
    program.Path = "."; // no csproj files exist in working dir which is "tests/bin/Debug/netcoreapp3.1"
    Assert.True(Directory.Exists(program.Path), $"Test prerequisite failed: Path '{program.Path}' should exist");

    // Act
    program.OnExecute();

    // Assert
    console.Out.Received(1).WriteLine("No csproj files found in path.");
    console.Out.Received(1).WriteLine(Arg.Any<string>());
  }

  [Fact]
  public void Prints_json_if_requested()
  {
    // Arrange
    var console = CreateMockConsole();
    var program = new Program(console);
    program.Path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.ToString() ?? "";
    program.Json = true;
    Assert.True(Directory.Exists(program.Path), $"Test prerequisite failed: Path '{program.Path}' should exist");

    // Act
    program.OnExecute();

    // Assert
    console.Out.Received(1).WriteLine(Arg.Is<string>(s => IsJson(s)));
    console.Out.Received(1).WriteLine(Arg.Any<string>());
  }

  private static bool IsJson(string s)
  {
    try
    {
      System.Text.Json.JsonSerializer.Deserialize<object>(s);
      return true;
    }
    catch
    {
      return false;
    }
  }

  private static IConsole CreateMockConsole()
  {
    var console = Substitute.For<IConsole>();
    console.Out.Returns(Substitute.For<TextWriter>());
    return console;
  }
}
