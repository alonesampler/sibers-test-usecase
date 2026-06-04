FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SB.Auth/SB.Auth.csproj", "SB.Auth/"]
RUN dotnet restore "SB.Auth/SB.Auth.csproj"
COPY . .
WORKDIR /src/SB.Auth
RUN dotnet publish "SB.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SB.Auth.dll"]