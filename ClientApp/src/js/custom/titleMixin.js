function getTitle(vm) {
    const { title } = vm.$options
    if (title) {
        return "EduGame \u2013 " + (typeof title === 'function'
            ? title.call(vm)
            : title)
    }
    else return "EduGame"
}
export default {
    created() {
        const title = getTitle(this)
        if (title) {
            document.title = title
        }
    }
}