import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const RedirectRoute = ({ children }) => {
  const { user } = useAuth();

  // Kullanıcı oturum açmışsa giriş yapmasına gerek yok, başka bir sayfaya yönlendir
  if (user && user.isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  // Kullanıcı giriş yapmamışsa mevcut bileşeni göster
  return children;
};

export default RedirectRoute; 