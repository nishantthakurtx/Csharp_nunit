import { jwtDecode } from 'jwt-decode';
import api from './api';

const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';

const getToken = () => localStorage.getItem(TOKEN_KEY);
const setToken = (token) => localStorage.setItem(TOKEN_KEY, token);
const removeToken = () => localStorage.removeItem(TOKEN_KEY);

const getRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);
const setRefreshToken = (token) => localStorage.setItem(REFRESH_TOKEN_KEY, token);
const removeRefreshToken = () => localStorage.removeItem(REFRESH_TOKEN_KEY);

const authService = {
  getToken,
  setToken,
  removeToken,

  async login(credentials) {
    try {
      const response = await api.post('/api/Authentications/login', credentials);
      const { accessToken, refreshToken } = response.data.data;
      
      if (!accessToken || !refreshToken) {
        throw new Error('Token not received');
      }

      setToken(accessToken);
      setRefreshToken(refreshToken);
      return response.data;
    } catch (error) {
      if (error.response?.status === 400) {
        throw new Error('Invalid email or password');
      }
      if (error.response?.status === 404) {
        throw new Error('User not found');
      }
      throw new Error('Login failed. Please try again.');
    }
  },

  async refreshToken() {
    try {
      const refreshToken = getRefreshToken();
      if (!refreshToken) {
        throw new Error('No refresh token found');
      }

      const response = await api.post('/api/Authentications/create-token-by-refresh-token', {
        token: refreshToken
      });

      const { accessToken, newRefreshToken } = response.data.data;
      
      if (!accessToken || !newRefreshToken) {
        throw new Error('Token not received');
      }

      setToken(accessToken);
      setRefreshToken(newRefreshToken);
      return response.data;
    } catch (error) {
      this.logout();
      throw new Error('Session expired. Please login again.');
    }
  },

  async logout() {
    try {
      const refreshToken = getRefreshToken();
      if (refreshToken) {
        await api.post('/api/Authentications/revoke-refresh-token', {
          token: refreshToken
        });
      }
    } catch (error) {
      console.error('Error revoking refresh token:', error);
    } finally {
      removeToken();
      removeRefreshToken();
    }
  },

  isAuthenticated() {
    const token = getToken();
    if (!token) return false;

    try {
      const decoded = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      
      // Token süresi dolmak üzereyse yenile
      if (decoded.exp - currentTime < 300) { // 5 dakika kaldıysa
        this.refreshToken().catch(() => {
          // Yenileme başarısız olursa sessiz kal
        });
      }
      
      return decoded.exp > currentTime;
    } catch {
      return false;
    }
  },

  isInstructor: () => {
    const token = getToken();
    if (!token) return false;
    
    try {
      const decoded = jwtDecode(token);
      return decoded.Roles?.includes('Instructor');
    } catch {
      return false;
    }
  },

  getCurrentUser() {
    const token = getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch {
      return null;
    }
  }
};

export { authService }; 