import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './Cart.css';

const Cart = () => {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Kullanıcı girişi kontrolü
  useEffect(() => {
    // Burada gerçek auth kontrolü yapılacak
    const checkAuth = () => {
      const isLoggedIn = false; // Örnek olarak false
      setIsAuthenticated(isLoggedIn);
      
      if (!isLoggedIn) {
        navigate('/login', { state: { from: '/cart' } });
      }
    };

    checkAuth();
  }, [navigate]);

  // Örnek sepet verileri
  const [cartItems, setCartItems] = useState([
    {
      id: 1,
      title: "React ile Modern Web Geliştirme",
      instructor: "Ahmet Yılmaz",
      image: "https://picsum.photos/800/600",
      price: 199.99,
      originalPrice: 399.99,
      discount: 50
    },
    {
      id: 2,
      title: "Node.js Backend Development",
      instructor: "Mehmet Demir",
      image: "https://picsum.photos/800/601",
      price: 149.99,
      originalPrice: 299.99,
      discount: 50
    }
  ]);

  const removeFromCart = (id) => {
    setCartItems(cartItems.filter(item => item.id !== id));
  };

  const calculateTotal = () => {
    return cartItems.reduce((total, item) => total + item.price, 0);
  };

  if (!isAuthenticated) {
    return null;
  }

  return (
    <div className="cart-container">
      <div className="cart-content">
        <div className="cart-header">
          <h1>Sepetim</h1>
          <p>{cartItems.length} kurs</p>
        </div>

        <div className="cart-main">
          <div className="cart-items">
            {cartItems.length > 0 ? (
              cartItems.map(item => (
                <div key={item.id} className="cart-item">
                  <img src={item.image} alt={item.title} className="course-image" />
                  <div className="course-info">
                    <h3>{item.title}</h3>
                    <p className="instructor">Eğitmen: {item.instructor}</p>
                    <div className="price-info">
                      <span className="current-price">₺{item.price.toFixed(2)}</span>
                      <span className="original-price">₺{item.originalPrice.toFixed(2)}</span>
                      <span className="discount">%{item.discount} indirim</span>
                    </div>
                  </div>
                  <button 
                    onClick={() => removeFromCart(item.id)}
                    className="remove-btn"
                  >
                    <i className="fas fa-trash"></i>
                    Kaldır
                  </button>
                </div>
              ))
            ) : (
              <div className="empty-cart">
                <i className="fas fa-shopping-cart"></i>
                <h2>Sepetiniz boş</h2>
                <p>Hemen kurs keşfetmeye başlayın!</p>
                <Link to="/" className="browse-courses-btn">
                  Kursları Keşfet
                </Link>
              </div>
            )}
          </div>

          {cartItems.length > 0 && (
            <div className="cart-summary">
              <div className="summary-header">
                <h2>Toplam Tutar</h2>
              </div>
              <div className="summary-details">
                <div className="summary-row">
                  <span>Ara Toplam:</span>
                  <span>₺{calculateTotal().toFixed(2)}</span>
                </div>
                <div className="summary-row total">
                  <span>Toplam:</span>
                  <span>₺{calculateTotal().toFixed(2)}</span>
                </div>
                <button className="checkout-btn">
                  Ödemeye Geç
                </button>
                <div className="secure-checkout">
                  <i className="fas fa-lock"></i>
                  <span>Güvenli Ödeme</span>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Cart; 