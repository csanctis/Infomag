export const apiService = {
  async simulateTelemetry() {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/realtime/simulate`, {
      method: 'POST'
    });
    if (!response.ok) {
      throw new Error('Simulation failed');
    }
    return response;
  }
};