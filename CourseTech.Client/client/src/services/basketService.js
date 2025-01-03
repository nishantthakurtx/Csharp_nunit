import api from './api';

export const basketService = {
    // Kullanıcının aktif sepetini getir
    getActiveBasket: async (userId) => {
        try {
            const response = await api.get(`/api/Baskets/${userId}`);
            console.log('Basket response:', response);
            
            // Sepet boş kontrolü
            if (!response.data?.data) {
                console.log('No basket found');
                return { data: null };
            }
            // API'den gelen veriyi dönüştür
            const mappedData = {
                data: {
                    id: response.data.data.id,
                    userId: response.data.data.userId,
                    courses: response.data.data.items?.map(item => ({
                        basketId: item.basketId,
                        courseId: item.courseId,
                        title: item.courseTitle,
                        price: item.price
                    })) || [],
                    totalPrice: response.data.data.items?.reduce((total, item) => total + (item.price || 0), 0) || 0,
                    status: response.data.data.status
                }
            };

            return mappedData;
        } catch (error) {
            console.error('Error fetching basket:', error);
            console.error('Error details:', {
                message: error.message,
                response: error.response?.data,
                status: error.response?.status
            });
            if (error.response?.status === 404) {
                return { data: null };
            }
            throw error;
        }
    },

    // Sepette kurs var mı kontrolü
    isItemInBasket: async (userId, courseId) => {
        try {
            const response = await basketService.getActiveBasket(userId);
            if (!response?.data?.courses) return false;
            
            return response.data.courses.some(item => item.courseId === courseId);
        } catch (error) {
            console.error('Error checking item in basket:', error);
            return false;
        }
    },

    // Sepete kurs ekle
    addCourseToBasket: async (userId, courseId) => {
        try {
            // Önce sepette var mı kontrol et
            const isInBasket = await basketService.isItemInBasket(userId, courseId);
            if (isInBasket) {
                throw new Error('This course is already in your basket');
            }
            const response = await api.post(`/api/Baskets/users/${userId}/courses/${courseId}`);
            return response.data;
        } catch (error) {
            if (error.message === 'This course is already in your basket') {
                throw error;
            }
            console.error('Error adding course to basket:', error);
            throw new Error('Unable to add course to basket.');
        }
    },

    // Sepetten kurs çıkar
    removeCourseFromBasket: async (userId, courseId) => {
        try {
            const response = await api.delete(`/api/Baskets/users/${userId}/courses/${courseId}`);
            return response.data;
        } catch (error) {
            console.error('Error removing course from basket:', error);
            throw new Error('Unable to remove course from basket.');
        }
    },

    // Sepeti temizle
    clearBasket: async (userId) => {
        try {
            const response = await api.delete(`/api/Baskets/users/${userId}`);
            return response.data;
        } catch (error) {
            console.error('Error clearing basket:', error);
            throw new Error('Unable to clear basket.');
        }
    },

    // Sepeti tamamla
    completeBasket: async (userId) => {
        try {
            const response = await api.post(`/api/Baskets/users/${userId}/complete`);
            return response.data;
        } catch (error) {
            console.error('Error completing basket:', error);
            throw new Error('Unable to complete basket.');
        }
    },

    // Sepeti detaylarıyla getir
    getBasketWithItems: async (basketId) => {
        try {
            const response = await api.get(`/api/Baskets/${basketId}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching basket details:', error);
            throw new Error('Unable to fetch basket details.');
        }
    }
}; 