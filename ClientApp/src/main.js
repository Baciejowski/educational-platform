import Vue from "vue"
import App from "./App.vue"
import router from "./router"
import { domain, clientId, audience } from "../auth_config.json";
import { Auth0Plugin } from "./auth"
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import Axios from "./plugins/axios";

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(Axios);

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

new Vue({
    router,
    render: (h) => h(App)
}).$mount("#app")
