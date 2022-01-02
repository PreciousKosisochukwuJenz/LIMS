$(function () {
    $('#MenuTreeView').jstree({
        "core": {
            "themes": {
                "variant": "large",
                "dots": true,
                "responsive": true
            }
        },
        "checkbox": {
            "keep_selected_style": true
        },
        "plugins": [ "checkbox"]
    }).on('changed.jstree', function (e, data) {
        var i, j;
        var selectedItems = [];


        for (i = 0, j = data.selected.length; i < j; i++) {
            //Add the Node to the JSON Array.
            selectedItems.push({
                _stringText: data.instance.get_node(data.selected[i]).text,
            });
        }

        //Serialize the JSON Array and save in Hidden Field.
        $('#selectedItems').val(JSON.stringify(selectedItems));
    });

});
