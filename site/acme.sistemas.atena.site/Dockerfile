#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["acme.sistemas.atena.site/acme.sistemas.atena.site.csproj", "acme.sistemas.atena.site/"]
RUN dotnet restore "acme.sistemas.atena.site/acme.sistemas.atena.site.csproj"
COPY . .
WORKDIR "/src/acme.sistemas.atena.site"
RUN dotnet build "acme.sistemas.atena.site.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "acme.sistemas.atena.site.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "acme.sistemas.atena.site.dll"]