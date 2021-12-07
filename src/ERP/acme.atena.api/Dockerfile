#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["acme.atena.api/acme.atena.api.csproj", "acme.atena.api/"]
RUN dotnet restore "acme.atena.api/acme.atena.api.csproj"
COPY . .
WORKDIR "/src/acme.atena.api"
RUN dotnet build "acme.atena.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "acme.atena.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "acme.atena.api.dll"]