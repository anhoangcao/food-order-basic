<template>
  <div class="sidebar">
    <div class="logo">
      <img class="logo-image" src="/src/assets/images/logo2.png" alt="sweet.harmony logo">
      sweet.harmony
    </div>
    <ul>
      <li v-for="option in options" :key="option.name" :class="{ active: isActive(option.name) }"
        @click="setActive(option.name)">
        <img :src="getIconPath(option.name)" class="option-icon" alt="icon" />
        {{ option.name }}
      </li>
    </ul>
    <div class="user-profile">
      <img class="user-avatar" src="/src/assets/images/avatar.jpg" alt="User Avatar">
      <div class="user-info">
        <span class="user-name">An Cao</span>
        <span class="user-email">anhoangcao@gmail.com</span>
      </div>
      <div class="user-options">
        <button @click="toggleTooltip" class="logout">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
            <path
              d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z" />
          </svg>
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
      options: [
        { name: 'Users', iconPath: '/src/assets/icons/Admin.svg' },
        { name: 'Orders', iconPath: '/src/assets/icons/order.svg' },
        { name: 'Items', iconPath: '/src/assets/icons/item.svg' },
        { name: 'Import CSV', iconPath: '/src/assets/icons/import_csv.svg' }
      ],
      isTooltipVisible: false,
    };
  },
  methods: {
    isActive(option) {
      return this.currentPage === option;
    },
    setActive(option) {
      this.currentPage = option;
      this.$emit('updatePage', option);
    },
    toggleTooltip() {
      this.isTooltipVisible = !this.isTooltipVisible;
    },
    getIconPath(name) {
      const option = this.options.find(option => option.name === name);
      return option ? option.iconPath : '';
    }
  }
};
</script>

<style src="@/assets/style.css"></style>