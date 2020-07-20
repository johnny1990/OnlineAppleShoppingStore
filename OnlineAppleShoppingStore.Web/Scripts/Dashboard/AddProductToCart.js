$(function () {
    $(".AddToCart").click(function () {
        var recordToAdd = $(this).attr("data-id");
        if (recordToAdd != '') {
            $.post("/Cart/AddToCart", { "id": recordToAdd },
                function (data) {
                    $('#cart-status').text(data.CartCount);
                });
        }
    });
});