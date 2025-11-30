import React from 'react';
import { Dialog, DialogTitle, DialogContent, Grid, Typography, IconButton, Box, Chip } from '@mui/material';
import { Close } from '@mui/icons-material';

interface PumpModalProps {
  pump: any;
  onClose: () => void;
}

const PumpModal: React.FC<PumpModalProps> = ({ pump, onClose }) => {
  if (!pump) return null;

  const mapUrl = `https://www.openstreetmap.org/export/embed.html?bbox=${pump.location.longitude-0.01},${pump.location.latitude-0.01},${pump.location.longitude+0.01},${pump.location.latitude+0.01}&layer=mapnik&marker=${pump.location.latitude},${pump.location.longitude}`;

  return (
    <Dialog open={!!pump} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography variant="h5">Pump Details: {pump.deviceId}</Typography>
          <IconButton onClick={onClose}>
            <Close />
          </IconButton>
        </Box>
      </DialogTitle>
      <DialogContent>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Typography variant="h6" gutterBottom>Telemetry Data</Typography>
            <Box display="flex" flexDirection="column" gap={1}>
              <Box display="flex" alignItems="center" gap={1}>
                <Typography variant="body2" fontWeight="bold">Status:</Typography>
                <Chip label={pump.status} color={pump.status === 'Warning' ? 'warning' : 'success'} size="small" />
              </Box>
              <Typography variant="body2"><strong>Temperature:</strong> {pump.temperature?.toFixed(1)}°C</Typography>
              <Typography variant="body2"><strong>Pressure:</strong> {pump.pressure?.toFixed(1)} PSI</Typography>
              <Typography variant="body2"><strong>Flow Rate:</strong> {pump.flowRate?.toFixed(1)} L/min</Typography>
              <Typography variant="body2"><strong>Vibration:</strong> {pump.vibration?.toFixed(2)}</Typography>
              <Typography variant="body2"><strong>Timestamp:</strong> {new Date(pump.timestamp).toLocaleString()}</Typography>
              <Typography variant="body2"><strong>Location:</strong> {pump.location.latitude.toFixed(4)}, {pump.location.longitude.toFixed(4)}</Typography>
            </Box>
          </Grid>
          <Grid item xs={12} md={6}>
            <Typography variant="h6" gutterBottom>Location Map</Typography>
            <Box
              component="iframe"
              src={mapUrl}
              width="100%"
              height={200}
              sx={{ border: 1, borderColor: 'grey.300', borderRadius: 1 }}
              title="Pump Location"
            />
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
};

export default PumpModal;