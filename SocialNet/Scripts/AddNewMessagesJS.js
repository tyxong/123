function AddNewMessages() {
    var lastMessageId = $('#AllMessages .message:last-child').attr('id');
    var url = $('#actionLink').attr('href') + '?lastMessageId=' + lastMessageId;
    if (lastMessageId == null)
    {
        url = $('#actionLink').attr('href') + '?lastMessageId=-1';
    }
    $.get(url, function (response) {
        $('#AllMessages').append(response);
    });
}

function ScrollToBottom() {
    var height = $("body").height();
    $("html,body").animate({ "scrollTop": height }, 1);
}

$(document).ready(function () {
    $('#actionLink').hide();
    ScrollToBottom();
    setInterval(AddNewMessages, 1000)
});

