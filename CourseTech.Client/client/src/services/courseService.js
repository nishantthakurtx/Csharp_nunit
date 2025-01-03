import api from './api';

export const courseService = {
  // Get all courses (admin için)
  getAllCourses: async () => {
    try {
      const response = await api.get('/api/Courses');
      return response.data;
    } catch (error) {
      console.error('Error fetching all courses:', error);
      throw error;
    }
  },

  // Get published courses (ana sayfa ve browse için)
  getPublishedCourses: async () => {
    try {
      const response = await api.get('/api/Courses/published');
      return response.data;
    } catch (error) {
      console.error('Error fetching published courses:', error);
      throw error;
    }
  },

  // Get course summaries for cards (ana sayfa için)
  getAllCourseSummaries: async () => {
    try {
      const response = await api.get('/api/Courses/summaries');
      return response.data;
    } catch (error) {
      console.error('Error fetching course summaries:', error);
      throw error;
    }
  },

  // Get single course by ID
  getCourseById: async (id) => {
    try {
      const response = await api.get(`/api/Courses/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching course:', error);
      throw error;
    }
  },

  // Get course with details
  getCourseWithDetails: async (id) => {
    try {
      const response = await api.get(`/api/Courses/details/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching course details:', error);
      throw error;
    }
  },

  // Get courses by category
  getCoursesByCategory: async (categoryId) => {
    try {
      const response = await api.get(`/api/Courses/by-category/${categoryId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching courses by category:', error);
      throw error;
    }
  },

  // Get courses by instructor
  getCoursesByInstructor: async (instructorId) => {
    try {
      const response = await api.get(`/api/Courses/by-instructor/${instructorId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching courses by instructor:', error);
      throw error;
    }
  },

  // Create new course
  createCourse: async (courseData) => {
    try {
      const response = await api.post('/api/Courses', courseData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Update course
  updateCourse: async (courseData) => {
    try {
      const response = await api.put('/api/Courses', courseData);
      return response.data;
    } catch (error) {
      console.error('Error updating course:', error);
      throw error;
    }
  },

  // Soft delete course
  deleteCourse: async (id) => {
    try {
      const response = await api.delete(`/api/Courses/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting course:', error);
      throw error;
    }
  },

  // Publish course
  publishCourse: async (id) => {
    try {
      const response = await api.patch(`/api/Courses/publish/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error publishing course:', error);
      throw error;
    }
  },

  // Unpublish course
  unpublishCourse: async (id) => {
    try {
      const response = await api.patch(`/api/Courses/unpublish/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error unpublishing course:', error);
      throw error;
    }
  }
}; 