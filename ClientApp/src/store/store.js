import Vue from "vue"
import Vuex from "vuex"
import axios from "axios"

Vue.use(Vuex)

const state = {
    teacherData: [],
    topics: [],
    scenario: {},
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
    },
    urlPrefix: { local: "http://localhost:5000", dev: "https://zpi2021.westeurope.cloudapp.azure.com:5001" },
    local: true,
    generalReport: {
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
    },
    getPrefix: (state) => () => {
        return state.local ? state.urlPrefix.local : state.urlPrefix.dev
    }
}

const actions = {
    authorizedGET_Promise({ getters }, url) {
        return getters.getAuthToken().then((token) =>
            axios
                .get(getters.getPrefix() + url, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                .then((res) => res.data)
        )
    },
    authorizedGET_PromiseWithHeaders({ getters }, url) {
        return getters.getAuthToken().then((token) =>
            axios.get(getters.getPrefix() + url, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    authorizedHEAD_PromiseWithHeaders({ getters }, { url }) {
        return getters.getAuthToken().then((token) =>
            axios.head(getters.getPrefix() + url, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    authorizedPOST_Promise({ getters }, { url, data }) {
        return getters.getAuthToken().then((token) =>
            axios
                .post(getters.getPrefix() + url, data, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                .then((res) => res.data)
        )
    },
    authorizedPOST_PromiseWithHeaders({ getters }, { url, data }) {
        return getters.getAuthToken().then((token) =>
            axios.post(getters.getPrefix() + url, data, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    authorizedPUT_PromiseWithHeaders({ getters }, { url, data }) {
        return getters.getAuthToken().then((token) =>
            axios.put(getters.getPrefix() + url, data, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    authorizedCOPY_PromiseWithHeaders({ getters }, { _url }) {
        return getters.getAuthToken().then((token) =>
            axios({
                method: "COPY",
                url: getters.getPrefix() + _url,
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    authorizedDELETE_PromiseWithHeaders({ getters }, { url }) {
        return getters.getAuthToken().then((token) =>
            axios.delete(getters.getPrefix() + url, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        )
    },
    getTeacherData({ state, dispatch }) {
        dispatch("authorizedGET_Promise", "/api/teachers")
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
    },
    getTopicsWithScenarios({ state, dispatch }) {
        state.loadingData = true
        dispatch("authorizedGET_PromiseWithHeaders", "/api/topics?includeScenarios=true")
            .then((resp) => {
                state.topics = resp.data
                state.loadingData = false
            })
            .catch((err) => (state.classesMessage = err))
    },
    getScenarioByID({ state, dispatch }, { id }) {
        state.isLoading = true
        dispatch("authorizedGET_Promise", `/api/scenarios/${id}?includeQuestions=true&includeAnswers=true`)
            .then((data) => {
                state.scenario = data
                state.loadingData = false
            })
            .catch((err) => (state.classesMessage = err))
    },
    getGeneralData({ state, dispatch }) { 
        state.loadingData = true
        dispatch("authorizedGET_PromiseWithHeaders", "/api/report")
            .then((res) => res.data)
            .then((data) => {
                console.log("request", data)
                Object.entries(data).forEach((entry) => {
                    const [key, value] = entry
                    state.generalReport[key] = JSON.parse(value)
                })
                console.log("request 2", state.generalReport)
                state.loadingData = false
            })
            .catch(console.log)
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
