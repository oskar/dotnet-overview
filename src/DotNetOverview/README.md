# dotnet-overview

A .NET tool that displays an overview of all projects in a directory, showing information such as project names, target frameworks, and SDK format. It recursively scans `.csproj` files and displays a formatted table with key project information. It can also output JSON for advanced filtering and scripting.

## Install

```bash
dotnet tool install -g dotnet-overview
```

## Use

Display an overview of projects in the current directory:
```bash
dotnet overview
```

Display an overview of projects in a specific path:
```bash
dotnet overview /path/to/directory
```

Show project file paths instead of names:
```bash
dotnet overview --show-paths
```

Show absolute paths:
```bash
dotnet overview --absolute-paths
```

Show count of projects found:
```bash
dotnet overview --count
```

Output as JSON for scripting:
```bash
dotnet overview --json
```

## Advanced usage

Some examples using PowerShell to filter the JSON output.

Show projects matching a specific name:
```powershell
(dotnet overview -j | ConvertFrom-Json) | where Name -eq 'MyProject'
```

Show projects with names matching a pattern:
```powershell
(dotnet overview -j | ConvertFrom-Json) | where Name -like 'test*'
```

Group by target framework all projects not using the new SDK csproj format and sort by count descending:
```powershell
(dotnet overview -j | ConvertFrom-Json) | where SdkFormat -eq $false | select TargetFramework | group -Property TargetFramework -NoElement | sort -Property Count -Descending
```

## Upgrade

```bash
dotnet tool update -g dotnet-overview
```

## Uninstall

```bash
dotnet tool uninstall -g dotnet-overview
```
