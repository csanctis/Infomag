-- Real-time telemetry processing
SELECT 
    DeviceId,
    Temperature,
    Pressure,
    FlowRate,
    Vibration,
    Status,
    Location.Latitude as Lat,
    Location.Longitude as Lng,
    System.Timestamp() AS EventTime
INTO [realtime-output]
FROM [iothub-input]

-- Alert detection for critical conditions
SELECT 
    DeviceId,
    'Critical Temperature' as AlertType,
    Temperature as Value,
    System.Timestamp() AS AlertTime
INTO [alerts-output]
FROM [iothub-input]
WHERE Temperature > 80

UNION

SELECT 
    DeviceId,
    'Low Pressure' as AlertType,
    Pressure as Value,
    System.Timestamp() AS AlertTime
INTO [alerts-output]
FROM [iothub-input]
WHERE Pressure < 10

-- Store all telemetry in Cosmos DB
SELECT *
INTO [cosmosdb-output]
FROM [iothub-input]

-- Aggregated analytics (5-minute windows)
SELECT 
    DeviceId,
    AVG(Temperature) as AvgTemperature,
    AVG(Pressure) as AvgPressure,
    AVG(FlowRate) as AvgFlowRate,
    MAX(Vibration) as MaxVibration,
    COUNT(*) as MessageCount,
    System.Timestamp() AS WindowEnd
INTO [analytics-output]
FROM [iothub-input]
GROUP BY DeviceId, TumblingWindow(minute, 5)