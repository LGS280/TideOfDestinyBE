# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy everything at once - more reliable for Render.com
# This ensures all files are available in the build context
COPY . .

# Restore dependencies using solution file
RUN dotnet restore "TideOfDestiniy.sln"

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

