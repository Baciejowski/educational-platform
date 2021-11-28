import Vue from "vue"
import VueRouter from "vue-router"
import Home from "@/views/Home.vue"
import Profile from "@/views/Profile.vue"
import { authGuard } from "@/auth/authGuard"
import ClassManagement from "@/views/ClassManagement.vue"
import CreateScenario from "@/views/CreateScenario.vue"
import Counter from "@/views/Counter.vue";
import TopicsManagement from "@/views/TopicsManagement.vue"
import ViewScenario from "@/views/ViewScenario.vue"
import PageNotFound from "@/views/PageNotFound.vue"
import StudentsManagement from "@/views/StudentsManagement.vue"
import AiView from "@/views/AiView.vue"
import TeacherView from "@/views/TeacherView.vue"
import Report from "@/views/Report.vue"

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
        path: "/teacher",
        name: "teacher panel",
        component: TeacherView,
        beforeEnter: authGuard
    },
    {
        path: "/AiProposals",
        name: "AiProposals",
        component: AiView,
        beforeEnter: authGuard
    },
    {
        path: "/topics",
        name: "topics",
        component: TopicsManagement,
        beforeEnter: authGuard
    },
    {
        path: "/scenario",
        name: "scenario",
        component: ViewScenario,
        beforeEnter: authGuard
    },
    {
        path: "/students",
        name: "students",
        component: StudentsManagement,
        beforeEnter: authGuard
    },
    {
        path: "/class-management",
        name: "class-management",
        component: ClassManagement,
        beforeEnter: authGuard
    },
    {
        path: "/create-scenario",
        name: "create-scenario",
        component: CreateScenario,
        beforeEnter: authGuard
    },
    {
        path: "/report",
        name: "Report",
        component: Report,
    },
    {
        path: "/Counter",
        name: "Counter",
        component: Counter,
    },
    {
        path: "*",
        component: PageNotFound
    }
]

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes
})

export default router
