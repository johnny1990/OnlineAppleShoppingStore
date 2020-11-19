$(document).ready(function () {
    $("#btnCreate").click(function () {
        var txtName = $("#txtName");

        $.ajax({
            type: "POST",
            url: "/Categories/InsertCategory",
            data: '{Name: "' + txtName.val() + '"  }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                window.location = 'https://localhost:44328/Categories/Index'
            }
        })
    });
})