FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install CosmosDB Emulator dependencies
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PumpMaster.Api/PumpMaster.Api.csproj", "PumpMaster.Api/"]
COPY ["IotHub/IotHub.csproj", "IotHub/"]
RUN dotnet restore "PumpMaster.Api/PumpMaster.Api.csproj"
COPY . .
WORKDIR "/src/PumpMaster.Api"
RUN dotnet build "PumpMaster.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PumpMaster.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Install CosmosDB Emulator
RUN wget -q https://aka.ms/cosmosdb-emulator -O /tmp/cosmosdb-emulator.deb \
    && dpkg -i /tmp/cosmosdb-emulator.deb || apt-get install -f -y \
    && rm /tmp/cosmosdb-emulator.deb

COPY --from=publish /app/publish .

# Start CosmosDB Emulator and API
COPY start.sh /app/start.sh
RUN chmod +x /app/start.sh

ENTRYPOINT ["/app/start.sh"]