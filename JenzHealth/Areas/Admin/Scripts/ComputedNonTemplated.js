
$(function () {
    $(".odd").remove();

    // Set  Customer Type
    var selectedMicroscopy = $("input[name='MicroscopyType']:checked").val();
    var radios = $(".MicroscopyTypeStatus");
    $.each(radios, function (i, radio) {
        if (selectedMicroscopy == undefined) {
            if (radio.id == "NA") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else {
                radio.checked = false;
            }
        } else {
            if (radio.id == selectedMicroscopy && selectedMicroscopy == "WET_MOUNT") {
                radio.checked = true;
                $("#WetAmountDiv").show();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "STAIN") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").show();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "SFA") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").show();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "KOH") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").show();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "OTHER_MICROSCOPY") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").show();
            }
            else {
                radio.checked = false;
            }
        }

    })


    var selectedStain = $("input[name='StainType']:checked").val();
    var radios = $(".StainTypeStatus");
    $.each(radios, function (i, radio) {
        if (selectedStain == undefined) {
            if (radio.id == "GRAIN_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").show();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else {
                radio.checked = false;
            }
        } else {
            if (radio.id == selectedStain && selectedStain == "GRAIN_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").show();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "GIEMSA_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").show();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "ZIEHL_NEELSON_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").show();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "INDIAN_INK_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").show();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "IODINE_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").show();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "METHYLENE_BLUE_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").show();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "OTHER_STAINS") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").show();
            }
            else {
                radio.checked = false;
            }
        }

    })

    UpdateOrganismTbl();

})

function UpdateOrganismTbl() {
    var id = $("#Idd").html();
    $.ajax({
        url: "/Admin/Laboratory/GetComputedAntibioticsAndOrgansm/" + id,
        method: "GET",
        success: function (response) {
            let html = "";
            $.each(response, function (i, data) {
                html = `<tr>
                            <td data-orgranismid='${data.OrganismID}'>${data.Organism}</td>
                            <td data-antibioticsid='${data.AntiBioticID}'>
                             ${data.AntiBiotic}
                            </td>
                            <td>
                                <ul class="list-group list-group-small list-group-flush">
                                    <li class="list-group-item d-flex px-3">
                                     ${data.IsSensitive ? "<i class='fa fa-check fa-2x text-success'></i>" : "<i class='fa fa-close fa-2x text-danger'></i>"}
                                    </li>
                                    <li class="list-group-item d-flex px-3">
                                     <p class='text-center'>${data.SensitiveDegree}</p>
                                    </li>
                                </ul>
                            </td>
                            <td>
                                ${data.IsIntermediate ? "<i class='fa fa-check fa-2x text-success'></i>" : "<i class='fa fa-close fa-2x text-danger'></i>"}
                            </td>
                            <td>
                                <ul class="list-group list-group-small list-group-flush">
                                    <li class="list-group-item d-flex px-3">
                                     ${data.IsResistance ? "<i class='fa fa-check fa-2x text-success'></i>" : "<i class='fa fa-close fa-2x text-danger'></i>"}
                                    </li>
                                    <li class="list-group-item d-flex px-3">
                                     <p class='text-center'>${data.ResistanceDegree}</p>
                                    </li>
                                </ul>
                            </td>
                       </tr>`;
                $("#OrganismBody").append(html);
            })

            $("#organismField").val("");
            $("#OrganismTableLoader").hide();
        },
        error: function (e) {
            $("#OrganismTableLoader").hide();
            toastr.error("Error", "An error occured!", { showDuration: 500 })
        }
    })
}
function CheckValidity() {
    let states = [];
    var inputs = $("input");
    var selects = $("select");
    $.each(inputs, function (i, input) {
        if (input.value === "" && input.required) {
            input.classList.add("is-invalid");
            states.push(false);
        }
        else {
            input.classList.remove("is-invalid");
            states.push(true);
        }
    });


    return states.every(hasAllPassed);
}
function hasAllPassed(status) {
    return status ? true : false;
}



document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})

$("#step").steps({
    headerTag: "h3",
    bodyTag: "section",
    transitionEffect: "slideLeft",
    autoFocus: true,
    stepsOrientation: "vertical",
    labels: {finish: "Approve"},
    onFinished: function () {
        var id = $("#ResultApprovalID").html();
        Approve(id);
    }
});

$("input[name='MicroscopyType']").change(function () {
    if ($("input[name='MicroscopyType']:checked").val() == "NA") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "WET_MOUNT") {
        $("#WetAmountDiv").show();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "STAIN") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").show();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "SFA") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").show();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "KOH") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").show();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "OTHER_MICROSCOPY") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").show();
    }
    else {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }

});

$("input[name='StainType']").change(function () {
    if ($("input[name='StainType']:checked").val() == "GRAIN_STAIN") {
        $("#GrainStainDiv").show();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "GIEMSA_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").show();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "ZIEHL_NEELSON_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").show();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "INDIAN_INK_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").show();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "IODINE_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").show();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "METHYLENE_BLUE_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").show();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "OTHER_STAINS") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").show();
    }
    else {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
});
