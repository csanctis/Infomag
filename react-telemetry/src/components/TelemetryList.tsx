import React, { useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Chip } from '@mui/material';
import PumpModal from './PumpModal';

interface TelemetryListProps {
  telemetry: any[];
}

const TelemetryList: React.FC<TelemetryListProps> = ({ telemetry }) => {
  const [selectedPump, setSelectedPump] = useState<any>(null);

  return (
    <>
      <TableContainer component={Paper} sx={{ maxHeight: 500 }}>
        <Table stickyHeader>
          <TableHead>
            <TableRow>
              <TableCell>Device ID</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Temperature (°C)</TableCell>
              <TableCell>Pressure (PSI)</TableCell>
              <TableCell>Flow Rate (L/min)</TableCell>
              <TableCell>Vibration</TableCell>
              <TableCell>Timestamp</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {telemetry.map((data, index) => (
              <TableRow 
                key={index} 
                hover 
                sx={{ cursor: 'pointer' }}
                onClick={() => setSelectedPump(data)}
              >
                <TableCell>{data.deviceId}</TableCell>
                <TableCell>
                  <Chip 
                    label={data.status} 
                    color={data.status === 'Warning' ? 'warning' : 'success'}
                    size="small"
                  />
                </TableCell>
                <TableCell>{data.temperature?.toFixed(1)}</TableCell>
                <TableCell>{data.pressure?.toFixed(1)}</TableCell>
                <TableCell>{data.flowRate?.toFixed(1)}</TableCell>
                <TableCell>{data.vibration?.toFixed(2)}</TableCell>
                <TableCell>{new Date(data.timestamp).toLocaleTimeString()}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <PumpModal pump={selectedPump} onClose={() => setSelectedPump(null)} />
    </>
  );
};

export default TelemetryList;