import api from './api';

export const userService = {
    getAll: async () => {
        try {
            const response = await api.get('/api/Users/all');
            return response.data;
        } catch (error) {
            console.error('Error fetching users:', error);
            throw new Error('Unable to fetch users.');
        }
    },

    getById: async (id) => {
        try {
            const response = await api.get(`/api/Users/${id}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching user:', error);
            throw new Error('Unable to fetch user details.');
        }
    },

    getInstructors: async () => {
        try {
            const response = await api.get('/api/Users/instructors');
            return response.data;
        } catch (error) {
            console.error('Error fetching instructors:', error);
            throw new Error('Unable to fetch instructors.');
        }
    },

    getStudents: async () => {
        try {
            const response = await api.get('/api/Users/students');
            return response.data;
        } catch (error) {
            console.error('Error fetching students:', error);
            throw new Error('Unable to fetch students.');
        }
    },

    register: async (userData) => {
        try {
            const response = await api.post('/api/Users/register', userData);
            return response.data;
        } catch (error) {
            console.error('Error registering user:', error);
            throw new Error('Unable to register user.');
        }
    },

    update: async (userData) => {
        try {
            const response = await api.put(`/api/Users/${userData.id}`, userData);
            return response.data;
        } catch (error) {
            console.error('Error updating user:', error);
            throw new Error('Unable to update user.');
        }
    },

    delete: async (id) => {
        try {
            const response = await api.delete(`/api/Users/${id}`);
            return response.data;
        } catch (error) {
            console.error('Error deleting user:', error);
            throw new Error('Unable to delete user.');
        }
    }
}; 