$(document).ready(function () {
    $("#btnCreate").click(function () {
        var txtName = $("#txtName");
        var txtPrice = $("#txtPrice");
        var txtDescription = $("#txtDescription");
        var txtLastUpdated = $("#txtLastUpdated");
        var ddCategoryId = $("#ddCategoryId");

        $.ajax({
            type: "POST",
            url: "/Products/InsertProduct",
            data: '{Name: "' + txtName.val() + '", Price: "' + txtPrice.val() + '", Description: "'
                + txtDescription.val() + '", LastUpdated: "' + txtLastUpdated.val() + '", CategoryId: "'+ddCategoryId.val()+ '"  }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                window.location = 'https://localhost:44328/Products/Index'
            }
        })
    });
})