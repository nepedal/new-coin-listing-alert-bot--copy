#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY src/Directory.Packages.props /
COPY ["src/CryptoCoinsParser.Api/CryptoCoinsParser.Api.csproj", "CryptoCoinsParser.Api/"]
COPY ["src/CryptoCoinsParser.Application/CryptoCoinsParser.Application.csproj", "CryptoCoinsParser.Application/"]
COPY ["src/CryptoCoinsParser.Domain/CryptoCoinsParser.Domain.csproj", "CryptoCoinsParser.Domain/"]
COPY ["src/Shared/Shared.csproj", "Shared/"]
COPY ["src/CryptoCoinsParser.Persistence/CryptoCoinsParser.Persistence.csproj", "CryptoCoinsParser.Persistence/"]

COPY ["src/CryptoCoinsParser.Binance.Scraper/CryptoCoinsParser.Binance.Scraper.csproj", "CryptoCoinsParser.Binance.Scraper/"]
COPY ["src/CryptoCoinsParser.Bybit.Scraper/CryptoCoinsParser.Bybit.Scraper.csproj", "CryptoCoinsParser.Bybit.Scraper/"]

COPY ["src/CryptoCoinsParser.KuCoin.Scraper/CryptoCoinsParser.KuCoin.Scraper.csproj", "CryptoCoinsParser.KuCoin.Scraper/"]
COPY ["src/CryptoCoinsParser.Okx.Scraper/CryptoCoinsParser.Okx.Scraper.csproj", "CryptoCoinsParser.Okx.Scraper/"]
COPY ["src/CryptoCoinsParser.Scrapers/CryptoCoinsParser.Scrapers.csproj", "CryptoCoinsParser.Scrapers/"]
RUN dotnet restore "CryptoCoinsParser.Api/CryptoCoinsParser.Api.csproj"
COPY src/ .
WORKDIR "/src/CryptoCoinsParser.Api"
RUN dotnet build "CryptoCoinsParser.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoCoinsParser.Api.csproj" -c Release -o /app/publish -r debian.11-x64 --no-self-contained

FROM base AS final
RUN apt-get update \
    && apt-get install -y libfontconfig1 fontconfig \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/publish/
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/libSkiaSharp.so /usr/lib/
COPY fonts /usr/share/fonts
RUN fc-cache -fv
ENV ASPNETCORE_URLS http://+:80
ENTRYPOINT ["dotnet", "CryptoCoinsParser.Api.dll"]
