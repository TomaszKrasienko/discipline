# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers for better caching
COPY ["discipline.gateway.csproj", "/src/"]
RUN dotnet restore "/src/discipline.gateway.csproj"

# Copy the rest of the source and publish
COPY . /src
RUN dotnet publish "/src/discipline.gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Containers commonly listen on 8080 in .NET 8/9 images
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy the published app
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "discipline.gateway.dll"]
