import * as signalR from '@microsoft/signalr';

class SignalRService {
  constructor() {
    this.connection = null;
  }

  createConnection() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/telemetryHub')
      .build();
    return this.connection;
  }

  async startConnection() {
    if (this.connection) {
      await this.connection.start();
    }
  }

  onTelemetryUpdate(callback) {
    if (this.connection) {
      this.connection.on('TelemetryUpdate', callback);
    }
  }

  getConnectionState() {
    return this.connection?.state || 'Disconnected';
  }
}

export default new SignalRService();