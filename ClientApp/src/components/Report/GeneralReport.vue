<template>
    <div>
        XYZ
        <D3PieChart :config="chart_config" :datum="chart_data"></D3PieChart>
    </div>
</template>

<script>
import { D3PieChart } from "vue-d3-charts"

export default {
    components: {
        D3PieChart
    },
    data() {
        return {
            chart_config: {
                margin: { top: 100, bottom: 20 },
                key: "name",
                value: "total",
                color: {
                    scheme: ["#55D6BE", "#ACFCD9", "#7D5BA6", "#DDDDDD", "#FC6471"]
                },
                radius: { inner: 50 }
            },
            chart_data: [
                {
                    name: "Lorem",
                    total: 30,
                    color: "#0000FF"
                },
                {
                    name: "Ipsum",
                    total: 21,
                    color: "#00FF00"
                },
                {
                    name: "Dolor",
                    total: 20,
                    color: "#FF0000"
                }
            ]
        }
    }
}
</script>

<style>
</style>