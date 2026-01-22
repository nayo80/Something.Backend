FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7089

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Authorization.Api/Authorization.Api.csproj", "Authorization.Api/"]
COPY ["Authorization.Application/Authorization.Application.csproj", "Authorization.Application/"]
COPY ["Authorization.Domain/Authorization.Domain.csproj", "Authorization.Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Authorization.Infrastructure/Authorization.Infrastructure.csproj", "Authorization.Infrastructure/"]
RUN dotnet restore "Authorization.Api/Authorization.Api.csproj"
COPY . .
WORKDIR "/src/Authorization.Api"
RUN dotnet build "./Authorization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Authorization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorization.Api.dll"]
