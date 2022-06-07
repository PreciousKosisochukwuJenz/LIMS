
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