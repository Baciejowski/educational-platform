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
            <a v-else class="right waves-effect waves-light btn orange grey-text text-darken-4" @click="addClass()" style="margin-right:10px;">Add class</a>
            <div class="divider orange" />
        </div>
        <form>
            <div class="input-field">
                <input id="search" placeholder="search" type="search" v-model="search" required autocomplete="off" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                <i v-if="search" @click="clearSearch()" class="material-icons black-text">close</i>
            </div>
        </form>
        <!--<div v-if="!search" class="card grey darken-3">
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
        </div>-->
        <ul v-for="group in filteredClasses" :key="group.classID" class="collection with-header">
            <li class="collection-header">
                <h3>{{group.friendlyName}}</h3>
                <div class="right">
                    <a class="waves-effect waves-light btn-small orange grey-text text-darken-4" @click="addStudentToClass(group.classID)" style="margin-right:10px;">Add student</a>
                    <a class="waves-effect waves-light btn-small orange grey-text text-darken-4" @click="editClass(group.classID)" style="margin-right:10px;">Edit class</a>
                    <a class="waves-effect waves-light btn-small orange grey-text text-darken-4" @click="removeClass(group.classID)">Remove class</a>
                </div>
            </li>
            <li v-for="student in group.students.filter(s => s.studentID)" :key="student.studentID" class="collection-item avatar black-text" :id="'studentEntry'+student.studentID">
                <i class="circle green">{{student.lastName[0]+student.firstName[0]}}</i>
                <span class="title">{{student.lastName}} {{student.firstName}}</span>
                <p>{{student.email}}</p>
                <div class="secondary-content">
                    <span class="badge green lighten-3 left grey-text text-darken-3">{{student.sessions.filter(s=> !s.Attempts).length}} waiting</span>
                    <span class="badge orange lighten-3 left grey-text text-darken-3">{{student.sessions.filter(s=> s.Attempts).length}} finished</span>
                    <a @click="editStudent(student.studentID)" class="grey-link hovered-orange" style="margin-left:7px;"><i class="material-icons">edit</i></a>
                    <a class="grey-link hovered-salmon"><i class="material-icons">trending_up</i></a>
                    <a @click="deleteStudent(student.studentID)" class="grey-link hovered-red"><i class="material-icons">delete</i></a>
                </div>
            </li>
            <li v-if="!group.students.length" class="collection-item avatar black-text">
                <i class="circle green material-icons">warning_amber</i>
                <span class="title">No students added yet!</span>
                <p>Add student using buttons located on the top of the page</p>
            </li>
        </ul>
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
                                <input id="editedStudentFirstName" type="text" required autocomplete="off" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label class="grey-text">First name</label>
                            </div>
                            <div class="input-field col s4">
                                <input id="editedStudentLastName" type="text" required autocomplete="off" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label class="grey-text">Last name</label>
                            </div>
                            <div class="input-field col s4">
                                <input id="editedStudentEmail" type="email" required autocomplete="off" class="validate" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label for="editedStudentEmail">Email</label>
                            </div>
                        </div>
                        <button id="submitButton" class="right btn waves-effect waves-light orange" type="submit" name="submit"></button>
                    </form>
                </div>
            </div>
        </div>
        <div id="modal2" class="modal bottom-sheet" style="max-height: 100% !important; height: fit-content; overflow-y: auto; ">
            <div class="modal-content">
                <div class="container">
                    <h4 id="classModalHeader">Class editing</h4>
                    <form id="classForm" @submit="classOnSubmit">
                        <div class="row" style="margin-bottom: 0px">
                            <div class="input-field col s12">
                                <input id="editedClassFriendlyName" type="text" required autocomplete="off" style="border-bottom: 1px solid #9e9e9e; box-shadow: none;">
                                <label class="grey-text">Friendly name</label>
                            </div>
                        </div>
                        <button id="submitButtonClass" class="right btn waves-effect waves-light orange" type="submit" name="submit"></button>
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
            isLoading() {
                return this.$store.state.loadingData
            },
            filteredClasses() {
                let result = JSON.parse(JSON.stringify(this.classes.filter(c=>c.classID)))
                if (result.length) {
                    result = result.sort(function (a, b) {
                        if (a.friendlyName < b.friendlyName) { return -1; }
                        if (a.friendlyName > b.friendlyName) { return 1; }
                        return 0;
                    })
                    for (const group of result) {
                        group.students = group.students.filter(s=>s.studentID).sort(function (a, b) {
                            if (a.lastName < b.lastName) { return -1; }
                            if (a.lastName > b.lastName) { return 1; }
                            if (a.firstName < b.firstName) { return -1; }
                            if (a.firstName > b.firstName) { return 1; }
                            return 0;
                        })
                    }
                }
                if (!this.search) return result
                let lSearch = this.search.toLowerCase()
                for (const group of result)
                    if(!group.friendlyName.toLowerCase().includes(lSearch)) 
                        group.students = group.students.filter(s => s.firstName.toLowerCase().includes(lSearch) || s.lastName.toLowerCase().includes(lSearch) || s.email.toLowerCase().includes(lSearch))
                result = result.filter(r => r.students.length || r.friendlyName.toLowerCase().includes(lSearch))
                return result
            }
        },
        data() {
            return {
                classes: [],
                editedStudentID: 0,
                search: "",
                editedClassID: 0
            }
        },
        created() {
            this.$store.state.loadingData = true
            this.reload()
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
            clearSearch() {
                this.search = ""
            },
            studentOnSubmit(event) {
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
                    this.$store
                        .dispatch("authorizedPUT_PromiseWithHeaders", { url: `/api/students?classID=${document.getElementById("assignedGroup").value}`, data: editedStudent })
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
                    this.$store
                        .dispatch("authorizedPOST_PromiseWithHeaders", { url: `/api/students?classID=${document.getElementById("assignedGroup").value}`, data: editedStudent })
                        .then((resp) => {
                            if (resp.status == 201) {
                                M.toast({ html: "<div class='black-text'>Student was created</div>", classes: "green lighten-3" })
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
            classOnSubmit(event) {
                event.preventDefault()
                this.$store.state.loadingData = true
                const name = document.getElementById("editedClassFriendlyName").value
                if (name.length >= 1 && name.length <= 25) {
                    if (this.editedClassID) {
                        this.$store
                            .dispatch("authorizedPUT_PromiseWithHeaders", { url: "/api/classes", data: { classID: this.editedClassID, friendlyName: name } })
                            .then((data) => {
                                if (data.status == 200) {
                                    M.toast({ html: "<div class='black-text'>Class was modified</div>", classes: "green lighten-3" })
                                    M.Modal.getInstance(document.getElementById("modal2")).close()
                                    this.reload()
                                }
                            })
                            .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                    }
                    else {
                        this.$store
                            .dispatch("authorizedPOST_PromiseWithHeaders", { url: "/api/classes", data: { friendlyName: name } })
                            .then((data) => {
                                if (data.status == 200) {
                                    M.toast({ html: "<div class='black-text'>Class was created</div>", classes: "green lighten-3" })
                                    M.Modal.getInstance(document.getElementById("modal2")).close()
                                    this.reload()
                                }
                            })
                            .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                    }
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
                for (const group of this.classes.sort(function (a, b) {
                    if (a.friendlyName < b.friendlyName) { return -1; }
                    if (a.friendlyName > b.friendlyName) { return 1; }
                    return 0;
                })) {
                    let opt = document.createElement("option")
                    opt.value = group.classID
                    opt.innerHTML = group.friendlyName
                    groupsSelector.appendChild(opt)
                }
                if(assignedGroup) document.getElementById("assignedGroup").value = assignedGroup
                M.FormSelect.init(document.querySelectorAll('select'));
                M.updateTextFields();
                instance.open()
            },
            addStudent() {
                for (var group of this.classes) {
                    group.students = group.students.filter(s => s.studentID)
                }
                this.classes[0].students.push(this.getEmptyStudent())
                this.editStudent(0)
            },
            addStudentToClass(classID) {
                for (var group of this.classes) {
                    group.students = group.students.filter(s => s.studentID)
                }
                console.log(this.classes[0])
                this.classes.find(c => c.classID===classID).students.push(this.getEmptyStudent())
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
            removeClass(id) {
                this.$store.state.loadingData = true
                this.$store
                    .dispatch("authorizedDELETE_PromiseWithHeaders", { url: `/api/classes?id=${id}` })
                    .then((data) => {
                        if (data.status == 200) {
                            M.toast({ html: "<div class='black-text'>Chosen class was deleted!</div>", classes: "green lighten-3" })
                            this.reload()
                        }
                    })
                    .catch((err) => M.toast({ html: `<div class='black-text'>Something went wrong!<br/>${err.message}</div>`, classes: "red lighten-3" }))
                this.$store.state.loadingData = false
            },
            editClass(id) {
                this.editedClassID = id
                const instance = M.Modal.getInstance(document.getElementById("modal2"))
                let group = this.classes.find(c => c.classID===id)
                document.getElementById("editedClassFriendlyName").value = group.friendlyName
                document.getElementById("submitButtonClass").textContent = id ? "Update" : "Create"
                document.getElementById("classModalHeader").textContent = id ? "Class editing" : "Create a class"
                M.updateTextFields();
                instance.open()
            },
            addClass() {
                this.classes = this.classes.filter(c => c.classID)
                this.classes.push({ classID: 0, friendlyName: "", students: [] })
                this.editClass(0)
            }
        }
    }
</script>
<style scoped>
    a.grey-link:link, a.grey-link:visited, a.grey-link {
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