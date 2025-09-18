dotnet pack -o out
dotnet tool uninstall -g dotnet-overview
dotnet tool install -g dotnet-overview --add-source out --no-cache
