import React from 'react';
import { Container, Box } from '@mui/material';
import LoginForm from '../components/LoginForm';

interface LoginProps {
  onLogin: (username: string, password: string) => Promise<void>;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  return (
    <Container maxWidth="sm">
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
        <LoginForm onLogin={onLogin} />
      </Box>
    </Container>
  );
};

export default Login;