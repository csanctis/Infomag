#!/bin/bash

# Start CosmosDB Emulator in background
/opt/microsoft/cosmosdb-emulator/CosmosDB.Emulator &

# Wait for CosmosDB to be ready
echo "Waiting for CosmosDB Emulator to start..."
sleep 30

# Start the API
dotnet PumpMaster.Api.dll