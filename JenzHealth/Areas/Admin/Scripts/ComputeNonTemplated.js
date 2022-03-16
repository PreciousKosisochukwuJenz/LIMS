$(function () {
    $(".odd").remove();
})
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

$("#AddOrganism").click(function (e) {
    var organism = $("#organismField").val();

    if (organism === "") {
        $("#organismField").addClass("is-invalid");
    }
    else {
        $("#organismField").removeClass("is-invalid");
        e.target.innerHTML = "Adding..."

        $.ajax({
            url: "/Seed/GetAntiBioticByOrganismName?organismName="+organism,
            method: "GET",
            success: function (response) {
                let html = "";
                html = "<tr><td><button class='btn btn-danger' type='button' onclick='DeleteOrganism(this)'>Remove</button></td><td data-orgranismid='" + response.OrganismID + "'>" + organism + "</td><td data-antibioticsid='" + response.Id +"'>" + response.Name + "</td></tr>";
                $("#OrganismBody").append(html);
                $("#organismField").val("");

                e.target.innerHTML = "Add"
            },
            error: function (e) {
                toastr.error("Error", "An error occured!", { showDuration: 500 })
            }
        })

    }
});

$(function () {
    $("#organismField").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Seed/GetOrganismAutoComplete",
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


function DeleteOrganism(e) {
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
            installmentCount--;
            toastr.success("Removed", "Organism removed", { showDuration: 500 })
            updateInstallmentNetAmount();
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


$("#FinishBtn").click(function () {


    if (CheckValidity()) {
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

                var data = {
                    Appearance: $("#Appearance").val(),
                    Color: $("#Color").val(),
                    MacrosopyBlood: $("#MacrosopyBlood").val(),
                    AdultWarm: $("#AdultWarm").val(),
                    Mucus: $("#Mucus").val(),
                    SpecificGravity: $("#SpecificGravity").val(),
                    Blirubin: $("#Blirubin").val(),
                    Acidity: $("#Acidity").val(),
                    Urobilinogen: $("#Urobilinogen").val(),
                    Glucose: $("#Glucose").val(),
                    AscorbicAcid: $("#AscorbicAcid").val(),
                    Protein: $("#Protein").val(),
                    DipstickBlood: $("#DipstickBlood").val(),
                    Niterite: $("#Niterite").val(),
                    LeucocyteEsterase: $("#LeucocyteEsterase").val(),
                    Ketones: $("#Ketones").val(),
                    Temperature: $("#Temperature").val(),
                    Duration: $("#Duration").val(),
                    Atomsphere: $("#Atomsphere").val(),
                    Plate: $("#Plate").val(),
                    Incubatio: $("#Incubatio").val(),
                    BillInvoiceNumber: $("#billnumber").val(),
                    ServiceParameterID: $("#ServiceParameterID").val(),
                }
                var specimencollectedID = $("#SpecimenCollectedID").val();
                let OrganismList = [];
                var table = $("#OrganismBody")[0].children;
                $.each(table, function (i, tr) {
                    // Create installment
                    let organism = {};
                    organism.OrganismID = tr.children[1].dataset.orgranismid;
                    organism.AntiBioticID = tr.children[2].dataset.antibioticsid;

                    // Add to Installment list
                    OrganismList.push(organism);
                });
                // Send ajax call to server
                $.ajax({
                    url: 'UpdateNonTemplatedLabResults',
                    method: 'Post',
                    dataType: "json",
                    data: { vmodel: data, organisms : OrganismList },
                    success: function (response) {
                        location.href = "Prepare?ID=" + specimencollectedID + "&Saved=" + response;
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

document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})