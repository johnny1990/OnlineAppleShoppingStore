$("#OrderId").change(function () {
    $.ajax({
        type: "Get",
        url: '/Customers/GetCustomersDetails',  
        data: { orderId: $("#OrderId").val() },
        dataType: "json",
        success: function (data) {
            $("#FirstName").val(data[0]);
            $("#LastName").val(data[1]);
            $("#Address").val(data[2]);
            $("#Email").val(data[3]);
        }
    });
})