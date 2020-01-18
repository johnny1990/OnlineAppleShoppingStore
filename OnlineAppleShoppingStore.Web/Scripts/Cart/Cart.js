$(document).ready(function () {
    $(".RemoveLink").click(function (e) {
        e.preventDefault();

        var recordToDelete = $(this).attr("data-id");
        if (recordToDelete != '') {

            $.post("/Cart/RemoveFromCart", { "id": recordToDelete },
                function (data) {

                    if (data.ItemCount == 0) {
                        $('#row-' + data.DeleteId).fadeOut('slow');
                    } else {
                        $('#item-count-' + data.DeleteId).text(data.ItemCount);
                    }
                    $('#cart-total').text(data.CartTotal);
                    $('#update-message').text(data.Message);
                    $('#cart-status').text('Cart (' + data.CartCount + ')');
                });
        }
    });
})