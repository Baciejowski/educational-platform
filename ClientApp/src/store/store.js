import Vue from "vue"
import Vuex from "vuex"
import axios from "axios"

Vue.use(Vuex)

const state = {
    classData: [],
    loadingData: true,
    teacher: null,
    auth: null
}

const getters = {
    getTopics: (state) => (payload) => {
        const { classId } = payload
        return state.classData?.find((item) => item.classID === classId)?.teacher.topics
    },

    getScenarios: (state) => (payload) => {
        const { classId, topicId } = payload
        return state.classData?.find((item) => item.classID === classId)?.teacher.topics.find((item) => item.topicID === topicId).scenarios
    },
    getAuthToken: (state) => () => {
        return state.auth.getTokenSilently()
    }
}

const actions = {
    getClassData({ state, dispatch }) {
        dispatch("authorizedGET_Promise", "/api/classes")
            .then((data) => {
                state.classData = data
                state.loadingData = false
                state.teacher = data[0].teacher.teacherID
            })
            .catch((err) => (state.classesMessage = err))
    },
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
