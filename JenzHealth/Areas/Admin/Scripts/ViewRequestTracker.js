

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
                const services = BillServices(billedServices, data.TemplateID);

                content += `<tr>
                                <td> <a href="LabReport?billnumber=${billnumber}&templateID=${data.TemplateID}&templated=${data.Templated}" data-templateID="${data.TemplateID}" data-billnumber="${billnumber}" target="_blank" class="btn btn-white print" title="Click to print">
                                                <i class="material-icons">print</i> Print result
                                            </a></td>
                                <td>${data.Template}</td>
                                ${services}
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
    $.each(billServices, function (i, service) {
        if (service.TemplateID == templateID) {
            content += `<li class="list-group-item d-flex px-3">
                <span class="text-semibold text-fiord-blue ">${service.Service}</span>  &emsp; &emsp;&emsp;
                <p>${service.Approved ? "<i class='fa fa-check text-success'> READY</i>" : "<i class='fa fa-clock text-warning'> PROCESSING</i>"}</p>
             </li>` 
        }
    });
    let services = `<td><ul class="list-group list-group-small list-group-flush">${content}</ul></td>`;
    return services;
}


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
        data: { model: data },
        success: function (response) {
            return response.isConfirmed = true;
        }
    })
}