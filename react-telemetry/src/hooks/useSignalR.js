import { useState, useEffect } from 'react';
import signalRService from '../services/signalRService';

export const useSignalR = () => {
  const [status, setStatus] = useState('Disconnected');
  const [telemetry, setTelemetry] = useState([]);

  useEffect(() => {
    const connection = signalRService.createConnection();

    signalRService.startConnection()
      .then(() => {
        setStatus('Connected');
        signalRService.onTelemetryUpdate((data) => {
          setTelemetry(prev => [data, ...prev.slice(0, 9)]);
        });
      })
      .catch(() => setStatus('Connection failed'));

    return () => {
      connection?.stop();
    };
  }, []);

  return { status, telemetry };
};