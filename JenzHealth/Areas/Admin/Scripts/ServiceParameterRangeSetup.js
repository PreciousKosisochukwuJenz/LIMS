$(function () {
    $(".odd").remove();
})


function DeleteRange(e) {
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
            toastr.success("Removed", "Range removed", { showDuration: 500 })
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


$("#AddRange").click(function (e) {
    var parameter = $("#Parameter option:selected").text();
    var parameterID = $("#Parameter").val();
    var range = $("#RangeField").val();
    var unit = $("#UnitField").val();
    if (parameter === "-- Select --") {
        $("#Parameter").addClass("is-invalid");
    }
    else if (range === "") {
        $("#RangeField").addClass("is-invalid");
    }
    else if (unit === "") {
        $("#UnitField").addClass("is-invalid");
    }
    else {
        $("#Parameter").removeClass("is-invalid");
        $("#RangeField").removeClass("is-invalid");
        $("#UnitField").removeClass("is-invalid");

        e.target.innerHTML = "Adding..."
        let html = "";
        html = "<tr><td><button class='btn btn-danger' onclick='DeleteRange(this)'>Remove</button></td><td id='"+parameterID+"'>" + parameter + "</td><td class='range'>" + range + "</td><td>" + unit + "</td></tr>";
        $("#RangeBody").append(html);
        $("#RangeField").val("");
        $("#UnitField").val("");

        e.target.innerHTML = "Add range"
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
        minLength: 1,
    });

})

$("#FinishBtn").click(function () {
    var service = $("#Service").val();

    if (service === "") {
        $("#Service").addClass("is-invalid");
    } else {
        $("#Service").removeClass("is-invalid");

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

                let RangeSetupList = [];
                var table = $("#RangeBody")[0].children;
                $.each(table, function (i, tr) {
                    i += 1;
                    // Create Parameter
                    let setup = {};
                    setup.ParameterID = tr.children[1].id;
                    setup.ServiceParameterSetupID = tr.children[1].dataset.id;
                    setup.Range = tr.children[2].innerText;
                    setup.Unit = tr.children[3].innerText;

                    // Add to Parameter list
                    RangeSetupList.push(setup);
                });
                // Send ajax call to server
                $.ajax({
                    url: 'UpdateServiceParameterRanges',
                    method: 'Post',
                    dataType: "json",
                    data: { serviceParameterSetups: RangeSetupList },
                    success: function (response) {
                        Swal.fire({
                            title: 'Range parameter set successfully',
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

function Populate(service) {
    $.ajax({
        url: 'GetServiceParameterSetups?service=' + service,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (datas) {
            if (datas != null) {
                $("#Parameter").empty();
                label = "<option> -- Select -- </option>";
                $("#Parameter").append(label);
                $.each(datas, function (i, data) {
                    let html = "";
                    html = "<option value='" + data.Id + "'> " + data.Name + "</option>";
                    $("#Parameter").append(html);
                });
            }
        },
        error: function (err) {
            toastr.warning("Oops", "No parameter set for this service yet.", { showDuration: 500 })
        }
    })
    $.ajax({
        url: 'GetServiceParameterRanges?service=' + service,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (datas) {
            if (datas != null) {
                $("#RangeBody").empty()
                $.each(datas, function (i, data) {
                    let html = "";
                    html = "<tr><td><button class='btn btn-danger' onclick='DeleteRange(this)'>Remove</button></td><td data-id='"+data.Id+"' id='" + data.ServiceParameterSetupID + "'>" + data.ServiceParameterSetup + "</td><td class='range'>" + data.Range + "</td><td>" + data.Unit + "</td></tr>";
                    $("#RangeBody").append(html);
                });
            }
            $("#RangeTableLoader").hide();
            $("#RangeTableDiv").show();
        },
        error: function (err) {
            $("#RangeTableLoader").hide();
            $("#RangeTableDiv").show();
        }
    })
}

$("#Service").on("blur", function () {
    $("#RangeTableLoader").show();
    $("#RangeTableDiv").hide();

    setTimeout(function () {
        let service = $("#Service").val();
        Populate(service);
    },200);
})

document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})