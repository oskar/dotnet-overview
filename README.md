# dotnet-overview

A collection of .NET tools for common development tasks.

## [dotnet-overview](src/DotNetOverview/README.md)

Display an overview of all `.csproj` files in the current directory or any specified path, showing project names, target frameworks, and SDK format information. Supports JSON output for advanced filtering.

```bash
dotnet tool install -g dotnet-overview
dotnet overview
```

## [dotnet-open](src/DotNetOpen/README.md)

Find and open solution files in the current directory or any specified path. Presents an interactive menu when multiple solutions are found.

```bash
dotnet tool install -g dotnet-open
dotnet open
```
