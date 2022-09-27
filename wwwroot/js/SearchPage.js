const query = document.getElementById("queryString")
const filterDataCamps = document.getElementById("filterDataCamps").value
const filterDataCampers = document.getElementById("filterDataCampers").value
const filterDataCounselors = document.getElementById("filterDataCounselors").value
const campsObj = JSON.parse(filterDataCamps)
const campersObj = JSON.parse(filterDataCampers)
const counselorsObj = JSON.parse(filterDataCounselors)
const campsArea = document.getElementById("camps")
const campersArea = document.getElementById("campers")
const staffArea = document.getElementById("counselors")
const searchSubmit = document.getElementById("searchSubmit")
const campBtn = document.getElementById("campTrigger")
const camperBtn = document.getElementById("camperTrigger")
const counselorBtn = document.getElementById("staffTrigger")

const areas = [ campsArea, campersArea, staffArea ]
const btns = [ campBtn, camperBtn, counselorBtn ]

window.onload = () => {
        searchSubmit.addEventListener("click", event => {
        event.preventDefault()
});

    query.addEventListener('change', () => {
        btns.forEach(btn => btn.style.backgroundColor = 'inherit')
        areas.forEach(area => area.innerHTML = "")
        
        let campsResult = campsObj.filter(x => x.Name.toLowerCase().includes(query.value.toLowerCase()));
        let campersResult = campersObj.filter(x => x.FirstName.toLowerCase().includes(query.value.toLowerCase()) || x.LastName.toLowerCase().includes(query.value.toLowerCase()));
        let counselorsResult = counselorsObj.filter(x => x.FirstName.toLowerCase().includes(query.value.toLowerCase()) || x.LastName.toLowerCase().includes(query.value.toLowerCase()));

        campsList(campsResult, "Camps");
        campersList(campersResult, "Campers");
        counselorsList(counselorsResult, "Counselors");
    });

}


const campsList = (input, header) => {
    if (input.length > 0) {

        input.forEach(x => {
            let innerHTML = x.Name
            campsArea.appendChild(createListLink(header, x.Id, innerHTML))
        });

        if (campsArea.classList.contains('hide')) {
            campsArea.classList.add('show')
            campsArea.classList.remove('hide')
        }

    } else {

        if (campsArea.classList.contains('show')) {
            campsArea.classList.add('hide')
            campsArea.classList.remove('show')
        }
    }
    console.log(campsArea.classList)
};

const campersList = (input, header) => {
    if (input.length > 0) {
        input.forEach(x => {
            let innerHTML = x.FirstName + " " + x.LastName
            campersArea.appendChild(createListLink(header, x.Id, innerHTML))
        });

        if (campersArea.classList.contains('hide')) {
            campersArea.classList.add('show')
            campersArea.classList.remove('hide')
        }

    } else {

        if (campersArea.classList.contains('show')) {
            campersArea.classList.add('hide')
            campersArea.classList.remove('show')
        }
    }
};
const counselorsList = (input, header) => {
    if (input.length > 0) {
        input.forEach(x => {
            let innerHTML = x.FirstName + " " + x.LastName
            staffArea.append(createListLink(header, x.Id, innerHTML))
        });

        if (staffArea.classList.contains('hide')) {
            staffArea.classList.add('show')
            staffArea.classList.remove('hide')
        }

    } else {

        if (staffArea.classList.contains('show')) {
            staffArea.classList.add('hide')
            staffArea.classList.remove('show')
        }
    }
};

//function createHeader(header) {
//    let h = document.createElement("h3")
//    h.innerHTML = header
//    searchContents.appendChild(h)
//};

campBtn.addEventListener('click', () => {
    campsArea.classList.toggle('show')
});

camperBtn.addEventListener('click', () => {
    campersArea.classList.toggle('show')
});

counselorBtn.addEventListener('click', () => {
    staffArea.classList.toggle('show')
})

function createListLink(header, id, innerhtml) {
    let el = document.createElement("a")
    let li = document.createElement("li")
    li.appendChild(el)

    el.href = `/${header}/Details/${id}`
    el.innerHTML = innerhtml

    return li;
}