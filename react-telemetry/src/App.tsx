import React from 'react';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { useAuth } from './hooks/useAuth';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';

const theme = createTheme();

function App() {
  const { isAuthenticated, loading, login, logout } = useAuth();

  if (loading) {
    return <div style={{ padding: '20px' }}>Loading...</div>;
  }

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      {isAuthenticated ? 
        <Dashboard onLogout={logout} /> : 
        <Login onLogin={login} />
      }
    </ThemeProvider>
  );
}

export default App;