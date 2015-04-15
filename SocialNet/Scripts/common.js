$(document).ready(function () {
    $('.linkForJS').hide();
    $('div.new-section-link').click(function (event) {
        window.location.href = $(this).children('.linkForJS').attr('href');
    })
})