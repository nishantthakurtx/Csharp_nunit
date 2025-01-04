import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { courseService } from '../services/courseService';
import { FaEdit, FaTrash, FaChevronLeft, FaChevronRight, FaEye, FaEyeSlash } from 'react-icons/fa';
import { BiLoaderAlt } from 'react-icons/bi';
import { useAuth } from '../contexts/AuthContext';
import alertify from 'alertifyjs';
import 'alertifyjs/build/css/alertify.css';
import 'alertifyjs/build/css/themes/default.css';
import './MyCourses.css';

const ITEMS_PER_PAGE = 5;

const MyCourses = () => {
  const navigate = useNavigate();
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { user, isAuthenticated } = useAuth();
  const [currentPage, setCurrentPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }

    if (user?.sub) {
      fetchCourses();
    }
  }, [user, isAuthenticated, navigate]);

  const fetchCourses = async () => {
    try {
      const response = await courseService.getCoursesByInstructor(user.sub);
      const coursesData = Array.isArray(response.data) ? response.data : [];
      console.log('Raw courses data:', coursesData);
      setCourses(coursesData);
    } catch (error) {
      console.error('Error fetching courses:', error);
      setError('Failed to load courses');
      setCourses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (courseId, courseTitle) => {
    alertify.confirm(
      'Delete Course',
      `Are you sure you want to delete "${courseTitle}"? This action cannot be undone.`,
      async function() {
        try {
          await courseService.deleteCourse(courseId);
          alertify.success(`"${courseTitle}" has been deleted successfully`);
          fetchCourses();
        } catch (error) {
          console.error('Error deleting course:', error);
          alertify.error('Failed to delete course');
        }
      },
      function() {
        alertify.message('Delete cancelled');
      }
    ).set({
      'labels': {ok: 'Yes, Delete', cancel: 'Keep Course'},
      'movable': false,
      'transition': 'zoom',
      'closableByDimmer': true,
      'defaultFocus': 'cancel'
    });
  };

  const handlePublishToggle = async (courseId, isPublished, courseTitle) => {
    const action = isPublished ? 'unpublish' : 'publish';
    
    try {
      if (isPublished) {
        await courseService.unpublishCourse(courseId);
      } else {
        await courseService.publishCourse(courseId);
      }
      
      setCourses(prevCourses => 
        prevCourses.map(course => 
          course.id === courseId 
            ? { ...course, isPublished: !isPublished }
            : course
        )
      );
      
      alertify.success(`Course "${courseTitle}" has been ${action}ed successfully`);
    } catch (error) {
      console.error(`Error ${action}ing course:`, error);
      alertify.error(`Failed to ${action} course`);
    }
  };

  // Filtreleme fonksiyonu
  const filteredCourses = courses.filter(course =>
    course.title.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Pagination calculations güncellendi
  const totalPages = Math.ceil(filteredCourses.length / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const endIndex = startIndex + ITEMS_PER_PAGE;
  const currentCourses = filteredCourses.slice(startIndex, endIndex);

  // Search değiştiğinde sayfa 1'e dön
  useEffect(() => {
    setCurrentPage(1);
  }, [searchTerm]);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  if (loading) {
    return (
      <div className="my-courses-container">
        <div className="loading-container">
          <BiLoaderAlt className="loading-spinner" />
          <p className="loading-text">Loading your courses...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="my-courses-container">
        <div className="error-message">{error}</div>
      </div>
    );
  }

  return (
    <div className="my-courses-container">
      <div className="my-courses-header">
        <h1>My Courses</h1>
        <Link to="/courses/create" className="add-course-btn">
          Create New Course
        </Link>
      </div>

      {courses.length === 0 ? (
        <div className="no-courses">
          <p>You haven't created any courses yet.</p>
          <Link to="/courses/create" className="start-teaching-btn">
            Start Teaching Today
          </Link>
        </div>
      ) : (
        <>
          <div className="search-container">
            <input
              type="text"
              placeholder="Search by course title..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
          </div>

          <div className="courses-table-container">
            <table className="courses-table">
              <thead>
                <tr>
                  <th>Image</th>
                  <th>Title</th>
                  <th>Category</th>
                  <th>Level</th>
                  <th>Duration</th>
                  <th>Price</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {currentCourses.map(course => (
                  <tr key={course.id} className="course-row">
                    <td className="course-image-cell">
                      <img src={course.imageUrl || '/default-course.jpg'} alt={course.title} />
                    </td>
                    <td>{course.title}</td>
                    <td>{course.categoryName || 'Uncategorized'}</td>
                    <td>
                      <span className="course-level">{course.courseLevel}</span>
                    </td>
                    <td>{course.duration}</td>
                    <td>
                      <span className="course-price">${course.price}</span>
                    </td>
                    <td className="course-actions-cell">
                      <Link to={`/courses/edit/${course.id}`} className="edit-btn">
                        <FaEdit /> Edit
                      </Link>
                      <button 
                        className="delete-btn"
                        onClick={() => handleDelete(course.id, course.title)}
                      >
                        <FaTrash /> Delete
                      </button>
                      <button 
                        className={`publish-btn ${course.isPublished ? 'unpublish' : 'publish'}`}
                        onClick={() => handlePublishToggle(course.id, course.isPublished, course.title)}
                      >
                        {course.isPublished ? (
                          <><FaEyeSlash /> Unpublish</>
                        ) : (
                          <><FaEye /> Publish</>
                        )}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {filteredCourses.length === 0 ? (
            <div className="no-results">
              <p>No courses found matching "{searchTerm}"</p>
            </div>
          ) : totalPages > 1 && (
            <div className="pagination">
              <button 
                className="pagination-btn"
                onClick={() => handlePageChange(currentPage - 1)}
                disabled={currentPage === 1}
              >
                <FaChevronLeft />
              </button>
              
              {[...Array(totalPages)].map((_, index) => (
                <button
                  key={index + 1}
                  className={`pagination-btn ${currentPage === index + 1 ? 'active' : ''}`}
                  onClick={() => handlePageChange(index + 1)}
                >
                  {index + 1}
                </button>
              ))}
              
              <button 
                className="pagination-btn"
                onClick={() => handlePageChange(currentPage + 1)}
                disabled={currentPage === totalPages}
              >
                <FaChevronRight />
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default MyCourses; 