let param, ul, a;

function UrlSearch() {
    // param
    param = $("#SearchInp").val().toLowerCase();

    foundLinks = [];
    // Loop through each ul and get the li
    a = document.querySelectorAll(".nav--no-borders a.dropdown-item");
    $.each(a, function (j, target) {
        if (target.innerText.toLowerCase().indexOf(param) > -1) {
            target.style.display = "";
            foundLinks.push(target);
            //    target.parentElement.parentElement.parentElement.style.display = "";
        }
        else {
            target.style.display = "none";
            target.parentElement.parentElement.parentElement.style.display = "none";
        }
    });

    for (let link of foundLinks) {
        link.parentElement.parentElement.parentElement.style.display = "";
    }
}