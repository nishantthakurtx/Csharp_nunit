import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const RedirectRoute = ({ children }) => {
  const { user } = useAuth();

  if (user && user.isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default RedirectRoute; 