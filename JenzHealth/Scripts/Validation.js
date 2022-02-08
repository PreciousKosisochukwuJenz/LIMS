// Example starter JavaScript for disabling form submissions if there are invalid fields
(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('validate');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } 
                form.classList.add('was-validated');
                let buttons = form.getElementsByTagName("button");
                $.each(buttons, function (i, btn) {
                    if (btn.classList.contains("ladda-button")) {
                        if (btn.innerText == "Update changes" || btn.innerText == "Update") {
                            btn.innerText = "Updating...";
                        }
                        else if (btn.innerText == "Create") {
                            btn.innerText = "Creating...";
                        } else if (btn.innerText == "Save Changes") {
                            btn.innerText = "Saving...";
                        }
                        else {
                            btn.innerText = "Processing...";
                        }
                    }
                })
            }, false);
        });
    }, false);
})();
