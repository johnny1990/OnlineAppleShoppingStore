$(document).ready(function () {
    $("#btnCreate").click(function () {
        var txtName = $("#txtName");
        var txtPhone = $("#txtPhone");

        $.ajax({
            type: "POST",
            url: "/Shippers/InsertShipper",
            data: '{Name: "' + txtName.val() + '", Phone: "' + txtPhone.val() + '"  }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                window.location = 'https://localhost:44328/Shippers/Index'
            }
        })
    });
})