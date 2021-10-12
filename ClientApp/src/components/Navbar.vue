<template>
    <div>
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <div class="container-fluid">
                <a class="navbar-brand" href="/">Auth0</a>
                <button
                    class="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarSupportedContent"
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                        <li class="nav-item">
                            <router-link to="/" class="nav-link active">Home</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/about" class="nav-link active">About</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link v-if="$auth.isAuthenticated" to="/profile" class="nav-link active">Profile</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/external-api" class="nav-link active">External Api</router-link>
                        </li>
                    </ul>
                    <ul class="navbar-nav d-flex">
                        <li class="nav-item" v-if="$auth.isAuthenticated">
                            <b-dropdown right>
                                <template slot="button-content">
                                    <img :src="$auth.user.picture" alt="User's profile picture" class="nav-user-profile rounded-circle" width="30" />
                                </template>
                                <div class="dropdown-header">{{ $auth.user.name }}</div>
                                <b-dropdown-item
                                    ><router-link to="/profile" class="dropdown-item dropdown-profile"> Profile </router-link></b-dropdown-item
                                >
                                <b-dropdown-item><a @click="logout" class="dropdown-item dropdown-profile">Log out</a></b-dropdown-item>
                            </b-dropdown>
                        </li>
                        <button
                            id="qsLoginBtn"
                            v-if="!$auth.isAuthenticated && !$auth.loading"
                            class="btn btn-outline-primary btn-block"
                            @click="login"
                        >
                            Log in
                        </button>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
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
