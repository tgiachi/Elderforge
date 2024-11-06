FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Elderforge.Server/Elderforge.Server.csproj", "src/Elderforge.Server/"]
RUN dotnet restore "src/Elderforge.Server/Elderforge.Server.csproj"
COPY . .
WORKDIR "/src/src/Elderforge.Server"
RUN dotnet build "Elderforge.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Elderforge.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elderforge.Server.dll"]
