import React from 'react';
import { Card, CardContent, Typography, Chip, Box } from '@mui/material';

interface TelemetryCardProps {
  data: any;
}

const TelemetryCard: React.FC<TelemetryCardProps> = ({ data }) => {
  return (
    <Card variant="outlined">
      <CardContent>
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
          <Typography variant="h6" component="div">
            {data.deviceId}
          </Typography>
          <Chip 
            label={data.status} 
            color={data.status === 'Warning' ? 'warning' : 'success'}
            size="small"
          />
        </Box>
        <Typography variant="body2" color="text.secondary">
          Temp: {data.temperature?.toFixed(1)}°C | Pressure: {data.pressure?.toFixed(1)} PSI
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Flow: {data.flowRate?.toFixed(1)} L/min | Vibration: {data.vibration?.toFixed(2)}
        </Typography>
        <Typography variant="caption" display="block" sx={{ mt: 1 }}>
          {new Date(data.timestamp).toLocaleTimeString()}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default TelemetryCard;