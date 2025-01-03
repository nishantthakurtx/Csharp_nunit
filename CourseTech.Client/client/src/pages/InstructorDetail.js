import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import slugify from 'react-slugify';
import { userService } from '../services/userService';
import { courseService } from '../services/courseService';
import './InstructorDetail.css';

const DEFAULT_INSTRUCTOR_IMAGE = 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&q=80';

const InstructorDetail = () => {
  const { id, slug } = useParams();
  const [instructor, setInstructor] = useState(null);
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [instructorResponse, coursesResponse] = await Promise.all([
          userService.getById(id),
          courseService.getCoursesByInstructor(id)
        ]);

        if (instructorResponse?.data) {
          setInstructor(instructorResponse.data);
          // Update URL if slug doesn't match
          const correctSlug = slugify(instructorResponse.data.fullName);
          if (slug !== correctSlug) {
            window.history.replaceState(
              null, 
              '', 
              `/instructor/${id}/${correctSlug}`
            );
          }
        }

        if (coursesResponse?.data) {
          setCourses(coursesResponse.data);
        }
      } catch (err) {
        setError('Failed to load instructor details.');
        console.error('Error fetching instructor:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, slug]);

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loader"></div>
        <p>Loading instructor details...</p>
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
    <div className="instructor-detail">
      {instructor && (
        <>
          <div className="instructor-header">
            <div className="instructor-profile">
              <img 
                src={instructor.imageUrl || DEFAULT_INSTRUCTOR_IMAGE} 
                alt={instructor.fullName} 
                className="instructor-avatar"
              />
              <div className="instructor-info">
                <h1>{instructor.fullName}</h1>
                <p className="instructor-title">{instructor.title}</p>
                <div className="instructor-stats">
                  <div className="stat">
                    <i className="fas fa-book-reader"></i>
                    <span>{courses.length} Courses</span>
                  </div>
                  <div className="stat">
                    <i className="fas fa-users"></i>
                    <span>{instructor.totalStudents || 0} Students</span>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="instructor-courses">
            <h2>Courses by {instructor.fullName}</h2>
            <div className="courses-grid">
              {courses.map(course => (
                <Link 
                  to={`/course/${course.id}/${slugify(course.title)}`}
                  key={course.id} 
                  className="course-card"
                >
                  <div className="course-image">
                    <img src={course.imageUrl} alt={course.title} />
                  </div>
                  <div className="course-info">
                    <h3>{course.title}</h3>
                    <p className="course-stats">
                      <span className="students">
                        <i className="fas fa-users"></i>
                        {course.totalStudents || 0} students
                      </span>
                      <span className="price">${course.price}</span>
                    </p>
                  </div>
                </Link>
              ))}
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default InstructorDetail; 