import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './Login.css';

const Login = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Giriş işlemleri burada yapılacak
    console.log('Giriş yapılıyor:', formData);
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <div className="login-left">
          <div className="welcome-text">
            <h2>Tekrar Hoş Geldiniz!</h2>
            <p>Binlerce kursa erişim için hemen giriş yapın.</p>
          </div>
          <div className="social-login">
            <button className="social-btn google">
              <i className="fab fa-google"></i>
              Google ile devam et
            </button>
            <button className="social-btn microsoft">
              <i className="fab fa-microsoft"></i>
              Microsoft hesabı ile devam et
            </button>
          </div>
          <div className="divider">
            <span>veya</span>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="email">E-posta</label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="ornek@email.com"
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="password">Şifre</label>
              <input
                type="password"
                id="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                placeholder="••••••••"
                required
              />
            </div>
            <div className="form-footer">
              <div className="remember-me">
                <input type="checkbox" id="remember" />
                <label htmlFor="remember">Beni hatırla</label>
              </div>
              <Link to="/forgot-password" className="forgot-password">
                Şifremi unuttum
              </Link>
            </div>
            <button type="submit" className="login-btn">
              Giriş Yap
            </button>
          </form>
        </div>

        <div className="vertical-divider"></div>

        <div className="login-right">
          <div className="welcome-text">
            <h2>Henüz Üye Değil misiniz?</h2>
            <p>Hemen üye olun ve yüzlerce kursa anında erişim sağlayın.</p>
          </div>
          <img 
            src="https://via.placeholder.com/400x300?text=Online+Education" 
            alt="Online Eğitim"
            style={{
              width: '100%',
              height: 'auto',
              borderRadius: '8px',
              marginBottom: '2rem'
            }}
          />
          <div className="register-link">
            <Link to="/register" className="register-btn">
              Hemen Üye Ol
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login; 