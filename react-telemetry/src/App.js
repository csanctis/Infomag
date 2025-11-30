import React from 'react';
import { useAuth } from './hooks/useAuth';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';

function App() {
  const { isAuthenticated, loading, login, logout } = useAuth();

  if (loading) {
    return <div style={{ padding: '20px' }}>Loading...</div>;
  }

  return isAuthenticated ? 
    <Dashboard onLogout={logout} /> : 
    <Login onLogin={login} />;
}

export default App;