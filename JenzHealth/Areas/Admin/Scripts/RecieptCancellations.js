
$("input[name='RecieptTypes']").change(function () {
    if ($("input[name='RecieptTypes']:checked").val() == "PAYMENT_RECIEPT") {

    }
    else if ($("input[name='RecieptTypes']:checked").val() == "DEPOSITE_RECIEPT") {

    } else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
    }
});

$("input[name='RecieptTypes']").change(function () {

    if ($("input[name='RecieptTypes']:checked").val() == "PAYMENT_RECIEPT") {

    }
    else {

    }
})
$(window).ready(function () {
    var radios = $(".Status");
    $.each(radios, function (i, radio) {
        if (radio.id == "PAYMENT_RECIEPT") {
            radio.checked = true;
        }
        else {
            radio.checked = false;
        }
    })

    if ($("input[name='RecieptTypes']:checked").val() == "PAYMENT_RECIEPT") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
    }
    else if ($("input[name='RecieptTypes']:checked").val() == "DEPOSITE_RECIEPT") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
    } else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
    }
});

$("#SearchCustomer").click(function (e) {
    e.preventDefault();
    e.stopPropagation();


    var reciept = $("#PaymentReciept").val();
    if (reciept === "") {
        $("#PaymentReciept").addClass("is-invalid");
    }
    else {
        $("#PaymentReciept").removeClass("is-invalid");
        e.target.innerHTML = "Searching..."

        $("#ServiceBody").empty();
        $("#WaiveAmount").html("₦0.00");
        $("#BalanceAmount").html("₦0.00");
        $("#NetAmount").html("₦0.00");
        $("#customerInfoLoader").show();
        $("#ServiceTableLoader").show();
        $("#serviceTableDiv").hide();
        $("#customerinfoDiv").hide();
        $("#Customername").empty();
        $("#Customergender").empty();
        $("#Customerphonenumber").empty();
        $("#Customerage").empty();
        $("#ServiceBody").empty();
        $.ajax({
            url: "GetBillInvoiceWithReciept?recipet=" + reciept,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (invoiceNumber) {
                $.ajax({
                    url: 'GetCustomerByInvoiceNumber?invoiceNumber=' + invoiceNumber,
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

                        // Get Services
                        $.ajax({
                            url: 'GetServicesByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                            method: "Get",
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            success: function (datas) {
                                $("#ServiceBody").empty()
                                $.each(datas, function (i, data) {
                                    let html = "";
                                    html = "<tr id='" + data.Id + "' ><td><button class='btn btn-danger' disabled onclick='Delete(this)'>Remove</button></td><td>" + data.ServiceName + "</td><td><input type='number' value=" + data.Quantity + " class='form-control quantity-" + data.Id + "' onchange='UpdateAmount(this)' onkeyup='UpdateAmount(this)' /></td><td class='sellingprice-" + data.Id + "' data-id='" + data.SellingPrice + "'>" + data.SellingPriceString + "</td><td><strong class='gross-" + data.Id + " gross'>₦00.00</strong></td></tr>";
                                    $("#ServiceBody").append(html);
                                    $("#ServiceName").val("");
                                    CalculateGrossAmount(data.Quantity, data.SellingPrice, data.Id);
                                });
                                updateNetAmount();
                            },
                            error: function (err) {
                                toastr.error(err, "Data not retrieved successfully", { showDuration: 500 });
                            }
                        });

                        // Get Waived Amount
                        $.ajax({
                            url: 'GetWaivedAmountsForInvoiceNumber?invoiceNumber=' + invoiceNumber,
                            method: "Get",
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.Id != 0) {
                                    $("#WaiveAmount").html("₦" + numberWithCommas(data.WaiveAmount) + ".00");
                                    $("#BalanceAmount").html("₦" + numberWithCommas(data.AvailableAmount) + ".00");
                                } else {
                                    var netamount = $("#NetAmount").html();
                                    $("#BalanceAmount").html(netamount);
                                }

                                $("#ServiceTableLoader").hide();
                                $("#serviceTableDiv").show();
                            },
                            error: function (err) {
                                toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 });
                                $("#ServiceTableLoader").hide();
                                $("#serviceTableDiv").show();
                            }
                        });

                        // Populate installmentt
                        $.ajax({
                            url: 'GetInstallmentsByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                            method: "Get",
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            success: function (datas) {
                                if (datas.length > 0) {
                                    installmentCount = 0;
                                    $.each(datas, function (i, data) {
                                        installmentCount++;
                                        let html = "";
                                        html = "<option value='" + data.Id + "'>" + data.InstallmentName + " (₦" + numberWithCommas(data.PartPaymentAmount) + ".00)</option>";
                                        $("#installmentdrp").append(html);
                                    });
                                    $("#InstallmentDiv").show();
                                }

                            },
                            error: function (err) {
                                toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 });
                            }
                        })


                        e.target.innerHTML = "Search"
                    },
                    error: function (err) {
                        toastr.error(err.responseText, "An Error Occurred", { showDuration: 500 })
                        e.target.innerHTML = "Search";
                        $("#customerInfoLoader").hide();
                        $("#customerinfoDiv").show();
                        $("#ServiceTableLoader").hide();
                        $("#serviceTableDiv").show();
                        $("#InstallmentTableLoader").hide();
                    }
                })

            },
            error: function (err) {
                toastr.error(err, "Data not retrieved successfully", { showDuration: 500 });
            }
        })

    }

})
$("#FinishBtn").click(function () {
    var valPaymentReciept = $("input[name='PaymentReciept']").val();
    var valComment = $("#Comment").val();
    if (valPaymentReciept === ""  || valComment == "") {
        $("input[name='PaymentReciept']").addClass("is-invalid");
        $("#Comment").addClass("is-invalid");
        toastr.error("Please, fill the form properly", "Validation error", { showDuration: 500 })

    }
    else {

        $("input[name='PaymentReciept']").removeClass("is-invalid");
        $("#Comment").removeClass("is-invalid");

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
                let data = {
                    PaymentReciept: $("input[name='PaymentReciept']").val(),
                    Comment: $("#Comment").val()
                };
                // Send ajax call to server
                $.ajax({
                    url: 'CancelReciept',
                    method: 'Post',
                    dataType: "json",
                    data: { vmodel: data },
                    success: function (response) {
                        if (response == "success") {
                            location.href = "RecieptCancellations?Saved=true";
                        }
                    }
                })
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
    }
})

function CalculateGrossAmount(quantity, price, RowID) {
    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");
}
function UpdateAmount(e) {
    let RowID = e.parentElement.parentElement.id;

    let quantity = e.value;
    let price = $(".sellingprice-" + RowID)[0].dataset.id;

    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");

    updateNetAmount();
    UpdateBalanceAmount();

}
function updateNetAmount() {
    let grosses = $(".gross");
    let total = 0;
    $.each(grosses, function (i, gross) {
        var amount = ConvertToDecimal(gross.innerText);
        total += amount;
    });
    $("#NetAmount").empty();
    $("#NetAmount").html("₦" + numberWithCommas(total) + ".00")
}
function UpdateBalanceAmount() {
    var netamount = $("#NetAmount").html();
    var waiveamount = $("#WaiveAmount").html();

    var balance = ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount);
    $("#BalanceAmount").html("₦" + numberWithCommas(balance) + ".00")
}

document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})