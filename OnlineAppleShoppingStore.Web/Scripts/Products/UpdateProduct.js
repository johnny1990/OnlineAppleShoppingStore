$(document).ready(function () {
    $("#btnUpdate").click(function () {

        var product = {};
        product.Id = $("#hidId").val();
        product.Name = $("#txtName").val();
        product.Price = $("#txtPrice").val();
        product.Description = $("#txtDescription").val();
        product.LastUpdated = $("#txtLastUpdated").val();
        product.CategoryId = $("#ddCategoryId").val();

        $.ajax({
            type: "POST",
            url: "/Products/UpdateProduct",
            data: '{product:' + JSON.stringify(product) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Products/Index'            }
        });

    });
})