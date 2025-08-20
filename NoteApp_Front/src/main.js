// main.js
import { createApp } from 'vue'
import { createPinia } from "pinia";
import App from './App.vue'
import router from './router/index.js'

// Import Tailwind CSS
import './assets/main.css'

// Import Navbar
import Navbar from './components/Navbar.vue'

const app = createApp(App)

// Register Navbar globally
app.component('Navbar', Navbar)

const pinia = createPinia();
app.use(pinia);

app.use(router)
app.mount('#app')