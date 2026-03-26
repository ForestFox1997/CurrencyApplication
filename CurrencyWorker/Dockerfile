FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY CurrencyWorker/CurrencyWorker.csproj CurrencyWorker/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore CurrencyWorker/CurrencyWorker.csproj

COPY . .

RUN dotnet publish CurrencyWorker/CurrencyWorker.csproj \
    -c Release \
    -o /publish \
    /p:UseAppHost=false \
    /p:DisableTransitiveProjectReferences=true

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "CurrencyWorker.dll"]