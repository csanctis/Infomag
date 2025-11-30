const getAuthHeaders = (): Record<string, string> => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token && { 'Authorization': `Bearer ${token}` })
  };
};

export const apiService = {
  async simulateTelemetry(): Promise<Response> {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/realtime/simulate`, {
      method: 'POST',
      headers: getAuthHeaders()
    });
    if (!response.ok) {
      throw new Error('Simulation failed');
    }
    return response;
  },

  async getTelemetryData(): Promise<any[]> {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/telemetry`, {
      headers: getAuthHeaders()
    });
    if (!response.ok) {
      throw new Error('Failed to fetch telemetry data');
    }
    return response.json();
  }
};