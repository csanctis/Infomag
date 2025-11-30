import React from 'react';
import { useSignalR } from '../hooks/useSignalR';
import StatusBar from '../components/StatusBar';
import TelemetryList from '../components/TelemetryList';

const Dashboard = () => {
  const { status, telemetry } = useSignalR();

  return (
    <div style={{ padding: '20px', fontFamily: 'Arial' }}>
      <h1>Pump Telemetry Dashboard</h1>
      <StatusBar status={status} />
      <TelemetryList telemetry={telemetry} />
    </div>
  );
};

export default Dashboard;