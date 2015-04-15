$(document).ready(function () {
    $('.linkForJS').hide();
    $('div.dialog').click(function (event) {
        window.location.href = $(this).children('.linkForJS').attr('href');
    })
})
