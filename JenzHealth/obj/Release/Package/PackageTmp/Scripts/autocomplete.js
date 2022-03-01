$(function () {
    $("#ServiceName").autoComplete({
        resolver: "custom",
        events: {
            search: function(qry, callback) {
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
        minLength:1
    });

})