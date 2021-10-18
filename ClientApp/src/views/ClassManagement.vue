<template>
    <div class="container">
        <div class="d-flex justify-content-center" v-if="loadingData" >
            <b-spinner label="Loading..." class="me-auto"></b-spinner>
        </div>
        <div v-else>
            <div v-for="item in classesMessage" :key="item.classID">
                <div class="d-flex">
                <b-button v-b-toggle="'collapse-'+item.classID">{{ item.friendlyName }}</b-button>
                </div>
                <b-collapse :id="'collapse-'+item.classID" class="mt-2">
                    <b-card>
                        <p class="card-text text-white">                                
                            <ul>
                                <li v-for="student in item.students" :key="student.studentID">{{ getStudentFullName(student) }}</li>
                            </ul>
                        </p>
                    </b-card>
                </b-collapse>
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
    computed: {
        classesMessage() {
            return this.$store.state.classData
        },
        loadingData() {
            return this.$store.state.loadingData
        }
    },
    created() {
        this.$store.dispatch("getClassData")
    },
    methods: {
        getStudentFullName(student) {
            return `${student.firstName ?? ""} ${student.lastName ? student.lastName + " - " : ""} ${student.email}`.trim()
        },
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