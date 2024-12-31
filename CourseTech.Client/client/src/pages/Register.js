import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './Register.css';

const Register = () => {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    agreeToTerms: false
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Şifre kontrolü
    if (formData.password !== formData.confirmPassword) {
      alert('Şifreler eşleşmiyor!');
      return;
    }
    // Kayıt işlemleri burada yapılacak
    console.log('Kayıt yapılıyor:', formData);
  };

  return (
    <div className="register-container">
      <div className="register-box">
        <div className="register-left">
          <div className="welcome-text">
            <h2>Öğrenmeye Başlayın</h2>
            <p>Yeni beceriler kazanın ve kariyerinizi geliştirin.</p>
          </div>
          <div className="social-login">
            <button className="social-btn google">
              <i className="fab fa-google"></i>
              Google ile kayıt ol
            </button>
            <button className="social-btn microsoft">
              <i className="fab fa-microsoft"></i>
              Microsoft hesabı ile kayıt ol
            </button>
          </div>
          <div className="divider">
            <span>veya</span>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="fullName">Ad Soyad</label>
              <input
                type="text"
                id="fullName"
                name="fullName"
                value={formData.fullName}
                onChange={handleChange}
                placeholder="Ad Soyad"
                required
              />
            </div>
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
                minLength="8"
              />
              <small className="password-hint">
                En az 8 karakter uzunluğunda olmalıdır
              </small>
            </div>
            <div className="form-group">
              <label htmlFor="confirmPassword">Şifre Tekrar</label>
              <input
                type="password"
                id="confirmPassword"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                placeholder="••••••••"
                required
                minLength="8"
              />
            </div>
            <div className="form-group checkbox-group">
              <input
                type="checkbox"
                id="agreeToTerms"
                name="agreeToTerms"
                checked={formData.agreeToTerms}
                onChange={handleChange}
                required
              />
              <label htmlFor="agreeToTerms">
                <Link to="/terms" className="terms-link">Kullanım Şartları</Link>'nı ve{' '}
                <Link to="/privacy" className="terms-link">Gizlilik Politikası</Link>'nı kabul ediyorum
              </label>
            </div>
            <button type="submit" className="register-btn" disabled={!formData.agreeToTerms}>
              Kayıt Ol
            </button>
          </form>
        </div>

        <div className="vertical-divider"></div>

        <div className="register-right">
          <div className="welcome-text">
            <h2>Neden CourseTech?</h2>
            <p>Binlerce kurs ve uzman eğitmenle, öğrenme yolculuğunuzda yanınızdayız.</p>
          </div>
          <ul className="features-list">
            <li>
              <i className="fas fa-check-circle"></i>
              <span>Uzman eğitmenler tarafından hazırlanmış kaliteli içerikler</span>
            </li>
            <li>
              <i className="fas fa-check-circle"></i>
              <span>7/24 erişilebilir video dersler</span>
            </li>
            <li>
              <i className="fas fa-check-circle"></i>
              <span>Pratik yapma ve ödev imkanları</span>
            </li>
            <li>
              <i className="fas fa-check-circle"></i>
              <span>Sertifika alma fırsatı</span>
            </li>
            <li>
              <i className="fas fa-check-circle"></i>
              <span>Topluluk desteği ve forum</span>
            </li>
          </ul>
          <img 
            src="https://via.placeholder.com/400x200?text=Learning+Platform" 
            alt="Öğrenme Platformu"
            style={{
              width: '100%',
              height: 'auto',
              borderRadius: '8px',
              marginBottom: '2rem'
            }}
          />
          <div className="login-link">
            Zaten hesabın var mı? <Link to="/login">Giriş Yap</Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register; 