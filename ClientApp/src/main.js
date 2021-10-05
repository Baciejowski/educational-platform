import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import Axios from 'axios';
Axios.defaults.baseURL = '//localhost:5000/';

createApp(App).use(router).mount('#app')
