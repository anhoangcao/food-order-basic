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
  padding: 30px 20px 20px 20px;
  width: 1091px;
  margin: 0 0 0 136px;
  background-color: #e8ecf0;
}

</style>
