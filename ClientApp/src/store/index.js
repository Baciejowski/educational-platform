import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";

Vue.use(Vuex);

const state = {
    classData: [],
    loadingData: true
};

const getters = {};

const actions = {
    getClassData({ state }) {
        axios
            .get("/api/classes")
            .then((res) => res.data)
            .then((res) => {
                state.classData = res;
                state.loadingData = false;
            })
            .catch((err) => (state.classesMessage = err));
    }
};

const mutations = {};

export const store = new Vuex.Store({
    state,
    getters,
    actions,
    mutations
});
