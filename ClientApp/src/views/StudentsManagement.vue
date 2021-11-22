<template>
    <div class="container">
        <div class="section">
            <h1 style="display: inline-block;">Classes</h1>
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
            <div class="divider orange" />
        </div>
        <div>
            <a class="waves-effect waves-light btn orange" style="margin-bottom: 10px" @click="addStudent()">Add Student</a>
        </div>
        <div class="card grey darken-3">
            <div class="card-content">
                <form id="create_new" @submit="onSubmit">
                    <h3>Create new</h3>
                    <div class="row" style="margin-bottom: 0">
                        <div class="input-field col s12">
                            <input id="friendlyName" type="text" class="validate white-text">
                            <label for="friendlyName" class="white-text">Friendly name</label>
                            <span class="helper-text white-text">Must be between 1 and 25 characters</span>
                        </div>
                    </div>
                    <button class="right btn waves-effect waves-light orange" type="submit" name="submit">Create</button>
                </form>
            </div>
        </div>
        <div v-for="group in classes" :key="group.classID">
            <ul class="collection with-header">
                <li class="collection-header">
                    <h3>{{group.friendlyName}}</h3>
                </li>
                <div v-for="student in group.students" :key="student.studentID">
                    <li v-if="student.studentID" class="collection-item avatar black-text" :id="'studentEntry'+student.studentID">
                    <i class="circle green">{{student.firstName[0]+student.lastName[0]}}</i>
                    <span class="title">{{student.firstName}} {{student.lastName}}</span>
                    <p>{{student.email}}</p>
                    <div class="secondary-content">
                        <span class="badge green lighten-3 left grey-text text-darken-3">4 waiting</span>
                        <span class="badge orange lighten-3 left grey-text text-darken-3">4 finished</span>
                        <a href="#!" @click="editStudent(student.studentID)" class="grey-link hovered-orange" style="margin-left:7px;"><i class="material-icons">edit</i></a>
                        <a href="#!" class="grey-link hovered-salmon"><i class="material-icons">trending_up</i></a>
                        <a href="#!" @click="deleteStudent(student.studentID)" class="grey-link hovered-red"><i class="material-icons">delete</i></a>
                    </div>
                </li>
                </div>
                
                <!--<div v-if="!topic.scenarios.length">
                <div class="collection-item">
                    No scenarios assigned!
                    <div class="secondary-content">
                        <button class="center-align waves-effect waves-light btn-small orange grey-text text-darken-4" @click="deleteTopic(topic.topicID)">Remove topic</button>
                    </div>
                </div>
            </div>-->
            </ul>
        </div>
        <div id="modal1" class="modal bottom-sheet" style="max-height: 100% !important; height: fit-content; overflow-y: auto; ">
            <div class="modal-content">
                <div class="container">
                    <h4 id="modalHeader">Student editing</h4>
                    <form style="margin-bottom: 15px; margin-top: 15px; ">
                        <div class="input-field" style="display: inline-block; margin-right: 20px; min-width: 200px">
                            <select id="assignedGroup" class="black-text">
                            </select>
                            <label>Assigned to class</label>
                        </div>
                    </form>
                    <form id="studentForm" @submit="studentOnSubmit">
                        <div class="row" style="margin-bottom: 0px">
                            <div class="input-field col s4">
                                <input id="editedStudentFirstName" type="text" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label class="grey-text">First name</label>
                            </div>
                            <div class="input-field col s4">
                                <input id="editedStudentLastName" type="text" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label class="grey-text">Last name</label>
                            </div>
                            <div class="input-field col s4">
                                <input id="editedStudentEmail" type="email" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label for="editedStudentEmail">Email</label>
                            </div>
                        </div>
                        <button id="submitButton" class="right btn waves-effect waves-light orange" type="submit" name="submit"></button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import M from "materialize-css"
    export default {
        computed: {
            topicsFromApi() {
                return this.$store.state.topics
            },
            isLoading() {
                return this.$store.state.loadingData
            }
        },
        data() {
            return {
                classes: [],
                editedStudentID: 0
            }
        },
        created() {
            this.$store.state.loadingData = true
            this.$store
                .dispatch("authorizedGET_PromiseWithHeaders", "/api/classes")
                .then((data) => {
                    if (data.status == 200) {
                        this.classes = data.data
                    }
                })
                .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
            this.$store.state.loadingData = false
        },
        mounted() {
            M.Modal.init(document.querySelectorAll('.modal'))
        },
        methods: {
            getEmptyStudent() {
                return {
                    studentID: 0,
                    firstName: "",
                    lastName: "",
                    email: ""
                }
            },
            reload() {
                this.$store
                    .dispatch("authorizedGET_PromiseWithHeaders", "/api/classes")
                    .then((data) => {
                        if (data.status == 200) {
                            this.classes = data.data
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
            },
            async studentOnSubmit(event) {
                event.preventDefault()
                let found = false
                let editedStudent = {}
                for (const group of this.classes) {
                    for (const s of group.students)
                        if (s.studentID === this.editedStudentID) {
                            found = true
                            editedStudent = s
                        }
                    if (found) break
                }

                this.$store.state.loadingData = true
                editedStudent.firstName = document.getElementById("editedStudentFirstName").value
                editedStudent.lastName = document.getElementById("editedStudentLastName").value
                editedStudent.email = document.getElementById("editedStudentEmail").value
                
                M.Modal.getInstance(document.getElementById("modal1")).close()

                if (editedStudent.studentID) {
                    await this.$store
                        .dispatch("authorizedPUT_PromiseWithHeaders", { url: `api/students?classID=${document.getElementById("assignedGroup").value}`, data: editedStudent })
                        .then((data) => {
                            if (data.status == 200) {
                                M.toast({ html: "<div class='black-text'>Student was modified</div>", classes: "green lighten-3" })
                                this.reload()
                            }
                        })
                        .catch((err) => {
                            M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" })
                            this.reload()
                        })
                }
                else {
                    await this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: `api/students?classID=${document.getElementById("assignedGroup").value}`, data: editedStudent })
                        .then((resp) => {
                            if (resp.status == 201) {
                                M.toast({ html: "<div class='black-text'>Student was modified</div>", classes: "green lighten-3" })
                                this.reload()
                                this.editedStudentID = resp.data
                            }
                        })
                        .catch((err) => {
                            M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" })
                            this.reload()
                        })
                }
                this.$store.state.loadingData = false
                document.getElementById(`studentEntry${this.editedStudentID}`).scrollIntoView({
                    behavior: 'smooth'
                });

            },
            onSubmit(event) {
                event.preventDefault()
                this.$store.state.loadingData = true
                const name = document.getElementById("friendlyName").value
                if (name.length >= 1 && name.length <= 25) {
                    this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: "/api/classes", data: { friendlyName: name } })
                        .then((data) => {
                            if (data.status == 200) {
                                M.toast({ html: "<div class='black-text'>New class was created!</div>", classes: "green lighten-3" })
                                document.getElementById("friendlyName").value = ""
                                this.reload()
                            }
                        })
                        .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                }
                else if (name.length == 0) M.toast({ html: "<div class='black-text'>Class name is obligatory!</div>", classes: "red lighten-3" })
                else M.toast({ html: "<div class='black-text'>Class name is too long!</div>", classes: "red lighten-3" })
                this.$store.state.loadingData = false
            },
            editStudent(id) {
                document.getElementById("modalHeader").innerText = id ? "Student editing" : "Create a student"
                this.editedStudentID = id
                const instance = M.Modal.getInstance(document.getElementById("modal1"))
                let found = false
                let student = {}
                let assignedGroup = 0
                for (const group of this.classes) {
                    for (const s of group.students)
                        if (s.studentID === id) {
                            found = true
                            student = s
                            assignedGroup = group.classID
                        }
                    if (found) break
                }
                document.getElementById("editedStudentFirstName").value = student.firstName
                document.getElementById("editedStudentLastName").value = student.lastName
                document.getElementById("editedStudentEmail").value = student.email
                document.getElementById("submitButton").textContent = id ? "Update" : "Create"
                let groupsSelector = document.getElementById("assignedGroup")
                groupsSelector.innerHTML=""
                for (const group of this.classes) {
                    let opt = document.createElement("option")
                    opt.value = group.classID
                    opt.innerHTML = group.friendlyName
                    groupsSelector.appendChild(opt)
                }
                if (student.studentID) document.getElementById("assignedGroup").value = assignedGroup
                M.FormSelect.init(document.querySelectorAll('select'));
                M.updateTextFields();
                instance.open()
            },
            addStudent() {
                for (var group of this.classes) {
                    group.students = group.students.filter(s => s.studentID)
                }
                console.log(this.classes[0])
                this.classes[0].students.push(this.getEmptyStudent())
                this.editStudent(0)
            },
            deleteStudent(id) {
                this.$store.state.loadingData = true
                this.$store
                    .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/students?id=${id}` })
                    .then((data) => {
                        if (data.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen student was deleted!</div>", classes: "green lighten-3" })
                            this.reload()
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                this.$store.state.loadingData = false
            },
        }
    }
</script>
<style scoped>
    a.grey-link:link, a.grey-link:visited {
        color: gray;
    }
    a.btn-small:hover, a.btn-small:active, a.collection-item:hover, a.collection-item:active {
        text-decoration: none;
    }
    a.hovered-red:hover, a.hovered-red:active {
        color: crimson;
    }
    a.hovered-blue:hover, a.hovered-blue:active {
        color: deepskyblue;
    }
    a.hovered-green:hover, a.hovered-green:active {
        color: green;
    }
    a.hovered-orange:hover, a.hovered-orange:active {
        color: darkorange;
    }
    a.hovered-turquoise:hover, a.hovered-turquoise:active {
        color: darkturquoise;
    }
    a.hovered-salmon:hover, a.hovered-salmon:active {
        color: darkmagenta;
    }
    .collection {
        border: none;
    }
    .collection.with-header .collection-header {
        border-color: orange;
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
    }
    .collection.with-header .collection-header h3 {
        display: inline-block;
    }
    .collection .collection-item {
        border: none;
        min-height: fit-content;
    }

    input:not([type]):focus:not([readonly]), input[type=text]:not(.browser-default):focus:not([readonly]), input[type=password]:not(.browser-default):focus:not([readonly]), input[type=email]:not(.browser-default):focus:not([readonly]), input[type=url]:not(.browser-default):focus:not([readonly]), input[type=time]:not(.browser-default):focus:not([readonly]), input[type=date]:not(.browser-default):focus:not([readonly]), input[type=datetime]:not(.browser-default):focus:not([readonly]), input[type=datetime-local]:not(.browser-default):focus:not([readonly]), input[type=tel]:not(.browser-default):focus:not([readonly]), input[type=number]:not(.browser-default):focus:not([readonly]), input[type=search]:not(.browser-default):focus:not([readonly]), textarea.materialize-textarea:focus:not([readonly]) {
        border-bottom: 1px solid orange;
        box-shadow: 0 1px 0 0 orange;
    }
    .select-wrapper.valid > input.select-dropdown, input:not([type]).valid, input:not([type]):focus.valid, input[type=text]:not(.browser-default).valid, input[type=text]:not(.browser-default):focus.valid, input[type=password]:not(.browser-default).valid, input[type=password]:not(.browser-default):focus.valid, input[type=email]:not(.browser-default).valid, input[type=email]:not(.browser-default):focus.valid, input[type=url]:not(.browser-default).valid, input[type=url]:not(.browser-default):focus.valid, input[type=time]:not(.browser-default).valid, input[type=time]:not(.browser-default):focus.valid, input[type=date]:not(.browser-default).valid, input[type=date]:not(.browser-default):focus.valid, input[type=datetime]:not(.browser-default).valid, input[type=datetime]:not(.browser-default):focus.valid, input[type=datetime-local]:not(.browser-default).valid, input[type=datetime-local]:not(.browser-default):focus.valid, input[type=tel]:not(.browser-default).valid, input[type=tel]:not(.browser-default):focus.valid, input[type=number]:not(.browser-default).valid, input[type=number]:not(.browser-default):focus.valid, input[type=search]:not(.browser-default).valid, input[type=search]:not(.browser-default):focus.valid, textarea.materialize-textarea.valid, textarea.materialize-textarea:focus.valid {
        border-bottom: 1px solid orange;
        box-shadow: 0 1px 0 0 orange;
    }
    .badge{
        font-weight: normal;
        margin-left: 7px;
        margin-right: 7px;
    }
</style>