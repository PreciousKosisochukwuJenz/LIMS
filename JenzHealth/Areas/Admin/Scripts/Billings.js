
$("#SearchBy").change(function () {
    if ($(this).val().trim() == "New") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
    }
    else if ($(this).val().trim() == "Existing") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
    }else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
    }
})

$(window).ready(function () {
    // Set  Customer Type
    var radios = $(".Status");
    $.each(radios, function (i, radio) {
        if (radio.id == "REGISTERED_CUSTOMER") {
            radio.checked = true;
        }
        else {
            radio.checked = false;
        }
    })

    if ($("#SearchBy").val().trim() == "New") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
    }
    else if ($("#SearchBy").val().trim() == "Existing") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
    } else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
    }
});

$("#SearchCustomer").click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    e.target.innerHTML = "Searching..."
    var username = $("#CustomerUniqueID").val();
    $.ajax({
        url: 'GetCustomerByUsername?username='+username,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            debugger
            $("#Customername").html(response.Firstname + " " + response.Lastname);
            $("#Customergender").html(response.Gender);
            $("#Customerphonenumber").html(response.PhoneNumber);

            e.target.innerHTML = "Search"
        },
        error: function (err) {
            alert(err);
            e.target.innerHTML = "Search"
        }
    })
})