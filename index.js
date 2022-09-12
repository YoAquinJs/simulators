let icons = 47
let iconsInRow = Math.round(window.innerWidth / 56) + 1
let rows = Math.round(window.innerHeight / 56) + Math.ceil((Math.tan(0.261799)*730)/60)

function CreateRow(container, count){
    row = `<div class='row'>`

    randomSpeed = Math.round(Math.random() * (30) + 10)
    randomStart = Math.round(Math.random() * randomSpeed)
    randomDirection = Math.round(Math.random())

    subRow = `<div style='animation: anim${randomDirection} ${randomSpeed}s linear infinite; animation-delay: -${randomStart}s;'>`

    for (let i = 0; i < iconsInRow; i++) {
        subRow += `<img class='px-1 py-1' src='icons/icon${Math.floor(Math.random() * icons)}.svg'/>`
    }

    subRow += "</div>"
    row += `${subRow}</div>`

    container.innerHTML += row
}

window.onload = function(){
    container = document.getElementById("background")
    container.style.height = `${document.documentElement.scrollHeight-1}px`

    if(window.innerWidth < 365){
        carousel = document.getElementById("carousel")
        if(carousel != null){
            carousel.style.margin = "0px 0px"
            console.log(carousel)

        }
    }

    for(let i = 0; i < rows; i++){
        CreateRow(container, i)
    }
}