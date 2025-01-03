import api from './api';

export const categoryService = {
  // Fetch all categories
  getAll: async () => {
    try {
      const response = await api.get('/api/Categories');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw new Error('Unable to fetch categories.');
    }
  },

  // Fetch a category by slug
  getBySlug: async (slug) => {
    try {
      const response = await api.get(`/api/Categories/by-slug/${slug}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with slug ${slug}:`, error);
      throw new Error('Unable to fetch category details.');
    }
  },

  // Fetch a category by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Categories/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with ID ${id}:`, error);
      throw new Error('Unable to fetch category details.');
    }
  },

  // Fetch all categories with courses
  getCategoriesWithCourses: async () => {
    try {
      const response = await api.get('/api/Categories/courses');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories with courses:', error);
      throw new Error('Unable to fetch categories with courses.');
    }
  },

  // Fetch a single category with its courses
  getCategoryWithCourses: async (id) => {
    try {
      const response = await api.get(`/api/Categories/${id}/courses`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with courses for ID ${id}:`, error);
      throw new Error('Unable to fetch category with courses.');
    }
  },

  // Create a new category
  create: async (categoryData) => {
    try {
      const response = await api.post('/api/Categories', categoryData);
      return response.data;
    } catch (error) {
      console.error('Error creating category:', error);
      throw new Error('Unable to create category.');
    }
  },

  // Update an existing category
  update: async (id, categoryData) => {
    try {
      const response = await api.put(`/api/Categories/${id}`, categoryData);
      return response.data;
    } catch (error) {
      console.error(`Error updating category with ID ${id}:`, error);
      throw new Error('Unable to update category.');
    }
  },

  // Soft delete a category
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Categories/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting category with ID ${id}:`, error);
      throw new Error('Unable to delete category.');
    }
  },
};