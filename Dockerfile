FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /build

COPY src ./src
COPY *.sln .

RUN dotnet restore

RUN dotnet publish src/DotNetOverview --no-restore -c Release -o out

FROM build AS testrunner
RUN dotnet test --no-restore

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /build/out ./
ENTRYPOINT ["dotnet", "dotnet-overview.dll"]
