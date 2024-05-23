import { createRouter, createWebHistory } from 'vue-router';
import Register from '@/components/Register.vue';
import Login from '@/components/Login.vue';
import UserManager from '@/components/UserManager.vue';

const routes = [
  { path: '/register', component: Register },
  { path: '/login', component: Login },
  { 
    path: '/users', 
    component: UserManager,
    // Xử lý logout khi đã xóa token, kiểm tra lại token nếu không còn token thì quay về login
    beforeEnter: (to, from, next) => {
      const token = localStorage.getItem('userToken');
      if (token) {
        next();
      } else {
        next('/login');
      }
    }
  },
  {
    path: '/:catchAll(.*)',
    redirect: '/login'
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
