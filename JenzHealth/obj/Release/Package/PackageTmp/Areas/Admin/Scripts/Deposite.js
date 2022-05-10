
$("#Search").click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    let value = $("#Searchby").val();

    if (value === "") {
        $("#Searchby").addClass("is-invalid");
    }
    else {
        $("#Searchby").removeClass("is-invalid");

        e.target.innerHTML = "Searching...";
        $("#Customername").empty();
        $("#Customergender").empty();
        $("#Customerphonenumber").empty();
        $("#Customerage").empty();
        $("#customerInfoLoader").show();
        $("#customerinfoDiv").hide();

        $.ajax({
            url: '/Admin/Customer/SearchCustomerWithIDorPhoneNumber?value=' + value,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#Customername").html(response.Firstname + " " + response.Lastname);
                $("#Customergender").html(response.Gender);
                $("#Customerphonenumber").html(response.PhoneNumber);
                $("#CustomerUID").val(response.CustomerUniqueID);
                // Calcualte age
                let customerDOBYear = new Date(+response.DOB.replace(/\D/g, '')).getFullYear();
                let currentYear = new Date().getFullYear();
                let customerAge = parseInt(currentYear - customerDOBYear);
                $("#Customerage").html(customerAge);

                $("#customerInfoLoader").hide();
                $("#customerinfoDiv").show();

                e.target.innerHTML = "Search"
            },
            error: function (err) {
                toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                e.target.innerHTML = "Search";
                $("#customerInfoLoader").hide();
                $("#customerinfoDiv").show();

            }
        })
    }
});

$("#PaymentType").change(function () {
    var selected = $(this).val();

    if (selected == 1) {
        $("#ReferenceNumber").removeAttr("required");
        $("#refDiv").hide();
    } else {
        $("#ReferenceNumber").attr("required", true);
        $("#refDiv").show();
    }
})
document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})