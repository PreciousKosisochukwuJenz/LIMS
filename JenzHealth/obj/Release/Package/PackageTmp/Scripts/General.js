$(".currency").inputmask('currency', {
    rightAlign: true,
    "prefix": "₦"
});

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function ConvertToDecimal(amount) {
    return Number(amount.replace(/[^0-9.-]+/g, ""));
}

$("#close").click(function () {
    $(".main-content-container").empty();
});

var url = window.location.href.split('?')[0];
for (let i = 0; i < document.links.length; i++) {
    let link = document.links[i];
    if (link.href == url) {
        //link.parentNode.className = 'show';
        //link.parentNode.parentNode.className = 'show';
    }
}