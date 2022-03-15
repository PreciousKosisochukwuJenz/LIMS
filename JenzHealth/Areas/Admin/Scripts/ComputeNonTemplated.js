function validate(input) {
    if (input.classList.contains("is-invalid")) {
        return true;
    } else {
        return false;
    }
}

function HasAllPass(status) {
    return status ? true : false;
}

$("#FinishBtn").click(function () {
    let validityStatus = [];
    var inputs = $("input");
    $.each(inputs, function (i, input) {
        validityStatus.push(validate(input));
    })

    hasAllPassed = validityStatus.every(HasAllPass);


    if (hasAllPassed) {
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
    }
   
})
