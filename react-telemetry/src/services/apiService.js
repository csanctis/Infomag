const API_BASE_URL = 'http://localhost:5000/api';

export const apiService = {
  async simulateTelemetry() {
    const response = await fetch(`${API_BASE_URL}/realtime/simulate`, {
      method: 'POST'
    });
    if (!response.ok) {
      throw new Error('Simulation failed');
    }
    return response;
  }
};