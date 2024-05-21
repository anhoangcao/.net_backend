import { createRouter, createWebHistory } from 'vue-router'
import Register from '@/components/Register.vue'
import Login from '@/components/Login.vue'
import UserManager from '@/components/UserManager.vue'

const routes = [
  { path: '/users', component: UserManager},
  { path: '/register', component: Register },
  { path: '/login', component: Login },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
