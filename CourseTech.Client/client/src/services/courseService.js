import api from './api';

// Course API endpoints
const COURSE_ENDPOINTS = {
  GET_BY_ID: (id) => `/api/Courses/${id}`,
  GET_ALL: '/api/Courses',
  GET_PUBLISHED: '/api/Courses/published',
  GET_BY_CATEGORY: (categoryId) => `/api/Courses/by-category/${categoryId}`,
  GET_BY_INSTRUCTOR: (instructorId) => `/api/Courses/by-instructor/${instructorId}`,
  GET_DETAILS: (id) => `/api/Courses/details/${id}`,
  GET_SUMMARIES: '/api/Courses/summaries',
  CREATE: '/api/Courses',
  UPDATE: '/api/Courses',
  DELETE: (id) => `/api/Courses/${id}`,
  PUBLISH: (id) => `/api/Courses/publish/${id}`,
  UNPUBLISH: (id) => `/api/Courses/unpublish/${id}`
};

/**
 * Get a course by ID
 * @param {string} id - Course ID
 * @returns {Promise} Response with course data
 */
export const getCourse = async (id) => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_BY_ID(id));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get all courses
 * @returns {Promise} Response with all courses
 */
export const getAllCourses = async () => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_ALL);
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get published courses
 * @returns {Promise} Response with published courses
 */
export const getPublishedCourses = async () => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_PUBLISHED);
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get courses by category
 * @param {string} categoryId - Category ID
 * @returns {Promise} Response with courses in category
 */
export const getCoursesByCategory = async (categoryId) => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_BY_CATEGORY(categoryId));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get courses by instructor
 * @param {string} instructorId - Instructor ID
 * @returns {Promise} Response with instructor's courses
 */
export const getCoursesByInstructor = async (instructorId) => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_BY_INSTRUCTOR(instructorId));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get course details
 * @param {string} id - Course ID
 * @returns {Promise} Response with detailed course information
 */
export const getCourseDetails = async (id) => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_DETAILS(id));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Get course summaries for cards
 * @returns {Promise} Response with course summaries
 */
export const getCourseSummaries = async () => {
  try {
    const response = await api.get(COURSE_ENDPOINTS.GET_SUMMARIES);
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Create a new course
 * @param {Object} courseDto - Course data
 * @returns {Promise} Response with created course
 */
export const createCourse = async (courseDto) => {
  try {
    const response = await api.post(COURSE_ENDPOINTS.CREATE, courseDto);
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Update an existing course
 * @param {Object} courseDto - Course data with ID
 * @returns {Promise} Response with updated course
 */
export const updateCourse = async (courseDto) => {
  try {
    const response = await api.put(COURSE_ENDPOINTS.UPDATE, courseDto);
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Delete a course
 * @param {string} id - Course ID
 * @returns {Promise} Response indicating success
 */
export const deleteCourse = async (id) => {
  try {
    const response = await api.delete(COURSE_ENDPOINTS.DELETE(id));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Publish a course
 * @param {string} id - Course ID
 * @returns {Promise} Response with published course
 */
export const publishCourse = async (id) => {
  try {
    const response = await api.patch(COURSE_ENDPOINTS.PUBLISH(id));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

/**
 * Unpublish a course
 * @param {string} id - Course ID
 * @returns {Promise} Response with unpublished course
 */
export const unpublishCourse = async (id) => {
  try {
    const response = await api.patch(COURSE_ENDPOINTS.UNPUBLISH(id));
    return response.data;
  } catch (error) {
    throw error.response?.data || error.message;
  }
};

export default {
  getCourse,
  getAllCourses,
  getPublishedCourses,
  getCoursesByCategory,
  getCoursesByInstructor,
  getCourseDetails,
  getCourseSummaries,
  createCourse,
  updateCourse,
  deleteCourse,
  publishCourse,
  unpublishCourse
}; 