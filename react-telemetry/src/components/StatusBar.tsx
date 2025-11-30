import React from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { PlayArrow, Logout } from '@mui/icons-material';
import { apiService } from '../services/apiService';

interface StatusBarProps {
  status: string;
  onLogout: () => void;
}

const StatusBar: React.FC<StatusBarProps> = ({ status, onLogout }) => {
  const handleSimulate = async () => {
    try {
      await apiService.simulateTelemetry();
    } catch (error) {
      console.error('Simulation failed:', error);
    }
  };

  return (
    <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
      <Box display="flex" alignItems="center" gap={2}>
        <Typography variant="body1">Status:</Typography>
        <Chip 
          label={status} 
          color={status === 'Connected' ? 'success' : 'warning'}
          variant="outlined"
        />
        <Button 
          variant="contained" 
          startIcon={<PlayArrow />}
          onClick={handleSimulate}
        >
          Simulate Data
        </Button>
      </Box>
      <Button 
        variant="outlined" 
        startIcon={<Logout />}
        onClick={onLogout}
      >
        Logout
      </Button>
    </Box>
  );
};

export default StatusBar;