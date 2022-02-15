let installmentCount;
$("#Search").click(function (e) {
    e.preventDefault();
    e.stopPropagation();

    let invoicenumber = $("#Searchby").val();

    if (invoicenumber === "") {
        $("#Searchby").addClass("is-invalid");
    }
    else {
        $("#Searchby").removeClass("is-invalid");
        e.target.innerHTML = "Searching...";
        $("#WaiveAmount").html("₦0.00");
        $("#BalanceAmount").html("₦0.00");
        $("#NetAmount").html("₦0.00");
        $("#InstallmentNetAmount").html("₦0.00");
        $("#customerInfoLoader").show();
        $("#ServiceTableLoader").show();
        $("#InstallmentTableLoader").show();
        $("#serviceTableDiv").hide();
        $("#InstallmentTableDiv").hide();
        $("#customerinfoDiv").hide();
        $("#Customername").empty();
        $("#Customergender").empty();
        $("#Customerphonenumber").empty();
        $("#Customerage").empty();
        $("#ServiceBody").empty();
        $("#InstallmentBody").empty();

        $.ajax({
            url: 'GetCustomerByInvoiceNumber?invoiceNumber=' + invoicenumber,
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

                // Populate Service
                $.ajax({
                    url: 'GetServicesByInvoiceNumber?invoiceNumber=' + invoicenumber,
                    method: "Get",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (datas) {
                        $("#ServiceBody").empty()
                        $.each(datas, function (i, data) {
                            let html = "";
                            html = "<tr id='" + data.Id + "' ><td>" + data.ServiceName + "</td><td>" + data.Quantity + "<td class='sellingprice-" + data.Id + "' data-id='" + data.SellingPrice + "'>" + data.SellingPriceString + "</td><td><strong class='gross-" + data.Id + " gross'>₦00.00</strong></td></tr>";
                            $("#ServiceBody").append(html);
                            $("#ServiceName").val("");
                            CalculateGrossAmount(data.Quantity, data.SellingPrice, data.Id);
                        });
                        updateNetAmount();
                    },
                    error: function (err) {
                        toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 })
                    }
                });

                // Get Waived Amount
                $.ajax({
                    url: 'GetWaivedAmountsForInvoiceNumber?invoiceNumber=' + invoicenumber,
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
                    url: 'GetInstallmentsByInvoiceNumber?invoiceNumber=' + invoicenumber,
                    method: "Get",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (datas) {
                        $("#InstallmentBody").empty()
                        installmentCount = 0;
                        $.each(datas, function (i, data) {
                            installmentCount++;
                            let html = "";
                            html = "<tr id='" + data.Id + "'><td><button class='btn btn-danger' onclick='DeleteInstallment(this)'>Remove</button></td><td class='installmentName-" + installmentCount + "'>" + data.InstallmentName + "</td><td class='installmentAmount installmentamount-" + installmentCount + "'>₦" + numberWithCommas(data.PartPaymentAmount) + ".00</td></tr>";
                            $("#InstallmentBody").append(html);
                        });
                        updateInstallmentNetAmount();
                        $("#InstallmentTableLoader").hide();
                        $("#InstallmentTableDiv").show();
                    },
                    error: function (err) {
                        toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 });
                        $("#InstallmentTableLoader").hide();
                        $("#InstallmentTableDiv").show();
                    }
                })

                e.target.innerHTML = "Search"
            },
            error: function (err) {
                toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                e.target.innerHTML = "Search";
                $("#customerInfoLoader").hide();
                $("#customerinfoDiv").show();
                $("#InstallmentTableLoader").hide();
                $("#InstallmentTableDiv").show();
                $("#ServiceTableLoader").hide();
                $("#serviceTableDiv").show();
            }
        })

    }

})
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

            let balanceAmount = $("#BalanceAmount").html();
            let InstallmentNetAmount = $("#InstallmentNetAmount").html();

            if (ConvertToDecimal(balanceAmount) === ConvertToDecimal(InstallmentNetAmount)) {
                let InstallmentList = [];
                var table = $("#InstallmentBody")[0].children;
                $.each(table, function (i, tr) {
                    i += 1;
                    // Create installment
                    let installment = {};
                    installment.Id = tr.id;
                    installment.InstallmentName = tr.children[1].innerText;
                    installment.BillInvoiceNumber = $("#Searchby").val();
                    installment.PartPaymentAmount = ConvertToDecimal(tr.children[2].innerText);

                    // Add to Installment list
                    InstallmentList.push(installment);
                });
                // Send ajax call to server
                $.ajax({
                    url: 'PartPayments',
                    method: 'Post',
                    dataType: "json",
                    data: { vmodel: InstallmentList },
                    success: function (response) {
                        location.href = "PartPayments?Saved=true";
                    }
                })
            }
            else {
                toastr.error("Installment net amount must be equal to service net amount", "Validation failed", { showDuration: 500 })
            }

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

$("#AddInstallment").click(function (e) {
    var installmentamount = $("#installmentAmountField").val();

    if (installmentamount === "₦0.00") {
        $("#installmentAmountField").addClass("is-invalid");
    }
    else {
        $("#installmentAmountField").removeClass("is-invalid");
        e.target.innerHTML = "Adding..."
        installmentCount++;
        var installmentname = "INSTALLMENT " + installmentCount;

        let html = "";
        html = "<tr><td><button class='btn btn-danger' onclick='DeleteInstallment(this)'>Remove</button></td><td class='installmentName-" + installmentCount + "'>" + installmentname + "</td><td class='installmentAmount installmentamount-" + installmentCount + "'>" + installmentamount + "</td></tr>";
        $("#InstallmentBody").append(html);
        $("#installmentField").val("");
        $("#installmentAmountField").val("");

        updateInstallmentNetAmount();
        e.target.innerHTML = "Add"
    }
});

function updateInstallmentNetAmount() {
    let installmentAmounts = $(".installmentAmount");
    let total = 0;
    $.each(installmentAmounts, function (i, installmentAmount) {
        var amount = ConvertToDecimal(installmentAmount.innerText);
        total += amount;
    });
    $("#InstallmentNetAmount").empty();
    $("#InstallmentNetAmount").html("₦" + numberWithCommas(total) + ".00")
}
function DeleteInstallment(e) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            e.parentElement.parentElement.remove();
            toastr.success("Removed", "Installment removed", { showDuration: 500 })
            updateInstallmentNetAmount();
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Deactivation cancelled :)',
                'error'
            )
        }
    })
}
