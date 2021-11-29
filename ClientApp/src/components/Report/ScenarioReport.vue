<template>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <D3PieChart :config="pie_config" :datum="participation_data" title="Participation"></D3PieChart>
            </div>
            <div class="col-6">
                <D3PieChart :config="pie_config" :datum="passed_data" title="Scenario passed"></D3PieChart>
            </div>
            Długość gry x correctness /group
            Długość gry x ilośc pytań /group
        </div>
    </div>
</template>

<script>
import { D3PieChart } from "vue-d3-charts"
export default {
    components: {
        D3PieChart,
        // D3BarChart,
        // D3LineChart
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
            ]
        }
    }
}
</script>

<style>
</style>