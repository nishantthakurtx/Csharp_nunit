import React from 'react';
import CourseCard from '../components/CourseCard';
import './Home.css';

const Home = () => {
  // Örnek kategoriler
  const categories = [
    'Yazılım Geliştirme',
    'İş',
    'Finans',
    'Tasarım',
    'Marketing',
    'Kişisel Gelişim',
    'Sağlık',
    'Müzik'
  ];

  // Örnek kurslar
  const courses = [
    {
      id: 1,
      title: 'Modern Web Geliştirme Kursu',
      instructor: 'Ahmet Yılmaz',
      description: 'React, Node.js ve MongoDB ile modern web uygulamaları geliştirmeyi öğrenin.',
      price: 199.99,
      rating: 4.8,
      students: 1234,
      image: 'https://via.placeholder.com/300x200?text=Web+Development'
    },
    {
      id: 2,
      title: 'Python ile Veri Bilimi',
      instructor: 'Ayşe Demir',
      description: 'Python programlama ve veri bilimi temellerini öğrenin.',
      price: 149.99,
      rating: 4.6,
      students: 856,
      image: 'https://via.placeholder.com/300x200?text=Data+Science'
    },
    {
      id: 3,
      title: 'UI/UX Tasarım Temelleri',
      instructor: 'Mehmet Kaya',
      description: 'Modern kullanıcı arayüzü tasarımı prensiplerini öğrenin.',
      price: 179.99,
      rating: 4.7,
      students: 2341,
      image: 'https://via.placeholder.com/300x200?text=UI+Design'
    },
    {
      id: 4,
      title: 'JavaScript Masterclass',
      instructor: 'Zeynep Şahin',
      description: 'Modern JavaScript ile web uygulamaları geliştirmeyi öğrenin.',
      price: 229.99,
      rating: 4.9,
      students: 3421,
      image: 'https://via.placeholder.com/300x200?text=JavaScript'
    }
  ];

  return (
    <div className="home">
      {/* Hero Section */}
      <section className="hero-section">
        <div className="hero-content">
          <h1>Yeni Beceriler Öğrenin</h1>
          <p>Binlerce kurs arasından size uygun olanı seçin ve kariyerinizi geliştirin.</p>
        </div>
      </section>

      {/* Categories Section */}
      <section className="categories-section">
        <div className="section-container">
          <h2>Popüler Kategoriler</h2>
          <div className="categories-grid">
            {categories.map((category, index) => (
              <button key={index} className="category-btn">
                {category}
              </button>
            ))}
          </div>
        </div>
      </section>

      {/* Featured Courses Section */}
      <section className="courses-section">
        <div className="section-container">
          <div className="section-header">
            <h2>Öne Çıkan Kurslar</h2>
            <button className="see-all-btn">Tümünü Gör</button>
          </div>
          <div className="courses-grid">
            {courses.map(course => (
              <CourseCard key={course.id} course={course} />
            ))}
          </div>
        </div>
      </section>

      {/* Benefits Section */}
      <section className="benefits-section">
        <div className="section-container">
          <h2>Neden CourseTech?</h2>
          <div className="benefits-grid">
            <div className="benefit-card">
              <i className="fas fa-laptop-code"></i>
              <h3>Uzman Eğitmenler</h3>
              <p>Alanında uzman eğitmenlerden öğrenin</p>
            </div>
            <div className="benefit-card">
              <i className="fas fa-clock"></i>
              <h3>Ömür Boyu Erişim</h3>
              <p>Satın aldığınız kurslara ömür boyu erişin</p>
            </div>
            <div className="benefit-card">
              <i className="fas fa-certificate"></i>
              <h3>Sertifika</h3>
              <p>Kurs bitiminde sertifikanızı alın</p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home; 