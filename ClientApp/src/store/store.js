import Vue from "vue"
import Vuex from "vuex"
import axios from "axios"

Vue.use(Vuex)

const state = {
    teacherData: [],
    loadingData: true,
    teacher: null,
    auth: null,
    questionForm: {
        scenario: "",
        topic: "",
        questions: {},
        file: null
    },
    createScenario: {
        name: "",
        topic: "",
        teacherTopics: []
    }
}

const getters = {
    getTopics: (state) => () => {
        return state.teacherData?.topics
    },

    getScenarios: (state) => (payload) => {
        const { topicId } = payload
        return state.teacherData.topics.find((item) => item.topicID === topicId).scenarios
    },
    getAuthToken: (state) => () => {
        return state.auth.getTokenSilently()
    }
}

const actions = {
    authorizedGET_Promise({ getters }, url) {
        return getters.getAuthToken().then((token) =>
            axios
                .get(url, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                .then((res) => res.data)
        )
    },
    authorizedPOST_Promise({ getters }, { url, data }) {
        return getters.getAuthToken().then((token) =>
            axios
                .post(url, data, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                .then((res) => res.data)
        )
    },
    getTeacherData({ state, dispatch }) {
        dispatch("authorizedGET_Promise", "/api/classes")
            .then((data) => {
                state.teacherData = data
                state.loadingData = false
                state.teacher = data.teacherID
            })
            .catch((err) => (state.classesMessage = err))
    },
    getTeacherTopics({ state, dispatch }) {
        dispatch("authorizedGET_Promise", "/api/create-scenario")
            .then((data) => {
                state.createScenario.teacherTopics = data?.map((item) => item.topicName)
            })
            .catch((err) => (state.classesMessage = err))
    }
}

const mutations = {
    setAuth(state, auth) {
        state.auth = auth
    }
}

export const store = new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})
