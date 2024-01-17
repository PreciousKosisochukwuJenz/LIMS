
function GetDetails(billnumber) {
    $('#displayTable').hide();
    $("#loader").show();
    $.ajax({
        url: 'ViewResult?billnumber=' + billnumber,
        method: "Get",
        success: function (response) {
            const { distinctServices, billedServices } = response;
            let content = '';

            $.each(distinctServices, function (i, data) {
                const templateServices = BillServices(billedServices, data.TemplateID);

                content += `<tr>
                                <td colspan="12" style="text-align:center;background-color: #007bff; color:white">${data.Template}</td>
                                ${templateServices}
                            </tr>`
            })

            $("#tbody").html(content);
            $('#displayTable').show();
            $("#loader").hide();
        },
        error: function (err) {
            toastr.error(err.responseText, "No Data Found", { showDuration: 500 })
            $('#displayTable').show();
            $("#loader").hide();
        }
    })
}

function BillServices(billServices, templateID) {
    let content = '';
    var HasBeenComputed = billServices[0].HasBeenComputed;
    var ComputedBy = billServices[0].ComputedBy;
    var DateComputed = billServices[0].DateComputed;
    $.each(billServices, function (i, service) {
        if (service.TemplateID == templateID) {
            content += `<tr>
                <td>${service.Service}</td>
                <td>${service.Approved ? "<i class='fa fa-check text-success'> READY</i>" : "<i class='fa fa-clock text-warning'> PENDING</i>"}</td>
                <td>${HasBeenComputed ? "<i class='fa fa-check text-success'></i>" : "<i class='fa fa-times text-danger'></i>"}</td>
                <td>${ComputedBy}</td>
                <td>${DateComputed}</td>
                <td>${service.Approved ? "<i class='fa fa-check text-success'></i>" : "<i class='fa fa-times text-danger'></i>"}</td>
                <td>${service.ApprovedBy}</td>
                <td>${service.DateApproved}</td>
             </tr>`;
        }
    });

    return content;
}



function GetServiceDetails(invoiceNumber) {
    $('#displayTable').hide();
    $("#loader").show();
    $.ajax({
        url: '/Admin/Payment/GetServicesByInvoiceNumber?invoiceNumber=' + invoiceNumber,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (datas) {
            $("#ServiceBody").empty()
            $.each(datas, function (i, data) {
                let html = "";
                html = "<tr id='" + data.Id + "'><td>" + data.ServiceName + "</td><td>" + data.Quantity +"</td><td class='sellingprice-" + data.Id + "' data-id='" + data.SellingPrice + "'>" + data.SellingPriceString + "</td><td><strong class='gross-" + data.Id + " gross'>₦00.00</strong></td></tr>";
                $("#ServiceBody").append(html);
                $("#ServiceName").val("");
                CalculateGrossAmount(data.Quantity, data.SellingPrice, data.Id);
            });
            updateNetAmount();
            var netamount = $("#NetAmount").html();
            var waiveamount = $("#WaiveAmount").html();
            var paidamount = $("#PaidAmount").html();
            var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - ConvertToDecimal(paidamount);

            $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
            $("#ServiceTableLoader").hide();
            $("#serviceTableDiv").show();
        },
        error: function (err) {
            toastr.error("No record found", "Not Found", { showDuration: 500 })
            $("#ServiceTableLoader").hide();
            $("#serviceTableDiv").show();
        }
    })

    // Get Waived Amount
    $.ajax({
        url: '/Admin/Payment/GetWaivedAmountsForInvoiceNumber?invoiceNumber=' + invoiceNumber,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Id != 0) {
                $("#WaiveAmount").html("₦" + numberWithCommas(data.WaiveAmount) + ".00");
            }
            var netamount = $("#NetAmount").html();
            var waiveamount = $("#WaiveAmount").html();
            var paidamount = $("#PaidAmount").html();
            var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - ConvertToDecimal(paidamount);

            $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
        },
        error: function (err) {
            toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 });
            $("#ServiceTableLoader").hide();
            $("#serviceTableDiv").show();
        }
    });

    // Get Total Amount Paid
    $.ajax({
        url: '/Admin/Payment/GetTotalPaidBillAmount?invoiceNumber=' + invoiceNumber,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (totalamountpaid) {
            updateNetAmount();
            if (totalamountpaid != null) {
                $("#PaidAmount").html("₦" + numberWithCommas(totalamountpaid) + ".00");
                var netamount = $("#NetAmount").html();
                var waiveamount = $("#WaiveAmount").html();
                var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - totalamountpaid;

                $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
            } else {
                totalamount = "₦0.00";
                $("#PaidAmount").html(totalamount);
                var netamount = $("#NetAmount").html();
                var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - totalamountpaid;

                $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
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

}


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