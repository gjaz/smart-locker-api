# Etapa 1: construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY SmartLocker.Api/SmartLocker.Api.csproj SmartLocker.Api/

RUN dotnet restore SmartLocker.Api/SmartLocker.Api.csproj

COPY SmartLocker.Api/ SmartLocker.Api/

WORKDIR /src/SmartLocker.Api

RUN dotnet publish SmartLocker.Api.csproj -c Release -o /app/publish /p:UseAppHost=false


# Etapa 2: ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SmartLocker.Api.dll"]