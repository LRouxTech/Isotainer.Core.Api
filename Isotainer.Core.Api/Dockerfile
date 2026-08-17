FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Isotainer.Core.Api/Isotainer.Core.Api.csproj", "Isotainer.Core.Api/"]
COPY ["Isotainer.Module.Tank/Isotainer.Module.Tank.csproj", "Isotainer.Module.Tank/"]
COPY ["Isotainer.Module.Wash/Isotainer.Module.Wash.csproj", "Isotainer.Module.Wash/"]

RUN --mount=type=secret,id=NGINX_USERNAME \
    --mount=type=secret,id=NGINX_PASSWORD \
    dotnet nuget add source "https://nuget.lodewykroux.co.za/v3/index.json" \
      --name "PrivateRegistry" \
      --username "$(cat /run/secrets/NGINX_USERNAME)" \
      --password "$(cat /run/secrets/NGINX_PASSWORD)" \
      --store-password-in-clear-text
    
RUN dotnet restore "Isotainer.Core.Api/Isotainer.Core.Api.csproj"
COPY . .
WORKDIR "/src/Isotainer.Core.Api"
RUN dotnet build "./Isotainer.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Isotainer.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Isotainer.Core.Api.dll"]
