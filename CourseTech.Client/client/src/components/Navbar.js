import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useTheme } from '../context/ThemeContext';
import './Navbar.css';

const Navbar = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const { isDarkMode, toggleTheme } = useTheme();

  const handleSearch = (e) => {
    e.preventDefault();
    // Arama işlemi burada yapılacak
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-left">
          <Link to="/" className="navbar-logo">
            CourseTech
          </Link>
          <div className="navbar-categories">
            <button className="categories-btn">
              Kategoriler
              <i className="fas fa-chevron-down"></i>
            </button>
          </div>
        </div>

        <div className="navbar-search">
          <form onSubmit={handleSearch}>
            <div className="search-box">
              <i className="fas fa-search search-icon"></i>
              <input
                type="text"
                placeholder="Kurs ara..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </form>
        </div>

        <div className="navbar-right">
          <button className="theme-toggle" onClick={toggleTheme} aria-label="Tema Değiştir">
            {isDarkMode ? (
              <i className="fas fa-sun"></i>
            ) : (
              <i className="fas fa-moon"></i>
            )}
          </button>
          {isLoggedIn ? (
            <>
              <Link to="/my-courses" className="nav-link">
                <i className="fas fa-graduation-cap"></i>
                <span>Kurslarım</span>
              </Link>
              <Link to="/cart" className="nav-link">
                <i className="fas fa-shopping-cart"></i>
                <span>Sepet</span>
              </Link>
              <div className="nav-profile">
                <button className="profile-btn">
                  <img src="https://via.placeholder.com/32" alt="Profile" className="profile-img" />
                </button>
              </div>
            </>
          ) : (
            <>
              <Link to="/cart" className="nav-link">
                <i className="fas fa-shopping-cart"></i>
              </Link>
              <Link to="/login" className="nav-link">Giriş Yap</Link>
              <Link to="/register" className="nav-link register-btn">Kayıt Ol</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar; 