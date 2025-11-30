import React from 'react';

interface PumpModalProps {
  pump: any;
  onClose: () => void;
}

const PumpModal: React.FC<PumpModalProps> = ({ pump, onClose }) => {
  if (!pump) return null;

  const modalStyle = {
    position: 'fixed' as const,
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    backgroundColor: 'rgba(0,0,0,0.5)',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 1000
  };

  const contentStyle = {
    backgroundColor: 'white',
    padding: '20px',
    borderRadius: '8px',
    width: '600px',
    maxHeight: '80vh',
    overflow: 'auto'
  };

  const mapUrl = `https://www.openstreetmap.org/export/embed.html?bbox=${pump.location.longitude-0.01},${pump.location.latitude-0.01},${pump.location.longitude+0.01},${pump.location.latitude+0.01}&layer=mapnik&marker=${pump.location.latitude},${pump.location.longitude}`;

  return (
    <div style={modalStyle} onClick={onClose}>
      <div style={contentStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
          <h2>Pump Details: {pump.deviceId}</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: '24px', cursor: 'pointer' }}>×</button>
        </div>
        
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
          <div>
            <h3>Telemetry Data</h3>
            <p><strong>Status:</strong> {pump.status}</p>
            <p><strong>Temperature:</strong> {pump.temperature?.toFixed(1)}°C</p>
            <p><strong>Pressure:</strong> {pump.pressure?.toFixed(1)} PSI</p>
            <p><strong>Flow Rate:</strong> {pump.flowRate?.toFixed(1)} L/min</p>
            <p><strong>Vibration:</strong> {pump.vibration?.toFixed(2)}</p>
            <p><strong>Timestamp:</strong> {new Date(pump.timestamp).toLocaleString()}</p>
            <p><strong>Location:</strong> {pump.location.latitude.toFixed(4)}, {pump.location.longitude.toFixed(4)}</p>
          </div>
          
          <div>
            <h3>Location Map</h3>
            <iframe
              src={mapUrl}
              width="100%"
              height="200"
              style={{ border: '1px solid #ccc', borderRadius: '4px' }}
              title="Pump Location"
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default PumpModal;