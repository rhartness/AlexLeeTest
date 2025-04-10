import axios from 'axios';

const API_URL = 'http://localhost:5268';

// Create axios instance with default configs
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

export const purchaseDetailItemsService = {
    getItems: async (filters = {}) => {
      const params = new URLSearchParams();
      
      // Add any provided filters to the query params
      if (filters.purchaseOrderNumber) params.append('purchaseOrderNumber', filters.purchaseOrderNumber);
      if (filters.itemNumber) params.append('itemNumber', filters.itemNumber);
      if (filters.itemName) params.append('itemName', filters.itemName);
      if (filters.itemDescription) params.append('itemDescription', filters.itemDescription);
      if (filters.purchasePrice) params.append('purchasePrice', filters.purchasePrice);
      if (filters.purchaseQuantity) params.append('purchaseQuantity', filters.purchaseQuantity);
      
      try {
        const response = await apiClient.get(`/api/PurchaseDetailItems/Search?${params}`);
        return response.data;
      } catch (error) {
        console.error('Error fetching purchase detail items:', error);
        throw error;
      }
    },
    
    // Add a new item
    addItem: async (itemData) => {
      try {
        const response = await apiClient.post('/api/PurchaseDetailItems/Add', itemData);
        return response.data;
      } catch (error) {
        console.error('Error adding purchase detail item:', error);
        throw error;
      }
    },
    
    // Update an existing item
    updateItem: async (itemData) => {
      try {
        const response = await apiClient.post('/api/PurchaseDetailItems/Update', itemData);
        return response.data;
      } catch (error) {
        console.error('Error updating purchase detail item:', error);
        throw error;
      }
    }
  };