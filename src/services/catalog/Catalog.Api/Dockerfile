#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-composite AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
WORKDIR /src
COPY ["src/services/catalog/Directory.Build.props", "src/services/catalog/"]
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/services/catalog/Catalog.Api/Catalog.Api.csproj", "src/services/catalog/Catalog.Api/"]
COPY ["src/buildingblocks/TeckShop.Core/TeckShop.Core.csproj", "src/buildingblocks/TeckShop.Core/"]
COPY ["src/buildingblocks/TeckShop.Infrastructure/TeckShop.Infrastructure.csproj", "src/buildingblocks/TeckShop.Infrastructure/"]
COPY ["src/services/catalog/Catalog.Application/Catalog.Application.csproj", "src/services/catalog/Catalog.Application/"]
COPY ["src/buildingblocks/Teckshop.Events/Teckshop.Events.csproj", "src/buildingblocks/Teckshop.Events/"]
COPY ["src/services/catalog/Catalog.Domain/Catalog.Domain.csproj", "src/services/catalog/Catalog.Domain/"]
COPY ["src/services/catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/services/catalog/Catalog.Infrastructure/"]
COPY ["src/buildingblocks/TeckShop.Persistence/TeckShop.Persistence.csproj", "src/buildingblocks/TeckShop.Persistence/"]
RUN dotnet restore "./src/services/catalog/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/services/catalog/Catalog.Api"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build -r linux-x64 /p:PublishReadyToRun=true

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish -r linux-x64 --self-contained true --no-restore /p:UseAppHost=true /p:PublishReadyToRun=true /p:PublishTrimmed=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
