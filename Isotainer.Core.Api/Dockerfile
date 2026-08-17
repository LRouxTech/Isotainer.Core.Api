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

RUN cat << 'EOF' > nuget.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="PrivateRegistry" value="https://nuget.lodewykroux.co.za/v3/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <PrivateRegistry>
      <add key="Username" value="%NGINX_USERNAME%" />
      <add key="ClearTextPassword" value="%NGINX_PASSWORD%" />
    </PrivateRegistry>
  </packageSourceCredentials>
</configuration>
EOF

# 2. Mount secrets directly as process environment variables during restore
RUN --mount=type=secret,id=NGINX_USERNAME,env=NGINX_USERNAME \
    --mount=type=secret,id=NGINX_PASSWORD,env=NGINX_PASSWORD \
    dotnet restore "Isotainer.Core.Api/Isotainer.Core.Api.csproj" --configfile nuget.config && \
    rm nuget.config
    
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
