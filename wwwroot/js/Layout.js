let stBtn = document.getElementById("staffBtn")
let admBtn = document.getElementById("adminBtn")

let stMenu = document.getElementById("staffActions")
let admMenu = document.getElementById("adminActions")

stBtn.addEventListener("click", () => {
    stMenu.classList.toggle("show")
})

admBtn.addEventListener('click', () => {
    admMenu.classList.toggle('show')
})

window.onclick = function (e) {
    if (!e.target.matches("#staffBtn")) {
        if (stMenu.classList.contains("show")) {
            stMenu.classList.remove("show");
        }
    }
    if (!e.target.matches("#adminBtn")) {
        if (admMenu.classList.contains("show")) {
            admMenu.classList.remove("show");
        }
    }
}