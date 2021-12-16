<template>
    <div>
        <ObligatorySectionModal />
        <ObligatoryQuestionModal />
        <NormalSectionModal />
        <b-form @submit="onSubmit" @reset="onReset" v-if="show" class="m-5 align-self-end">
            <div class="file py-2">
                <h4>Upload your study materials</h4>
                <p>
                    To provide better question generation, please, send to us your material. It can be an article or part of a handbook, anything that
                    contains information for test topic
                </p>
                <b-form-file 
                             placeholder="Choose a file or drop it here..."
                             drop-placeholder="Drop file here..."
                             accept="text/*"
                             multiple 
                             @change="fileChange($event.target.files)"></b-form-file>
            </div>
            <div class="py-2">
                <h4>Study materials - URL</h4>
                <p>To let students familiarize themselves with the topic before the game, please add ulr to your study materials.</p>

                <b-row class="my-1">
                    <b-col>
                        <b-form-input id="input-small" size="sm" placeholder="Enter your url" v-model="url"></b-form-input>
                    </b-col>
                </b-row>
            </div>
            <div v-for="(_, difficulty) in 6" :key="difficulty">
                <div v-if="difficulty === 0" class="d-flex justify-content-between">
                    <h4>Obligatory questions</h4>
                    <b-button variant="outline-secondary" v-b-modal.obligatorySectionModal class="border-0"> ? </b-button>
                </div>
                <div v-else class="d-flex justify-content-between">
                    <h4>
                        <span v-for="n in difficulty" :key="n"> <b-icon-star-fill class="text-warning"></b-icon-star-fill></span>
                    </h4>
                    <b-button variant="outline-secondary" v-b-modal.normalSectionModal class="border-0"> ? </b-button>
                </div>
                <b-button @click="addQuestion(difficulty)" variant="primary"> Add question </b-button>
                <div v-for="(item, questionIndex) in form[difficulty]" :key="questionIndex" class="mx-5">
                    <div class="d-flex justify-content-end">
                        <b-button variant="outline-secondary" v-b-modal.obligatoryQuestion class="border-0"> ? </b-button>
                        <b-button variant="outline-secondary" @click="deleteQuestion(difficulty, questionIndex)" class="border-0"> X </b-button>
                    </div>
                    <div class="d-flex justify-content-between">
                        <h5 class="text-secondary">Assignment #{{ questionIndex + 1 }}</h5>
                        <b-form-rating v-if="difficulty === 0"
                                       v-model="item.difficulty"
                                       inline
                                       no-border
                                       variant="warning"
                                       class="mb-2"></b-form-rating>
                    </div>

                    <b-form-group label="Enter the question:"
                                  :label-for="'q[' + questionIndex + ']'"
                                  description="Try to make it a closed question."
                                  :invalid-feedback="invalidQuestionFeedback"
                                  label-size="lg">
                        <b-form-input v-model="item.content"
                                      type="text"
                                      placeholder="Enter question"
                                      :state="validateOnSubmit && isQuestionNotEmpty(difficulty, questionIndex)"
                                      :name="'q[' + questionIndex + ']'"
                                      required></b-form-input>
                    </b-form-group>

                    <div v-if="difficulty !== 0">
                        <b-form-group :label-for="'ch[' + questionIndex + ']'"
                                      :invalid-feedback="invalidImportantQuestionFeedback"
                                      :state="validateOnSubmit && hasImportantQuestion(difficulty)">
                            <b-form-checkbox v-model="item.isImportant"
                                             :name="'ch[' + questionIndex + ']'"
                                             value.boolean="true"
                                             unchecked-value.boolean="false">
                                &nbsp; Important question
                            </b-form-checkbox>
                        </b-form-group>
                    </div>

                    <div v-for="(answer, index) in item.answers" :key="index" class="my-2 ms-5">
                        <b-form-group label="Enter answer:"
                                      :label-for="'a[' + questionIndex + '][' + index + ']'"
                                      :invalid-feedback="invalidAnswersFeedback">
                            <b-form-input :value="answer.value"
                                          v-model="answer.value"
                                          placeholder="Enter answer"
                                          :name="'a[' + questionIndex + '][' + index + ']'"
                                          @change="onChangeInput(difficulty, questionIndex)"
                                          :state="validateOnSubmit && hasEnoughAnswers(difficulty, questionIndex)"></b-form-input>
                        </b-form-group>

                        <b-form-group :label-for="'ch[' + questionIndex + '][' + index + ']'"
                                      :invalid-feedback="invalidCorrectAnswerFeedback"
                                      :state="validateOnSubmit && hasCorrectAnswer(difficulty, questionIndex)">
                            <b-form-checkbox v-model="answer.isCorrect"
                                             :name="'ch[' + questionIndex + '][' + index + ']'"
                                             value.boolean="true"
                                             unchecked-value.boolean="false">
                                &nbsp; Correct answer
                            </b-form-checkbox>
                        </b-form-group>
                    </div>
                </div>
                <hr class="my-5" />
            </div>

            <div class="d-flex justify-content-end">
                <b-button @click="validateForm" type="submit" variant="primary">Submit</b-button>
                <b-button type="reset" variant="danger">Reset</b-button>
            </div>
        </b-form>
