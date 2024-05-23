import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css'
import './assets/style.css'
import axios from 'axios'

// Thay đổi URL cơ bản của axios để sử dụng HTTPS
axios.defaults.baseURL = 'https://localhost:44345/api'

const options = {
  // Cấu hình tùy chọn cho Toast
  position: 'bottom-center', // Vị trí hiển thị thông báo
  timeout: 2000, // Thời gian hiển thị thông báo (ms)
  closeOnClick: true, // Đóng thông báo khi click vào
  pauseOnFocusLoss: true, // Dừng thời gian hiển thị khi mất focus
  pauseOnHover: true, // Dừng thời gian hiển thị khi hover
  draggable: true, // Cho phép kéo thông báo
  draggablePercent: 0.6, // Phần trăm có thể kéo
  showCloseButtonOnHover: false, // Hiển thị nút đóng khi hover
  hideProgressBar: false, // Ẩn thanh tiến trình
  closeButton: 'button', // Nút đóng
  icon: true, // Hiển thị icon
  rtl: false // Hỗ trợ chế độ RTL
}

const app = createApp(App)
app.use(router)
app.use(Toast, options)
app.config.globalProperties.$axios = axios
app.mount('#app')

// Thêm CSS để đổi nền thành gradient
const style = document.createElement('style')
style.textContent = `
  body {
    background: linear-gradient(to bottom, #0081A7, #00AFB9);
  }
`
document.head.appendChild(style)
