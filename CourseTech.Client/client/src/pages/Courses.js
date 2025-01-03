import React, { useState, useEffect } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { courseService } from '../services/courseService';
import PageLayout from '../components/PageLayout';
import slugify from 'react-slugify';
import './Courses.css';

const Courses = () => {
  const [searchParams] = useSearchParams();
  const [courses, setCourses] = useState([]);
  const [filteredCourses, setFilteredCourses] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        setIsLoading(true);
        const response = await courseService.getPublishedCourses();
        if (response?.data) {
          setCourses(response.data);
          setFilteredCourses(response.data);
        }
      } catch (err) {
        setError('Failed to load courses. Please try again later.');
        console.error('Error fetching courses:', err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCourses();
  }, []);

  // URL'den search parametresini al ve kurslarda filtreleme yap
  useEffect(() => {
    const searchTerm = searchParams.get('search')?.toLowerCase();
    if (searchTerm) {
      const filtered = courses.filter(course => 
        course.title.toLowerCase().includes(searchTerm) ||
        course.instructorName.toLowerCase().includes(searchTerm) ||
        (course.description && course.description.toLowerCase().includes(searchTerm))
      );
      setFilteredCourses(filtered);
    } else {
      setFilteredCourses(courses);
    }
  }, [searchParams, courses]);

  const getCourseLevelColor = (level) => {
    const colors = {
      'Beginner': '#4CAF50',
      'Intermediate': '#FF9800',
      'Advanced': '#f44336',
      'AllLevels': '#2196F3'
    };
    return colors[level] || '#2196F3';
  };

  if (isLoading) {
    return (
      <PageLayout>
        <div className="loading-container">
          <div className="loader"></div>
          <p>Loading courses...</p>
        </div>
      </PageLayout>
    );
  }

  if (error) {
    return (
      <PageLayout>
        <div className="error-container">
          <p>{error}</p>
          <button onClick={() => window.location.reload()} className="btn-primary">
            Try Again
          </button>
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout>
      <div className="courses-container">
        <div className="courses-header">
          <h1>All Courses</h1>
          <p>Expand your knowledge with our comprehensive course catalog</p>
          {searchParams.get('search') && (
            <div className="search-results">
              <h2>Search Results for "{searchParams.get('search')}"</h2>
              <p>{filteredCourses.length} courses found</p>
            </div>
          )}
        </div>

        <div className="courses-grid">
          {filteredCourses.length > 0 ? (
            filteredCourses.map(course => (
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
                      {course.level}
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
              <p>No courses found matching your search criteria.</p>
            </div>
          )}
        </div>
      </div>
    </PageLayout>
  );
};

export default Courses; 