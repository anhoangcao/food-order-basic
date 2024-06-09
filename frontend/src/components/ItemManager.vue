<template>
  <div class="item-manager">

    <!-- Form tìm kiếm và nút thêm -->
    <form @submit.prevent="searchItems" class="form-inline">
      <div class="input-row">
        <input type="text" v-model="itemID" placeholder="ItemID" />
        <input type="text" v-model="itemName" placeholder="ItemName" />
      </div>
      <div class="input-row">
        <input type="text" v-model="price" placeholder="Price" />
        <input type="text" v-model="available" placeholder="Available" />
      </div>
      <div class="btn-row">
        <button @click="openAddModal" class="btn add-btn">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" class="add-icon">
            <path
              d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z" />
          </svg>
          Add Item
        </button>
        <button type="submit" class="btn search-btn">Search</button>
      </div>
    </form>

    <!-- Bảng hiển thị các mục -->
    <table>
      <thead>
        <tr>
          <th><input type="checkbox" @change="checkAllItems" v-model="isAllSelected" /></th>
          <th class="col1">ItemID</th>
          <th class="col2">ItemName</th>
          <th class="col3">Description</th>
          <th class="col4">Price</th>
          <th class="col5">Available</th>
          <th class="col6">ItemImage</th>
          <th class="col7">Updated Date</th>
          <th class="col8">Created Date</th>
          <th class="col9">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="paginatedItems.length === 0">
          <td colspan="9" class="no-data">Không có dữ liệu</td>
        </tr>
        <tr v-for="(item, index) in paginatedItems" :key="item.itemID">
          <td><input type="checkbox" v-model="item.isSelected" @change="checkSelectAll" /></td>
          <td class="col1">{{ item.itemID }}</td>
          <td class="col2">{{ item.itemName }}</td>
          <td class="col3">{{ item.description }}</td>
          <td class="col4">{{ item.price }}</td>
          <td class="col5">{{ item.availableDisplay }}</td>
          <!-- <td class="col6">{{ item.imageUrl }}</td> -->
          <td>
            <img :src="item.imageUrl" alt="Item Image" style="width: 100px; height: auto;" />
          </td>
          <td class="col7">{{ item.updatedAt }}</td>
          <td class="col8">{{ item.createdAt }}</td>
          <td class="col9">
            <button @click="viewItem(item.itemID)" class="action-button view-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                <path
                  d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z" />
              </svg>
            </button>
            <button @click="openEditModal(item)" class="action-button edit-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <path
                  d="M441 58.9L453.1 71c9.4 9.4 9.4 24.6 0 33.9L424 134.1 377.9 88 407 58.9c9.4-9.4 24.6-9.4 33.9 0zM209.8 256.2L344 121.9 390.1 168 255.8 302.2c-2.9 2.9-6.5 5-10.4 6.1l-58.5 16.7 16.7-58.5c1.1-3.9 3.2-7.5 6.1-10.4zM373.1 25L175.8 222.2c-8.7 8.7-15 19.4-18.3 31.1l-28.6 100c-2.4 8.4-.1 17.4 6.1 23.6s15.2 8.5 23.6 6.1l100-28.6c11.8-3.4 22.5-9.7 31.1-18.3L487 138.9c28.1-28.1 28.1-73.7 0-101.8L474.9 25C446.8-3.1 401.2-3.1 373.1 25zM88 64C39.4 64 0 103.4 0 152V424c0 48.6 39.4 88 88 88H360c48.6 0 88-39.4 88-88V312c0-13.3-10.7-24-24-24s-24 10.7-24 24V424c0 22.1-17.9 40-40 40H88c-22.1 0-40-17.9-40-40V152c0-22.1 17.9-40 40-40H200c13.3 0 24-10.7 24-24s-10.7-24-24-24H88z" />
              </svg>
            </button>
            <button @click="deleteItem(item.itemID)" class="action-button delete-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <path
                  d="M256 48a208 208 0 1 1 0 416 208 208 0 1 1 0-416zm0 464A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c-9.4 9.4-9.4 24.6 0 33.9l47 47-47 47c-9.4 9.4-9.4 24.6 0 33.9s24.6 9.4 33.9 0l47-47 47 47c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-47-47 47-47c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-47 47-47-47c-9.4-9.4-24.6-9.4-33.9 0z" />
              </svg>
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Add Item Modal -->
    <div v-if="isAddModalOpen" class="overlay" @click="closeAddModal">
      <div class="modal" @click.stop>
        <div class="modal-header">
          <h2>Add Item</h2>
          <button @click="closeAddModal" class="modal-close2">x</button>
        </div>
        <div class="modal-content">
          <form @submit.prevent="addItem">
            <p>
              <strong>ItemName:</strong>
              <input type="text" v-model="newItem.itemName" class="styled-input" required />
            </p>
            <p>
              <strong>Description:</strong>
              <input type="text" v-model="newItem.description" class="styled-input" required />
            </p>
            <p>
              <strong>Price:</strong>
              <input type="number" v-model="newItem.price" class="styled-input" step="0.01" min="0" required />
            </p>
            <p>
              <strong>Available:</strong>
              <select v-model="newItem.available" class="styled-select" required>
                <option value="true">Yes</option>
                <option value="false">No</option>
              </select>
            </p>
            <p>
              <strong>Image:</strong>
              <input type="file" ref="addImageFile" @change="handleFileUpload('add')" class="styled-file-input"
                accept="image/*" required />
            </p>
            <button type="submit" class="btn save-btn">Add</button>
          </form>
        </div>
      </div>
    </div>

    <!-- View Item Modal -->
    <div v-if="isModalOpen" class="overlay" @click="closeItemDetail">
      <div class="modal" @click.stop>
        <div class="modal-header">
          <h2>Item Detail</h2>
          <button @click="closeItemDetail" class="modal-close">x</button>
        </div>
        <div class="modal-content">
          <p><strong>ItemID:</strong> {{ selectedItem.itemID }}</p>
          <p><strong>ItemName:</strong> {{ selectedItem.itemName }}</p>
          <p><strong>Description:</strong> {{ selectedItem.description }}</p>
          <p><strong>Price:</strong> {{ selectedItem.price }}</p>
          <p><strong>Available:</strong> {{ selectedItem.available ? 'Yes' : 'No' }}</p>
          <p><strong>ImageUrl:</strong> <a :href="selectedItem.imageUrl">{{ selectedItem.imageUrl }}</a></p>
          <p><strong>UpdatedAt:</strong> {{ selectedItem.updatedAt }}</p>
          <p><strong>CreatedAt:</strong> {{ selectedItem.createdAt }}</p>
        </div>
      </div>
    </div>

    <!-- Edit Item Modal -->
    <div v-if="isEditModalOpen" class="overlay" @click="closeEditModal">
      <div class="modal" @click.stop>
        <div class="modal-header">
          <h2>Edit Item</h2>
          <button @click="closeEditModal" class="modal-close2">x</button>
        </div>
        <div class="modal-content">
          <form @submit.prevent="saveItem">
            <p>
              <strong>ItemName:</strong>
              <input type="text" v-model="editItemData.itemName" class="styled-input" required />
            </p>
            <p>
              <strong>Description:</strong>
              <input type="text" v-model="editItemData.description" class="styled-input" required />
            </p>
            <p>
              <strong>Price:</strong>
              <input type="number" v-model="editItemData.price" class="styled-input" step="0.01" min="0" required />
            </p>
            <p>
              <strong>Available:</strong>
              <select v-model="editItemData.available" class="styled-select" required>
                <option value="true">Yes</option>
                <option value="false">No</option>
              </select>
            </p>
            <p>
              <strong>Image:</strong>
              <input type="file" ref="editImageFile" @change="handleFileUpload('edit')" class="styled-file-input"
                accept="image/*" />
            </p>
            <button type="submit" class="btn save-btn">Save</button>
          </form>
        </div>
      </div>
    </div>

    <!-- Pagination Controls -->
    <div class="pagination" v-if="totalPages > 1">
      <button @click="prevPage" :disabled="currentPage === 1">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512">
          <path
            d="M41.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l160 160c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L109.3 256 246.6 118.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-160 160z" />
        </svg>
      </button>
      <button v-for="page in totalPages" :key="page" @click="changePage(page)"
        :class="{ active: currentPage === page }">
        {{ page }}
      </button>
      <button @click="nextPage" :disabled="currentPage >= totalPages">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512">
          <path
            d="M278.6 233.4c12.5 12.5 12.5 32.8 0 45.3l-160 160c-12.5 12.5-32.8 12.5-45.3 0s-12.5-32.8 0-45.3L210.7 256 73.4 118.6c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l160 160z" />
        </svg>
      </button>
    </div>

  </div>
