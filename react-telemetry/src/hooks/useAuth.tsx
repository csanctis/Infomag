import { useState, useEffect } from 'react';
import { authService } from '../services/authService';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setIsAuthenticated(authService.isAuthenticated());
    setLoading(false);
  }, []);

  const login = async (username: string, password: string): Promise<void> => {
    await authService.login(username, password);
    setIsAuthenticated(true);
  };

  const logout = (): void => {
    authService.logout();
    setIsAuthenticated(false);
  };

  return { isAuthenticated, loading, login, logout };
};