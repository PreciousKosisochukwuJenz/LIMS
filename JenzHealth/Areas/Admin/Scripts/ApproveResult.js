function Approve(Id) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to approve this test result?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.value) {
            //Send ajax call to server
            $.ajax({
                url: '/Admin/Laboratory/ApproveResult/'+Id,
                method: 'Get',
                contentType: "application/json;charset=UTF-8",
                dataType: "json",                success: function (response) {
                    if (response) {
                        Swal.fire({
                            title: 'Test approved successfully',
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
