<template>
    <div>
        <D3PieChart :config="config" :datum="data.data" :title="title"></D3PieChart>
        <div class="row text-center">
            <div class="col-6">
                <b>{{ data.var1.name }}:</b> {{ data.var1.value }}
            </div>
            <div class="col-6">
                <b>{{ data.var2.name }}:</b> {{ data.var2.value }}
            </div>
        </div>
    </div>
</template>

<script>
import { D3PieChart } from "vue-d3-charts"
export default {
    components: {
        D3PieChart
    },
    props: ["config", "data", "title"]
}
</script>
