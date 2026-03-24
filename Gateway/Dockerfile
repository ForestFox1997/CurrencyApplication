FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY Gateway/Gateway.csproj Gateway/

RUN dotnet restore Gateway/Gateway.csproj

COPY . .

RUN dotnet publish Gateway/Gateway.csproj \
    -c Release \
    -o /publish \
    /p:UseAppHost=false \
    /p:DisableTransitiveProjectReferences=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]