using System;
using System.IO;
using Spectre.Console.Testing;
using Xunit;

namespace DotNetOverview.Tests;

public class OverviewCommandTests
{
    [Fact]
    public void Prints_version_and_exits_when_Version_set()
    {
        // Arrange
        var semVerPattern = @"^\d+\.\d+\.\d+$";

        var console = new TestConsole();
        var command = new OverviewCommand(console);
        command.Version = true;

        // Act
        command.OnExecute();

        // Assert
        Assert.Matches(semVerPattern, console.Output);
    }

    [Fact]
    public void Checks_for_invalid_path()
    {
        // Arrange
        var console = new TestConsole();
        var command = new OverviewCommand(console);
        command.Path = "apaththatdoesnotexist";
        Assert.False(Directory.Exists(command.Path), $"Test prerequisite failed: Path '{command.Path}' should not exist");

        // Act
        command.OnExecute();

        // Assert
        Assert.Equal("Path does not exist: apaththatdoesnotexist.", console.Output.Trim());
    }

    [Fact]
    public void Stops_if_no_csproj_files_found()
    {
        // Arrange
        var console = new TestConsole();
        var command = new OverviewCommand(console);
        command.Path = "."; // no csproj files exist in working directory when running tests
        Assert.True(Directory.Exists(command.Path), $"Test prerequisite failed: Path '{command.Path}' should exist");

        // Act
        command.OnExecute();

        // Assert
        Assert.Equal("No csproj files found in path.", console.Output.Trim());
    }

    [Fact]
    public void Prints_json_if_requested()
    {
        // Arrange
        var console = new TestConsole();
        var command = new OverviewCommand(console);
        command.Path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.ToString() ?? "";
        command.Json = true;
        Assert.True(Directory.Exists(command.Path), $"Test prerequisite failed: Path '{command.Path}' should exist");

        // Act
        command.OnExecute();

        // Assert
        Assert.True(IsJson(console.Output));
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
}
