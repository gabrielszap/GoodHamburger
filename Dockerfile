# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os csproj primeiro para aproveitar cache
COPY GoodHamburger.API/GoodHamburger.API.csproj GoodHamburger.API/
COPY GoodHamburger.Application/GoodHamburger.Application.csproj GoodHamburger.Application/
COPY GoodHamburger.Domain/GoodHamburger.Domain.csproj GoodHamburger.Domain/
COPY GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj GoodHamburger.Infrastructure/

RUN dotnet restore GoodHamburger.API/GoodHamburger.API.csproj

# Copia o restante do código
COPY . .

RUN dotnet publish GoodHamburger.API/GoodHamburger.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GoodHamburger.API.dll"]