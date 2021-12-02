<template>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <PieChart2Var :config="pie_config" title="Participation" :data="participation" />
            </div>
            <div class="col-6">
                <PieChart2Var :config="pie_config" title="Scenarios Results" :data="scenarioResults" />
            </div>
        </div>

        <D3BarChart :config="scenarioResultsPerGroup_config" :datum="getFromStore('scenarioResultsPerGroup')" title="Succesed sessions Per Group "></D3BarChart>
        <D3BarChart :config="bar_multiple_config" :datum="getFromStore('avgAnsweredQuestionsPerScenario')" title="Avg Answered Questions %"></D3BarChart>
        <D3BarChart :config="bar_multiple_config" :datum="getFromStore('successPerScenario')" title="Succesed sessions Per Scenario %"></D3BarChart>


        <D3BarChart :config="avgTime_config" :datum="getFromStore('avgTimePerScenario')" title=" Avg Time per scenario"></D3BarChart>

        <D3LineChart :config="line_config" :datum="difficultyDevelopment_data" title="Difficulty scaling factor"></D3LineChart>
        
        <scatter-chart :data="getFromStore('difficultyScaling')" title="Difficulty scaling factor" xtitle="Attempt" ytitle="Level"></scatter-chart>

        <D3BarChart :config="bar_multiple_config" :datum="correctness_data" title="Question correctness"></D3BarChart>

        <!-- <scatter-chart  :data="avgPerAttempt" title="time per attempt" xtitle="Attempt" ytitle="Time"></scatter-chart> -->

        <scatter-chart :data="getFromStore('timePerAttempt')" title="Avg Time per Attempt" xtitle="Attempt" ytitle="Time"></scatter-chart>
        <scatter-chart :data="getFromStore('timePerSkills')" title="time per skills set" xtitle="Scenario" ytitle="Scaled Time"></scatter-chart>
    </div>
</template>

<script>
import { D3BarChart, D3LineChart } from "vue-d3-charts"
import PieChart2Var from "@/components/Report/PieChart2Var"
export default {
    components: {
        D3BarChart,
        D3LineChart,
        PieChart2Var
    },
    created() {
        this.$store.dispatch("getGeneralData")
    },
    computed: {
        participation() {
            return {
                data: this.$store.state.generalReport.participation,
                var1: {
                    name: "Finished",
                    value: this.$store.state.generalReport.participation.find((x) => x.name === "Finished")?.data
                },
                var2: { name: "Pending", value: this.$store.state.generalReport.participation.find((x) => x.name === "Pending")?.data }
            }
        },
        scenarioResults() {
            return {
                data: this.$store.state.generalReport.scenarioResults,
                var1: { name: "Success", value: this.$store.state.generalReport.scenarioResults.find((x) => x.name === "Success")?.data },
                var2: { name: "Fail", value: this.$store.state.generalReport.scenarioResults.find((x) => x.name === "Fail")?.data }
            }
        },
        // timePerSkills() {
        //     return this.$store.state.generalReport.timePerSkills
        // },
        // avgTimePerScenario() {
        //     return this.$store.state.generalReport.avgTimePerScenario
        // },
        // timePerAttempt() {
        //     return this.$store.state.generalReport.timePerAttempt
        // },
        // avgAnsweredQuestionsPerScenario() {
        //     return this.$store.state.generalReport.avgAnsweredQuestionsPerScenario
        // },
        // successPerScenario() {
        //     return this.$store.state.generalReport.successPerScenario
        // }
    },
    methods: {
        getFromStore(property) {
            return this.$store.state.generalReport[property]
        }
    },
    data() {
        return {
            pie_config: {
                key: "name",
                value: "data",
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            bar_config: {
                key: "name",
                values: ["data"],

                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            bar_multiple_config: {
                key: "name",
                values: ["random", "basic", "ai"],
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            avgTime_config: {
                key: "name",
                values: ["random", "basic", "ai", "total"],
                axis: {
                    yTicks: 4
                },
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            scenarioResultsPerGroup_config: {
                key: "name",
                values: ["fail", "success"],
                axis: {
                    yTicks: 2
                },
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            line_config: {
                values: ["aiDifficulty", "teacherDifficulty"],
                date: {
                    key: "date",
                    inputFormat: "%Y",
                    outputFormat: "%Y"
                },
                color: {
                    scheme: ["#7D5BA6", "#FC6471"]
                }
            },
            skills_data: [
                {
                    name: "Vision",
                    data: 30
                },
                {
                    name: "Light",
                    data: 21
                },
                {
                    name: "Speed",
                    data: 60
                }
            ],
            time_data: [
                {
                    name: "Part1",
                    data: 80
                },
                {
                    name: "Part2",
                    data: 21
                },
                {
                    name: "Intelligent",
                    data: 60
                },
                {
                    name: "Loneliness",
                    data: 54
                }
            ],
            difficultyDevelopment_data: [
                { aiDifficulty: 1.2, teacherDifficulty: 1.0, date: 1 },
                { aiDifficulty: 1.8, teacherDifficulty: 1.6, date: 2 },
                { aiDifficulty: 2.1, teacherDifficulty: 1.5, date: 3 },
                { aiDifficulty: 2.9, teacherDifficulty: 1.9, date: 4 }
            ],
            correctness_data: [
                { random: 30, basic: 40, ai: 60, name: "Scenario 1" },
                { random: 40, basic: 17, ai: 92, name: "Scenario 2" },
                { random: 60, basic: 50, ai: 66, name: "Scenario 3" },
                { random: 15, basic: 24, ai: 59, name: "Scenario 4" }
            ],
            data_test: [
                {
                    name: "No adaptivity",
                    data: [
                        [1, 290],
                        [1, 271],
                        [1, 211]
                    ]
                },
                {
                    name: "Basic adaptivity",
                    data: [
                        [1, 74],
                        [2, 97],
                        [1, 314],
                        [2, 135],
                        [3, 145],
                        [4, 190],
                        [1, 105]
                    ]
                },
                {
                    name: "Advanced adaptivity",
                    data: [
                        [1, 114],
                        [2, 69],
                        [1, 139],
                        [2, 164],
                        [3, 147],
                        [1, 140],
                        [2, 108],
                        [3, 80],
                        [1, 115],
                        [2, 116],
                        [3, 82],
                        [1, 68],
                        [1, 127],
                        [2, 70],
                        [3, 72],
                        [4, 98]
                    ]
                }
            ]
        }
    }
}
</script>

<style>
</style>