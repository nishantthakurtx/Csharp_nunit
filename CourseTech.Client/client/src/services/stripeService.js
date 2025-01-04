import api from './api';

export const stripeService = {
  // Create a new Stripe customer
  createCustomer: async (customerData) => {
    try {
      const response = await api.post('/api/Stripes/customer', customerData);
      console.log('Stripe customer creation response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error creating Stripe customer:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to create Stripe customer.');
    }
  },

  // Create a new Stripe charge
  createCharge: async (chargeData) => {
    try {
      const response = await api.post('/api/Stripes/charge', chargeData);
      console.log('Stripe charge creation response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error creating Stripe charge:', error);
      console.error('Error details:', {
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        message: error.message
      });
      throw new Error('Unable to create Stripe charge.');
    }
  }
}; 