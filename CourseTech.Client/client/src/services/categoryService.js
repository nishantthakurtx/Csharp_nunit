import api from './api';

export const categoryService = {
  // Get all categories
  getAll: async () => {
    try {
      console.log('Fetching categories...');
      const response = await api.get('/api/Categories');
      console.log('Categories response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch categories.');
    }
  },

  // Get category by slug
  getBySlug: async (slug) => {
    try {
      const response = await api.get(`/api/Categories/by-slug/${slug}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with slug ${slug}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch category details.');
    }
  },

  // Get category by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Categories/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with ID ${id}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch category details.');
    }
  },

  // Get all categories with courses
  getCategoriesWithCourses: async () => {
    try {
      const response = await api.get('/api/Categories/courses');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories with courses:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch categories with courses.');
    }
  },

  // Get category with its courses
  getCategoryWithCourses: async (id) => {
    try {
      const response = await api.get(`/api/Categories/${id}/courses`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with courses for ID ${id}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
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
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to create category.');
    }
  },

  // Update an existing category
  update: async (categoryData) => {
    try {
      const response = await api.put(`/api/Categories/${categoryData.id}`, categoryData);
      return response.data;
    } catch (error) {
      console.error(`Error updating category with ID ${categoryData.id}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
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
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to delete category.');
    }
  }
};