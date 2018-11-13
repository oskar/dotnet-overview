FROM microsoft/dotnet:2.1-sdk-alpine AS build

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
FROM microsoft/dotnet:2.1-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /src/out ./

# run
WORKDIR /data
VOLUME "/data"
ENTRYPOINT ["dotnet", "/app/dotnet-overview.dll"]
