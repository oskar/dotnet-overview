# dotnet-overview
A .NET Core global tool to display a simple overview of all projects in a directory, with information such as target framework.

## Installation

Install: `dotnet tool install -g dotnet-overview`

Update: `dotnet tool update -g dotnet-overview`

Uninstall: `dotnet tool uninstall -g dotnet-overview`

## Usage

Run `dotnet overview` in any directory that contains .csproj files to print an overview.

Run `dotnet overview -h` to see options and usage.

## Advanced usage

Some examples using PowerShell to filter the output. Note the parentheses.

Show project(s) which exactly maches name:

`(dotnet overview -j | ConvertFrom-Json) | where Name -eq 'MyProject'`

Show only projects with names matching pattern:

`(dotnet overview -j | ConvertFrom-Json) | where Name -like 'test*'`

Group by target framework all projects not using the new SDK csproj format and sort by count descending:

`(dotnet overview -j | ConvertFrom-Json) | where NewCsProjFormat -eq $false | select TargetFramework | group -Property TargetFramework -NoElement | sort -Property Count -Descending`
