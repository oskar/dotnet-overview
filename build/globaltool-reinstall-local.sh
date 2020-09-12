dotnet pack src -o src/out
dotnet tool uninstall -g dotnet-overview
dotnet tool install -g dotnet-overview --add-source src/out --no-cache
