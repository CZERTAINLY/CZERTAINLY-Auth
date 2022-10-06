#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY ["src/Czertainly.Auth/Czertainly.Auth.csproj", "Czertainly.Auth/"]
RUN dotnet restore "Czertainly.Auth/Czertainly.Auth.csproj"
COPY . .
WORKDIR "/src/Czertainly.Auth"
RUN dotnet build "Czertainly.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Czertainly.Auth.csproj" -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish /opt/czertainly
COPY ./docker /opt/czertainly
WORKDIR /opt/czertainly
RUN find . -name "*.sh" -exec chmod +x {} \;
ENTRYPOINT ["/opt/czertainly/entry.sh"]