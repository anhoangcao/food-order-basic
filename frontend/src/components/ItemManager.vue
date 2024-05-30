<template>
  <div class="item-manager">
    <form @submit.prevent="searchItems" class="form-inline">
      <input type="text" v-model="remID" placeholder="ItemID" />
      <input type="text" v-model="itemName" placeholder="ItemName" />
      <input type="text" v-model="price" placeholder="Price" />
      <input type="text" v-model="available" placeholder="Available" />
      <button type="submit" class="btn search-btn">Search</button>
      <button @click="addItem" class="btn add-btn">Add Item</button>
    </form>
    <table>
      <thead>
        <tr>
          <th><input type="checkbox" /></th>
          <th class="col1">Item ID</th>
          <th class="col2">Item Name</th>
          <th class="col3">Description</th>
          <th class="col4">Price</th>
          <th class="col5">Available</th>
          <th class="col6">ImageUrl</th>
          <th class="col7">Updated At</th>
          <th class="col8">Created At</th>
          <th class="col9">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in paginatedItems" :key="item.id">
          <td><input type="checkbox" /></td>
          <td class="col1">{{ item.id }}</td>
          <td class="col2">{{ item.name }}</td>
          <td class="col3">{{ item.description }}</td>
          <td class="col4">{{ item.price }}</td>
          <td class="col5">{{ item.available }}</td>
          <td class="col6">{{ item.image }}</td>
          <td class="col7">{{ item.updatedAt }}</td>
          <td class="col8">{{ item.createdAt }}</td>
          <td class="col9">
            <button @click="viewItem(item.id)" class="action-button view-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                <path
                  d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
              </svg>
            </button>
            <button @click="editItem(item.id)" class="action-button edit-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <path
                  d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z" />
              </svg>
            </button>
            <button @click="deleteItem(item.id)" class="action-button delete-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <path
                  d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c9.4-9.4 24.6-9.4 33.9 0l47 47 47-47c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9l-47 47 47 47c9.4 9.4 9.4 24.6 0 33.9s-24.6 9.4-33.9 0l-47-47-47 47c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l47-47-47-47c-9.4-9.4-9.4-24.6 0-33.9z" />
              </svg>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <!-- Pagination Controls -->
    <div class="pagination">
      <button @click="currentPage--" :disabled="currentPage === 1">
        &lt;
      </button>
      <button v-for="page in totalPages" :key="page" @click="currentPage = page"
        :class="{ active: currentPage === page }">
        {{ page }}
      </button>
      <span v-if="totalPages > 5 && currentPage < totalPages - 3">...</span>
      <button v-for="page in [totalPages - 2, totalPages - 1, totalPages]" :key="page" @click="currentPage = page"
        :class="{ active: currentPage === page }" v-if="page > 3">
        {{ page }}
      </button>
      <button @click="currentPage++" :disabled="currentPage === totalPages">
        &gt;
      </button>
    </div>
  </div>
</template>


<script>
export default {
  name: 'ItemManager',
  data() {
    return {
      remID: '',
      itemName: '',
      price: '',
      available: '',
      items: [
        { id: 1, name: 'Hamburger', description: 'Ngon bổ rẻ, ngon rẻ bổ ngon ngon quá', price: 200, available: 'Yes', image: 'D:\\TTTT\\frontend\\cd\\tt\\image', updatedAt: '11:08am 5/30/2024', createdAt: '11:08am 5/30/2024' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ Ngon bổ rẻ Ngon bổ rẻ Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 4, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 5, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 1, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 2, name: 'hamburger', description: 'Ngon bổ rẻ', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },
        { id: 3, name: 'hamburger', description: 'Ngon', price: 200, available: 'Yes', image: 'url', updatedAt: 'auto', createdAt: 'auto' },

      ],
      currentPage: 1,
      itemsPerPage: 10
    };
  },
  computed: {
    paginatedItems() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.items.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.items.length / this.itemsPerPage);
    }
  },
  methods: {
    searchItems() {
      // Implement search functionality here
    },
    addItem() {
      // Implement add item functionality here
    },
    viewItem(id) {
      // Implement view item functionality here
    },
    editItem(id) {
      // Implement edit item functionality here
    },
    deleteItem(id) {
      // Implement delete item functionality here
    }
  }
};
</script>

<style scoped>
.item-manager {
  margin-top: 20px;
}

.form-inline {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
  align-items: center;
}

input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  flex: 1;
}

button {
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn {
  background-color: #ff7f00;
  color: white;
}

.search-btn {
  background-color: #007bff;
}

td button.action-button {
  background: none;
  border: none;
  cursor: pointer;
  padding: 5px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

td button.action-button:hover {
  opacity: 0.7;
}

.action-button svg {
  width: 20px;
  height: 20px;
  fill: #299BE4;
}

.action-button.view-icon svg {
  fill: #17a2b8;
}

.action-button.edit-icon svg {
  fill: #ffc107;
}

.action-button.delete-icon svg {
  fill: #dc3545;
}

.odd-row td {
  background-color: #fcfcfc;
}

.even-row td {
  background-color: #fff;
}

.add-btn {
  background-color: #28a745;
}

table {
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
  margin-right: 30px;
  background-color: #ffffff;
}

th,
td {
  padding: 5px 10px;
  border-bottom: 1px solid #ddd;
  text-align: center;
  color: black;
  word-wrap: break-word;
  /* Cho phép xuống hàng */
  white-space: normal;
  /* Cho phép xuống hàng */
}

td {
  font-size: 14px;
  padding: 3px 10px;
}

thead {
  background-color: #f5f5f5;
}

th {
  font-weight: 600;
}

/* Định nghĩa chiều rộng cho từng cột */
.col1 {
  width: 30%;
}

.col2 {
  width: 30%;
  text-align: left;
}

.col3 {
  width: 40%;
  text-align: left;
}

.col4 {
  width: 13%;
}

.col5 {
  width: 21%;
}

.col6 {
  width: 40%;
  text-align: left;
}

.col7 {
  width: 23%;
  text-align: left;
}

.col8 {
  width: 22%;
  text-align: left;
}

.col9 {
  width: 26%;
}

.action-btn {
  border: none;
  background: none;
  cursor: pointer;
  margin-right: 5px;
}

.view-btn {
  color: #007bff;
}

.edit-btn {
  color: #ffc107;
}

.delete-btn {
  color: #dc3545;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}

.pagination button,
.pagination span {
  margin: 0 5px;
  padding: 8px 12px;
  border: 1px solid #ddd;
  background-color: #fff;
  cursor: pointer;
  border-radius: 4px;
}

.pagination button.active,
.pagination span.active {
  background-color: #e6f0ff;
  border-color: #0066cc;
}

.pagination button:hover:not(:disabled),
.pagination span:hover:not(:disabled) {
  background-color: #f0f0f0;
}

.pagination button:disabled,
.pagination span:disabled {
  cursor: not-allowed;
  opacity: 0.5;
  background-color: #f5f5f5;
  border-color: #ddd;
}

.pagination .dots {
  border: none;
  background: none;
  cursor: default;
  pointer-events: none;
}

.pagination button,
.pagination span,
.pagination .dots {
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: 36px;
  height: 36px;
}
</style>
