const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token && { 'Authorization': `Bearer ${token}` })
  };
};

export const apiService = {
  async simulateTelemetry() {
    const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/realtime/simulate`, {
      method: 'POST',
      headers: getAuthHeaders()
    });
    if (!response.ok) {
      throw new Error('Simulation failed');
    }
    return response;
  }
};