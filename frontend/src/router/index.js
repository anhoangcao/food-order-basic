import { createRouter, createWebHistory } from 'vue-router';
import ItemManager from '../components/ItemManager.vue';
// Import other components as needed, e.g., User, Order, etc.

const routes = [
  {
    path: '/',
    name: 'ItemManager',
    component: ItemManager
  },
  // Define other routes here
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

export default router;
