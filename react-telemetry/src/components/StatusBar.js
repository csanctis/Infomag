import React from 'react';
import { apiService } from '../services/apiService';

const StatusBar = ({ status }) => {
  const handleSimulate = async () => {
    try {
      await apiService.simulateTelemetry();
    } catch (error) {
      console.error('Simulation failed:', error);
    }
  };

  return (
    <div style={{ marginBottom: '20px' }}>
      <span>Status: {status}</span>
      <button onClick={handleSimulate} style={{ marginLeft: '20px' }}>
        Simulate Data
      </button>
    </div>
  );
};

export default StatusBar;