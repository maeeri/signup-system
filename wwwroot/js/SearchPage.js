const query = document.getElementById("queryString")
const filterDataCamps = document.getElementById("filterDataCamps").value
const filterDataCampers = document.getElementById("filterDataCampers").value
const filterDataCounselors = document.getElementById("filterDataCounselors").value
const campsObj = JSON.parse(filterDataCamps)
const campersObj = JSON.parse(filterDataCampers)
const counselorsObj = JSON.parse(filterDataCounselors)
const searchContents = document.getElementById("searchContents")
const searchSubmit = document.getElementById("searchSubmit")

window.onload = () => {
        searchSubmit.addEventListener("click", event => {
        event.preventDefault()
});

    query.addEventListener('change', () => {
        console.log("hello")
        searchContents.innerHTML = ""
        let campsResult = campsObj.filter(x => x.Name.toLowerCase().includes(query.value.toLowerCase()));
        let campersResult = campersObj.filter(x => x.FirstName.toLowerCase().includes(query.value.toLowerCase()) || x.LastName.toLowerCase().includes(query.value.toLowerCase()));
        let counselorsResult = counselorsObj.filter(x => x.FirstName.toLowerCase().includes(query.value.toLowerCase()) || x.LastName.toLowerCase().includes(query.value.toLowerCase()));
        campsList(campsResult, "Camps");
        peoplesList(campersResult, "Campers");
        peoplesList(counselorsResult, "Counselors");
    });

}


const campsList = (input, header) => {
    createHeader(header);
    input.forEach(x => {
        let el = document.createElement("a")
        let li = document.createElement("li")
        li.appendChild(el)

        el.href = '/Camps/Details/' + x.Id
        el.innerHTML = x.Name
        searchContents.appendChild(li)
    });
};

const peoplesList = (input, header) => {
    createHeader(header);
    input.forEach(x => {
        let el = document.createElement("a")
        let li = document.createElement("li")
        li.appendChild(el)

        el.href = `/${header}/Details/${x.Id}`
        el.innerHTML = x.FirstName + " " + x.LastName
        searchContents.appendChild(li)
    });
};

function createHeader(header) {
    let h = document.createElement("h3")
    h.innerHTML = header
    searchContents.appendChild(h)
};