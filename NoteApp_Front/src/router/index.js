
import { createRouter, createWebHistory } from "vue-router";
import LoginPage from "../pages/LoginPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";
import NotesList from "../pages/NotesList.vue";

const routes = [
    { path: "/", redirect: "/login" },
    { path: "/login", component: LoginPage },
    { path: "/register", component: RegisterPage },
    { 
        path: "/notes", 
        component: NotesList,
        meta: { requiresAuth: true }
    },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Simple navigation guard for auth
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  if (to.meta.requiresAuth && !token) {
    next("/login");
  } else {
    next();
  }
});

export default router;