#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0-jammy-chiseled-composite AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-jammy AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/gateways/yarp/Directory.Build.props", "src/gateways/yarp/"]
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/gateways/yarp/Yarp.Gateway/Yarp.Gateway.csproj", "src/gateways/yarp/Yarp.Gateway/"]
COPY ["src/buildingblocks/TeckShop.Infrastructure/TeckShop.Infrastructure.csproj", "src/buildingblocks/TeckShop.Infrastructure/"]
COPY ["src/buildingblocks/TeckShop.Core/TeckShop.Core.csproj", "src/buildingblocks/TeckShop.Core/"]
RUN dotnet restore "./src/gateways/yarp/Yarp.Gateway/Yarp.Gateway.csproj"
COPY . .
WORKDIR "/src/src/gateways/yarp/Yarp.Gateway"
RUN dotnet build "./Yarp.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build -r linux-x64 /p:PublishReadyToRun=true

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Yarp.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish -r linux-x64 --self-contained true --no-restore /p:UseAppHost=true /p:PublishReadyToRun=true /p:PublishTrimmed=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yarp.Gateway.dll"]
