import React, { useState, useRef, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { FiMenu, FiX, FiShoppingCart, FiLogOut, FiSettings, FiClock, FiCreditCard, FiTrash2 } from 'react-icons/fi';
import { deleteUser } from '../services/userService';
import { toast } from 'react-toastify';
import styles from '../styles/Navbar.module.css';

const ProfileDropdown = ({ user, handleLogout }) => {
  return (
    <div className={styles.profileDropdown}>
      <div className={styles.profileInfo}>
        <span className={styles.userName}>{user.fullName}</span>
        <span className={styles.userEmail}>{user.email}</span>
      </div>
      <div className={styles.dropdownDivider} />
      <Link to="/settings/profile" className={styles.dropdownItem}>
        <FiSettings className={styles.dropdownIcon} />
        Settings
      </Link>
      <Link to="/settings/orders" className={styles.dropdownItem}>
        <FiClock className={styles.dropdownIcon} />
        Order History
      </Link>
      <Link to="/settings/payments" className={styles.dropdownItem}>
        <FiCreditCard className={styles.dropdownIcon} />
        Payment History
      </Link>
      <div className={styles.dropdownDivider} />
      <button onClick={handleLogout} className={styles.dropdownItem}>
        <FiLogOut className={styles.dropdownIcon} />
        Logout
      </button>
    </div>
  );
};

const Navbar = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const profileRef = useRef(null);

  const getInitials = (name) => {
    if (!name) return '?';
    return name
      .split(' ')
      .map(word => word[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (profileRef.current && !profileRef.current.contains(event.target)) {
        setIsProfileOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleLogout = async () => {
    try {
      await logout();
      navigate('/');
    } catch (error) {
      console.error('Error logging out:', error);
    }
  };

  const handleNavigation = (path) => {
    navigate(path);
    setIsProfileOpen(false);
    setIsMenuOpen(false);
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const toggleProfile = () => {
    setIsProfileOpen(!isProfileOpen);
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.navContainer}>
        <Link to="/" className={styles.logo}>
          CourseTech
        </Link>

        <button className={styles.menuButton} onClick={toggleMenu}>
          {isMenuOpen ? <FiX /> : <FiMenu />}
        </button>

        <div className={`${styles.navLinks} ${isMenuOpen ? styles.active : ''}`}>
          <Link to="/" className={styles.navLink} onClick={() => setIsMenuOpen(false)}>
            Home
          </Link>
          <Link to="/courses" className={styles.navLink} onClick={() => setIsMenuOpen(false)}>
            Courses
          </Link>
          
          {user ? (
            <>
              <Link to="/basket" className={styles.navLink} onClick={() => setIsMenuOpen(false)}>
                <FiShoppingCart />
                <span className={styles.navText}></span>
              </Link>
              <div className={styles.profileWrapper} ref={profileRef}>
                <button onClick={toggleProfile} className={styles.profileButton}>
                  <div className={styles.initialsCircle}>
                    {getInitials(user.fullName)}
                  </div>
                </button>
                {isProfileOpen && (
                  <ProfileDropdown user={user} handleLogout={handleLogout} />
                )}
              </div>
            </>
          ) : (
            <div className={styles.authButtons}>
              <Link to="/login" className={styles.loginButton} onClick={() => setIsMenuOpen(false)}>
                Sign In
              </Link>
              <Link to="/register" className={styles.registerButton} onClick={() => setIsMenuOpen(false)}>
                Sign Up
              </Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar; 