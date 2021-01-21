$(document).ready(function () {
    $("#btnDelete").click(function () {
        var ShipperId = $("#hidId");

        $.ajax({
            type: "POST",
            url: "/Shippers/DeleteShipper",
            data: '{id: ' + ShipperId.val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Shippers/Index'
            }
        });
    });
})