FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/AntiStickerBot
COPY ["AntiStickerBot/AntiStickerBot.csproj", "./"]
RUN dotnet restore "AntiStickerBot.csproj"
COPY ["AntiStickerBot/", "./"]
RUN dotnet build "AntiStickerBot.csproj" -c Release -o /app/bot/build

FROM build AS test
WORKDIR /src/Tests
COPY ["Tests/Tests.csproj", "./"]
RUN dotnet restore "Tests.csproj"
COPY ["Tests/", "./"]
RUN dotnet build "Tests.csproj" -c Release -o /app/test/build
RUN dotnet test

FROM build AS publish
RUN dotnet publish "AntiStickerBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntiStickerBot.dll"]
