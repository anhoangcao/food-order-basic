import { createRouter, createWebHistory } from "vue-router";
import ItemManager from "../components/ItemManager.vue";

const routes = [
  {
    path: "/",
    name: "ItemManager",
    component: ItemManager,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;