# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file if exists
COPY ["*.sln", "./"]

# Copy project files for dependency resolution
COPY ["TideOfDestiniy.API/TideOfDestiniy.API.csproj", "TideOfDestiniy.API/"]
COPY ["TideOfDestiniy.BLL/TideOfDestiniy.BLL.csproj", "TideOfDestiniy.BLL/"]
COPY ["TideOfDestiniy.DAL/TideOfDestiniy.DAL.csproj", "TideOfDestiniy.DAL/"]

# Restore dependencies
RUN dotnet restore "TideOfDestiniy.API/TideOfDestiniy.API.csproj"

# Copy all source files
COPY . .

# Build the project
WORKDIR "/src/TideOfDestiniy.API"
RUN dotnet build "TideOfDestiniy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TideOfDestiniy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TideOfDestiniy.API.dll"]

