<template>
  <div id="app">
    <Sidebar />
    <div class="content">
      <Header />
      <ItemManager />
    </div>
  </div>
</template>

<script>
import Sidebar from './components/Sidebar.vue';
import Header from './components/Header.vue';
import ItemManager from './components/ItemManager.vue';

export default {
  name: 'App',
  components: {
    Sidebar,
    Header,
    ItemManager
  }
};
</script>

<style>
#app {
  display: flex;
}

.content {
  flex: 1;
  padding: 20px;
  margin-left: 154px;
  background-color: #f9f9f9;
}

/* Responsive Design Enhancements */
@media (max-width: 768px) {
  .content {
    margin-left: 0;
  }
}
</style>
