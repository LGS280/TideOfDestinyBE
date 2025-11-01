# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TideOfDestiniy.API/TideOfDestiniy.API.csproj", "TideOfDestiniy.API/"]
COPY ["TideOfDestiniy.BLL/TideOfDestiniy.BLL.csproj", "TideOfDestiniy.BLL/"]
COPY ["TideOfDestiniy.DAL/TideOfDestiniy.DAL.csproj", "TideOfDestiniy.DAL/"]
RUN dotnet restore "./TideOfDestiniy.API/TideOfDestiniy.API.csproj"
COPY . .
WORKDIR "/src/TideOfDestiniy.API"
RUN dotnet build "./TideOfDestiniy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build	

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TideOfDestiniy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# BẮT BUỘC: Lắng nghe port Render cấp
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE 10000

ENTRYPOINT ["dotnet", "TideOfDestiniy.API.dll"]

