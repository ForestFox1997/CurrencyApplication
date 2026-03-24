FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /build

COPY MigrationService/MigrationService.csproj MigrationService/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore MigrationService/MigrationService.csproj

COPY . .

RUN dotnet publish MigrationService/MigrationService.csproj \
    -c Release \
    -o /publish \
    /p:UseAppHost=false \
    /p:DisableTransitiveProjectReferences=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "MigrationService.dll"]