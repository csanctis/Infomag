import React from 'react';
import LoginForm from '../components/LoginForm';

const Login = ({ onLogin }) => {
  return (
    <div style={{ fontFamily: 'Arial' }}>
      <LoginForm onLogin={onLogin} />
    </div>
  );
};

export default Login;