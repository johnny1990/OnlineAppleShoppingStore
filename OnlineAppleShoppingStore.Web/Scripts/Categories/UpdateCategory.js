$(document).ready(function () {
    $("#btnUpdate").click(function () {

        var category = {};
        category.Id = $("#hidId").val();
        category.Name = $("#txtName").val();

        $.ajax({
            type: "POST",
            url: "/Categories/UpdateCategory",
            data: '{category:' + JSON.stringify(category) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                window.location = 'https://localhost:44328/Categories/Index'
            }
        });

    });
})