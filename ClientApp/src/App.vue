<template>
    <div id="app">
        <Navbar />
        <router-view />
    </div>
</template>
<script>
import Navbar from "@/components/Navbar.vue"

export default {
    name: "app",
    created() {
        this.$store.commit("setAuth", this.$auth)
    },
    components: {
        Navbar
    }
}
</script>

<style>
@import "./css/materialize.css";
@import "https://fonts.googleapis.com/icon?family=Material+Icons";
</style>
