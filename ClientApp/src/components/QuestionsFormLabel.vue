<template>
    <div class="px-5" style="margin-top: 20px">
        <h1>Create new scenario</h1>
        <div class="pt-2 px-3">
            <b-form-group label="Name">
                <b-form-input v-model="$store.state.createScenario.name" placeholder="Enter scenario name"></b-form-input>
            </b-form-group>
            <b-form-group label="Topic">
                <b-form-radio-group v-model="topic.selected" :options="topic.options" name="radio-inline-form-topic"></b-form-radio-group>
            </b-form-group>
            <b-form-group v-if="topic.selected === 'New Topic'">
                <b-form-input v-model="$store.state.createScenario.topic" placeholder="Enter topic name"></b-form-input>
            </b-form-group>
            <b-form-group v-else>
                <b-form-select v-model="$store.state.createScenario.topic" :options="createdTopics"></b-form-select>
            </b-form-group>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            topic: {
                selected: "Select from list",
                options: ["Select from list", "New Topic"]
            }
        }
    },
    computed: {
        createdTopics() {
            return this.$store.state.createScenario.teacherTopics
        }
    },
    created() {
        this.$store.dispatch("getTeacherTopics")
    }
}
</script>