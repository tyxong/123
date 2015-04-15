$(function () {
    $('.image').on('click', function () {
        var image = $('#image');
        var imageRel = $(this).attr('rel');

        image.hide().fadeIn('slow');
        image.html('<img src="' + imageRel + '">');

        return false;
    });

});