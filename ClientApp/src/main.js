import Vue from "vue"
import App from "./App.vue"
import router from "./router"
import {store} from './store/store.js'
import { domain, clientId, audience } from "../auth_config.json";
import { Auth0Plugin } from "./auth"
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import Chartkick from 'vue-chartkick'
import Chart from 'chart.js'
import Axios from "./plugins/axios";
import titleMixin from './js/custom/titleMixin'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(Axios)
Vue.use(Chartkick.use(Chart))

Vue.use(Auth0Plugin, {
  domain,
  clientId,
  audience,
  onRedirectCallback: appState => {
    router.push(
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname
    );
  }
});

Vue.config.productionTip = false;
Vue.mixin(titleMixin)

new Vue({
    router,
    store,
    render: (h) => h(App)
}).$mount("#app")
