
$("#Search").click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    if (document.getElementById("Search").checkValidity()) {

        e.target.innerHTML = "Searching..."
        let value = $("#Searchby").val();
        $.ajax({
            url: '/Admin/Customer/SearchCustomerWithIDorPhoneNumber?value=' + value,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#Customername").html(response.Firstname + " " +response.Lastname);
                $("#Customergender").html(response.Gender);
                $("#Customerphonenumber").html(response.PhoneNumber);
                $("#CustomerUID").val(response.CustomerUniqueID);
                // Calcualte age
                let customerDOBYear = new Date(+response.DOB.replace(/\D/g, '')).getFullYear();
                let currentYear = new Date().getFullYear();
                let customerAge = parseInt(currentYear - customerDOBYear);
                $("#Customerage").html(customerAge);

                e.target.innerHTML = "Search"
            },
            error: function (err) {
                toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                e.target.innerHTML = "Search"
            }
        })
    }
});

$("#PaymentType").change(function () {
    var selected = $(this).val();
    debugger

    if (selected == 1) {
        $("#ReferenceNumber").removeAttr("required");
        $("#refDiv").hide();
    } else {
        $("#ReferenceNumber").attr("required",true);
        $("#refDiv").show();
    }
})