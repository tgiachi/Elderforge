FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /

RUN apt-get update && apt-get install -y lua5.4 \
    && rm -rf /var/lib/apt/lists/*
COPY ["src", "src/"]
RUN dotnet restore "src/Elderforge.Server/Elderforge.Server.csproj"
COPY . .
WORKDIR "/src/Elderforge.Server"
RUN dotnet build "Elderforge.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Elderforge.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elderforge.Server.dll"]
