# Multi-platform base stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Multi-platform build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Czertainly.Auth/Czertainly.Auth.csproj", "Czertainly.Auth/"]
RUN dotnet restore "Czertainly.Auth/Czertainly.Auth.csproj"
COPY . .
WORKDIR "/src/Czertainly.Auth"
RUN dotnet build "Czertainly.Auth.csproj" -c Release -o /app/build

# Multi-platform publish stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY --from=build /src /src
WORKDIR "/src/Czertainly.Auth"
RUN dotnet publish "Czertainly.Auth.csproj" -c Release -o /app/publish

# Multi-platform final stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish /app

MAINTAINER CZERTAINLY <support@czertainly.com>

RUN addgroup --system --gid 10001 czertainly && adduser --system --home /opt/czertainly --uid 10001 --ingroup czertainly czertainly
COPY ./docker /opt/czertainly
WORKDIR /opt/czertainly

ENV COMPlus_EnableDiagnostics=0
ENV AUTH_DB_CONNECTION_STRING=
ENV AUTH_CREATE_UNKNOWN_USERS=false
ENV AUTH_CREATE_UNKNOWN_ROLES=false

USER 10001

ENTRYPOINT ["/opt/czertainly/entry.sh"]
