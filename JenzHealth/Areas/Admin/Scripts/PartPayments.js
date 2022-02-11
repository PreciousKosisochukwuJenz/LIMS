let installmentCount;
$("#Search").click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    e.target.innerHTML = "Searching..."

    let invoicenumber = $("#Searchby").val();

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
                },
                error: function (err) {
                    toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                }
            })

            e.target.innerHTML = "Search"
        },
        error: function (err) {
            toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
            e.target.innerHTML = "Search"
        }
    })

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

            let netAmount = $("#NetAmount").html();
            let InstallmentNetAmount = $("#InstallmentNetAmount").html();

            if (ConvertToDecimal(netAmount) === ConvertToDecimal(InstallmentNetAmount)) {
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
    if (document.getElementById("installmentField").checkValidity() && document.getElementById("installmentAmountField").checkValidity()) {
        e.target.innerHTML = "Adding..."
        var installmentname = $("#installmentField").val();
        var installmentamount = $("#installmentAmountField").val();

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
