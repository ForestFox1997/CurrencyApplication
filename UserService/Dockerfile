FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY UserService/UserService.csproj UserService/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore UserService/UserService.csproj

COPY . .

RUN dotnet publish UserService/UserService.csproj \
    -c Release \
    -o /publish \
    /p:UseAppHost=false \
    /p:DisableTransitiveProjectReferences=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "UserService.dll"]