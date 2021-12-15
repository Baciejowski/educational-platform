<template>
    <div>
        <b-tabs v-model="tabIndex" card>
            <b-tab title="GeneralReport" :title-link-class="linkClass(0)"> </b-tab>
            <b-tab title="ScenarioReport" :title-link-class="linkClass(1)"> </b-tab>
        </b-tabs>
        <div v-if="tabIndex === 0">
            <GeneralReport />
        </div>
        <div v-if="tabIndex === 1">
            <ScenarioReport />
        </div>
    </div>
</template>

<script>
import GeneralReport from "@/components/Report/GeneralReport"
import ScenarioReport from "@/components/Report/ScenarioReport"

export default {
    components: {
        GeneralReport,
        ScenarioReport
    },
    data() {
        return {
            tabIndex: 0
        }
    },
    methods: {
        linkClass(idx) {
            if (this.tabIndex === idx) {
                return ["bg-primary", "text-light"]
            } else {
                return ["bg-light", "text-info"]
            }
        }
    }
}
</script>

<style>
</style>