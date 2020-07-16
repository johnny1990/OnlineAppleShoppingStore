$("#OrderId").change(function () {
    $.ajax({
        type: "Get",
        url: '/DeliverOrders/GetOrderDetails',
        data: { orderId: $("#OrderId").val() },
        dataType: "json",
        success: function (data) {
            $("#FirstName").val(data[0]);
            $("#LastName").val(data[1]);
            $("#Address").val(data[2]);
            $("#Phone").val(data[3]);
            $("#Email").val(data[4]);
            $("#OrderDate").val(data[5]);
            $("#Amount").val(data[6]);
        }
    });
})