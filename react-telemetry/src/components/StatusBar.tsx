import React from 'react';
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
    <div style={{ marginBottom: '20px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
      <div>
        <span>Status: {status}</span>
        <button onClick={handleSimulate} style={{ marginLeft: '20px' }}>
          Simulate Data
        </button>
      </div>
      <button onClick={onLogout} style={{ padding: '5px 10px' }}>
        Logout
      </button>
    </div>
  );
};

export default StatusBar;