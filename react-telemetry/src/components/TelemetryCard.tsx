import React from 'react';

interface TelemetryCardProps {
  data: any;
}

const TelemetryCard: React.FC<TelemetryCardProps> = ({ data }) => {
  const cardStyle = {
    border: '1px solid #ccc',
    padding: '10px',
    borderRadius: '5px',
    backgroundColor: data.status === 'Warning' ? '#fff3cd' : '#d4edda'
  };

  return (
    <div style={cardStyle}>
      <strong>{data.deviceId}</strong> - {data.status}
      <div>Temp: {data.temperature?.toFixed(1)}°C | Pressure: {data.pressure?.toFixed(1)} PSI</div>
      <div>Flow: {data.flowRate?.toFixed(1)} L/min | Vibration: {data.vibration?.toFixed(2)}</div>
      <small>{new Date(data.timestamp).toLocaleTimeString()}</small>
    </div>
  );
};

export default TelemetryCard;