# Pump Telemetry System

Real-time IoT pump monitoring system with SignalR, CosmosDB, and React dashboard.

## Prerequisites

- .NET 8.0 SDK
- Node.js 18+
- Docker (optional)

## Running the Application

### 1. .NET API (PumpMaster.Api)

```bash
cd IotHub/PumpMaster.Api
dotnet restore
dotnet run
```

API will be available at: `https://localhost:7180`

### 2. IoT Hub Library

```bash
cd IotHub/IotHub
dotnet build
```

### 3. Event Hub Producer

```bash
cd IotHub/EventHubProducer
dotnet build
```

### 4. React Website

```bash
cd react-telemetry
npm install
npm start
```

Website will be available at: `http://localhost:3000`

**Default Login:**
- Username: `admin`
- Password: `password123`

### 5. Docker with CosmosDB (Alternative)

[CosmosDb Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/emulator)


```bash
cd IotHub
docker-compose up --build
```

This will start both the API and CosmosDB emulator. API available at: `http://localhost:8080`

## Architecture

- **PumpMaster.Api** - REST API with SignalR hub
- **IotHub** - Core telemetry models and services
- **EventHubProducer** - Event Hub integration
- **react-telemetry** - Real-time dashboard

## Features

- Real-time telemetry streaming via SignalR
- JWT authentication
- CosmosDB integration
- Event Hub publishing
- Responsive React dashboard