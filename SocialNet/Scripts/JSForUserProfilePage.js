$(document).ready(function () {
    $('div.AllComments').hide();
    $('div.block-link').click(function () {
        $(this).next().toggle();
        $(this).children('p').toggle();
    });

    $('.commentForm').submit(function (event) {
        event.preventDefault();
        var data = $(this).serialize();
        var url = $(this).attr('action');
        var newComment = this;
        $(newComment).parent().prev().show();
        $('.comment').val('');
        $.post(url, data, function (response) {
            $(newComment).parent().prev().append(response);
        });
    });

    $('.RatingLink').click(function (event) {
        event.preventDefault();
        var data = $(this).serialize();
        var url = $(this).attr('href');
        var newComment = this;
        $.get(url, data, function (response) {
            $(newComment).parent().replaceWith(response);
        });
    });

    $('.deleteLink').click(function (event) {
        event.preventDefault();
        var data = $(this).serialize();
        var url = $(this).attr('href');
        var newComment = this;
        $.get(url, data, function (response) {
            $(newComment).parent().parent().parent().parent().replaceWith(response);
        });
    });
});

function hideComments() {
    $('div.AllComments').hide();
}