let alinput = document.getElementById("trigger0")
let alinput0 = document.getElementById("trigger1")
let alinput1 = document.getElementById("trigger2")
let alinput2 = document.getElementById("trigger3")
let alinput3 = document.getElementById("trigger4")
let medinput = document.getElementById("trigger10")
let medinput0 = document.getElementById("trigger5")
let medinput1 = document.getElementById("trigger6")
let medinput2 = document.getElementById("trigger7")
let medinput3 = document.getElementById("trigger8")
let medinput4 = document.getElementById("trigger9")

let alshow = document.getElementById("allergies0")
let alshow1 = document.getElementById("allergies1")
let alshow2 = document.getElementById("allergies2")
let alshow3 = document.getElementById("allergies3")
let alshow4 = document.getElementById("allergies4")
let medshow = document.getElementById("medication0")
let medshow1 = document.getElementById("medication1")
let medshow2 = document.getElementById("medication2")
let medshow3 = document.getElementById("medication3")
let medshow4 = document.getElementById("medication4")
let medshow5 = document.getElementById("medication5")

alinput.addEventListener('click', el => {
    el.preventDefault()
    alshow.classList.toggle('show')
})

alinput0.addEventListener('click', el => {
    el.preventDefault()
    alshow1.classList.toggle('show')
})

alinput1.addEventListener('click', el => {
    el.preventDefault()
    alshow2.classList.toggle('show')
})

alinput2.addEventListener('click', el => { 
    el.preventDefault()
    alshow3.classList.toggle('show')
})


alinput3.addEventListener('click', el => { 
    el.preventDefault()
    alshow4.classList.toggle('show')
})

medinput.addEventListener('click', el => {
    el.preventDefault()
    medshow.classList.toggle('show')
})

medinput0.addEventListener('click', el => {
    el.preventDefault()
    medshow1.classList.toggle('show')
})

medinput1.addEventListener('click', el => {
    el.preventDefault()
    medshow2.classList.toggle('show')
})

medinput2.addEventListener('click', el => {
    el.preventDefault()
    medshow3.classList.toggle('show')
})

medinput3.addEventListener('click', el => {
    el.preventDefault()
    medshow4.classList.toggle('show')
})

medinput4.addEventListener('click', el => {
    el.preventDefault()
    medshow5.classList.toggle('show')
})

