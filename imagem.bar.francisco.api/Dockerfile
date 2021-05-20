#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["imagem.bar.francisco.api/imagem.bar.francisco.api.csproj", "imagem.bar.francisco.api/"]
RUN dotnet restore "imagem.bar.francisco.api/imagem.bar.francisco.api.csproj"
COPY . .
WORKDIR "/src/imagem.bar.francisco.api"
RUN dotnet build "imagem.bar.francisco.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "imagem.bar.francisco.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "imagem.bar.francisco.api.dll"]