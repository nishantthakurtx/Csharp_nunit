import api from './api';

export const enrollmentService = {
  // Enroll user in a course
  enroll: async (userId, courseId) => {
    try {
      const response = await api.post('/api/Enrollments', { userId, courseId });
      console.log('Enrollment response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error enrolling in course:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to enroll in course.');
    }
  },

  // Unenroll user from a course
  unenroll: async (userId, courseId) => {
    try {
      const response = await api.delete('/api/Enrollments', { 
        data: { userId, courseId } 
      });
      console.log('Unenrollment response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error unenrolling from course:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to unenroll from course.');
    }
  },

  // Get all courses enrolled by a user
  getEnrolledCoursesByUser: async (userId) => {
    try {
      const response = await api.get(`/api/Enrollments/user/${userId}`);
      console.log('Enrolled courses response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error fetching enrolled courses:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch enrolled courses.');
    }
  },

  // Get all users enrolled in a course
  getEnrolledUsersByCourse: async (courseId) => {
    try {
      const response = await api.get(`/api/Enrollments/course/${courseId}`);
      console.log('Enrolled users response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error fetching enrolled users:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch enrolled users.');
    }
  }
}; 