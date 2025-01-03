import React, { useState, useRef, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FaSearch, FaShoppingCart, FaUser, FaBook, FaCog, FaSignOutAlt, FaPlus, FaChalkboardTeacher } from 'react-icons/fa';
import { useAuth } from '../contexts/AuthContext';
import { basketService } from '../services/basketService';
import { categoryService } from '../services/categoryService';
import './Navbar.css';

const Navbar = () => {
  const navigate = useNavigate();
  const { user, isAuthenticated, logout } = useAuth();
  const [showDropdown, setShowDropdown] = useState(false);
  const [showExplore, setShowExplore] = useState(false);
  const [categories, setCategories] = useState([]);
  const [basketItemCount, setBasketItemCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const dropdownRef = useRef(null);
  const exploreRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
      if (exploreRef.current && !exploreRef.current.contains(event.target)) {
        setShowExplore(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await categoryService.getAll();
        setCategories(response.data);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };

    fetchCategories();
  }, []);

  useEffect(() => {
    const fetchBasketCount = async () => {
      try {
        const basket = await basketService.getActiveBasket();
        setBasketItemCount(basket?.items?.length || 0);
      } catch (error) {
        console.error('Error fetching basket:', error);
      }
    };

    if (isAuthenticated) {
      fetchBasketCount();
    }
  }, [isAuthenticated]);

  const handleSearch = (e) => {
    e.preventDefault();
    if (searchTerm.trim()) {
      navigate(`/courses?search=${encodeURIComponent(searchTerm.trim())}`);
    }
  };

  const getInitials = () => {
    if (!user?.given_name) return 'U';
    const names = user.given_name.split(' ');
    if (names.length >= 2) {
      return `${names[0][0]}${names[1][0]}`.toUpperCase();
    }
    return names[0][0].toUpperCase();
  };

  const isInstructor = user?.roles === "Instructor";

  return (
    <nav className="navbar">
      <div className="navbar-left">
        <Link to="/" className="logo">
          CourseTech
        </Link>
        <div className="explore-menu" ref={exploreRef}>
          <button 
            className="explore-btn"
            onClick={() => setShowExplore(!showExplore)}
          >
            Explore
          </button>
          {showExplore && (
            <div className="explore-dropdown">
              {categories.map(category => (
                <Link
                  key={category.id}
                  to={`/category/${category.id}`}
                  className="explore-item"
                  onClick={() => setShowExplore(false)}
                >
                  {category.name}
                </Link>
              ))}
            </div>
          )}
        </div>
      </div>

      <div className="navbar-center">
        <form onSubmit={handleSearch} className="search-form">
          <input
            type="text"
            placeholder="Search for courses..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <button type="submit">
            <FaSearch />
          </button>
        </form>
      </div>

      <div className="navbar-right">
        {isAuthenticated ? (
          <>
            {isInstructor && (
              <Link to="/courses/create" className="add-course-btn">
                <FaPlus /> Add Course
              </Link>
            )}
            <Link to="/my-courses" className="nav-link">
              <FaBook /> My Courses
            </Link>
            <Link to="/basket" className="basket-icon">
              <FaShoppingCart />
              {basketItemCount > 0 && (
                <span className="basket-count">{basketItemCount}</span>
              )}
            </Link>
            <div className="user-profile" ref={dropdownRef}>
              <button
                className="profile-button"
                onClick={() => setShowDropdown(!showDropdown)}
              >
                <div className="profile-initials">{getInitials()}</div>
              </button>
              {showDropdown && (
                <div className="profile-dropdown">
                  <div className="profile-header">
                    <div className="profile-name">{user.given_name}</div>
                    <div className="profile-email">{user.email}</div>
                  </div>
                  <div className="profile-menu">
                    <Link to="/profile" className="profile-menu-item" onClick={() => setShowDropdown(false)}>
                      <FaUser />
                      Profile
                    </Link>
                    <Link to="/my-courses" className="profile-menu-item" onClick={() => setShowDropdown(false)}>
                      <FaBook />
                      My Courses
                    </Link>
                    <Link to="/settings" className="profile-menu-item" onClick={() => setShowDropdown(false)}>
                      <FaCog />
                      Settings
                    </Link>
                    <div className="profile-divider"></div>
                    <button
                      className="logout-button"
                      onClick={() => {
                        logout();
                        setShowDropdown(false);
                        navigate('/');
                      }}
                    >
                      <FaSignOutAlt />
                      Logout
                    </button>
                  </div>
                </div>
              )}
            </div>
          </>
        ) : (
          <div className="auth-buttons">
            <Link to="/login" className="login-btn">
              Log in
            </Link>
            <Link to="/register" className="signup-btn">
              Sign up
            </Link>
          </div>
        )}
      </div>
    </nav>
  );
};

export default Navbar; 