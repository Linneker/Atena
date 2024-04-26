#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["site/acme.sistemas.atena.mvc.site/acme.sistemas.atena.mvc.site.csproj", "site/acme.sistemas.atena.mvc.site/"]
RUN dotnet restore "site/acme.sistemas.atena.mvc.site/acme.sistemas.atena.mvc.site.csproj"
COPY . .
WORKDIR "/src/site/acme.sistemas.atena.mvc.site"
RUN dotnet build "acme.sistemas.atena.mvc.site.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "acme.sistemas.atena.mvc.site.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "acme.sistemas.atena.mvc.site.dll"]