FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AntiEmoteBot/AntiEmoteBot.csproj", "AntiEmoteBot/"]
RUN dotnet restore "AntiEmoteBot/AntiEmoteBot.csproj"
COPY . .
WORKDIR "/src/AntiEmoteBot"
RUN dotnet build "AntiEmoteBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AntiEmoteBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntiEmoteBot.dll"]
