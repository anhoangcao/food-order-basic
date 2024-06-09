import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css'
import axios from 'axios'
import './assets/style.css';

axios.defaults.baseURL = 'https://localhost:44345/api'

const options = {
    // Cấu hình tùy chọn cho Toast
    position: 'bottom-center',
    timeout: 2000, 
    closeOnClick: true, 
    pauseOnFocusLoss: true, 
    pauseOnHover: true, 
    draggable: true,
    draggablePercent: 0.6,
    showCloseButtonOnHover: false,
    hideProgressBar: false,
    closeButton: 'button',
    icon: true, 
    rtl: false
}

const app = createApp(App);
app.use(router);
app.use(Toast, options);
app.config.globalProperties.$axios = axios
app.mount('#app');