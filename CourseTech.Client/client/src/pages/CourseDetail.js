import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { useAuth } from '../context/AuthContext';
import './CourseDetail.css';

const CourseDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { addToCart, isInCart } = useCart();
  const { isAuthenticated } = useAuth();
  const [showLoginModal, setShowLoginModal] = useState(false);

  // Örnek kurs verisi (normalde API'den gelecek)
  const course = {
    id: id,
    title: 'React ile Modern Web Uygulamaları Geliştirme',
    instructor: 'Ahmet Yılmaz',
    description: 'React\'ın temellerinden ileri seviye konulara kadar kapsamlı bir eğitim serisi. Hooks, Context API, Redux ve daha fazlası!',
    price: 199.99,
    originalPrice: 499.99,
    discount: 60,
    rating: 4.8,
    students: 1234,
    lastUpdated: '2024-01',
    language: 'Türkçe',
    image: 'https://picsum.photos/800/400',
    category: 'Web Geliştirme',
    whatYouWillLearn: [
      'React\'ın temel kavramlarını ve çalışma prensiplerini',
      'Modern JavaScript (ES6+) özelliklerini',
      'React Hooks ve Custom Hook geliştirmeyi',
      'Context API ile state yönetimini',
      'Redux ile kompleks state yönetimini',
      'React Router ile sayfa yönetimini',
      'REST API entegrasyonunu',
      'Modern ve responsive tasarım tekniklerini'
    ],
    requirements: [
      'Temel HTML, CSS ve JavaScript bilgisi',
      'ES6+ JavaScript özelliklerine aşinalık',
      'Temel web geliştirme kavramlarına hakimiyet'
    ]
  };

  const handleAddToCart = () => {
    if (!isAuthenticated) {
      setShowLoginModal(true);
      return;
    }
    addToCart(course);
  };

  const handleLoginRedirect = () => {
    navigate('/login', { state: { from: `/course/${id}` } });
  };

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
    <div className="course-detail">
      <div className="course-header">
        <div className="course-header-content">
          <h1>{course.title}</h1>
          <p className="course-description">{course.description}</p>
          <div className="course-meta">
            <div className="rating">
              <span className="rating-score">{course.rating}</span>
              <span className="rating-stars">{renderStars(course.rating)}</span>
              <span className="students-count">({course.students} öğrenci)</span>
            </div>
            <div className="instructor-info">
              <i className="fas fa-user"></i>
              <span>{course.instructor}</span>
            </div>
            <div className="course-info-item">
              <i className="fas fa-folder"></i>
              <span>{course.category}</span>
            </div>
            <div className="course-info-item">
              <i className="fas fa-clock"></i>
              <span>Son güncelleme: {course.lastUpdated}</span>
            </div>
            <div className="course-info-item">
              <i className="fas fa-globe"></i>
              <span>{course.language}</span>
            </div>
          </div>
        </div>
      </div>

      <div className="course-content">
        <div className="main-content">
          <section className="what-you-learn">
            <h2>Bu kursta öğrenecekleriniz</h2>
            <ul>
              {course.whatYouWillLearn.map((item, index) => (
                <li key={index}>
                  <i className="fas fa-check"></i>
                  <span>{item}</span>
                </li>
              ))}
            </ul>
          </section>

          <section className="requirements">
            <h2>Ön Koşullar</h2>
            <ul>
              {course.requirements.map((req, index) => (
                <li key={index}>
                  <i className="fas fa-circle"></i>
                  <span>{req}</span>
                </li>
              ))}
            </ul>
          </section>
        </div>

        <div className="course-sidebar">
          <div className="course-purchase-card">
            <img src={course.image} alt={course.title} className="course-preview-image" />
            <div className="price-container">
              <span className="current-price">₺{course.price.toFixed(2)}</span>
              <span className="original-price">₺{course.originalPrice.toFixed(2)}</span>
              <span className="discount">%{course.discount} indirim</span>
            </div>
            <button 
              className={`add-to-cart-btn ${isInCart(course.id) ? 'in-cart' : ''}`}
              onClick={handleAddToCart}
              disabled={isInCart(course.id)}
            >
              {isInCart(course.id) ? (
                <>
                  <i className="fas fa-check"></i>
                  Sepete Eklendi
                </>
              ) : (
                <>
                  <i className="fas fa-shopping-cart"></i>
                  Sepete Ekle
                </>
              )}
            </button>
            <div className="guarantee-text">
              <i className="fas fa-medal"></i>
              30 Gün Para İade Garantisi
            </div>
            <div className="course-includes">
              <h3>Bu kursa dahil:</h3>
              <ul>
                <li>
                  <i className="fas fa-video"></i>
                  <span>12 saat video içeriği</span>
                </li>
                <li>
                  <i className="fas fa-file-alt"></i>
                  <span>25 makale</span>
                </li>
                <li>
                  <i className="fas fa-download"></i>
                  <span>İndirilebilir kaynaklar</span>
                </li>
                <li>
                  <i className="fas fa-infinity"></i>
                  <span>Ömür boyu erişim</span>
                </li>
                <li>
                  <i className="fas fa-mobile-alt"></i>
                  <span>Mobil ve TV erişimi</span>
                </li>
                <li>
                  <i className="fas fa-certificate"></i>
                  <span>Bitirme sertifikası</span>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      {showLoginModal && (
        <div className="login-modal">
          <div className="login-modal-content">
            <h2>Giriş Yapmanız Gerekiyor</h2>
            <p>Bu kursu satın almak için lütfen giriş yapın veya kayıt olun.</p>
            <div className="login-modal-buttons">
              <button onClick={handleLoginRedirect} className="login-btn">
                Giriş Yap
              </button>
              <button onClick={() => setShowLoginModal(false)} className="cancel-btn">
                İptal
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default CourseDetail; 