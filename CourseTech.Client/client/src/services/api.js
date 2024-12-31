import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;

// Axios instance oluştur
const api = axios.create({
  baseURL: API_URL,
  timeout: 5000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Request interceptor
api.interceptors.request.use(
  (config) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (user?.accessToken) {
      config.headers.Authorization = `Bearer ${user.accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const user = JSON.parse(localStorage.getItem('user'));
        const response = await axios.post(API_URL + '/api/auth/refresh-token', {
          refreshToken: user.refreshToken
        });

        const { accessToken, refreshToken } = response.data;
        
        // Local storage güncelle
        user.accessToken = accessToken;
        user.refreshToken = refreshToken;
        localStorage.setItem('user', JSON.stringify(user));

        // Yeni token ile orijinal isteği tekrarla
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return api(originalRequest);
      } catch (error) {
        // Token yenileme başarısız - kullanıcıyı çıkış yaptır
        localStorage.removeItem('user');
        window.location.href = '/login';
        return Promise.reject(error);
      }
    }

    return Promise.reject(error);
  }
);

export default api; 