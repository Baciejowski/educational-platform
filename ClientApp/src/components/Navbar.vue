<template>
    <header>
        <nav>
            <div class="nav-wrapper">
                <a class="brand-logo" href="./"><i class="material-icons">home</i></a>
                <ul id="nav-mobile">
                    <li>
                        <a href="#">About<i class="material-icons">info</i></a>
                    </li>
                    <li>
                        <a href="https://yarde.github.io/"> Join the game<i class="material-icons">sports_esports</i> </a>
                    </li>
                    <li>
                        <a href="#">Teacher zone<i class="material-icons">work</i></a>
                    </li>
                    <li>
                        <a @click="login" v-if="!$auth.isAuthenticated && !$auth.loading">Register/Log in<i class="material-icons">person_add</i></a>
                        <div v-if="$auth.isAuthenticated">
                            <b-dropdown right>
                                <template slot="button-content"> Hi, {{ $auth.user.name }}! </template>
                                <div class="dropdown-header">
                                    {{ $auth.user.name }}
                                    <img :src="$auth.user.picture" alt="User's profile picture" class="nav-user-profile rounded-circle" width="30" />
                                </div>
                                <b-dropdown-item
                                    ><router-link to="/profile" class="dropdown-item dropdown-profile"> Profile </router-link></b-dropdown-item
                                >
                                <b-dropdown-item><a @click="logout" class="dropdown-item dropdown-profile">Log out</a></b-dropdown-item>
                            </b-dropdown>
                        </div>
                    </li>
                </ul>
                <a class="sidenav-trigger" data-target="slide-out" href="#"><i class="material-icons">menu</i></a>
            </div>
        </nav>
        <ul class="sidenav" id="slide-out">
            <li>
                <a href="#">About<i class="material-icons">info</i></a>
            </li>
            <li>
                <a href="https://yarde.github.io/">Join the game<i class="material-icons">sports_esports</i></a>
            </li>
            <li>
                <a href="#">Teacher zone<i class="material-icons">work</i></a>
            </li>
            <li>
                <a @click="login" v-if="!$auth.isAuthenticated && !$auth.loading">Register/Log in<i class="material-icons">person_add</i></a>
                <div v-if="$auth.isAuthenticated">
                    <b-dropdown right>
                        <template slot="button-content"> Hi, {{ $auth.user.name }}! </template>
                        <div class="dropdown-header">
                            {{ $auth.user.name }}
                            <img :src="$auth.user.picture" alt="User's profile picture" class="nav-user-profile rounded-circle" width="30" />
                        </div>
                        <b-dropdown-item><router-link to="/profile" class="dropdown-item dropdown-profile"> Profile </router-link></b-dropdown-item>
                        <b-dropdown-item><a @click="logout" class="dropdown-item dropdown-profile">Log out</a></b-dropdown-item>
                    </b-dropdown>
                </div>
            </li>
        </ul>
    </header>
</template>
<script>
export default {
    methods: {
        login() {
            this.$auth.loginWithRedirect()
        },
        logout() {
            this.$auth.logout({
                returnTo: window.location.origin
            })
        }
    }
}
</script>
