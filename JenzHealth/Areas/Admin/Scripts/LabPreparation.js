$(".print").click(function (e) {
    e.preventDefault();
    Swal.fire({
        title: 'Enter Collector\'s Name',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Proceed',
        showLoaderOnConfirm: true,
        preConfirm: (input) => {
            if (input === '') {
                Swal.showValidationMessage(
                    `Collector's Name Required`
                )
            }
            const billnumber = e.target.dataset.billnumber;
            const templateID = e.target.dataset.templateid;
            PostCollector(input, billnumber, templateID);
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result.value) {
            let reportUrl = e.target.href;
            window.open(reportUrl, 'blank');
        }
    })
})

function PostCollector(collector, billnumber, templateID) {
    const data = {
        Collector: collector,
        BillNumber: billnumber,
        TemplateID: templateID
    };
    $.ajax({
        url: '/Admin/Laboratory/UpdateCollector',
        method: 'Post',
        dataType: "json",
        data: { model: data  },
        success: function (response) {
            return response.isConfirmed = true;
        }
    })
}