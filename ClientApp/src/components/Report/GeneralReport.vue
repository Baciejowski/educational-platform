<template>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <D3PieChart :config="pie_config" :datum="participation_data" title="Participation"></D3PieChart>
            </div>
            <div class="col-6">
                <D3PieChart :config="pie_config" :datum="passed_data" title="Scenario passed"></D3PieChart>
            </div>
        </div>

        <D3BarChart :config="bar_config" :datum="skills_data" title="Skills %"></D3BarChart>

        <D3BarChart :config="bar_config" :datum="time_data" title="Time per scenario"></D3BarChart>

        <D3LineChart :config="line_config" :datum="difficultyDevelopment_data" title="Difficulty scalling factory"></D3LineChart>

        <D3BarChart :config="bar_multiple_config" :datum="correctness_data" title="Question correctness"></D3BarChart>
        
        <line-chart :data="data_test" title="avg time per scenario" xtitle="Scenario" ytitle="Time"></line-chart>
    </div>
</template>

<script>
import { D3PieChart, D3BarChart, D3LineChart } from "vue-d3-charts"

export default {
    components: {
        D3PieChart,
        D3BarChart,
        D3LineChart
    },
    data() {
        return {
            pie_config: {
                key: "name",
                value: "total",
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            bar_config: {
                key: "name",
                values: ["total"],

                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                }
            },
            bar_multiple_config: {
                key: "scenario",
                values: ["random", "basic", "ai"],
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
            participation_data: [
                {
                    name: "Finished",
                    total: 30
                },
                {
                    name: "Pending",
                    total: 21
                }
            ],
            passed_data: [
                {
                    name: "Passed",
                    total: 80
                },
                {
                    name: "Failed",
                    total: 21
                }
            ],
            skills_data: [
                {
                    name: "Vision",
                    total: 30
                },
                {
                    name: "Light",
                    total: 21
                },
                {
                    name: "Speed",
                    total: 60
                }
            ],
            time_data: [
                {
                    name: "Part1",
                    total: 80
                },
                {
                    name: "Part2",
                    total: 21
                },
                {
                    name: "Intelligent",
                    total: 60
                },
                {
                    name: "Loneliness",
                    total: 54
                }
            ],
            difficultyDevelopment_data: [
                { aiDifficulty: 1.2, teacherDifficulty: 1.0, date: 1 },
                { aiDifficulty: 1.8, teacherDifficulty: 1.6, date: 2 },
                { aiDifficulty: 2.1, teacherDifficulty: 1.5, date: 3 },
                { aiDifficulty: 2.9, teacherDifficulty: 1.9, date: 4 }
            ],
            correctness_data: [
                { random: 30, basic: 40, ai: 60, scenario: "Scenario 1" },
                { random: 40, basic: 17, ai: 92, scenario: "Scenario 2" },
                { random: 60, basic: 50, ai: 66, scenario: "Scenario 3" },
                { random: 15, basic: 24, ai: 59, scenario: "Scenario 4" }
            ],
            data_test: [
                { name: "No adaptivity", data: [[1,5],[2,4],[3,7],[4,6]] },
                { name: "Basic adaptivity", data: { 1: 2, 2: 5, 3: 4, 4: 3 } },
                { name: "Advanced adaptivity", data: { 1: 3, 2: 6, 3: 5, 4: 7 } }
            ]
        }
    }
}
</script>

<style>
</style>