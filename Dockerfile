# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PortfolioApi.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ENABLE_SWAGGER=true

EXPOSE 8080

COPY --from=build /app/publish .

# Render provides $PORT; fall back to 8080 for local docker runs.
CMD ["sh", "-c", "dotnet PortfolioApi.dll --urls http://0.0.0.0:${PORT:-8080}"]
