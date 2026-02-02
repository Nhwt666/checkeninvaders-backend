# Use .NET 8 SDK as build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ChickenInvenders_BE.csproj", "./"]
RUN dotnet restore "./ChickenInvenders_BE.csproj"

# Copy the rest of the files and build
COPY . .
RUN dotnet publish "ChickenInvenders_BE.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use .NET 8 ASP.NET runtime as final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 10000 (Render requirement)
EXPOSE 10000

# Set environment variable for port
ENV ASPNETCORE_URLS=http://+:10000

# Run the application
ENTRYPOINT ["dotnet", "ChickenInvenders_BE.dll"]
