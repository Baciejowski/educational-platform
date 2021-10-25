<template>
    <div class="container">
        <ClassListView />
        <b-card class="mt-3 text-white" header="Data from API">
            <pre class="mt-3 text-white">{{ classesMessage }}</pre>
        </b-card>
    </div>
</template>

<script>
import ClassListView from "@/components/ClassListView"
import axios from "axios"

export default {
    components: { ClassListView },
    computed: {
        classesMessage() {
            return this.$store.state.classData
        }
    },
    methods: {
        async callApi() {
            const token = await this.$auth.getTokenSilently()

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