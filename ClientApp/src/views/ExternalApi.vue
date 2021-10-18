<template>
    <div>
        <button @click="callApi" class="btn btn-outline-primary btn-block">Call API</button>
        <p>{{ apiMessage }}</p>
        <pre>{{ classesMessage }}</pre>
    </div>
</template>

<script>
import axios from "axios"

export default {
    name: "external-api",
    data() {
        return {
            apiMessage: "",
            classesMessage: ""
        }
    },
    methods: {
        async callApi() {
            // Get the access token from the auth wrapper
            const token = await this.$auth.getTokenSilently()

            const { data } = await axios.get("/api/classes", {
                // headers: {
                //     Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
                //     // Access-Control-Allow-Origin: *
                // }
            })
            this.classesMessage = data

//na własnym serwerze walidacja działa dobrze
            axios
                .get("/api/external", {
                    headers: {
                        Authorization: `Bearer ${token}` 
                    }
                })
                .then((res) => res.data)
                .then((res) => alert(res.msg))
                .catch(console.log)
        }
    }
}
</script>