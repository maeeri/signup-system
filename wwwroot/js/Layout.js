//navbar dropdown elements
const stBtn = document.getElementById("staffBtn")
const stMenu = document.getElementById("staffActions")

//toggles visibility of staff menu in navbar
stBtn.addEventListener("click", () => {
    stMenu.classList.toggle("showStaffActions")
})

//hides contents of staff menu if user clicks anywhere else on the page
window.onclick = function (e) {
    if (!e.target.matches("#staffBtn")) {
        if (stMenu.classList.contains("showStaffActions")) {
            stMenu.classList.remove("showStaffActions");
        }
    }
}