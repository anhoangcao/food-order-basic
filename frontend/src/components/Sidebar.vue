<template>
  <div class="container">
    <div class="sidebar">
      <div class="logo">sweet.harmony</div>
      <div class="line"></div>
      <ul>
        <li>User</li>
        <li>Order</li>
        <li>Items</li>
        <li>Import CSV</li>
      </ul>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Sidebar'
};
</script>

<style scoped>
.sidebar {
  width: 210px;
  background-color: #f5f5f5;
  padding: 20px;
  margin: 0px 0px 0px -74px;
  position: fixed;
  height: 100vh;
  border-right: 1px solid #ccc;
}

.logo {
  font-size: 24px;
  font-weight: bold;
  margin-bottom: 20px;
  color: #333;
}

.line {
  border-bottom: 1px solid #333;
  width: 100%;
  margin-bottom: 20px;
}

ul {
  list-style: none;
  padding: 0;
  color: #333;
}

li {
  margin: 10px 0;
  padding: 10px;
  cursor: pointer;
  color: #333;
  border-radius: 4px;
  transition: background-color 0.3s;
}

li:hover {
  background-color: #e0e0e0;
}
</style>
