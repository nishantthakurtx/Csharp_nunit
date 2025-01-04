import api from './api';

export const userService = {
    // Get user by ID
    getById: async (id) => {
        try {
            const response = await api.get(`/api/Users/${id}`);
            console.log('User details response:', response.data);
            return response.data;
        } catch (error) {
            console.error(`Error fetching user with ID ${id}:`, error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to fetch user details.');
        }
    },

    // Get all instructors
    getInstructors: async () => {
        try {
            const response = await api.get('/api/Users/instructors');
            console.log('Instructors response:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching instructors:', error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to fetch instructors.');
        }
    },

    // Get all students
    getStudents: async () => {
        try {
            const response = await api.get('/api/Users/students');
            console.log('Students response:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching students:', error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to fetch students.');
        }
    },

    // Get all users
    getAll: async () => {
        try {
            const response = await api.get('/api/Users/all');
            console.log('All users response:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching all users:', error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to fetch all users.');
        }
    },

    // Register new user
    register: async (userData) => {
        try {
            const response = await api.post('/api/Users/register', userData);
            console.log('User registration response:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error registering user:', error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to register user.');
        }
    },

    // Update user
    update: async (userData) => {
        try {
            const response = await api.put(`/api/Users/${userData.id}`, userData);
            console.log('User update response:', response.data);
            return response.data;
        } catch (error) {
            console.error(`Error updating user with ID ${userData.id}:`, error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to update user.');
        }
    },

    // Soft delete user
    delete: async (id) => {
        try {
            const response = await api.delete(`/api/Users/${id}`);
            console.log('User deletion response:', response.data);
            return response.data;
        } catch (error) {
            console.error(`Error deleting user with ID ${id}:`, error);
            console.error('Error details:', {
                status: error.response?.status,
                statusText: error.response?.statusText,
                data: error.response?.data,
                url: error.config?.url,
                message: error.message
            });
            throw new Error('Unable to delete user.');
        }
    }
}; 