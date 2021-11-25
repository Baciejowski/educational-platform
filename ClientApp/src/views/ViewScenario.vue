<template>
    <div class="container">
        <div class="section">
            <h1 v-if="!scenario || Object.keys(scenario).length === 0 " style="display: inline-block;">Loading...</h1>
            <h1 v-if="scenario && Object.keys(scenario).length > 0 " style="display: inline-block;">{{scenario.Name}}</h1>
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
            <div class="divider orange" style="margin-bottom:10px" />
            <div v-if="scenario && Object.keys(scenario).length > 0 ">
                <div style="display: inline;">
                    <a class="waves-effect waves-light btn orange" style="margin-bottom: 10px" @click="add()">Add Question</a>
                </div>
                <div style="display: inline;">
                    <a :class="'right waves-effect waves-light btn orange'+(aiProposals ? '':' disabled')" style="margin-left: 10px; margin-bottom: 10px" :href="'/AiProposals?id='+scenario.ScenarioID">
                        AI proposals <span v-if="aiProposals" class="new badge white grey-text text-darken-4" style="font-weight:600;top:1px">{{aiProposals}}</span>
                    </a>
                </div>
                <div v-if="scenario.Url" style="display: inline;">
                    <a class="right waves-effect waves-light btn orange" :href="scenario.Url" target="_blank">Reference materials</a>
                </div>
                <form v-if="scenario.Questions.length">
                    <div class="input-field">
                        <input id="search" placeholder="search" type="search" v-model="search" required autocomplete="off" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                        <i v-if="search" @click="clearSearch()" class="material-icons black-text">close</i>
                    </div>
                </form>
                <div v-if="obligatoryQ.length" class="section">
                    <h3><i class="material-icons">tips_and_updates</i></h3><h5>Obligatory questions</h5>
                    <div v-for="question in obligatoryQ" :key="question.QuestionID">
                        <div class="card white z-depth-0" style="border:1px solid; border-color:darkorange;" :id="'questionCard'+question.QuestionID">
                            <div class="card-content">
                                <div class="card-title" style="text-align: left">
                                    <span class="right orange-text" style="display: inline; margin-left:10px;">{{difficultyRepresentation((question.AiDifficulty&&question.AiDifficulty>0)?question.AiDifficulty:question.Difficulty)}}</span>
                                    <span class="black-text" style="display: inline;">{{question.Content}}</span>
                                </div>
                                <div v-if="question.Hint" class="black-text">Hint: {{question.Hint}}</div>
                                <div v-for="answer in question.ABCDAnswers" :key="answer.AnswerID">
                                    <div v-if="answer.Correct" class="black-text">
                                        &check; {{answer.Content}}
                                        <a v-if="answer.Argumentation" class="black-text">({{answer.Argumentation}})</a>
                                    </div>
                                    <div v-if="!answer.Correct" class="black-text">
                                        &cross; {{answer.Content}}
                                        <a v-if="answer.Argumentation" class="black-text">({{answer.Argumentation}})</a>
                                    </div>
                                </div>
                                <div v-if="question.BooleanAnswer!=null" class="black-text">{{question.BooleanAnswer}}</div>
                            </div>
                            <div class="card-action">
                                <a v-if="(question.QuestionType==1)" @click="edit(question.QuestionID)">Edit</a>
                                <a @click="deleteQuestion(question.QuestionID)">Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-for="lvl in [1,2,3,4,5]" :key="lvl">
                    <div v-if="questions(lvl).length" class="section">
                        <h3>{{sectionHeaderIcon(lvl)}}</h3>
                        <h5>{{sectionHeaderText(lvl)}} questions</h5>
                        <div v-for="question in questions(lvl)" :key="question.QuestionID">
                            <div class="card white z-depth-0" style="border:1px solid; border-color:darkorange;" :id="'questionCard'+question.QuestionID">
                                <div class="card-content">
                                    <div class="card-title" style="text-align: left">
                                        <span v-if="question.IsImportant" class="right black-text orange material-icons" style="display: inline; margin-left:10px;">priority_high</span>
                                        <span class="black-text" style="display: inline;">{{question.Content}}</span>
                                    </div>
                                    <div v-if="question.Hint" class="black-text">Hint: {{question.Hint}}</div>
                                    <div v-for="answer in question.ABCDAnswers" :key="answer.AnswerID">
                                        <div v-if="answer.Correct" class="black-text">
                                            &check; {{answer.Content}}
                                            <a v-if="answer.Argumentation" class="black-text">({{answer.Argumentation}})</a>
                                        </div>
                                        <div v-if="!answer.Correct" class="black-text">
                                            &cross; {{answer.Content}}
                                            <a v-if="answer.Argumentation" class="black-text">({{answer.Argumentation}})</a>
                                        </div>
                                    </div>
                                    <div v-if="question.BooleanAnswer!=null" class="black-text">&check; {{question.BooleanAnswer}}</div>
                                </div>
                                <div class="card-action">
                                    <a v-if="(question.QuestionType==1)" @click="edit(question.QuestionID)">Edit</a>
                                    <a @click="deleteQuestion(question.QuestionID)">Remove</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal1" class="modal bottom-sheet" style="max-height: 100% !important; height: fit-content; overflow-y: auto; ">
            <div class="modal-content">
                <div class="container">
                    <h4 id="modalHeader">Question editing</h4>
                    <b-form style="margin-bottom: 15px; margin-top: 15px; ">
                        <div class="input-field" style="display: inline-block; margin-right: 20px; min-width: 200px">
                            <select id="questionType" class="black-text" disabled>
                                <option value="0"><span class="black-text">True/false</span></option>
                                <option value="1"><span class="black-text">Single/multiple choice</span></option>
                                <option value="2"><span class="black-text">Open</span></option>
                            </select>
                            <label>Question type</label>
                        </div>
                        <label style="margin-right: 20px">
                            <input type="checkbox" id="questionImportance" />
                            <span>Important</span>
                        </label>
                        <label>
                            <input type="checkbox" id="questionObligatory" />
                            <span>Obligatory</span>
                        </label>
                        <b-form-rating inline no-border variant="warning" class="mb-2" style="padding: 0px 0px 0px 0px;float:right;margin-top:20px" v-model="difficulty"></b-form-rating>
                    </b-form>
                    <form id="questionForm" @submit="onSubmit">
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import M from "materialize-css";
    export default {
        title() {
            return "scenario preview"
        },
        data() {
            return {
                editedQuestionID: 0,
                difficulty: 0,
                answersToKeep: [],
                addedAnswers: 0,
                search: ""
            }
        },
        computed: {
            scenario() {
                return this.$store.state.scenario
            },
            isLoading() {
                return this.$store.state.loadingData
            },
            obligatoryQ() {
                return this.scenario.Questions.filter(q => q.QuestionID && q.IsObligatory === true && (!this.search || (q.Content.toLowerCase().includes(this.search.toLowerCase()) || q.ABCDAnswers.filter(a=>a.Content.toLowerCase().includes(this.search.toLowerCase())).length)))
            },
            aiProposals() {
                return this.scenario.Questions.filter(q => !q.Difficulty || (q.AiDifficulty && q.AiDifficulty<0 && Math.abs(q.AiDifficulty) != q.Difficulty)).length
            }
        },
        created() {
            this.$store.state.loadingData = true
            const _id = (new URL(window.location.href)).searchParams.get("id")
            if (_id) {
                this.$store
                    .dispatch("authorizedHEAD_PromiseWithHeaders", { url: `/api/scenarios/${_id}?includeQuestions=true&includeAnswers=true` })
                    .then((data) => {
                        if (data.status == 200) {
                            this.$store.dispatch({
                                type: 'getScenarioByID',
                                id: _id
                            })
                        }
                    })
                    .catch(() => window.location.href = '/404')
                this.$store.state.loadingData = false
            }
            else
                window.location.href = '/404'
            
        },
        mounted() {
            M.Modal.init(document.querySelectorAll('.modal'));
            document.getElementById('questionImportance').addEventListener('change', this.importanceChange, false)
            document.getElementById('questionObligatory').addEventListener('change', this.obligatoryChange, false)
        },
        methods: {
            getEmptyQuestion() {
                return {
                    QuestionID: 0,
                    QuestionType: 1,
                    Difficulty: 1,
                    Content: "",
                    Hint: "",
                    IsImportant: false,
                    IsObligatory: false,
                    BooleanAnswer: null,
                    ABCDAnswers: [],
                    AiDifficulty: null
                }
            },
            clearSearch() {
                this.search = ""
            },
            sleep(ms) {
                return new Promise(resolve => setTimeout(resolve, ms));
            },
            importanceChange() {
                const o = document.getElementById("questionObligatory")
                if (document.getElementById("questionImportance").checked) {
                    o.checked = false
                    o.disabled = true
                }
                else {
                    o.disabled = false
                }
            },
            obligatoryChange() {
                const i = document.getElementById("questionImportance")
                if (document.getElementById("questionObligatory").checked) {
                    i.checked = false
                    i.disabled = true
                }
                else {
                    i.disabled = false
                }
            },
            deleteAnswer(event) {
                document.getElementById(event.path[2].id).remove()
            },
            deleteQuestion(id) {
                this.$store.state.loadingData = true
                this.$store
                    .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/questions?id=${id}` })
                    .then((data) => {
                        if (data.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen question was deleted!</div>", classes: "green lighten-3" })
                            this.$store.dispatch({
                                type: 'getScenarioByID',
                                id: (new URL(window.location.href)).searchParams.get("id")
                            })
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                this.$store.state.loadingData = false
            },
            addAnswer() {
                const questionForm = document.getElementById("questionForm")
                document.getElementById("addAnsButton").remove()
                document.getElementById("submitButton").remove()

                const id = this.addedAnswers

                const row = document.createElement("div");
                row.classList.add("row");
                row.id = `answer+${id}Row`
                row.innerHTML = `
                        <label style="margin-top:27px;margin-left:12px">
                            <input class="orange" id="correctAnswer+${id}" type="checkbox"/>
                            <span> </span>
                        </label>
                        <div class="input-field col s6">
                            <input id="Answer+${id}" type="text" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                            <label class="grey-text">Answer</label>
                        </div>
                        <div class="input-field col s5">
                            <input id="Argumentation+${id}" type="text" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                            <label class="grey-text">Argumentation</label>
                        </div>
                        <a href="#!" id="deleteAnswer+${id}" class="red-text text-darken-3" style="margin-top:25px;margin-right:12px"><i class="material-icons">delete</i></a>
                    `
                questionForm.appendChild(row)
                document.getElementById(`deleteAnswer+${id}`).addEventListener('click', this.deleteAnswer, true);

                this.addedAnswers += 1

                const addAnsButton = document.createElement("a")
                addAnsButton.href = "#!"
                addAnsButton.id = "addAnsButton"
                addAnsButton.classList.add("btn")
                addAnsButton.addEventListener('click', this.addAnswer, false)
                addAnsButton.textContent = "Add answer"
                questionForm.appendChild(addAnsButton)

                const button = document.createElement("button")
                button.classList.add("right", "btn", "waves-effect", "waves-light", "orange")
                button.id = "submitButton"
                button.type = "submit"
                button.name = "submit"
                button.textContent = "Update"
                questionForm.appendChild(button)
            },
            questions(lvl) {
                return this.scenario.Questions.filter(q => q.QuestionID && !q.IsObligatory && ((q.AiDifficulty && q.AiDifficulty > 0) ? q.AiDifficulty : q.Difficulty) === lvl && (!this.search || (q.Content.toLowerCase().includes(this.search.toLowerCase()) || q.ABCDAnswers.filter(a => a.Content.toLowerCase().includes(this.search.toLowerCase())).length)))
            },
            sectionHeaderIcon(lvl) {
                return "\u2605".repeat(lvl)
            },
            sectionHeaderText(lvl) {
                const arr = ["Very easy", "Easy", "Average", "Difficult", "Very difficult"]
                return arr[lvl-1]
            },
            difficultyRepresentation(lvl) {
                return "\u2605".repeat(lvl)+"\u2606".repeat(5-lvl)
            },
            async onSubmit(event) {
                event.preventDefault()
                let editedQuestion = this.scenario.Questions.find(q => q.QuestionID === this.editedQuestionID)
                
                    //check if at least one answer is correct
                if (document.getElementById("questionType").selectedIndex) {
                    let correctAnswerExists = false
                    if (editedQuestion.QuestionID) {
                        for (const a of editedQuestion.ABCDAnswers) {
                            if (document.getElementById(`correctAnswer${a.AnswerID}`) && document.getElementById(`correctAnswer${a.AnswerID}`).checked) {
                                correctAnswerExists = true
                                break
                            }
                        }
                    }
                    for (let i = 0; i < this.addedAnswers; i++) {
                        if (document.getElementById(`correctAnswer+${i}`) && document.getElementById(`correctAnswer+${i}`).checked) {
                            correctAnswerExists = true
                            break
                        }
                    }
                    console.log(correctAnswerExists)
                    if (!correctAnswerExists) {
                        M.toast({ html: "<div class='black-text'>At least one answer must be correct!<br/></div>", classes: "red lighten-3" })
                        return
                    }
                }

                this.$store.state.loadingData = true

                if (editedQuestion.QuestionID && ((editedQuestion.AiDifficulty && editedQuestion.AiDifficulty > 0) ? editedQuestion.AiDifficulty : editedQuestion.Difficulty) != this.difficulty) {
                    editedQuestion.Difficulty = this.difficulty
                    editedQuestion.AiDifficulty = null
                }
                editedQuestion.Content = document.getElementById("editedQuestionContent").value
                editedQuestion.Hint = document.getElementById("editedQuestionHint").value
                editedQuestion.QuestionType = document.getElementById("questionType").selectedIndex
                //editedQuestion.BooleanAnswer = 
                editedQuestion.IsImportant = document.getElementById("questionImportance").checked
                editedQuestion.IsObligatory = document.getElementById("questionObligatory").checked
                this.answersToKeep = []

                if (editedQuestion.QuestionID) {
                    for (const a of editedQuestion.ABCDAnswers) {
                        if (document.getElementById(`correctAnswer${a.AnswerID}`)) {
                            a.Correct = document.getElementById(`correctAnswer${a.AnswerID}`).checked
                            a.Content = document.getElementById(`Answer${a.AnswerID}`).value
                            a.Argumentation = document.getElementById(`Argumentation${a.AnswerID}`).value
                            this.answersToKeep.push(a.AnswerID)
                        }
                    }
                    editedQuestion.ABCDAnswers = editedQuestion.ABCDAnswers.filter(a => this.answersToKeep.includes(a.AnswerID))
                }

                for (let i = 0; i < this.addedAnswers; i++) {
                    if (document.getElementById(`correctAnswer+${i}`)) {
                        const answer =
                        {
                            Correct: document.getElementById(`correctAnswer+${i}`).checked,
                            Content: document.getElementById(`Answer+${i}`).value,
                            Argumentation: document.getElementById(`Argumentation+${i}`).value
                        }
                        editedQuestion.ABCDAnswers.push(answer)
                    }
                }
                M.Modal.getInstance(document.getElementById("modal1")).close()

                if (editedQuestion.QuestionID) {
                    await this.$store
                        .dispatch("authorizedPUT_PromiseWithHeaders", { url: "/api/questions", data: editedQuestion })
                        .then((data) => {
                            if (data.status == 200) {
                                M.toast({ html: "<div class='black-text'>Question was modified</div>", classes: "green lighten-3" })
                            }
                        })
                        .catch((err) => {
                            M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" })
                            this.$store.dispatch({
                                type: 'getScenarioByID',
                                id: (new URL(window.location.href)).searchParams.get("id")
                            })
                        })
                }
                else {
                    await this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: `/api/questions?scenarioID=${this.scenario.ScenarioID}`, data: editedQuestion })
                        .then((resp) => {
                            if (resp.status == 201) {
                                M.toast({ html: "<div class='black-text'>Question was modified</div>", classes: "green lighten-3" })
                                this.editedQuestionID = resp.data
                            }
                        })
                        .catch((err) => {
                            M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" })
                        })
                    await this.$store.dispatch({
                        type: 'getScenarioByID',
                        id: (new URL(window.location.href)).searchParams.get("id")
                    })
                }

                this.$store.state.loadingData = false
                document.getElementById(`questionCard${this.editedQuestionID}`).scrollIntoView({
                    behavior: 'smooth'
                });

            },
            edit(id) {
                document.getElementById("modalHeader").innerText = id ? "Question editing" : "Create question"
                this.addedAnswers = 0
                this.editedQuestionID = id
                const questionForm = document.getElementById("questionForm")
                const instance = M.Modal.getInstance(document.getElementById("modal1"))
                let question = this.scenario.Questions.find(q => q.QuestionID === id)
                this.difficulty = (question.AiDifficulty && question.AiDifficulty > 0) ? question.AiDifficulty : question.Difficulty
                document.getElementById("questionImportance").checked = question.IsImportant
                if (question.IsImportant) document.getElementById("questionObligatory").disabled = true
                document.getElementById("questionObligatory").checked = question.IsObligatory
                if (question.IsObligatory) document.getElementById("questionImportance").disabled = true
                document.getElementById("questionType").selectedIndex = question.QuestionType
                questionForm.innerHTML = `
                    <div class="row" style="margin-bottom: 0px">
                        <div class="input-field col s12">
                            <textarea id="editedQuestionContent" class="materialize-textarea" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;"></textarea>
                            <label class="grey-text">Question</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col s12">
                            <textarea id="editedQuestionHint" class="materialize-textarea" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;"></textarea>
                            <label class="grey-text">Hint</label>
                        </div>
                    </div>`
                document.getElementById("editedQuestionContent").value = question.Content
                document.getElementById("editedQuestionHint").value = question.Hint
                M.textareaAutoResize(document.getElementById("editedQuestionContent"))
                M.textareaAutoResize(document.getElementById("editedQuestionHint"))
                for (const a of question.ABCDAnswers) {
                    const row = document.createElement("div");
                    row.style = "margin-bottom: 0px"
                    row.classList.add("row");
                    row.id = `answer${a.AnswerID}Row`
                    row.innerHTML = `
                        <label style="margin-top:27px;margin-left:12px">
                            <input class="orange" id="correctAnswer${a.AnswerID}" type="checkbox"${a.Correct ? ' checked' : ''}/>
                            <span> </span>
                        </label>
                        <div class="input-field col s6">
                            <input id="Answer${a.AnswerID}" type="text" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                            <label class="grey-text">Answer</label>
                        </div>
                        <div class="input-field col s5">
                            <input id="Argumentation${a.AnswerID}" type="text" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                            <label class="grey-text">Argumentation</label>
                        </div>
                        <a href="#!" id="deleteAnswer${a.AnswerID}" class="red-text text-darken-3" style="margin-top:25px;margin-right:12px"><i class="material-icons">delete</i></a>
                    `
                    row.querySelector(`#Answer${a.AnswerID}`).value = a.Content
                    row.querySelector(`#Argumentation${a.AnswerID}`).value = a.Argumentation
                    questionForm.appendChild(row)
                    document.getElementById(`deleteAnswer${a.AnswerID}`).addEventListener('click', this.deleteAnswer, true);
                }
                const addAnsButton = document.createElement("a")
                addAnsButton.href = "#!"
                addAnsButton.id = "addAnsButton"
                addAnsButton.classList.add("btn")
                addAnsButton.textContent = "Add answer"
                addAnsButton.addEventListener('click', this.addAnswer, false)
                questionForm.appendChild(addAnsButton)

                const button = document.createElement("button")
                button.id = "submitButton"
                button.classList.add("right", "btn", "waves-effect", "waves-light", "orange")
                button.type = "submit"
                button.name = "submit"
                button.textContent = id ? "Update" : "Create"
                questionForm.appendChild(button)

                M.FormSelect.init(document.querySelectorAll('select'));
                M.updateTextFields();
                instance.open()
            },
            add() {
                this.scenario.Questions = this.scenario.Questions.filter(q => q.QuestionID)
                this.scenario.Questions.push(this.getEmptyQuestion())
                this.edit(0)
            }
        }
    }
</script>

<style scoped>
    a:hover, a:active {
        text-decoration: none;
    }
</style>