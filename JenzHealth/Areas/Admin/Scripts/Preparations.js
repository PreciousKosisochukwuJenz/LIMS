$("#analytics-overview-date-range").datepicker({});
"use strict"; !function (t) { t(".transaction-history").DataTable({ responsive: !0 }) }(jQuery);


function DisApprove(Id) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to disapprove this test result?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: 'UnapproveResult/' + Id,
                method: 'Get',
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function (response) {
                    if (response) {
                        Swal.fire({
                            title: 'Test disapproved successfully',
                            showCancelButton: false,
                            confirmButtonText: 'Ok',
                            showLoaderOnConfirm: true,
                        }).then((result) => {
                            if (result.value) {
                                location.href = "/Admin/Laboratory/ResultApproval";
                            } else if (
                                result.dismiss === Swal.DismissReason.cancel
                            ) {
                                location.href = "/Admin/Laboratory/ResultApproval";
                            }
                        })
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
