# dotnet-overview
A .NET Core global tool to display a simple overview of all projects in a directory, with information such as target framework.

## Docker

This tool is not yet published to Docker Hub so you'll need to build the docker image yourself:

`docker build -t dotnet-overview .`

To scan current directory ($PWD) run this:

`docker run -v $PWD:/data dotnet-overview`

## .NET Core Global Tool

This tool is not yet published to NuGet Gallery, but to install it as a Global Tool, run:

`./globaltool-reinstall-local.sh`

And then run either `dotnet-overview` or `dotnet overview` from any directory you want to scan.

## Advanced usage

Some examples using PowerShell to filter the output. Note the parentheses.

Show project(s) which exactly maches name:

`(dotnet overview -j | ConvertFrom-Json) | where Name -eq 'dotnet-overview'`

Show only projects with names matching pattern:

`(dotnet overview -j | ConvertFrom-Json) | where Name -like 'tes*'`

Show projects which does not use the new SDK csproj format:

`(dotnet overview -j | ConvertFrom-Json) | where -not NewCsProjFormat`
