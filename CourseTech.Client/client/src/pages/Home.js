import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { categoryService } from '../services/categoryService';
import { courseService } from '../services/courseService';
import slugify from 'react-slugify';
import './Home.css';

const Home = () => {
  const { user } = useAuth();
  const [featuredCourses, setFeaturedCourses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      setError(null);
      try {
        // Fetch categories and course summaries in parallel
        const [categoriesData, coursesData] = await Promise.all([
          categoryService.getCategoriesWithCourses(),
          courseService.getAllCourseSummaries()
        ]);
        
        // Map the categories response to match our UI needs
        const mappedCategories = categoriesData.data.map(category => ({
          id: category.id,
          name: category.name,
          count: category.count || 0,
          slug: slugify(category.name)
        }));

        // Get only first 3 categories
        setCategories(mappedCategories.slice(0, 3));
        
        // Map the course summaries and set as featured courses
        if (coursesData.data) {
          const mappedCourses = coursesData.data.map(course => ({
            id: course.id,
            title: course.title,
            price: course.price,
            imageUrl: course.imageUrl,
            instructor: course.instructorName,
            category: course.category,
            level: course.courseLevel
          }));
          
          // Get first 3 courses for featured section
          setFeaturedCourses(mappedCourses.slice(0, 3));
        } else {
          setError('No courses available.');
        }
      } catch (err) {
        setError('Failed to load categories and courses. Please try again later.');
        console.error('Error fetching data:', err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  // Helper function to get course level badge color
  const getCourseLevelColor = (level) => {
    const colors = {
      'Beginner': '#4CAF50',
      'Intermediate': '#FF9800',
      'Advanced': '#f44336',
      'AllLevels': '#2196F3'
    };
    return colors[level] || '#2196F3';
  };

  // Helper function to format course level display
  const formatCourseLevel = (level) => {
    if (!level) return 'All Levels';
    return level.charAt(0).toUpperCase() + level.slice(1).toLowerCase();
  };

  const getCategoryIcon = (categoryName) => {
    const icons = {
      'Development': 'fa-code',
      'Business': 'fa-briefcase',
      'Finance': 'fa-chart-line',
      'IT & Software': 'fa-laptop-code',
      'Design': 'fa-palette',
      'Marketing': 'fa-bullhorn',
      'Music': 'fa-music',
      'Health & Fitness': 'fa-heartbeat',
      'Photography': 'fa-camera',
      'Teaching': 'fa-chalkboard-teacher',
      'Programming': 'fa-code',
      'Data Science': 'fa-database',
      'Personal Development': 'fa-user-graduate',
      'Language': 'fa-language'
    };
    return icons[categoryName] || 'fa-book';
  };

  if (isLoading) {
    return (
      <div className="loading-container">
        <div className="loader"></div>
        <p>Loading amazing courses for you...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <p>{error}</p>
        <button onClick={() => window.location.reload()} className="btn-primary">
          Try Again
        </button>
      </div>
    );
  }

  return (
    <div className="home-container">
      {/* Hero Section */}
      <section className="hero-section">
        <div className="hero-background"></div>
        <div className="hero-content">
          <h1>Unlock Your Potential</h1>
          <p>Join millions of learners worldwide and transform your career with expert-led courses</p>
          <div className="hero-buttons">
            <Link to="/courses" className="btn-primary">Browse Courses</Link>
            {!user && <Link to="/register" className="btn-secondary">Start Learning</Link>}
          </div>
        </div>
      </section>

      {/* Categories Section */}
      <section className="categories-section">
        <div className="section-header">
          <h2>Popular Categories</h2>
          <Link to="/categories" className="view-all">Explore All Categories</Link>
        </div>
        <div className="categories-grid">
          {categories.map(category => (
            <Link 
              to={`/category/${category.slug}`} 
              key={category.id} 
              className="category-card"
            >
              <div className="category-icon">
                <i className={`fas ${getCategoryIcon(category.name)}`}></i>
              </div>
              <div className="category-info">
                <h3>{category.name}</h3>
              </div>
            </Link>
          ))}
        </div>
      </section>

      {/* Featured Courses Section */}
      <section className="featured-courses-section">
        <div className="section-header">
          <h2>Featured Courses</h2>
          <Link to="/courses" className="view-all">View All Courses</Link>
        </div>
        <div className="courses-grid">
          {featuredCourses.length > 0 ? (
            featuredCourses.map(course => (
              <div key={course.id} className="course-card">
                <div className="course-image">
                  <img 
                    src={course.imageUrl} 
                    alt={course.title}
                    onError={(e) => {
                      e.target.src = 'https://via.placeholder.com/300x200?text=Course+Image';
                    }}
                  />
                </div>
                <div className="course-content">
                  <h3 className="course-title">{course.title}</h3>
                  <p className="instructor">
                    <Link 
                      to={`/instructor/${slugify(course.instructorName)}`}
                      className="instructor-link"
                    >
                      {course.instructorName}
                    </Link>
                  </p>
                  <div className="course-meta">
                    <span 
                      className="course-level"
                      style={{ backgroundColor: getCourseLevelColor(course.level) }}
                    >
                      {formatCourseLevel(course.level)}
                    </span>
                    <span className="course-price">${course.price.toFixed(2)}</span>
                  </div>
                  <Link 
                    to={`/course/${slugify(course.title)}`} 
                    className="details-button"
                  >
                    Course Details
                  </Link>
                </div>
              </div>
            ))
          ) : (
            <div className="no-courses-message">
              <p>No courses available at the moment.</p>
            </div>
          )}
        </div>
      </section>

      {/* Features Section */}
      <section className="features-section">
        <h2>Why Choose Our Platform</h2>
        <div className="features-grid">
          <div className="feature-card">
            <i className="fas fa-graduation-cap"></i>
            <h3>Expert Instructors</h3>
            <p>Learn from industry professionals with real-world experience.</p>
          </div>
          <div className="feature-card">
            <i className="fas fa-clock"></i>
            <h3>Flexible Learning</h3>
            <p>Study at your own pace with lifetime access to courses.</p>
          </div>
          <div className="feature-card">
            <i className="fas fa-certificate"></i>
            <h3>Verified Certificates</h3>
            <p>Earn recognized certificates upon course completion.</p>
          </div>
          <div className="feature-card">
            <i className="fas fa-users"></i>
            <h3>Global Community</h3>
            <p>Connect with millions of learners from around the world.</p>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-section">
        <div className="cta-background"></div>
        <div className="cta-content">
          <h2>Transform Your Life Through Education</h2>
          <p>Take the first step towards your goals today</p>
          <Link to="/courses" className="btn-primary">Get Started Now</Link>
        </div>
      </section>
    </div>
  );
};

export default Home; 