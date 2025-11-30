import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

// Clear any existing authentication data on app launch
localStorage.clear();

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(<App />);