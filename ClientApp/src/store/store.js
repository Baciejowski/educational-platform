import Vue from "vue"
import Vuex from "vuex"
import axios from "axios"

Vue.use(Vuex)

const state = {
    classData: [],
    loadingData: true,
    teacher: null
}

const getters = {
    getTopics: (state) => (payload) => {
        const { classId } = payload
        return state.classData?.find((item) => item.classID === classId)?.teacher.topics
    },

    getScenarios: (state) => (payload) => {
        const { classId, topicId } = payload
        return state.classData?.find((item) => item.classID === classId)?.teacher.topics.find((item) => item.topicID === topicId).scenarios
    }
}

const actions = {
    getClassData({ state }) {
        axios
            .get("/api/classes")
            .then((res) => res.data)
            .then((res) => {
                state.classData = res
                state.loadingData = false
                state.teacher= res[0].teacher.teacherID
            })
            .catch((err) => (state.classesMessage = err))
    }
}

const mutations = {}

export const store = new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})
