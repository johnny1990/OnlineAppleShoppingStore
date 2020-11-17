$(document).ready(function () {

    $(".UpdateProductQuantity").click(function () {

        var updatedProduct = $(this).attr("data-id");
        var updatedQuantity = 0;
        if ($("#" + $(this).attr("txt-id")).val().trim() !== '') {
            updatedQuantity = $("#" + $(this).attr("txt-id")).val();
        }
        if (updatedProduct != '') {
            clearUpdateMessage();

            $.post("/Cart/UpdateQuantity", { "id": updatedProduct, "cartCount": updatedQuantity },
                function (data) {

                    if (data.ItemCount == 0) {
                        $('#row-' + data.DeleteId).fadeOut('slow');
                    }
                    $('#update-message').text(htmlDecode(data.Message));


                    if (data.ItemCount != -1) {

                        $('#cart-total').text(data.CartTotal);
                        $('#cart-status').text('Cart (' + data.CartCount + ')');


                    }
                });
        }
    });
})

function clearUpdateMessage() {

    $('#update-message').text('');
}
function htmlDecode(value) {
    if (value) {
        return $('<div />').html(value).text();
    }
    else {
        return '';
    }
}
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}