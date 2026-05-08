# syntax=docker/dockerfile:1.6
# Build context esperado: raiz do repositório.
# Uso: docker build -t atena-api -f src/Api/Acme.Sistemas.Atena.Api/Dockerfile .

ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY Atena.sln ./
COPY src/ ./src/

RUN dotnet restore src/Api/Acme.Sistemas.Atena.Api/Acme.Sistemas.Atena.Api.csproj
RUN dotnet publish src/Api/Acme.Sistemas.Atena.Api/Acme.Sistemas.Atena.Api.csproj \
      -c Release \
      -o /app/publish \
      --no-restore \
      /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

ENTRYPOINT ["dotnet", "Acme.Sistemas.Atena.Api.dll"]
