$("#FinishBtn").click(function () {
    //if (CheckValidity()) {

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
            var labnote = $("#labnote").val();
            var comment = $("#comment").val();
            var billnumber = $("#billnumber").val();
            var specimencollectedID = $("#specimencollectedID").val();

            let results = [];
            var tbodys = $("#resultBody").find("tbody");
            $.each(tbodys, function (i, tbody) {
                $.each(tbody.children, function (i, tr) {
                    let result = {};
                    result.ServiceID = tr.children[0].dataset.parent;
                    result.Service = tr.children[0].dataset.service;
                    result.BillInvoiceNumber = billnumber;
                    result.KeyID = tr.children[0].dataset.child;
                    result.Key = tr.children[0].innerText;
                    result.Value = tr.children[1].getElementsByClassName("richText-editor")[0].innerHTML;
                    results.push(result);
                });
            });
            //Send ajax call to server
            $.ajax({
                url: 'UpdateDocLabResults',
                method: 'Post',
                dataType: "json",
                data:{ results: results, labnote: labnote, comment: comment },
                success: function (response) {
                    Swal.fire({
                        title: 'Test computed successfully',
                        showCancelButton: false,
                        confirmButtonText: 'Ok',
                        showLoaderOnConfirm: true,
                    }).then((result) => {
                        if (result.value) {
                            location.href = "Prepare?ID=" + specimencollectedID + "&Saved=" + response;
                        } else if (
                            result.dismiss === Swal.DismissReason.cancel
                        ) {
                            location.href = "Prepare?ID=" + specimencollectedID + "&Saved=" + response;
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
    //    }
})

function setEditor(id) {
    $("#parameter-" + id).richText({

        // text formatting
        bold: true,
        italic: true,
        underline: true,

        // text alignment
        leftAlign: true,
        centerAlign: true,
        rightAlign: true,
        justify: true,

        // lists
        ol: true,
        ul: true,

        // title
        heading: true,

        // fonts
        fonts: false,
        fontList: [
            "Arial",
            "Arial Black",
            "Comic Sans MS",
            "Courier New",
            "Geneva",
            "Georgia",
            "Helvetica",
            "Impact",
            "Lucida Console",
            "Tahoma",
            "Times New Roman",
            "Verdana"
        ],
        fontColor: false,
        backgroundColor: false,
        fontSize: true,

        // uploads
        imageUpload: false,
        fileUpload: false,

        // media
        videoEmbed: false,

        // link
        urls: false,

        // tables
        table: true,

        // code
        removeStyles: false,
        code: false,

        // colors
        colors: [],

        // dropdowns
        //fileHTML: '',
        //imageHTML: '',

        // translations
        translations: {
            'title': 'Title',
            'white': 'White',
            'black': 'Black',
            'brown': 'Brown',
            'beige': 'Beige',
            'darkBlue': 'Dark Blue',
            'blue': 'Blue',
            'lightBlue': 'Light Blue',
            'darkRed': 'Dark Red',
            'red': 'Red',
            'darkGreen': 'Dark Green',
            'green': 'Green',
            'purple': 'Purple',
            'darkTurquois': 'Dark Turquois',
            'turquois': 'Turquois',
            'darkOrange': 'Dark Orange',
            'orange': 'Orange',
            'yellow': 'Yellow',
            'imageURL': 'Image URL',
            'fileURL': 'File URL',
            'linkText': 'Link text',
            'url': 'URL',
            'size': 'Size',
            'responsive': 'Responsive',
            'text': 'Text',
            'openIn': 'Open in',
            'sameTab': 'Same tab',
            'newTab': 'New tab',
            'align': 'Align',
            'left': 'Left',
            'justify': 'Justify',
            'center': 'Center',
            'right': 'Right',
            'rows': 'Rows',
            'columns': 'Columns',
            'add': 'Add',
            'pleaseEnterURL': 'Please enter an URL',
            'videoURLnotSupported': 'Video URL not supported',
            'pleaseSelectImage': 'Please select an image',
            'pleaseSelectFile': 'Please select a file',
            'bold': 'Bold',
            'italic': 'Italic',
            'underline': 'Underline',
            'alignLeft': 'Align left',
            'alignCenter': 'Align centered',
            'alignRight': 'Align right',
            'addOrderedList': 'Ordered list',
            'addUnorderedList': 'Unordered list',
            'addHeading': 'Heading/title',
            'addFont': 'Font',
            'addFontColor': 'Font color',
            'addBackgroundColor': 'Background color',
            'addFontSize': 'Font size',
            'addImage': 'Add image',
            'addVideo': 'Add video',
            'addFile': 'Add file',
            'addURL': 'Add URL',
            'addTable': 'Add table',
            'removeStyles': 'Remove styles',
            'code': 'Show HTML code',
            'undo': 'Undo',
            'redo': 'Redo',
            'close': 'Close',
            'save': 'Save'
        },

        // privacy
        youtubeCookies: false,

        // preview
        preview: false,

        // placeholder
        placeholder: '',

        // developer settings
        useSingleQuotes: false,
        height: 0,
        heightPercentage: 0,
        adaptiveHeight: false,
        id: "",
        class: "",
        useParagraph: false,
        maxlength: 0,
        maxlengthIncludeHTML: false,
        callback: undefined,
        useTabForNext: false,
        save: false,
        saveCallback: undefined,
        saveOnBlur: 0,
        undoRedo: true
    });

}
