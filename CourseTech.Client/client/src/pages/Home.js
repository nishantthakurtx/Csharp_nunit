import React, { useEffect } from 'react';
import { useCourse } from '../contexts/CourseContext';
import CourseCard from '../components/CourseCard';
import { Link } from 'react-router-dom';
import styles from '../styles/Home.module.css';

const Home = () => {
  const { courseSummaries, isLoading, loadCourseSummaries } = useCourse();

  useEffect(() => {
    loadCourseSummaries();
  }, []);

  return (
    <div className={styles.container}>
      <div className={styles.hero}>
        <h1>Discover Your Path to Success</h1>
        <p>Explore expert-led courses and transform your career today.</p>
        <div className={styles.searchContainer}>
          <input
            type="text"
            placeholder="What do you want to learn?"
            className={styles.searchInput}
          />
        </div>
      </div>

      <div className={styles.featuredSection}>
        <div className={styles.sectionHeader}>
          <h2>Featured Courses</h2>
          <Link to="/courses" className={styles.viewAllButton}>
            View All Courses
          </Link>
        </div>
        <div className={styles.courseGrid}>
          {isLoading ? (
            <div className={styles.loading}>Loading courses...</div>
          ) : courseSummaries?.length > 0 ? (
            courseSummaries.slice(0, 4).map((course) => (
              <div key={course.id} className={styles.courseCard}>
                <CourseCard course={course} />
              </div>
            ))
          ) : (
            <div className={styles.noCourses}>No courses available</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Home; 