<template>
    <div>
        <QuestionsFormLabel />
        <QuestionsForm />
    </div>
</template>

<script>
import QuestionsForm from "@/components/QuestionsForm.vue"
import QuestionsFormLabel from "@/components/QuestionsFormLabel.vue"
export default {
    components: {
        QuestionsFormLabel,
        QuestionsForm
    }
}
</script>

<style>
</style>