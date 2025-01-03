import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { basketService } from '../services/basketService';
import { useAuth } from '../contexts/AuthContext';
import PageLayout from '../components/PageLayout';
import alertify from 'alertifyjs';
import 'alertifyjs/build/css/alertify.css';
import 'alertifyjs/build/css/themes/default.css';
import './Basket.css';

const Basket = () => {
  const navigate = useNavigate();
  const { isAuthenticated, user } = useAuth();
  const [basket, setBasket] = useState({ courses: [], totalPrice: 0 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchBasket = async () => {
      if (!isAuthenticated || !user?.id) {
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        const response = await basketService.getActiveBasket(user.id);
        
        if (!response?.data) {
          setBasket(null);
        } else {
          setBasket({
            id: response.data.id,
            courses: response.data.courses,
            totalPrice: response.data.totalPrice,
            status: response.data.status
          });
        }
      } catch (error) {
        console.error('Error fetching basket:', error);
        setError('Failed to load basket. Please try again.');
      } finally {
        setLoading(false);
      }
    };

    fetchBasket();
  }, [isAuthenticated, user?.id]);

  const handleRemoveFromBasket = async (courseId, courseTitle) => {
    if (!user?.id || !basket?.id) return;

    alertify.confirm('Remove Course', 
      `Are you sure you want to remove "${courseTitle}" from your basket?`,
      async function() {
        try {
          await basketService.removeCourseFromBasket(user.id, courseId);
          const response = await basketService.getActiveBasket(user.id);
          if (!response?.data) {
            setBasket(null);
          } else {
            setBasket({
              id: response.data.id,
              courses: response.data.courses,
              totalPrice: response.data.totalPrice,
              status: response.data.status
            });
          }
          window.dispatchEvent(new Event('basketUpdated'));
          alertify.success('Course removed successfully');
        } catch (error) {
          console.error('Error removing course from basket:', error);
          alertify.error('Failed to remove course from basket');
        }
      },
      function() {
        alertify.error('Operation cancelled');
      }
    ).set({
      'labels': {ok: 'Yes', cancel: 'No'},
      'defaultFocus': 'cancel'
    });
  };

  const handleCheckout = async () => {
    if (!user?.id) return;

    try {
      await basketService.completeBasket(user.id);
      setBasket({ courses: [], totalPrice: 0 });
      // TODO: Başarılı mesajı göster ve/veya ödeme sayfasına yönlendir
    } catch (error) {
      console.error('Error completing checkout:', error);
      setError('Failed to complete checkout. Please try again.');
    }
  };

  if (!isAuthenticated) {
    return (
      <PageLayout>
        <div className="basket-container">
          <div className="empty-basket">
            <i className="fas fa-lock"></i>
            <h2>Please sign in to view your basket</h2>
            <p>Sign in to access your basket and continue shopping</p>
            <Link to="/login" className="btn-primary">Sign In</Link>
          </div>
        </div>
      </PageLayout>
    );
  }

  if (loading) {
    return (
      <PageLayout>
        <div className="basket-container">
          <div className="loading">
            <div className="loader"></div>
            <p>Loading your basket...</p>
          </div>
        </div>
      </PageLayout>
    );
  }

  if (error) {
    return (
      <PageLayout>
        <div className="basket-container">
          <div className="error-message">
            <i className="fas fa-exclamation-circle"></i>
            <h2>{error}</h2>
            <button onClick={() => window.location.reload()} className="btn-primary">
              Try Again
            </button>
          </div>
        </div>
      </PageLayout>
    );
  }

  if (!basket) {
    return (
      <PageLayout>
        <div className="basket-container">
          <div className="empty-basket">
            <i className="fas fa-shopping-cart"></i>
            <h2>Your basket is empty</h2>
            <p>Discover amazing courses and start learning today!</p>
            <Link to="/" className="btn-primary">Browse Courses</Link>
          </div>
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout>
      <div className="basket-container">
        <div className="basket-content">
          <div className="basket-header">
            <h1>Shopping Basket</h1>
            <p>{basket.courses.length} courses in basket</p>
          </div>

          <div className="basket-main">
            <div className="basket-items">
              {basket.courses.map(course => (
                <div key={course.courseId} className="basket-item">
                  <div className="course-image-container">
                    {course.imageUrl && (
                      <img 
                        src={course.imageUrl} 
                        alt={course.title}
                      />
                    )}
                  </div>
                  <div className="course-info">
                    <h3>{course.title}</h3>
                    <div className="instructor">By {course.instructorName}</div>
                    <div className="course-meta">
                      <span key="level"><i className="fas fa-signal"></i>{course.level}</span>
                      <span key="duration"><i className="fas fa-clock"></i>{course.duration}</span>
                    </div>
                    <div className="price-info">
                      <span className="current-price">${(course.price || 0).toFixed(2)}</span>
                    </div>
                  </div>
                  <button 
                    className="remove-btn"
                    onClick={() => handleRemoveFromBasket(course.courseId, course.title)}
                  >
                    <i className="fas fa-trash"></i>
                  </button>
                </div>
              ))}
              
              {basket.courses.length > 0 && (
                <div className="basket-actions">
                  <button 
                    className="clear-basket-btn"
                    onClick={() => {
                      alertify.confirm('Clear Basket', 
                        'Are you sure you want to clear your basket?',
                        function() {
                          basketService.clearBasket(user.id).then(() => {
                            setBasket({ courses: [], totalPrice: 0 });
                            window.dispatchEvent(new Event('basketUpdated'));
                            alertify.success('Basket cleared successfully');
                          });
                        },
                        function() {
                          alertify.error('Operation cancelled');
                        }
                      ).set({
                        'labels': {ok: 'Yes', cancel: 'No'},
                        'defaultFocus': 'cancel',
                        'movable': false,
                        'closable': false
                      });
                    }}
                  >
                    <i className="fas fa-trash-alt"></i>
                    Clear Basket
                  </button>
                </div>
              )}
            </div>

            <div className="basket-summary">
              <div className="summary-header">
                <h2>Total</h2>
              </div>
              <div className="summary-details">
                <div className="summary-row">
                  <span>Subtotal:</span>
                  <span>${(basket.totalPrice || 0).toFixed(2)}</span>
                </div>
                <div className="summary-row total">
                  <span>Total:</span>
                  <span>${(basket.totalPrice || 0).toFixed(2)}</span>
                </div>
                <button 
                  className="checkout-btn"
                  onClick={handleCheckout}
                >
                  Proceed to Checkout
                </button>
                <div className="secure-checkout">
                  <i className="fas fa-lock"></i>
                  <span>Secure Checkout</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </PageLayout>
  );
};

export default Basket; 