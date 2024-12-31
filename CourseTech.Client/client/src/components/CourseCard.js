import React from 'react';
import { Link } from 'react-router-dom';
import './CourseCard.css';

const CourseCard = ({ course }) => {
  const renderStars = (rating) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      if (i <= Math.floor(rating)) {
        stars.push(<i key={i} className="fas fa-star"></i>);
      } else if (i - 0.5 <= rating) {
        stars.push(<i key={i} className="fas fa-star-half-alt"></i>);
      } else {
        stars.push(<i key={i} className="far fa-star"></i>);
      }
    }
    return stars;
  };

  return (
    <div className="course-card">
      <div className="course-image-container">
        <img src={course.image} alt={course.title} className="course-image" />
        <div className="course-overlay">
          <div className="course-preview">
            <h4>Kurs Önizleme</h4>
            <p>{course.description}</p>
            <ul>
              <li><i className="fas fa-clock"></i> 12 saat video içeriği</li>
              <li><i className="fas fa-infinity"></i> Ömür boyu erişim</li>
              <li><i className="fas fa-certificate"></i> Bitirme sertifikası</li>
            </ul>
          </div>

        </div>
      </div>
      <div className="course-content">
        <h3 className="course-title">{course.title}</h3>
        <p className="course-instructor">{course.instructor}</p>
        <div className="course-rating">
          <span className="rating-score">{course.rating}</span>
          <span className="rating-stars">{renderStars(course.rating)}</span>
          <span className="students-count">({course.students} öğrenci)</span>
        </div>
        <div className="course-footer">
          <span className="course-price">{course.price} TL</span>
          <Link to={`/course/${course.id}`} className="details-button">
            Kursa Git
          </Link>
        </div>
      </div>
    </div>
  );
};

export default CourseCard; 