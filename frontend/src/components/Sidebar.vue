<template>
  <div class="sidebar">
    <div class="logo">
      <img class="logo-image" src="/src/assets/images/logo2.png" alt="sweet.harmony logo">
      sweet.harmony
    </div>
    <ul>
      <li v-for="option in options" :key="option" :class="{ active: isActive(option) }" @click="setActive(option)">
        {{ option }}
      </li>
    </ul>
    <!-- User Profile Section -->
    <div class="user-profile">
      <img class="user-avatar" src="/src/assets/images/logo2.png" alt="User Avatar">
      <div class="user-info">
        <span class="user-name">Elijah Scott</span>
        <span class="user-email">elijahscott@gmail.com</span>
      </div>
      <div class="user-options">
        <button @click="toggleTooltip" class="logout">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
            <path d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"/></svg>
        </button>
      </div>
    </div>
  </div>
</template>


<script>
export default {
  name: 'Sidebar',
  data() {
    return {
      currentPage: 'Items',
      options: ['User', 'Order', 'Items', 'Import CSV'],
      isTooltipVisible: false,
    };
  },
  methods: {
    isActive(option) {
      return this.currentPage === option;
    },
    setActive(option) {
      this.currentPage = option;
    },
    toggleTooltip() {
      this.isTooltipVisible = !this.isTooltipVisible;
    }
  }
};
</script>

<style scoped>
.sidebar {
  width: 210px;
  background-color: #f5f5f5;
  position: fixed;
  height: 100vh;
  border-right: 1px solid #ccc;
  padding: 20px 10px;
  margin: 0 0 0 -75px;
}

.logo {
  display: flex;
  align-items: center;
  font-weight: bold;
  margin-bottom: 20px;
  color: #333;
  font-size: 20px;
}

.logo-image {
  width: 50px;
  height: 50px;
  margin-right: -4px;
}

.user-profile {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  position: relative;
  top: 300px;
}

.user-avatar {
  width: 30px;
  height: 30px;
  border-radius: 50%;
}

.user-info {
  flex-grow: 1;
  margin-left: 5px;
}

.user-name {
  display: block;
  color: #000;
  font-weight: 600;
}

.user-email {
  display: block;
  color: #6B7280;
  font-size: 14px;
}

.user-options button {
  background: none;
  border: none;
  cursor: pointer;
}

.logout svg {
  width: 13px;
  height: 13px;
  fill: #6B7280;
}


ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

li {
  padding: 12px;
  cursor: pointer;
  transition: background-color 0.2s;
  color: #333;
  border-radius: 2px;
  font-weight: 700;
}

li:hover {
  background-color: #aca8a6;
  color: white;
}

.active {
  background-color: #FF7517;
  color: white;
}

.active:hover {
  background-color: #FF7517;
}

</style>

