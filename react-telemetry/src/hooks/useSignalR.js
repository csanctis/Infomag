import { useState, useEffect } from 'react';
import signalRService from '../services/signalRService';

export const useSignalR = () => {
  const [status, setStatus] = useState('Disconnected');
  const [telemetry, setTelemetry] = useState([]);

  useEffect(() => {
    const connection = signalRService.createConnection();

    // Register event handler before starting connection
    signalRService.onTelemetryUpdate((data) => {
      console.log('Received telemetry:', data);
      setTelemetry(prev => [data, ...prev.slice(0, 14)]);
    });

    signalRService.startConnection()
      .then(() => {
        setStatus('Connected');
        console.log('SignalR connected successfully');
      })
      .catch((error) => {
        console.error('SignalR connection failed:', error);
        setStatus('Connection failed');
      });

    return () => {
      connection?.stop();
    };
  }, []);

  return { status, telemetry };
};