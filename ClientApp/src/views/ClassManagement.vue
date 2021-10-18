<template>
    <div class="container">
        <div class="d-flex justify-content-center" v-if="loadingData" >
            <b-spinner label="Loading..." class="me-auto"></b-spinner>
        </div>
        <div v-else>
            <div v-for="item in classesMessage" :key="item.classID">
                <b-button v-b-toggle="'collapse-'+item.classID">{{ item.friendlyName }}</b-button>
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
        getStudentFullName(student) {
            return `${student.firstName ?? ""} ${student.lastName ? student.lastName + " - " : ""} ${student.email}`.trim()
        },
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