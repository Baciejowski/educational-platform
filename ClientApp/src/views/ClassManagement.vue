<template>
    <div class="container">
        <div class="d-flex justify-content-center">
            <b-spinner label="Loading..." v-if="loadingData" class="me-auto">
        <b class="text-info">{{ data.value.last.toUpperCase() }}</b>, <b>{{ data.value.first }}</b>
      </template>
            </b-spinner>
            <div v-else>
                <b-table striped hover :items="classesMessage"></b-table>
            </div>
        </div>

        <b-card class="mt-3 text-white" header="Data from API">
            <pre class="mt-3 text-white">{{ classesMessage }}</pre>
        </b-card>
    </div>
</template>

<script>
import axios from "axios"

export default {
    data() {
        return {
            classesMessage: [],
            loadingData: true
        }
    },
    created() {
        this.callApi()
    },
    methods: {
        async callApi() {
            const token = await this.$auth.getTokenSilently()

            axios
                .get("/api/classes", {
                    // headers: {
                    //     Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
                    //     // Access-Control-Allow-Origin: *
                    // }
                })
                .then((res) => res.data)
                .then((res) => {
                    this.classesMessage = res
                    this.loadingData = false
                })
                .catch((err) => (this.classesMessage = err))

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