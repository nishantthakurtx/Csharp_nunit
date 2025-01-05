import React, { createContext, useContext, useState, useEffect } from 'react';
import categoryService from '../services/categoryService';

const CategoryContext = createContext();

export const useCategory = () => {
  const context = useContext(CategoryContext);
  if (!context) {
    throw new Error('useCategory must be used within a CategoryProvider');
  }
  return context;
};

export const CategoryProvider = ({ children }) => {
  const [categories, setCategories] = useState([]);
  const [categoriesWithCourses, setCategoriesWithCourses] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  // Kategorileri yükle
  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await categoryService.getAllCategories();
      setCategories(response.data);
    } catch (err) {
      setError('Failed to load categories');
      console.error('Load categories error:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const loadCategoriesWithCourses = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await categoryService.getCategoriesWithCourses();
      setCategoriesWithCourses(response.data);
    } catch (err) {
      setError('Failed to load categories with courses');
      console.error('Load categories with courses error:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const getCategoryWithCourses = async (categoryId) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await categoryService.getCategoryWithCourses(categoryId);
      return response.data;
    } catch (err) {
      setError('Failed to load category details');
      console.error('Load category details error:', err);
      return null;
    } finally {
      setIsLoading(false);
    }
  };

  const createCategory = async (categoryData) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await categoryService.createCategory(categoryData);
      setCategories(prevCategories => [...prevCategories, response.data]);
      return response.data;
    } catch (err) {
      setError('Failed to create category');
      console.error('Create category error:', err);
      return null;
    } finally {
      setIsLoading(false);
    }
  };

  const updateCategory = async (categoryData) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await categoryService.updateCategory(categoryData);
      setCategories(prevCategories =>
        prevCategories.map(cat =>
          cat.id === categoryData.id ? response.data : cat
        )
      );
      return response.data;
    } catch (err) {
      setError('Failed to update category');
      console.error('Update category error:', err);
      return null;
    } finally {
      setIsLoading(false);
    }
  };

  const deleteCategory = async (categoryId) => {
    setIsLoading(true);
    setError(null);
    try {
      await categoryService.deleteCategory(categoryId);
      setCategories(prevCategories =>
        prevCategories.filter(cat => cat.id !== categoryId)
      );
      return true;
    } catch (err) {
      setError('Failed to delete category');
      console.error('Delete category error:', err);
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  const value = {
    categories,
    categoriesWithCourses,
    isLoading,
    error,
    loadCategories,
    loadCategoriesWithCourses,
    getCategoryWithCourses,
    createCategory,
    updateCategory,
    deleteCategory
  };

  return (
    <CategoryContext.Provider value={value}>
      {children}
    </CategoryContext.Provider>
  );
};

export default CategoryContext; 