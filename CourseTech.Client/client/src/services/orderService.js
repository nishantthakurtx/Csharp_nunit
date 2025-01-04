import api from './api';

export const orderService = {
  // Create order from basket
  createOrderFromBasket: async (basketId) => {
    try {
      const response = await api.post(`/api/Orders/from-basket/${basketId}`);
      console.log('Order creation response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error creating order from basket:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to create order from basket.');
    }
  },

  // Get order by ID
  getOrderById: async (orderId) => {
    try {
      const response = await api.get(`/api/Orders/${orderId}`);
      console.log('Order details response:', response.data);
      return response.data;
    } catch (error) {
      console.error(`Error fetching order with ID ${orderId}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch order details.');
    }
  },

  // Get orders by user ID
  getOrdersByUserId: async (userId) => {
    try {
      const response = await api.get(`/api/Orders/user/${userId}`);
      console.log('User orders response:', response.data);
      return response.data;
    } catch (error) {
      console.error(`Error fetching orders for user ${userId}:`, error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to fetch user orders.');
    }
  }
}; 