function toggleVisibilityById(id){
    const form = document.getElementById(id);
    form.style.visibility = form.style.visibility === "visible" ? "hidden" : "visible";
}