$(document).ready(function () {
    $("#btnDelete").click(function () {
        var CategoryId = $("#hidId");

        $.ajax({
            type: "POST",
            url: "/Categories/DeleteCategory",
            data: '{id: ' + CategoryId.val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Categories/Index'
            }
        });
    });
})