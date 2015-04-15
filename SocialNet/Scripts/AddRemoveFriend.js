$(document).ready(function () {
    $('.AddRemoveFriendFormJS').submit(function (event) {
        event.preventDefault();
        var data = $(this).serialize();
        var url = $(this).attr('action');
        $.get(url, data);
        $(this).parent().children('form').toggle();
    });
    $( "#searchUsers" ).keyup(function() {
        $('div.min-user').each(function () {
            if ($(this).attr('id').indexOf($('#searchUsers').val()) + 1){
                $(this).show();
            } else{
                $(this).hide();
                $(this).next().hide();
            }
        })
    });
})