import React from 'react';
import { Container, Typography, Box } from '@mui/material';
import { useSignalR } from '../hooks/useSignalR';
import StatusBar from '../components/StatusBar';
import TelemetryList from '../components/TelemetryList';

interface DashboardProps {
  onLogout: () => void;
}

const Dashboard: React.FC<DashboardProps> = ({ onLogout }) => {
  const { status, telemetry } = useSignalR();

  return (
    <Container maxWidth="xl">
      <Box py={3}>
        <Typography variant="h3" component="h1" gutterBottom>
          Pump Telemetry Dashboard
        </Typography>
        <StatusBar status={status} onLogout={onLogout} />
        <TelemetryList telemetry={telemetry} />
      </Box>
    </Container>
  );
};

export default Dashboard;