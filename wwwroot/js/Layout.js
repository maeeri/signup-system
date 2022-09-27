const stBtn = document.getElementById("staffBtn")
const stMenu = document.getElementById("staffActions")
const campArea = document.getElementById("camps")
const camperArea = document.getElementById("campers")
const counselorArea = document.getElementById("counselors")

stBtn.addEventListener("click", () => {
    stMenu.classList.toggle("show")
})


window.onclick = function (e) {
    if (!e.target.matches("#staffBtn")) {
        if (stMenu.classList.contains("show")) {
            stMenu.classList.remove("show");
        }
    }

    if (!e.target.matches("#campTrigger")) {
        if (campArea.classList.contains("show")) {
            campArea.classList.remove("show");
        }
    }

    if (!e.target.matches("#camperTrigger")) {
        if (camperArea.classList.contains("show")) {
            camperArea.classList.remove("show");
        }
    }

    if (!e.target.matches("#staffTrigger")) {
        if (counselorArea.classList.contains("show")) {
            counselorArea.classList.remove("show");
        }
    }
}