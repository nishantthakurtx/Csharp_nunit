import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { categoryService } from '../services/categoryService';
import slugify from 'react-slugify';
import PageLayout from '../components/PageLayout';
import './Categories.css';

const Categories = () => {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setLoading(true);
        const response = await categoryService.getAll();
        if (response?.data) {
          setCategories(response.data);
        }
      } catch (err) {
        setError('Failed to load categories. Please try again later.');
        console.error('Error fetching categories:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  if (loading) {
    return (
      <PageLayout>
        <div className="loading-container">
          <div className="loader"></div>
          <p>Loading categories...</p>
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
      <div className="categories-page">
        <h1>All Categories</h1>
        <div className="categories-grid">
          {categories.map(category => (
            <Link 
              to={`/category/${slugify(category.name)}`}
              key={category.id}
              className="category-card"
            >
              <i className={`fas ${category.name}`}></i>
              <h2>{category.name}</h2>
              <p>{category.courseCount || 0} courses</p>
            </Link>
          ))}
        </div>
      </div>
    </PageLayout>
  );
};

export default Categories; 