# Build stage: uses .NET SDK to compile the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlogApi.csproj", "."]
RUN dotnet restore "./BlogApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlogApi.csproj" -c Release -o /app/build

# Publish stage: optimizes the files for production
FROM build AS publish
RUN dotnet publish "BlogApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: uses the ASP.NET Core runtime image, which is lighter
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Exposes port 8080. ASPNETCORE_HTTP_PORTS controls the internal port of the container.
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "BlogApi.dll"]