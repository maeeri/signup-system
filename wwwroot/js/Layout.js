let stBtn = document.getElementById("staffBtn")
let stMenu = document.getElementById("staffActions")

stBtn.addEventListener("click", () => {
    stMenu.classList.toggle("show")
})


window.onclick = function (e) {
    if (!e.target.matches("#staffBtn")) {
        if (stMenu.classList.contains("show")) {
            stMenu.classList.remove("show");
        }
    }
  
}