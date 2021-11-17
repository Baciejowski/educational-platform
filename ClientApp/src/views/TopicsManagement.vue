<template>
    <div class="container">
        <div class="section">
            <h1 style="display: inline-block;">Topics</h1>
            <div v-if="isLoading" class="preloader-wrapper small active right">
                <div class="spinner-layer spinner-yellow-only">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div><div class="gap-patch">
                        <div class="circle"></div>
                    </div><div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
            <div class="divider orange" />
        </div>
        <div class="card grey darken-3">
            <div class="card-content">
                <form id="create_new" @submit="onSubmit">
                    <h3>Create new</h3>
                    <div class="row" style="margin-bottom: 0">
                        <div class="input-field col s12">
                            <input id="topic" type="text" class="validate white-text">
                            <label for="topic" class="white-text">Topic name</label>
                            <span class="helper-text white-text">Must be between 1 and 30 characters</span>
                        </div>
                    </div>
                    <button class="right btn waves-effect waves-light orange" type="submit" name="submit">Create</button>
                </form>
            </div>
        </div>
        <div v-for="topic in topicsFromApi" :key="topic.topicID">
            <div class="collection with-header">
                <div class="collection-header">
                    <h3>{{topic.topicName}}</h3>
                    <a :href="'/create-scenario?topic=' + topic.topicID" class="right waves-effect waves-light btn-small orange grey-text text-darken-4">Add scenario</a>
                </div>
                <div v-for="scenario in topic.scenarios" :key="scenario.scenarioID">
                    <a :href="'/scenario?id=' + scenario.scenarioID" class="collection-item black-text">
                        {{scenario.name}}
                        <a href="#!" @click="deleteScenario(scenario.scenarioID)" class="secondary-content grey-link hovered-red"><i class="material-icons">delete</i></a>
                        <a href="#!" @click="copyScenario(scenario.scenarioID)" class="secondary-content grey-link hovered-turquoise"><i class="material-icons">content_copy</i></a>
                        <a href="#!" class="secondary-content grey-link hovered-blue"><i class="material-icons">ios_share</i></a>
                        <a href="#!" class="secondary-content grey-link hovered-salmon"><i class="material-icons">trending_up</i></a>
                        <a :href="'/scenario?id=' + scenario.scenarioID" class="secondary-content grey-link hovered-orange"><i class="material-icons">edit</i></a>
                        <a :href="'/scenario?id=' + scenario.scenarioID" class="secondary-content grey-link hovered-green"><i class="material-icons">search</i></a>
                    </a>
                </div>
                <div v-if="!topic.scenarios.length">
                    <div class="collection-item">
                        No scenarios assigned!
                        <div class="secondary-content">
                            <button class="center-align waves-effect waves-light btn-small orange grey-text text-darken-4" @click="deleteTopic(topic.topicID)">Remove topic</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import M from "materialize-css"
    export default {
        computed: {
            topicsFromApi() {
                return this.$store.state.topics
            },
            isLoading() {
                return this.$store.state.loadingData
            }
        },
        created() {
            this.$store.dispatch("getTopicsWithScenarios")
        },
        methods: {
            onSubmit(event) {
                event.preventDefault()
                const topic = document.getElementById("topic").value
                if (topic.length >= 1 && topic.length <= 30) {
                    this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: "/api/topics", data: { topicName: topic }})
                        .then((data) => {
                            if (data.status == 200) {
                                M.toast({ html: "<div class='black-text'>New topic was created!</div>", classes: "green lighten-3" })
                                document.getElementById("topic").value = ""
                                this.$store.dispatch("getTopicsWithScenarios")
                            }
                        })
                        .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                }
                else if (topic.length == 0) M.toast({ html: "<div class='black-text'>Topic name is obligatory!</div>", classes: "red lighten-3" })
                else M.toast({ html: "<div class='black-text'>Topic name is too long!</div>", classes: "red lighten-3" })
            },
            deleteTopic(id) {
                this.$store
                    .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/topics?id=${id}` })
                    .then((data) => {
                        if (data.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen topic was deleted!</div>", classes: "green lighten-3" })
                            this.$store.dispatch("getTopicsWithScenarios")
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
            },
            deleteScenario(id) {
                this.$store
                    .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/scenarios?id=${id}` })
                    .then((data) => {
                        if (data.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen scenario was deleted!</div>", classes: "green lighten-3" })
                            this.$store.dispatch("getTopicsWithScenarios")
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
            },
            copyScenario(id) {
                this.$store
                    .dispatch("authorizedCOPY_PromiseWithHeaders", {_url: `/api/scenarios/${id}`})
                    .then((resp) => {
                        if (resp.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen scenario was copied!</div>", classes: "green lighten-3" })
                            this.$store.dispatch("getTopicsWithScenarios")
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
            }
        }
    }
</script>
<style scoped>
    a.grey-link:link, a.grey-link:visited {
        color: gray;
    }
    a.btn-small:hover, a.btn-small:active, a.collection-item:hover, a.collection-item:active {
        text-decoration: none;
    }
    a.hovered-red:hover, a.hovered-red:active {
        color: crimson;
    }
    a.hovered-blue:hover, a.hovered-blue:active {
        color: deepskyblue;
    }
    a.hovered-green:hover, a.hovered-green:active {
        color: green;
    }
    a.hovered-orange:hover, a.hovered-orange:active {
        color: darkorange;
    }
    a.hovered-turquoise:hover, a.hovered-turquoise:active {
        color: darkturquoise;
    }
    a.hovered-salmon:hover, a.hovered-salmon:active {
        color: darkmagenta;
    }
    .collection {
        border: none;
    }
    .collection.with-header .collection-header {
        border-color: orange;
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
    }
    .collection.with-header .collection-header h3 {
        display: inline-block;
    }
    .collection .collection-item {
        border: none;
    }

    input:not([type]):focus:not([readonly]), input[type=text]:not(.browser-default):focus:not([readonly]), input[type=password]:not(.browser-default):focus:not([readonly]), input[type=email]:not(.browser-default):focus:not([readonly]), input[type=url]:not(.browser-default):focus:not([readonly]), input[type=time]:not(.browser-default):focus:not([readonly]), input[type=date]:not(.browser-default):focus:not([readonly]), input[type=datetime]:not(.browser-default):focus:not([readonly]), input[type=datetime-local]:not(.browser-default):focus:not([readonly]), input[type=tel]:not(.browser-default):focus:not([readonly]), input[type=number]:not(.browser-default):focus:not([readonly]), input[type=search]:not(.browser-default):focus:not([readonly]), textarea.materialize-textarea:focus:not([readonly]) {
        border-bottom: 1px solid orange;
        box-shadow: 0 1px 0 0 orange;
    }
    .select-wrapper.valid > input.select-dropdown, input:not([type]).valid, input:not([type]):focus.valid, input[type=text]:not(.browser-default).valid, input[type=text]:not(.browser-default):focus.valid, input[type=password]:not(.browser-default).valid, input[type=password]:not(.browser-default):focus.valid, input[type=email]:not(.browser-default).valid, input[type=email]:not(.browser-default):focus.valid, input[type=url]:not(.browser-default).valid, input[type=url]:not(.browser-default):focus.valid, input[type=time]:not(.browser-default).valid, input[type=time]:not(.browser-default):focus.valid, input[type=date]:not(.browser-default).valid, input[type=date]:not(.browser-default):focus.valid, input[type=datetime]:not(.browser-default).valid, input[type=datetime]:not(.browser-default):focus.valid, input[type=datetime-local]:not(.browser-default).valid, input[type=datetime-local]:not(.browser-default):focus.valid, input[type=tel]:not(.browser-default).valid, input[type=tel]:not(.browser-default):focus.valid, input[type=number]:not(.browser-default).valid, input[type=number]:not(.browser-default):focus.valid, input[type=search]:not(.browser-default).valid, input[type=search]:not(.browser-default):focus.valid, textarea.materialize-textarea.valid, textarea.materialize-textarea:focus.valid {
        border-bottom: 1px solid orange;
        box-shadow: 0 1px 0 0 orange;
    }
</style>