</template>

<script>
import axios from 'axios';
import { useToast } from 'vue-toastification';

export default {
  name: 'ItemManager',
  data() {
    return {
      items: [],
      currentPage: 1,
      itemsPerPage: 10,
      totalPages: 1,
      itemID: '',
      itemName: '',
      price: '',
      available: '',
      isModalOpen: false,
      isAddModalOpen: false,
      isEditModalOpen: false,
      isAllSelected: false,
      selectedItem: {},
      editItemData: {},
      newItem: {
        itemName: '',
        description: '',
        price: '',
        available: '',
        imageUrl: ''
      },
    };
  },
  computed: {
    paginatedItems() {
      return this.items.map(item => ({
        ...item,
        availableDisplay: item.available ? 'Yes' : 'No'
      }));
    }
  },
  methods: {

    // Checkbox all
    toggleSelectAll() {
      this.isAllSelected = !this.isAllSelected;
      this.items.forEach(item => {
        item.isSelected = this.isAllSelected;
      });
    },
    checkSelectAll() {
      this.isAllSelected = this.items.every(item => item.isSelected);
    },
    checkAllItems() {
      this.items.forEach(item => {
        item.isSelected = this.isAllSelected;
      });
    },

    // Get all items
    async fetchItems(pageNumber = 1, pageSize = 10) {
      try {
        const response = await axios.get('https://localhost:44345/api/Items', {
          params: {
            pageNumber,
            pageSize,
          }
        });
        if (response.data && response.data.items) {
          this.items = response.data.items;
          this.totalPages = response.data.totalPages;
          this.currentPage = response.data.currentPage;
        } else {
          this.items = [];
          this.totalPages = 1;
        }
      } catch (error) {
        console.error('Failed to fetch items:', error);
        this.items = [];
        this.totalPages = 1;
      }
    },

    // Search items
    searchItems() {
      const params = {};
      if (this.itemID) params.itemID = this.itemID;
      if (this.itemName) params.itemName = this.itemName;
      if (this.price) params.price = this.price;
      if (this.available) params.available = this.available;

      this.fetchSearchItems(params);
    },
    async fetchSearchItems(params, pageNumber = 1, pageSize = 10) {
      try {
        const response = await axios.get('https://localhost:44345/api/Items', {
          params: {
            ...params,
            pageNumber,
            pageSize,
          },
        });
        if (response.data && response.data.items) {
          this.items = response.data.items;
          this.totalPages = response.data.totalPages;
          this.currentPage = response.data.currentPage;
        } else {
          this.items = [];
          this.totalPages = 1;
        }
      } catch (error) {
        console.error('Failed to fetch search items:', error);
        this.items = [];
        this.totalPages = 1;
      }
    },

    // Add item
    openAddModal() {
      console.log('Open Add Modal');
      this.newItem = {
        itemName: '',
        description: '',
        price: '',
        available: 'true',
        imageUrl: ''
      };
      this.isAddModalOpen = true;
    },
    closeAddModal() {
      console.log('Close Add Modal');
      this.isAddModalOpen = false;
    },
    handleFileUpload(mode) {
      const fileInput = mode === 'add' ? this.$refs.addImageFile : this.$refs.editImageFile;
      const file = fileInput.files[0];
      const formData = new FormData();
      formData.append('file', file);

      axios.post('https://localhost:44345/api/Items/UploadImage', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
        .then(response => {
          if (mode === 'add') {
            this.newItem.imageUrl = response.data.url;
          } else {
            this.editItemData.imageUrl = response.data.url;
          }
        })
        .catch(error => {
          console.error('Error uploading image:', error);
        });
    },
    async addItem() {
      const toast = useToast();
      try {
        if (!this.newItem.itemName) {
          console.error('Item name is required.');
          return;
        }
        if (!this.newItem.description) {
          console.error('Description is required.');
          return;
        }
        if (isNaN(this.newItem.price) || this.newItem.price === '') {
          console.error('Price must be a valid number.');
          return;
        }
        if (!this.newItem.imageUrl) {
          console.error('Image URL is required.');
          return;
        }

        const newItemData = {
          itemName: this.newItem.itemName,
          description: this.newItem.description,
          price: parseFloat(this.newItem.price),
          available: this.newItem.available === 'true',
          imageUrl: this.newItem.imageUrl
        };

        const response = await axios.post('https://localhost:44345/api/Items', newItemData, {
          headers: {
            'Content-Type': 'application/json'
          }
        });

        if (response.status === 201) {
          this.items.unshift(response.data);
          toast.success('Add item successfully');
          this.closeAddModal();
        } else {
          console.error('Failed to add new item');
        }
      } catch (error) {
        console.error('Error adding item:', error);
      }
    },

    // View item
    viewItem(id) {
      this.selectedItem = this.items.find(item => item.itemID === id);
      this.isModalOpen = true;
    },
    closeItemDetail() {
      this.isModalOpen = false;
      this.selectedItem = {};
    },

    // Edit item
    openEditModal(item) {
      this.editItemData = { ...item };
      this.isEditModalOpen = true;
    },
    async saveItem() {
      const toast = useToast();
      try {
        const updatedItemData = {
          ...this.editItemData,
          available: this.editItemData.available === 'true'
        };
        const response = await axios.put(`https://localhost:44345/api/Items/${this.editItemData.itemID}`, updatedItemData);
        if (response.status === 200 || response.status === 204) {
          console.log('Item saved:', response.data);
          toast.success('Update item successfully');
          this.fetchItems(this.currentPage, this.itemsPerPage);
          this.closeEditModal();
        } else {
          console.error(`Failed to update item with ID ${this.editItemData.itemID}: ${response.statusText}`);
        }
      } catch (error) {
        console.error('Error saving item:', error);
        toast.error('Failed to update item. Please check the data and try again.');
      }
    },
    closeEditModal() {
      this.isEditModalOpen = false;
      this.editItemData = {};
    },

    // Delete item
    async deleteItem(id) {
      const toast = useToast();
      const confirmed = confirm('Are you sure you want to delete this item?');
      if (!confirmed) return;

      try {
        const response = await axios.delete(`https://localhost:44345/api/Items/${id}`);
        if (response.status === 204) {
          this.items = this.items.filter(item => item.itemID !== id);
          toast.success('Delete item successfully');
          console.log(`Item with ID ${id} deleted successfully`);
        } else {
          console.error(`Failed to delete item with ID ${id}`);
          toast.error(`Failed to delete item with ID ${id}`);
        }
      } catch (error) {
        console.error('Error deleting item:', error);
        toast.error('Error deleting item');
      }
    },

    // Next page
    prevPage() {
      if (this.currentPage > 1) {
        this.changePage(this.currentPage - 1);
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.changePage(this.currentPage + 1);
      }
    },
    changePage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
        this.fetchItems(page, this.itemsPerPage);
      }
    },
  },
  mounted() {
    this.fetchItems(this.currentPage, this.itemsPerPage);
  }
};
</script>

<style src="@/assets/style.css"></style>