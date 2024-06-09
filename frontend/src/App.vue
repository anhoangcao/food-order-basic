<template>
  <div id="app" class="app-container">
    <Sidebar @updatePage="updatePage" />
    <div class="content">
      <header>
        <h1 v-if="currentPage !== 'Items'">{{ currentPage }}</h1>
        <Header v-else />
      </header>
      <div class="body-content">
        <component :is="currentComponent"></component>
      </div>
    </div>
  </div>
</template>

<script>
import Sidebar from './components/Sidebar.vue';
import Header from './components/Header.vue';
import ItemManager from './components/ItemManager.vue';
import NotFound from './components/NotFound.vue';

export default {
  name: 'App',
  components: {
    Sidebar,
    Header,
    ItemManager,
    NotFound
  },
  data() {
    return {
      currentPage: 'Items'
    };
  },
  computed: {
    currentComponent() {
      return this.currentPage === 'Items' ? 'ItemManager' : 'NotFound';
    }
  },
  methods: {
    updatePage(page) {
      this.currentPage = page;
    }
  }
};
</script>

<style>
.app-container {
  display: flex;
  height: 100vh;
}

.content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.body-content {
  flex: 1;
  padding: 1rem;
  background: #fff;
  overflow-y: auto;
}
</style>