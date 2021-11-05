<template>
    <div class="py-3">
        <div class="d-flex justify-content-center" v-if="loadingData">
            <b-spinner label="Loading..." class="me-auto"></b-spinner>
        </div>
        <div v-else>
            <div v-for="item in teacherMessage.classes" :key="item.classID">
                <div class="d-flex">
                    <h3>{{ item.friendlyName }}</h3>
                    <div class="ml-auto">
                        <b-button v-b-toggle="'collapse-' + item.classID">SHOW STUDENTS</b-button>
                        <b-button v-b-toggle="'collapse-menu-' + item.classID" class="btn-info ml-2" @click="selectClass(item.classID)"
                            >SELECT CLASS</b-button
                        >
                    </div>
                </div>

                <b-collapse :id="'collapse-' + item.classID" class="mt-2">
                    <b-card class="card-text text-white">
                        <ul>
                            <li v-for="student in item.students" :key="student.studentID">{{ getStudentFullName(student) }}</li>
                        </ul>
                    </b-card>
                </b-collapse>

                <b-collapse :id="'collapse-menu-' + item.classID" class="mt-2">
                    <b-card>
                        <div class="card-text text-white">
                            <h4>Assign game to class</h4>

                            <label :for="'start-game' + item.classID">Choose a date</label>
                            <Datepicker
                                format="YYYY-MM-DD H:i:s"
                                width="100%"
                                :id="'start-game' + item.classID"
                                v-model="startGame"
                                :state="checkData()"
                                class="mb-2 text-white"
                            />
                            <!-- <date-picker :id="'start-game' + item.classID" v-model="startGame" :state="checkData()" class="mb-2"></date-picker> -->
                            <p>'{{ startGame }}'</p>
                            <label :for="'end-game' + item.classID">Choose a date</label>
                            <Datepicker
                                format="YYYY-MM-DD H:i:s"
                                width="100%"
                                :id="'end-game' + item.classID"
                                v-model="endGame"
                                :state="checkData()"
                                class="mb-2 text-white"
                            />
                            <!-- <date-picker :id="'end-game' + item.classID" v-model="endGame" :state="checkData()" class="mb-2"></date-picker> -->
                            <p>'{{ endGame }}'</p>

                            <label :for="'select-topic' + item.classID">Topic</label>
                            <b-form-select :name="'select-topic' + item.classID" v-model="selectedTopic" :options="getTopics()"></b-form-select>

                            <div v-if="selectedTopic">
                                <label :for="'select-scenario' + item.classID">Scenario</label>
                                <b-form-select
                                    :name="'select-scenario' + item.classID"
                                    v-model="selectedScenario"
                                    :options="getScenarios()"
                                ></b-form-select>
                            </div>

                            <div v-if="checkData() && selectedTopic && selectedScenario" class="d-flex">
                                <b-button class="btn-success ml-auto mt-3" @click="sendGameNotification">SEND TO STUDENTS</b-button>
                            </div>
                        </div>
                    </b-card>
                </b-collapse>
            </div>
        </div>
    </div>
</template>

<script>
import Datepicker from "vuejs-datetimepicker"
// import axios from "axios"

export default {
    components: {
        Datepicker
    },
    data() {
        return {
            selectedClass: null,
            startGame: null,
            endGame: null,
            selectedTopic: null,
            selectedScenario: null,
            selectedTeacher: 1
        }
    },
    computed: {
        teacherMessage() {
            return this.$store.state.teacherData
        },
        loadingData() {
            return this.$store.state.loadingData
        }
    },
    created() {
        this.$store.dispatch("getTeacherData")
    },
    methods: {
        getStudentFullName(student) {
            return `${student.firstName ?? ""} ${student.lastName ? student.lastName + " - " : ""} ${student.email}`.trim()
        },
        selectClass(classID) {
            this.selectedClass = classID
        },
        checkData() {
            return this.startGame && this.endGame && this.endGame > this.startGame
        },
        getTopics() {
            return this.$store.getters
                .getTopics()
                ?.map(({ topicID, topicName }) => ({ value: topicID, text: topicName }))
        },
        getScenarios() {
            return this.$store.getters
                .getScenarios({ topicId: this.selectedTopic })
                ?.map(({ scenarioID, name }) => ({ value: scenarioID, text: name }))
        },
        sendGameNotification: async function () {
            const data = {
                classId: this.selectedClass,
                topicId: this.selectedTopic,
                scenarioId: this.selectedScenario,
                teacherId: this.selectedTeacher,
                startGame: new Date(this.startGame).toISOString(),
                endGame: new Date(this.endGame).toISOString()
            }

            this.$store
                .dispatch("authorizedPOST_Promise", { url: "/api/classes", data })
                .catch(() => console.log)
                .then(alert("Invitations created succesful"))
        }
    }
}
</script>
