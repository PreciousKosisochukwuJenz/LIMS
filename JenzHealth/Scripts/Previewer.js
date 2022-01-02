// Logo
function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    
    reader.onload = function(e) {
      $('#Logo').attr('src', e.target.result);
    }
    
    reader.readAsDataURL(input.files[0]);
  }
}

function readImageURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#Image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function readImageEditURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#ImageEdit').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

// Favicon
function readFaviconURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#Favicon').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function readFrontURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#FrontPage').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function readBackURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#BackPage').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}



// logo
$("#LogoInp").change(function () {
  readURL(this);
});

//Signature
$("#FaviconInp").change(function () {
    readFaviconURL(this);
});

//Signature
$("#FrontInp").change(function () {
    readFrontURL(this);
});

$("#BackInp").change(function () {
    readBackURL(this);
});