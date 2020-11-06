$(document).ready(function () {
    $("#btnDelete").click(function () {
        var ProductId = $("#hidId");

        $.ajax({
            type: "POST",
            url: "/Products/DeleteProduct",
            data: '{id: ' + ProductId.val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Products/Index'
            }
        });
    });
})