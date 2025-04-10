// components/PurchaseItemDetails.jsx
import { useState, useEffect } from 'react';
import { purchaseDetailItemsService } from '../services/purchaseDetailItemsService';

export default function PurchaseItemDetails() {
  // State for items and loading/error status
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // State for filters
  const [filters, setFilters] = useState({
    purchaseOrderNumber: '',
    itemNumber: '',
    itemName: '',
    itemDescription: '',
    purchasePrice: '',
    purchaseQuantity: ''
  });
  
  // State for add/edit item forms
  const [newItem, setNewItem] = useState({
    purchaseOrderNumber: '',
    itemNumber: '',
    itemName: '',
    itemDescription: '',
    purchasePrice: '',
    purchaseQuantity: ''
  });
  
  const [editItem, setEditItem] = useState({
    purchaseDetailItemAutoId: '',
    purchaseOrderNumber: '',
    itemNumber: '',
    itemName: '',
    itemDescription: '',
    purchasePrice: '',
    purchaseQuantity: ''
  });
  
  // State for modals
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [addItemError, setAddItemError] = useState('');
  const [editItemError, setEditItemError] = useState('');
  
  // Fetch items on component mount and when filters change
  useEffect(() => {
    fetchItems();
  }, []);
  
  const fetchItems = async (filterParams = {}) => {
    try {
      setLoading(true);
      const data = await purchaseDetailItemsService.getItems(filterParams);
      setItems(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch items: ' + err.message);
      console.error(err);
    } finally {
      setLoading(false);
    }
  };
  
  // Handle filter changes
  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters(prev => ({
      ...prev,
      [name]: value
    }));
  };
  
  // Apply filters
  const handleApplyFilter = (e) => {
    e.preventDefault();
    // Filter out empty values
    const activeFilters = Object.fromEntries(
      Object.entries(filters).filter(([_, value]) => value !== '')
    );
    fetchItems(activeFilters);
  };
  
  // Clear filters
  const handleClearFilter = () => {
    setFilters({
      purchaseOrderNumber: '',
      itemNumber: '',
      itemName: '',
      itemDescription: '',
      purchasePrice: '',
      purchaseQuantity: ''
    });
    fetchItems();
  };
  
  // Handle input change for new item
  const handleNewItemChange = (e) => {
    const { id, value } = e.target;
    const fieldName = id.replace('new', '').charAt(0).toLowerCase() + id.replace('new', '').slice(1);
    setNewItem(prev => ({
      ...prev,
      [fieldName]: value
    }));
  };
  
  // Handle input change for edit item
  const handleEditItemChange = (e) => {
    const { id, value } = e.target;
    const fieldName = id.replace('edit', '').charAt(0).toLowerCase() + id.replace('edit', '').slice(1);
    setEditItem(prev => ({
      ...prev,
      [fieldName]: value
    }));
  };
  
  // Open edit modal and populate data
  const handleOpenEditModal = (item) => {
    setEditItem({
      purchaseDetailItemAutoId: item.purchaseDetailItemAutoId,
      purchaseOrderNumber: item.purchaseOrderNumber,
      itemNumber: item.itemNumber,
      itemName: item.itemName,
      itemDescription: item.itemDescription || '',
      purchasePrice: item.purchasePrice,
      purchaseQuantity: item.purchaseQuantity
    });
    setEditItemError('');
    setShowEditModal(true);
  };
  
  // Add new item
  const handleAddItem = async () => {
    // Validate required fields
    if (!newItem.purchaseOrderNumber || !newItem.itemNumber || !newItem.itemName || 
        !newItem.purchasePrice || !newItem.purchaseQuantity) {
      setAddItemError('Please fill in all required fields.');
      return;
    }
    
    try {
      const formattedItem = {
        ...newItem,
        itemNumber: parseInt(newItem.itemNumber),
        purchasePrice: parseFloat(newItem.purchasePrice),
        purchaseQuantity: parseInt(newItem.purchaseQuantity)
      };
      
      const addedItem = await purchaseDetailItemsService.addItem(formattedItem);
      setItems(prev => [...prev, addedItem]);
      setShowAddModal(false);
      
      // Reset form
      setNewItem({
        purchaseOrderNumber: '',
        itemNumber: '',
        itemName: '',
        itemDescription: '',
        purchasePrice: '',
        purchaseQuantity: ''
      });
      
      setAddItemError('');
    } catch (err) {
      setAddItemError('Failed to add item: ' + err.message);
    }
  };
  
  // Update item
  const handleUpdateItem = async () => {
    // Validate required fields
    if (!editItem.purchaseOrderNumber || !editItem.itemNumber || !editItem.itemName || 
        !editItem.purchasePrice || !editItem.purchaseQuantity) {
      setEditItemError('Please fill in all required fields.');
      return;
    }
    
    try {
      const formattedItem = {
        ...editItem,
        itemNumber: parseInt(editItem.itemNumber),
        purchasePrice: parseFloat(editItem.purchasePrice),
        purchaseQuantity: parseInt(editItem.purchaseQuantity)
      };
      
      const updatedItem = await purchaseDetailItemsService.updateItem(formattedItem);
      setItems(prevItems => 
        prevItems.map(item => 
          item.purchaseDetailItemAutoId === editItem.purchaseDetailItemAutoId 
            ? updatedItem 
            : item
        )
      );
      setShowEditModal(false);
      setEditItemError('');
    } catch (err) {
      setEditItemError('Failed to update item: ' + err.message);
    }
  };
  
  if (loading) return (
    <div className="d-flex justify-content-center">
      <div className="spinner-border" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
    </div>
  );
  
  return (
    <div className="container-fluid">
      <h2>Purchase Details</h2>
      
      {error && (
        <div className="alert alert-danger">{error}</div>
      )}
      
      <div className="row mb-3">
        <div className="col-md-12">
          <button type="button" className="btn btn-primary" onClick={() => setShowAddModal(true)}>
            Add New Item
          </button>
        </div>
      </div>
      
      {/* Filter Card */}
      <div className="card mb-4">
        <div className="card-header">
          <h5>Filter</h5>
        </div>
        <div className="card-body">
          <form onSubmit={handleApplyFilter}>
            <div className="row">
              <div className="alert alert-info">
                PLEASE NOTE THAT THIS IS A NAIVE FILTER. ALL VALUES MUST MATCH A VALUE IN THE SPECIFIC FIELD!
              </div>
            </div>
            <div className="row">
              <div className="col-md-4 mb-3">
                <label htmlFor="purchaseOrderNumber" className="form-label">Purchase Order Number</label>
                <input 
                  id="purchaseOrderNumber" 
                  name="purchaseOrderNumber" 
                  className="form-control" 
                  value={filters.purchaseOrderNumber}
                  onChange={handleFilterChange}
                />
              </div>
              <div className="col-md-4 mb-3">
                <label htmlFor="itemNumber" className="form-label">Item Number</label>
                <input 
                  id="itemNumber" 
                  name="itemNumber" 
                  className="form-control" 
                  type="number" 
                  value={filters.itemNumber}
                  onChange={handleFilterChange}
                />
              </div>
              <div className="col-md-4 mb-3">
                <label htmlFor="itemName" className="form-label">Item Name</label>
                <input 
                  id="itemName" 
                  name="itemName" 
                  className="form-control" 
                  value={filters.itemName}
                  onChange={handleFilterChange}
                />
              </div>
            </div>
            <div className="row">
              <div className="col-md-4 mb-3">
                <label htmlFor="itemDescription" className="form-label">Item Description</label>
                <input 
                  id="itemDescription" 
                  name="itemDescription" 
                  className="form-control" 
                  value={filters.itemDescription}
                  onChange={handleFilterChange}
                />
              </div>
              <div className="col-md-4 mb-3">
                <label htmlFor="purchasePrice" className="form-label">Purchase Price</label>
                <input 
                  id="purchasePrice" 
                  name="purchasePrice" 
                  className="form-control" 
                  type="number" 
                  step="0.01" 
                  value={filters.purchasePrice}
                  onChange={handleFilterChange}
                />
              </div>
              <div className="col-md-4 mb-3">
                <label htmlFor="purchaseQuantity" className="form-label">Purchase Quantity</label>
                <input 
                  id="purchaseQuantity" 
                  name="purchaseQuantity" 
                  className="form-control" 
                  type="number" 
                  value={filters.purchaseQuantity}
                  onChange={handleFilterChange}
                />
              </div>
            </div>
            <div className="row">
              <div className="col-md-12">
                <button type="submit" className="btn btn-primary">Apply Filter</button>
                <button type="button" className="btn btn-secondary ms-2" onClick={handleClearFilter}>Clear Filter</button>
              </div>
            </div>
          </form>
        </div>
      </div>
      
      {/* Data Card */}
      <div className="card">
        <div className="card-header">
          <h5>Purchase Details</h5>
        </div>
        <div className="card-body">
          {items.length > 0 ? (
            <div className="table-responsive">
              <table className="table table-striped table-hover">
                <thead>
                  <tr>
                    <th>Purchase Order #</th>
                    <th>Item #</th>
                    <th>Item Name</th>
                    <th>Description</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Total</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {items.map(item => (
                    <tr key={item.purchaseDetailItemAutoId}>
                      <td>{item.purchaseOrderNumber}</td>
                      <td>{item.itemNumber}</td>
                      <td>{item.itemName}</td>
                      <td>{item.itemDescription}</td>
                      <td>${item.purchasePrice.toFixed(2)}</td>
                      <td>{item.purchaseQuantity}</td>
                      <td>${(item.purchasePrice * item.purchaseQuantity).toFixed(2)}</td>
                      <td>
                        <button 
                          type="button"
                          className="btn btn-sm btn-warning"
                          onClick={() => handleOpenEditModal(item)}
                        >
                          Edit
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : (
            <p>No purchase detail items found.</p>
          )}
        </div>
      </div>
      
      {/* Add Item Modal */}
      {showAddModal && (
        <div className="modal show d-block" tabIndex="-1">
          <div className="modal-dialog modal-lg">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Add New Purchase Detail Item</h5>
                <button type="button" className="btn-close" onClick={() => setShowAddModal(false)}></button>
              </div>
              <div className="modal-body">
                {addItemError && (
                  <div className="alert alert-danger">{addItemError}</div>
                )}
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="newPurchaseOrderNumber" className="form-label">Purchase Order Number *</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="newPurchaseOrderNumber" 
                      value={newItem.purchaseOrderNumber}
                      onChange={handleNewItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="newItemNumber" className="form-label">Item Number *</label>
                    <input 
                      type="number" 
                      className="form-control" 
                      id="newItemNumber" 
                      value={newItem.itemNumber}
                      onChange={handleNewItemChange}
                      required 
                    />
                  </div>
                </div>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="newItemName" className="form-label">Item Name *</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="newItemName" 
                      value={newItem.itemName}
                      onChange={handleNewItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="newItemDescription" className="form-label">Item Description</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="newItemDescription" 
                      value={newItem.itemDescription}
                      onChange={handleNewItemChange}
                    />
                  </div>
                </div>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="newPurchasePrice" className="form-label">Purchase Price *</label>
                    <input 
                      type="number" 
                      step="0.01" 
                      className="form-control" 
                      id="newPurchasePrice" 
                      value={newItem.purchasePrice}
                      onChange={handleNewItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="newPurchaseQuantity" className="form-label">Purchase Quantity *</label>
                    <input 
                      type="number" 
                      className="form-control" 
                      id="newPurchaseQuantity" 
                      value={newItem.purchaseQuantity}
                      onChange={handleNewItemChange}
                      required 
                    />
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => setShowAddModal(false)}>Cancel</button>
                <button type="button" className="btn btn-primary" onClick={handleAddItem}>Save</button>
              </div>
            </div>
          </div>
        </div>
      )}
      
      {/* Edit Item Modal */}
      {showEditModal && (
        <div className="modal show d-block" tabIndex="-1">
          <div className="modal-dialog modal-lg">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Edit Purchase Detail Item</h5>
                <button type="button" className="btn-close" onClick={() => setShowEditModal(false)}></button>
              </div>
              <div className="modal-body">
                {editItemError && (
                  <div className="alert alert-danger">{editItemError}</div>
                )}
                <input 
                  type="hidden" 
                  id="editPurchaseDetailItemAutoId" 
                  value={editItem.purchaseDetailItemAutoId}
                />
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="editPurchaseOrderNumber" className="form-label">Purchase Order Number *</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="editPurchaseOrderNumber" 
                      value={editItem.purchaseOrderNumber}
                      onChange={handleEditItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="editItemNumber" className="form-label">Item Number *</label>
                    <input 
                      type="number" 
                      className="form-control" 
                      id="editItemNumber" 
                      value={editItem.itemNumber}
                      onChange={handleEditItemChange}
                      required 
                    />
                  </div>
                </div>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="editItemName" className="form-label">Item Name *</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="editItemName" 
                      value={editItem.itemName}
                      onChange={handleEditItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="editItemDescription" className="form-label">Item Description</label>
                    <input 
                      type="text" 
                      className="form-control" 
                      id="editItemDescription" 
                      value={editItem.itemDescription}
                      onChange={handleEditItemChange}
                    />
                  </div>
                </div>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="editPurchasePrice" className="form-label">Purchase Price *</label>
                    <input 
                      type="number" 
                      step="0.01" 
                      className="form-control" 
                      id="editPurchasePrice" 
                      value={editItem.purchasePrice}
                      onChange={handleEditItemChange}
                      required 
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="editPurchaseQuantity" className="form-label">Purchase Quantity *</label>
                    <input 
                      type="number" 
                      className="form-control" 
                      id="editPurchaseQuantity" 
                      value={editItem.purchaseQuantity}
                      onChange={handleEditItemChange}
                      required 
                    />
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => setShowEditModal(false)}>Cancel</button>
                <button type="button" className="btn btn-primary" onClick={handleUpdateItem}>Update</button>
              </div>
            </div>
          </div>
        </div>
      )}
      
      {/* Modal Backdrop */}
      {(showAddModal || showEditModal) && (
        <div className="modal-backdrop show"></div>
      )}
    </div>
  );
}








// // components/PurchaseItemDetails.jsx
// import { useState, useEffect } from 'react';
// import { purchaseDetailItemsService } from '../services/purchaseDetailItemsService';

// export default function PurchaseItemDetails() {
//   const [items, setItems] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState(null);
  
//   // Fetch items on component mount
//   useEffect(() => {
//     const fetchItems = async () => {
//       try {
//         setLoading(true);
//         const data = await purchaseDetailItemsService.getItems();
//         setItems(data);
//         setError(null);
//       } catch (err) {
//         setError('Failed to fetch items');
//         console.error(err);
//       } finally {
//         setLoading(false);
//       }
//     };
    
//     fetchItems();
//   }, []);
  
//   // Example function to add a new item
//   const handleAddItem = async (newItem) => {
//     try {
//       const addedItem = await purchaseDetailItemsService.addItem(newItem);
//       setItems(prevItems => [...prevItems, addedItem]);
//     } catch (err) {
//       setError('Failed to add item');
//     }
//   };
  
//   // Example function to update an item
//   const handleUpdateItem = async (updatedItem) => {
//     try {
//       const result = await purchaseDetailItemsService.updateItem(updatedItem);
//       setItems(prevItems => 
//         prevItems.map(item => 
//           item.purchaseDetailItemAutoId === updatedItem.purchaseDetailItemAutoId 
//             ? result 
//             : item
//         )
//       );
//     } catch (err) {
//       setError('Failed to update item');
//     }
//   };
  
//   if (loading) return <div>Loading...</div>;
//   if (error) return <div>Error: {error}</div>;
  
//   return (
//     <div>
//       <h1>Purchase Detail Items</h1>
//       {/* Your component UI here */}
//       <ul>
//         {items.map(item => (
//           <li key={item.purchaseDetailItemAutoId}>
//             {item.itemName} - ${item.purchasePrice} x {item.purchaseQuantity}
//           </li>
//         ))}
//       </ul>
//       {/* Add forms for adding/updating items */}
//     </div>
//   );
// }