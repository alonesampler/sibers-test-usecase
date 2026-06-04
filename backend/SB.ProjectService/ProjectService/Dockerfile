FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProjectService/ProjectService.csproj", "ProjectService/"]
RUN dotnet restore "ProjectService/ProjectService.csproj"
COPY . .
WORKDIR /src/ProjectService
RUN dotnet publish "ProjectService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=10s --timeout=5s --retries=12 --start-period=120s \
    CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "ProjectService.dll"]