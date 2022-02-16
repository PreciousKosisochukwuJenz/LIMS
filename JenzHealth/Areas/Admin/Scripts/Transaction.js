$("#analytics-overview-date-range").datepicker({});
"use strict"; !function (t) { t(".transaction-history").DataTable({ responsive: !0 }) }(jQuery);

function CloseShift(id) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to close this shift?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, close it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/CloseShift/" + id,
                type: "Post",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function () {
                    toastr.success("Shift Closed", "Operation Successful", { showDuration: 500 })

                    window.location.reload(true);
                },
                error: function () {
                    toastr.error(message, "Operation failed", { showDuration: 500 })
                }
            })
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