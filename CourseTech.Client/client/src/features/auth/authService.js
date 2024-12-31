import api from '../../services/api';

const API_URL = '/api/auth';

// Kayıt ol
const register = async (userData) => {
  const response = await api.post(`${API_URL}/register`, userData);
  if (response.data) {
    localStorage.setItem('user', JSON.stringify(response.data));
  }
  return response.data;
};

// Giriş yap
const login = async (userData) => {
  const response = await api.post(`${API_URL}/login`, userData);
  if (response.data) {
    localStorage.setItem('user', JSON.stringify(response.data));
  }
  return response.data;
};

// Çıkış yap
const logout = async () => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (user?.refreshToken) {
    try {
      await api.post(`${API_URL}/logout`, {
        refreshToken: user.refreshToken
      });
    } catch (error) {
      console.error('Logout error:', error);
    }
  }
  localStorage.removeItem('user');
};

// Token yenile
const refreshToken = async () => {
  const user = JSON.parse(localStorage.getItem('user'));
  const response = await api.post(`${API_URL}/refresh-token`, {
    refreshToken: user.refreshToken
  });
  
  if (response.data) {
    user.accessToken = response.data.accessToken;
    user.refreshToken = response.data.refreshToken;
    localStorage.setItem('user', JSON.stringify(user));
  }
  
  return user;
};

const authService = {
  register,
  login,
  logout,
  refreshToken
};

export default authService; 