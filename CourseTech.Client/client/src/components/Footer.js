import React from 'react';
import { Link } from 'react-router-dom';
import './Footer.css';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="footer-section">
          <h3>CourseTech</h3>
          <p>Yeni nesil online eğitim platformu</p>
          <div className="social-links">
            <a href="#" aria-label="Facebook"><i className="fab fa-facebook-f"></i></a>
            <a href="#" aria-label="Twitter"><i className="fab fa-twitter"></i></a>
            <a href="#" aria-label="Instagram"><i className="fab fa-instagram"></i></a>
            <a href="#" aria-label="LinkedIn"><i className="fab fa-linkedin-in"></i></a>
          </div>
        </div>

        <div className="footer-section">
          <h4>Kategoriler</h4>
          <ul>
            <li><Link to="/category/development">Yazılım Geliştirme</Link></li>
            <li><Link to="/category/business">İş</Link></li>
            <li><Link to="/category/finance">Finans</Link></li>
            <li><Link to="/category/design">Tasarım</Link></li>
            <li><Link to="/category/marketing">Pazarlama</Link></li>
          </ul>
        </div>

        <div className="footer-section">
          <h4>Hızlı Linkler</h4>
          <ul>
            <li><Link to="/about">Hakkımızda</Link></li>
            <li><Link to="/contact">İletişim</Link></li>
            <li><Link to="/faq">SSS</Link></li>
            <li><Link to="/privacy">Gizlilik Politikası</Link></li>
            <li><Link to="/terms">Kullanım Şartları</Link></li>
          </ul>
        </div>

        <div className="footer-section">
          <h4>İletişim</h4>
          <ul className="contact-info">
            <li><i className="fas fa-map-marker-alt"></i> İstanbul, Türkiye</li>
            <li><i className="fas fa-phone"></i> +90 (212) 123 45 67</li>
            <li><i className="fas fa-envelope"></i> info@coursetech.com</li>
          </ul>
        </div>
      </div>
      
      <div className="footer-bottom">
        <p>&copy; {new Date().getFullYear()} CourseTech. Tüm hakları saklıdır.</p>
      </div>
    </footer>
  );
};

export default Footer; 