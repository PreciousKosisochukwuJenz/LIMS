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