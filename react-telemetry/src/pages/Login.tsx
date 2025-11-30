import React from 'react';
import LoginForm from '../components/LoginForm';

interface LoginProps {
  onLogin: (username: string, password: string) => Promise<void>;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  return (
    <div style={{ fontFamily: 'Arial' }}>
      <LoginForm onLogin={onLogin} />
    </div>
  );
};

export default Login;