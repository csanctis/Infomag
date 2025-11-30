import React, { useState } from 'react';
import PumpModal from './PumpModal';

interface TelemetryListProps {
  telemetry: any[];
}

const TelemetryList: React.FC<TelemetryListProps> = ({ telemetry }) => {
  const [selectedPump, setSelectedPump] = useState<any>(null);
  const containerStyle = {
    maxHeight: '500px',
    overflowY: 'auto' as const,
    border: '1px solid #ccc',
  };

  return (
    <>
    <div style={containerStyle}>
      <table style={{ width: '100%', borderCollapse: 'collapse', display: 'table' }}>
      <thead style={{ display: 'table-header-group', position: 'sticky', top: 0, backgroundColor: '#f5f5f5' }}>
        <tr>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Device ID</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Status</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Temperature (°C)</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Pressure (PSI)</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Flow Rate (L/min)</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Vibration</th>
          <th style={{ border: '1px solid #ccc', padding: '8px' }}>Timestamp</th>
        </tr>
      </thead>
      <tbody style={{ display: 'table-row-group' }}>
        {telemetry.map((data, index) => (
          <tr key={index} style={{ backgroundColor: data.status === 'Warning' ? '#fff3cd' : '#d4edda', cursor: 'pointer' }} onClick={() => setSelectedPump(data)}>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.deviceId}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.status}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.temperature?.toFixed(1)}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.pressure?.toFixed(1)}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.flowRate?.toFixed(1)}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{data.vibration?.toFixed(2)}</td>
            <td style={{ border: '1px solid #ccc', padding: '8px' }}>{new Date(data.timestamp).toLocaleTimeString()}</td>
          </tr>
        ))}
      </tbody>
    </table>
    </div>
    <PumpModal pump={selectedPump} onClose={() => setSelectedPump(null)} />
    </>
  );
};

export default TelemetryList;