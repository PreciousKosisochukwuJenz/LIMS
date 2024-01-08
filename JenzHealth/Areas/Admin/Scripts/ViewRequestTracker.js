
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

