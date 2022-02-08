
$("#SearchBy").change(function () {
    if ($(this).val().trim() == "New") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
    }
    else if ($(this).val().trim() == "Existing") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
    } else {
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

    let searchby = $("#SearchBy").val();

    if (searchby == "New") {
        var username = $("#CustomerUniqueID").val();
        $.ajax({
            url: 'GetCustomerByUsername?username=' + username,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#Customername").html(response.Firstname + " " + response.Lastname);
                $("#Customergender").html(response.Gender);
                $("#Customerphonenumber").html(response.PhoneNumber);

                // Calcualte age
                let customerDOBYear = new Date(+response.DOB.replace(/\D/g, '')).getFullYear();
                let currentYear = new Date().getFullYear();
                let customerAge = parseInt(currentYear - customerDOBYear);
                $("#Customerage").html(customerAge);


                e.target.innerHTML = "Search"
            },
            error: function (err) {
                alert(err);
                e.target.innerHTML = "Search"
            }
        })
    } else {
        var invoiceNumber = $("#InvoiceNumber").val();
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

                $.ajax({
                    url: 'GetServicesByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                    method: "Get",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (datas) {
                        $("#ServiceBody").empty()
                        $.each(datas, function (i, data) {
                            let html = "";
                            html = "<tr id='" + data.Id + "' ><td><button class='btn btn-danger' onclick='Delete(this)'>Remove</button></td><td>" + data.ServiceName + "</td><td><input type='number' value="+data.Quantity+" class='form-control quantity-" + data.Id + "' onchange='UpdateAmount(this)' onkeyup='UpdateAmount(this)' /></td><td class='sellingprice-" + data.Id + "' data-id='" + data.SellingPrice + "'>" + data.SellingPriceString + "</td><td><strong class='gross-" + data.Id + " gross'>₦00.00</strong></td></tr>";
                            $("#ServiceBody").append(html);
                            $("#ServiceName").val("");
                            CalculateGrossAmount(data.Quantity, data.SellingPrice, data.Id);
                        });
                        updateNetAmount();
                    },
                    error: function (err) {
                        alert(err);
                    }
                })

                e.target.innerHTML = "Search"
            },
            error: function (err) {
                alert(err);
                e.target.innerHTML = "Search"
            }
        })
    }

})
$("#AddService").click(function (e) {
    if (document.getElementById("ServiceName").checkValidity()) {
        e.target.innerHTML = "Adding..."
        var servicename = $("#ServiceName").val();
        $.ajax({
            url: 'GetServiceByName?servicename=' + servicename,
            method: "Get",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                let html = "";
                html = "<tr id='" + response.Id + "' ><td><button class='btn btn-danger' onclick='Delete(this)'>Remove</button></td><td>" + response.Description + "</td><td><input type='number' value='1' class='form-control quantity-"+response.Id+"' onchange='UpdateAmount(this)' onkeyup='UpdateAmount(this)' /></td><td class='sellingprice-"+response.Id+"' data-id='"+response.SellingPrice+"'>" + response.SellingPriceString + "</td><td><strong class='gross-" + response.Id + " gross'>₦00.00</strong></td></tr>";
                $("#ServiceBody").append(html);
                $("#ServiceName").val("");
                CalculateGrossAmount(1, response.SellingPrice, response.Id);
                updateNetAmount();
                e.target.innerHTML = "Add"
            },
            error: function (err) {
                alert(err);
                e.target.innerHTML = "Add"
            }
        });
    }
    document.getElementById("ServiceName").classList.add('was-validated');
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
            let serviceArr = [];
            let data = {
                InvoiceNumber: $("#InvoiceNumber").val(),
                CustomerType: $("input[name='CustomerType']").val()
            };
            var table = $("#ServiceBody")[0].children;
            $.each(table, function (i, tr) {
                let serviceObj = {};
                serviceObj.ServiceID = tr.id;
                serviceObj.Quantity = $(".quantity-" + tr.id).val();
                serviceObj.GrossAmount = ConvertToDecimal(tr.children[3].innerText);
                serviceArr.push(serviceObj);
            });
            debugger

            // Send ajax call to server
            $.ajax({
                url: 'Billings',
                method: 'Post',
                dataType: "json",
                data: { vmodel: data, serviceList: serviceArr },
                success: function (response) {
                    if (response == "success" && $("#SearchBy").val() == "New") {
                        location.href = "Billings?Saved=true";
                    } else {
                        location.href = "Billings?Updated=true";
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
})

function CalculateGrossAmount(quantity, price, RowID) {
    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");
}
function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function Delete(e) {
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
            updateNetAmount();
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
function UpdateAmount(e) {
    let RowID = e.parentElement.parentElement.id;

    let quantity = e.value;
    let price = $(".sellingprice-" + RowID)[0].dataset.id;

    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");

    updateNetAmount();
}
function updateNetAmount(){
    let grosses = $(".gross");
    let total = 0;
    $.each(grosses, function (i, gross) {
        var amount = ConvertToDecimal(gross.innerText);
        total += amount;
    });
    $("#NetAmount").empty();
    $("#NetAmount").html("₦" + numberWithCommas(total) + ".00")
}
function ConvertToDecimal(amount) {
    return Number(amount.replace(/[^0-9.-]+/g, ""));
}