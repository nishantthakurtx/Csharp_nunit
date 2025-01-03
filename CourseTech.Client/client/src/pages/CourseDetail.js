import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import slugify from 'react-slugify';
import { courseService } from '../services/courseService';
import { basketService } from '../services/basketService';
import { useAuth } from '../contexts/AuthContext';
import alertify from 'alertifyjs';
import 'alertifyjs/build/css/alertify.css';
import './CourseDetail.css';

const DEFAULT_IMAGE = 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&q=80';

const CourseDetail = () => {
  const { slug } = useParams();
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();
  const [course, setCourse] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCourse = async () => {
      try {
        setLoading(true);
        const response = await courseService.getAllCourses();
        if (response?.data) {
          const foundCourse = response.data.find(
            course => slugify(course.title) === slug
          );
          
          if (foundCourse) {
            const detailsResponse = await courseService.getCourseWithDetails(foundCourse.id);
            if (detailsResponse?.data) {
              setCourse(detailsResponse.data);
            } else {
              setError('Course details not found.');
            }
          } else {
            setError('Course not found.');
          }
        } else {
          setError('Failed to load courses.');
        }
      } catch (err) {
        setError('Failed to load course details.');
        console.error('Error fetching course:', err);
      } finally {
        setLoading(false);
      }
    };

    if (slug) {
      fetchCourse();
    } else {
      setError('Invalid course URL.');
      setLoading(false);
    }
  }, [slug]);

  const handleAddToCart = async () => {
    if (!isAuthenticated || !user?.id) {
      navigate('/login');
      return;
    }

    try {
      await basketService.addCourseToBasket(user.id, course.id);
      window.dispatchEvent(new Event('basketUpdated'));
      
      alertify.confirm('Course Added to Basket', 
        'Course has been added to your basket successfully!',
        function() {
          navigate('/basket');
        },
        function() {
          // Alışverişe devam et
        }
      ).set({
        'labels': {ok: 'Go to Basket', cancel: 'Continue Shopping'},
        'defaultFocus': 'cancel',
        'movable': false,
        'closable': false
      });
    } catch (error) {
      console.error('Error adding course to basket:', error);
      if (error.message === 'This course is already in your basket') {
        alertify.error("You have already added this course to your basket.");
      } else {
        alertify.error("Failed to add course to basket. Please try again.");
      }
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loader"></div>
        <p>Loading course details...</p>
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

  if (!course) {
    return (
      <div className="error-container">
        <p>Course not found.</p>
        <Link to="/" className="btn-primary">
          Return to Home
        </Link>
      </div>
    );
  }

  // Format duration to hours and minutes
  const formatDuration = (duration) => {
    if (!duration) return "0h 0m";
    
    // TimeSpan string format: "00:00:00"
    const [hours, minutes] = duration.split(':');
    return `${parseInt(hours)}h ${parseInt(minutes)}m`;
  };

  return (
    <div className="course-detail">
      <div className="course-header">
        <div className="course-header-content">
          <div className="course-breadcrumb">
            <span>Home</span>
            <i className="fas fa-chevron-right"></i>
            <span>Categories</span>
            <i className="fas fa-chevron-right"></i>
            <span>{course.categoryName}</span>
          </div>
          
          <h1>{course.title}</h1>
          <p className="course-description">{course.description}</p>
          
          <div className="course-rating">
            <span className="students">
              <i className="fas fa-user-graduate"></i>
              {course.studentCount} students
            </span>
          </div>

          <div className="course-creator">
            <span>Instructor:</span>
            <Link to={`/instructor/${slugify(course.instructorName)}`}>
              {course.instructorName}
            </Link>
          </div>

          <div className="course-tags">
            <span>
              <i className="fas fa-signal"></i>
              {course.level}
            </span>
            <span>
              <i className="fas fa-globe"></i>
              {course.language}
            </span>
            <span>
              <i className="fas fa-calendar-alt"></i>
              {new Date(course.publishedAt).toLocaleDateString()}
            </span>
          </div>
        </div>
      </div>

      <div className="course-content">
        <div className="course-main">
          <div className="course-section">
            <h2>About this course</h2>
            <p>{course.description}</p>
          </div>
        </div>

        <div className="course-sidebar">
          <div className="course-card">
            <img 
              src={course.imageUrl || DEFAULT_IMAGE} 
              alt={course.title}
              className="course-preview-image"
            />
            <div className="course-card-content">
              <div className="course-price">${course.price}</div>
              <button className="add-to-cart-btn" onClick={handleAddToCart}>
                <i className="fas fa-shopping-cart"></i>
                Add to cart
              </button>
              <div className="course-features">
                <ul>
                  <li>
                    <i className="fas fa-infinity"></i>
                    Full lifetime access
                  </li>
                  <li>
                    <i className="fas fa-mobile-alt"></i>
                    Access on mobile and TV
                  </li>
                  <li>
                    <i className="fas fa-certificate"></i>
                    Certificate of completion
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseDetail; 