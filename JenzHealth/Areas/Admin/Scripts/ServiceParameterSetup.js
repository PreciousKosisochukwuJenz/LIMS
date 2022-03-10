$(function () {
    $(".odd").remove();
})


function DeleteParameter(e) {
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
            toastr.success("Removed", "Parameter removed", { showDuration: 500 })
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


$("#AddParameter").click(function (e) {
    var parameter = $("#ParameterField").val();
    var rank = $("#RankField").val();

    if (parameter === "") {
        $("#ParameterField").addClass("is-invalid");
    }
    else if (rank === "") {
        $("#RankField").addClass("is-invalid");
    }
    else {
        $("#ParameterField").removeClass("is-invalid");
        $("#RankField").removeClass("is-invalid");

        e.target.innerHTML = "Adding..."
        let html = "";
        html = "<tr><td><button class='btn btn-danger' onclick='DeleteParameter(this)'>Remove</button></td><td class='Parameter-" + rank + "'>" + parameter + "</td><td class='rank rank-" + rank + "'>" + rank + "</td></tr>";
        $("#ParameterBody").append(html);
        $("#ParameterField").val("");
        $("#RankField").val("");

        e.target.innerHTML = "Add parameter"
    }
});


$(function () {
    $("#Service").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Seed/GetServiceAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1
    });

})

$(function () {
    $("#Specimen").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Seed/GetSpecimenAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1
    });

})
$(function () {
    $("#Template").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Seed/GetTemplateAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1
    });

})


$("#FinishBtn").click(function () {
    var service = $("#Service").val();
    var specimen = $("#Specimen").val();

    if (service === "") {
        $("#Service").addClass("is-invalid");
    } else if (specimen === "") {
        $("#Specimen").addClass("is-invalid");
    } else {
        $("#Service").removeClass("is-invalid");
        $("#Specimen").removeClass("is-invalid");


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

                var serviceParameter = {
                    Service: $("#Service").val(),
                    Specimen: $("#Specimen").val(),
                    RequireApproval: $("#newProjectsEmailsToggle").val()
                };

                let ParameterSetupList = [];
                var table = $("#ParameterBody")[0].children;
                $.each(table, function (i, tr) {
                    i += 1;
                    // Create Parameter
                    let setup = {};
                    setup.Name = tr.children[1].innerText;
                    setup.Rank = tr.children[2].innerText;

                    // Add to Parameter list
                    ParameterSetupList.push(setup);
                });
                // Send ajax call to server
                $.ajax({
                    url: 'UpdateServiceParameters',
                    method: 'Post',
                    dataType: "json",
                    data: { serviceParameter: serviceParameter, serviceParameterSetups: ParameterSetupList },
                    success: function (response) {
                        Swal.fire({
                            title: 'Parameters set successfully',
                            showCancelButton: false,
                            confirmButtonText: 'Ok',
                            showLoaderOnConfirm: true,
                        }).then((result) => {
                            if (result.value) {
                                window.location.reload(true);
                            } else if (
                                result.dismiss === Swal.DismissReason.cancel
                            ) {
                                window.location.reload(true);
                            }
                        })
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

   
})

$("#Service").on("keyup", function () {
    $("#ParameterTableLoader").show();
    $("#ParamterTableDiv").hide();

    var service = $("#Service").val();
    $.ajax({
        url: 'GetServiceParameter?service=' + service,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#Specimen").val(data.Specimen);
            $("#Template").val(data.Template);
            $("#newProjectsEmailsToggle").prop("checked", data.RequireApproval);

            $.ajax({
                url: 'GetServiceParameterSetups?service=' + service,
                method: "Get",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (datas) {
                    $("#ParameterBody").empty()
                    $.each(datas, function (i, data) {
                        let html = "";
                        html = "<tr><td><button class='btn btn-danger' onclick='DeleteParameter(this)'>Remove</button></td><td class='Parameter-" + data.Rank + "'>" + data.Name + "</td><td class='rank rank-" + data.Rank + "'>" + data.Rank + "</td></tr>";
                        $("#ParameterBody").append(html);
                    });

                    $("#ParameterTableLoader").hide();
                    $("#ParamterTableDiv").show();
                },
                error: function (err) {
                    $("#ParameterTableLoader").hide();
                    $("#ParamterTableDiv").show();
                    $("#ParameterBody").empty()
                    $("#Specimen").val("");
                    $("#newProjectsEmailsToggle").prop("checked", false);
                }
            })

        },
        error: function (err) {
            $("#ParameterTableLoader").hide();
            $("#ParamterTableDiv").show();
            $("#ParameterBody").empty()
            $("#Specimen").val("");
            $("#newProjectsEmailsToggle").prop("checked", false);
        }
    })


})