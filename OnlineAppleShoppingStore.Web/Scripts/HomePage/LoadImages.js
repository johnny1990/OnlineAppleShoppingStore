$(function () {
    $('img').Lazy({
        scrollDirection: 'vertical',
        effect: 'fadeIn',
        visibleOnly: false,
        onError: function (element) {
            console.log('error loading ' + element.data('src'));
        },
        onFinishedAll: function () {
            console.log('all images loaded')
        }
    });
});