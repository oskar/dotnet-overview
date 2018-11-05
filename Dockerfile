FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /app

# restore
COPY DotNetOverview.Console/*.csproj ./DotNetOverview.Console/
COPY DotNetOverview.Library/*.csproj ./DotNetOverview.Library/
COPY DotNetOverview.Library.Tests/*.csproj ./DotNetOverview.Library.Tests/
WORKDIR /app/DotNetOverview.Console
RUN dotnet restore
WORKDIR /app/DotNetOverview.Library.Tests
RUN dotnet restore

# build
WORKDIR /app/
COPY DotNetOverview.Console/. ./DotNetOverview.Console/
COPY DotNetOverview.Library/. ./DotNetOverview.Library/
WORKDIR /app/DotNetOverview.Console
RUN dotnet publish -c Release -o out

# test
FROM build AS testrunner
WORKDIR /app/DotNetOverview.Library.Tests
COPY DotNetOverview.Library.Tests/. .
RUN dotnet test

# run
FROM microsoft/dotnet:2.1-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/DotNetOverview.Console/out ./
ENTRYPOINT ["dotnet", "DotNetOverview.Console.dll"]
