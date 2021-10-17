import Vue from "vue"
import VueRouter from "vue-router"
import Home from "@/views/Home.vue"
import Profile from "@/views/Profile.vue"
import { authGuard } from "@/auth/authGuard"
import ExternalApiView from "@/views/ExternalApi.vue"
import Counter from "@/views/Counter.vue";

Vue.use(VueRouter)

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home
    },
    {
        path: "/profile",
        name: "profile",
        component: Profile,
        beforeEnter: authGuard
    },
    {
        path: "/external-api",
        name: "external-api",
        component: ExternalApiView,
        beforeEnter: authGuard
    },
    {
        path: "/Counter",
        name: "Counter",
        component: Counter,
    }
]

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes
})

export default router
