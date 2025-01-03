import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import slugify from 'react-slugify';
import './CategoryDetail.css';
import { courseService } from '../services/courseService';
import { userService } from '../services/userService';
import { categoryService } from '../services/categoryService';
import PageLayout from '../components/PageLayout';

const ITEMS_PER_PAGE = 10;
const DEFAULT_IMAGE = 'https://images.unsplash.com/photo-1497633762265-9d179a990aa6';

const CategoryDetail = () => {
  const { slug } = useParams();
  const [category, setCategory] = useState(null);
  const [courses, setCourses] = useState([]);
  const [filteredCourses, setFilteredCourses] = useState([]);
  const [instructors, setInstructors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [filters, setFilters] = useState({
    level: [],
    language: [],
    instructor: [],
    priceRange: { min: 0, max: 1000 }
  });

  // Calculate pagination values
  const totalPages = Math.ceil(filteredCourses.length / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const endIndex = startIndex + ITEMS_PER_PAGE;
  const currentCourses = filteredCourses.slice(startIndex, endIndex);

  // Reset to first page when filters change
  useEffect(() => {
    setCurrentPage(1);
  }, [filters]);

  // Reset scroll position when component mounts and when category changes
  useEffect(() => {
    window.scrollTo(0, 0);
  }, [slug]);

  // Fetch category and courses
  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        // First get all categories to find the one matching our slug
        const categoriesResponse = await categoryService.getAll();
        
        if (categoriesResponse?.data) {
          const foundCategory = categoriesResponse.data.find(
            cat => slugify(cat.name) === slug
          );

          if (foundCategory) {
            setCategory(foundCategory);
            const [coursesResponse, instructorsResponse] = await Promise.all([
              courseService.getCoursesByCategory(foundCategory.id),
              userService.getInstructors()
            ]);

            if (coursesResponse?.data) {
              setCourses(coursesResponse.data);
              setFilteredCourses(coursesResponse.data);
            }

            if (instructorsResponse?.data) {
              setInstructors(instructorsResponse.data);
            }
          } else {
            setError('Category not found.');
          }
        }
      } catch (err) {
        setError('Failed to load courses. Please try again later.');
        console.error('Error fetching data:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [slug]);

  // Handle filter changes
  const handleFilterChange = (filterType, value) => {
    setFilters(prev => {
      const newFilters = { ...prev };
      
      if (filterType === 'level' || filterType === 'language' || filterType === 'instructor') {
        if (newFilters[filterType].includes(value)) {
          newFilters[filterType] = newFilters[filterType].filter(item => item !== value);
        } else {
          newFilters[filterType] = [...newFilters[filterType], value];
        }
      } else if (filterType === 'priceRange') {
        newFilters.priceRange = value;
      }
      
      return newFilters;
    });
  };

  // Handle page change
  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // Apply filters
  useEffect(() => {
    let filtered = [...courses];

    if (filters.level.length > 0) {
      filtered = filtered.filter(course => filters.level.includes(course.level));
    }

    if (filters.language.length > 0) {
      filtered = filtered.filter(course => filters.language.includes(course.language));
    }

    if (filters.instructor.length > 0) {
      filtered = filtered.filter(course => filters.instructor.includes(course.instructorId));
    }

    filtered = filtered.filter(course => 
      course.price >= filters.priceRange.min && 
      course.price <= filters.priceRange.max
    );

    setFilteredCourses(filtered);
  }, [filters, courses]);

  if (loading) {
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
      <div className="category-page">
        {/* Filters Sidebar */}
        <aside className="filters-sidebar">
          <div className="filter-section">
            <h3>Level</h3>
            <div className="filter-options">
              {['Beginner', 'Intermediate', 'Advanced'].map(level => (
                <label key={level} className="filter-option">
                  <input
                    type="checkbox"
                    checked={filters.level.includes(level)}
                    onChange={() => handleFilterChange('level', level)}
                  />
                  <span>{level}</span>
                </label>
              ))}
            </div>
          </div>

          <div className="filter-section">
            <h3>Language</h3>
            <div className="filter-options">
              {['English', 'Turkish'].map(language => (
                <label key={language} className="filter-option">
                  <input
                    type="checkbox"
                    checked={filters.language.includes(language)}
                    onChange={() => handleFilterChange('language', language)}
                  />
                  <span>{language}</span>
                </label>
              ))}
            </div>
          </div>

          {instructors.length > 0 && (
            <div className="filter-section">
              <h3>Instructor</h3>
              <div className="filter-options">
                {instructors.map(instructor => (
                  <label key={instructor.id} className="filter-option">
                    <input
                      type="checkbox"
                      checked={filters.instructor.includes(instructor.id)}
                      onChange={() => handleFilterChange('instructor', instructor.id)}
                    />
                    <span>{instructor.fullName}</span>
                  </label>
                ))}
              </div>
            </div>
          )}

          <div className="filter-section">
            <h3>Price Range</h3>
            <div className="price-range">
              <input
                type="range"
                min="0"
                max="1000"
                value={filters.priceRange.max}
                onChange={(e) => handleFilterChange('priceRange', {
                  ...filters.priceRange,
                  max: parseInt(e.target.value)
                })}
              />
              <div className="price-inputs">
                <input
                  type="number"
                  value={filters.priceRange.min}
                  onChange={(e) => handleFilterChange('priceRange', {
                    ...filters.priceRange,
                    min: parseInt(e.target.value)
                  })}
                  min="0"
                />
                <span>-</span>
                <input
                  type="number"
                  value={filters.priceRange.max}
                  onChange={(e) => handleFilterChange('priceRange', {
                    ...filters.priceRange,
                    max: parseInt(e.target.value)
                  })}
                  max="1000"
                />
              </div>
            </div>
          </div>

          <button 
            onClick={() => setFilters({
              level: [],
              language: [],
              instructor: [],
              priceRange: { min: 0, max: 1000 }
            })}
            className="clear-filters-btn"
          >
            Clear Filters
          </button>
        </aside>

        {/* Courses List */}
        <main className="courses-list">
          <div className="courses-header">
            <h2>{category?.name || 'Loading...'}</h2>
            <p>{filteredCourses.length} courses available</p>
          </div>
          {currentCourses.length > 0 ? (
            <>
              <div className="courses-container">
                {currentCourses.map((course) => (
                  <Link 
                    to={`/course/${slugify(course.title)}`}
                    key={course.id} 
                    className="course-list-item"
                  >
                    <div className="course-image">
                      <img 
                        src={course.imageUrl || DEFAULT_IMAGE} 
                        alt={course.title}
                        onError={(e) => {
                          e.target.src = DEFAULT_IMAGE;
                        }}
                      />
                    </div>
                    <div className="course-info">
                      <h3>{course.title}</h3>
                      <Link 
                        to={`/instructor/${slugify(course.instructorName)}`}
                        className="instructor"
                        onClick={(e) => e.stopPropagation()}
                      >
                        Instructor: {course.instructorName}
                      </Link>
                      <p className="level">Level: {course.level}</p>
                      <p className="price">${course.price}</p>
                    </div>
                  </Link>
                ))}
              </div>
              {totalPages > 1 && (
                <div className="pagination">
                  <button 
                    onClick={() => handlePageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                  >
                    Previous
                  </button>
                  {[...Array(totalPages)].map((_, index) => (
                    <button
                      key={index + 1}
                      onClick={() => handlePageChange(index + 1)}
                      className={currentPage === index + 1 ? 'current-page' : ''}
                    >
                      {index + 1}
                    </button>
                  ))}
                  <button 
                    onClick={() => handlePageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                  >
                    Next
                  </button>
                </div>
              )}
            </>
          ) : (
            <div className="no-courses">
              <div className="no-courses-content">
                <i className="fas fa-search fa-3x"></i>
                <h3>No courses found</h3>
                <p>Try adjusting your filters or browse our other categories</p>
              </div>
            </div>
          )}
        </main>
      </div>
    </PageLayout>
  );
};

export default CategoryDetail; 