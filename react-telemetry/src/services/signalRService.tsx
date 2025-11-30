import * as signalR from '@microsoft/signalr';

class SignalRService {
  private connection: signalR.HubConnection | null = null;

  createConnection(): signalR.HubConnection {
    const token = localStorage.getItem('token');
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_SIGNALR_HUB_URL || '', {
        accessTokenFactory: () => token || '',
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
    return this.connection;
  }

  async startConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.start();
    }
  }

  onTelemetryUpdate(callback: (...args: any[]) => void): void {
    if (this.connection) {
      this.connection.on('TelemetryUpdate', callback);
      console.log('SignalR: Registered TelemetryUpdate handler');
    }
  }

  getConnectionState(): string {
    return this.connection?.state || 'Disconnected';
  }
}

export default new SignalRService();