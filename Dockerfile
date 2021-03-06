FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

# restore
WORKDIR /src
COPY src/*.csproj .
RUN dotnet restore
WORKDIR /tests
COPY tests/*.csproj .
RUN dotnet restore

# build
WORKDIR /src
COPY src/. .
RUN dotnet publish -c Release -o out

# test
FROM build AS testrunner
WORKDIR /tests
COPY tests/. .
RUN dotnet test

# copy app
FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS runtime
WORKDIR /app
COPY --from=build /src/out ./

# run
WORKDIR /data
VOLUME "/data"
ENTRYPOINT ["dotnet", "/app/dotnet-overview.dll"]
