FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY FinanceService/FinanceService.csproj FinanceService/

RUN dotnet restore FinanceService/FinanceService.csproj

COPY . .

RUN dotnet publish FinanceService/FinanceService.csproj \
    -c Release \
    -o /publish \
    /p:UseAppHost=false \
    /p:DisableTransitiveProjectReferences=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FinanceService.dll"]