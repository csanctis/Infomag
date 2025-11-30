import { useState, useEffect } from 'react';
import signalRService from '../services/signalRService';
import { apiService } from '../services/apiService';

export const useSignalR = () => {
  const [status, setStatus] = useState<string>('Disconnected');
  const [telemetry, setTelemetry] = useState<any[]>([]);

  useEffect(() => {
    const loadInitialData = async () => {
      try {
        const existingData = await apiService.getTelemetryData();
        setTelemetry(existingData.slice(0, 15));
      } catch (error) {
        console.error('Failed to load initial telemetry data:', error);
      }
    };

    const connection = signalRService.createConnection();

    // Register event handler before starting connection
    signalRService.onTelemetryUpdate((data: any) => {
      console.log('Received telemetry:', data);
      setTelemetry(prev => [data, ...prev.slice(0, 14)]);
    });

    signalRService.startConnection()
      .then(() => {
        setStatus('Connected');
        console.log('SignalR connected successfully');
        loadInitialData();
      })
      .catch((error: any) => {
        console.error('SignalR connection failed:', error);
        setStatus('Connection failed');
      });

    return () => {
      connection?.stop();
    };
  }, []);

  return { status, telemetry };
};