<!--        <b-card class="mt-3 text-white" header="Form Data Result">
            <pre class="m-0 text-white">{{ sendData }}</pre>
        </b-card>-->
    </div>
</template>

<script>
    import ObligatorySectionModal from "./questionsFormModals/ObligatorySectionModal"
    import ObligatoryQuestionModal from "./questionsFormModals/ObligatoryQuestionModal"
    import NormalSectionModal from "./questionsFormModals/NormalSectionModal"
    import M from "materialize-css";
    import axios from "axios";
    export default {
        components: {
            ObligatorySectionModal,
            ObligatoryQuestionModal,
            NormalSectionModal
        },
        data() {
            return {
                files: new FormData(),
                url: null,
                form: [
                    [
                        {
                            content: "",
                            isImportant: false,
                            difficulty: 0,
                            obligatory: false,
                            questionType: 1,
                            answers: [{ value: "", isCorrect: false }]
                        }
                    ]
                ],
                show: true,
                validateOnSubmit: null,
                invalidQuestionFeedback: "Question is not correct: enter content of question",
                invalidAnswersFeedback: "Not enough answers",
                invalidCorrectAnswerFeedback: "At least one answer must be correct",
                invalidImportantQuestionFeedback: "At least one question must be important"
            }
        },
        computed: {
            sendData() {
                const { name, topic } = this.$store.state.createScenario
                return {
                    name,
                    topic,
                    url: this.url,
                    questions: this.form
                }
            }
        },
        mounted() {
            this.resetData()
        },
        methods: {
            fileChange(fileList) {
                for (var i = 0; i < fileList.length; i++) {
                    this.files.append("file", fileList[i], fileList[i].name);
                }
            },
            validateForm() {
                this.validateOnSubmit = true
            },
            onSubmit(event) {
                event.preventDefault()
                const { name, topic } = this.$store.state.createScenario
                if (name && topic) {
                    this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: "/api/create-scenario", data: this.sendData })
                        .then((resp) => {
                            if (resp.status == 200) {
                                axios.post(`/api/scenarios/${resp.data}/media`, this.files, {
                                    headers: {
                                        'Content-Type': 'multipart/form-data'
                                    }
                                }).then((resp2) => {
                                    if (resp2.status == 200) {
                                        var url = new URL(`${this.$store.getters.getPrefix()}/scenario?id=${resp.data}`)
                                        if(!this.$store.state.local) url.port=""
                                        window.location.href = url.toString()
                                    }
                                }).catch((err) => {
                                    M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" })
                                    this.$store
                                        .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/scenarios?id=${resp.data}` })
                                        .then((data) => {
                                            if (data.status == 200) {
                                                M.toast({ html: "<div class='black-text'>Changes were revoked!</div>", classes: "orange lighten-3" })
                                                this.$store.dispatch("getTopicsWithScenarios")
                                            }
                                        })
                                        .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                                });
                            }
                        })
                        .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                }
            },
            resetData() {
                this.form = []
                for (let ix = 0; ix <= 6; ix++) {
                    this.form.push([
                        {
                            content: "",
                            isImportant: ix === 0 || false,
                            difficulty: ix,
                            questionType: 1,
                            obligatory: ix === 0 ? true : false,
                            answers: [{ value: "", isCorrect: false }]
                        }
                    ])
                }
            },
            onReset(event) {
                event.preventDefault()

                this.resetData()

                // Trick to reset/clear native browser form validation state
                this.show = false
                this.$nextTick(() => {
                    this.show = true
                    this.validateOnSubmit = null
                })
            },
            onChangeInput(difficulty, index) {
                this.form[difficulty][index].answers = this.form[difficulty][index].answers.filter((element) => element.value !== "")
                this.form[difficulty][index].answers.push({ value: "", isCorrect: false })
            },
            addQuestion(difficulty) {
                this.form[difficulty].push({
                    content: "",
                    isImportant: false,
                    difficulty,
                    obligatory: difficulty === 0 ? true : false,
                    questionType: 1,
                    answers: [{ value: "", isCorrect: false }]
                })
            },
            deleteQuestion(difficulty, questionIndex) {
                console.log(difficulty, questionIndex)
                this.form[difficulty].splice(questionIndex, 1)
            },
            isQuestionNotEmpty(difficulty, questionIndex) {
                return this.form[difficulty][questionIndex].content !== ""
            },
            hasCorrectAnswer(difficulty, questionIndex) {
                return this.form[difficulty][questionIndex].answers.reduce((acc, item) => {
                    if (item.isCorrect) {
                        acc = true
                    }
                    return acc
                }, false)
            },
            hasEnoughAnswers(difficulty, questionIndex) {
                return this.form[difficulty][questionIndex].answers.length >= 3
            },
            hasImportantQuestion(difficulty) {
                return this.form[difficulty].reduce((acc, item) => {
                    if (item.isImportant) {
                        acc = true
                    }
                    return acc
                }, false)
            }
        }
    }
</script>
<style scoped>
    button {
        margin-right: 1em;
    }

    .btn {
        -webkit-box-shadow: none;
        -moz-box-shadow: none;
        box-shadow: none;
    }
</style>