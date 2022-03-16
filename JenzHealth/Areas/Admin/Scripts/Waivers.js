
$(window).ready(function () {
    // Set  Customer Type
    var radios = $(".Status");
    $.each(radios, function (i, radio) {
        if (radio.id == "AMOUNT") {
            radio.checked = true;
        }
        else {
            radio.checked = false;
        }
    })
});
$("input[name='WaiveBy']").change(function () {

    if ($("input[name='WaiveBy']:checked").val() == "AMOUNT") {
        $("#waiverAmount").show();
        $("#waiverPercentage").hide();
    }
    else {
        $("#waiverPercentage").show();
        $("#waiverAmount").hide();
    }
})

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
        $("#customerInfoLoader").show();
        $("#ServiceTableLoader").show();
        $("#serviceTableDiv").hide();
        $("#customerinfoDiv").hide();
        $("#netAmountDisplay").html("₦0.00");
        $("#Customername").empty();
        $("#Customergender").empty();
        $("#Customerphonenumber").empty();
        $("#Customerage").empty();

        $.ajax({
            url: 'GetCustomerByInvoiceNumber?invoiceNumber=' + value,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#Customername").html(response.CustomerName);
                $("#Customergender").html(response.CustomerGender);
                $("#Customerphonenumber").html(response.CustomerPhoneNumber);
                $("#Customerage").html(response.CustomerAge);

                $("#customerInfoLoader").hide();
                $("#customerinfoDiv").show();

                $.ajax({
                    url: 'GetServicesByInvoiceNumber?invoiceNumber=' + value,
                    method: "Get",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (datas) {
                        let netAmount = 0;
                        $.each(datas, function (i, data) {
                            let grossAmount = data.Quantity * data.SellingPrice;
                            netAmount += grossAmount;
                        });
                        $("#netAmountDisplay").html("₦" + numberWithCommas(netAmount) + ".00");
                        $("#NetAmount").val(netAmount);

                        $("#ServiceTableLoader").hide();
                        $("#serviceTableDiv").show();
                    },
                    error: function (err) {
                        toastr.error("No customer record found", "Not Found", { showDuration: 500 });
                        $("#ServiceTableLoader").hide();
                        $("#serviceTableDiv").show();
                    }
                })

                e.target.innerHTML = "Search"
            },
            error: function (err) {
                toastr.error("No customer record found", "Not Found", { showDuration: 500 });
                e.target.innerHTML = "Search";
                $("#customerInfoLoader").hide();
                $("#customerinfoDiv").show();

                $("#ServiceTableLoader").hide();
                $("#serviceTableDiv").show();
            }
        })
    }
});

$("#FinishBtn").click(function () {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to proceed with this operation?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.value) {
            $("#WaiverForm").submit();
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Cancelled :)',
                'error'
            )
        }
    })
})


$("#waiverAmount").keyup(function () {
    var waiveby = $("input[name='WaiveBy']:checked").val();
    if (waiveby == "AMOUNT") {
        let netAmount = $("#NetAmount").val();
        let balance = (netAmount - ConvertToDecimal($("#waiverAmount").val()));
        $("#balanceAmountDisplay").html("₦" + numberWithCommas(balance) + ".00");
        $("#AvailableAmount").val(balance);
        $("#WaiveAmount").val(ConvertToDecimal($("#waiverAmount").val()));

    }
});

$("#waiverPercentage").keyup(function () {
    var waiveby = $("input[name='WaiveBy']:checked").val();
    if (waiveby == "PERCENTAGE") {
        let netAmount = $("#NetAmount").val();

        let waiveAmount = (netAmount * ($("#waiverPercentage").val() / 100));
        let balance = netAmount - waiveAmount;
        $("#balanceAmountDisplay").html("₦" + numberWithCommas(balance) + ".00");
        $("#AvailableAmount").val(balance);
        $("#WaiveAmount").val(waiveAmount);

    }
})

document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})