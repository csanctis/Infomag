import React from 'react';
import TelemetryCard from './TelemetryCard';

const TelemetryList = ({ telemetry }) => {
  return (
    <div style={{ display: 'grid', gap: '10px' }}>
      {telemetry.map((data, index) => (
        <TelemetryCard key={index} data={data} />
      ))}
    </div>
  );
};

export default TelemetryList;