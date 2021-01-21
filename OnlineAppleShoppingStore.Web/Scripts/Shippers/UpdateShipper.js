$(document).ready(function () {
    $("#btnUpdate").click(function () {

        var shipper = {};
        shipper.Id = $("#hidId").val();
        shipper.Name = $("#txtName").val();
        shipper.Phone = $("#txtPhone").val();

        $.ajax({
            type: "POST",
            url: "/Shippers/UpdateShipper",
            data: '{s:' + JSON.stringify(shipper) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Shippers/Index'
            }
        });

    });
})