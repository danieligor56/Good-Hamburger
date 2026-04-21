# Estágio de Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivo de projeto e restaurar dependências
COPY ["GoodHamburger.Api.csproj", "."]
RUN dotnet restore "./GoodHamburger.Api.csproj"

# Copiar todo o resto e buildar
COPY . .
WORKDIR "/src/."
RUN dotnet build "GoodHamburger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GoodHamburger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodHamburger.Api.dll"]
