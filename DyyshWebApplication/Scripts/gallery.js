var result = [];
$.cookie.raw = true;

function enableSelectable() {
    console.log("selectable enabled");
    $("#image-container").selectable("enable");
    $("#image-container").selectable("option", "cancel", "input,textarea,button,select,option");
    $("#selectBtn").prop('value', 'Cancel');
}

function disableSelectable() {
    console.log("selectable disabled");
    $("#image-container").selectable("disable");
    $('#image-container .ui-selected').removeClass('ui-selected')
    $("#image-container").selectable("option", "cancel", 'a');
    $("#selectBtn").prop('value', 'Select');
}

$(document).ready(function () {
    $("#image-container").selectable({ disabled: true, cancel: 'a' });

    $("#selectBtn").click(function () {
        var selectableDisabled = $("#image-container").selectable("option", "disabled");
        if (selectableDisabled == true)
        { enableSelectable() }
        else
        { disableSelectable() }
    });

    $("#deleteBtn").click(function () {
        if (result.length > 0) {
            $.ajax({
                url: '/Gallery/DeleteImages',
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ images: result }),
                success: function (data) {
                    alert(data);
                    setTimeout(function () { location.reload(true); }, 1000);
                }
                //error: function (data) {
                //    alert("There was an error in your request");
                //}
            });
        }
        else {
            alert("You have not selected any items")
        }
    });

    $("#showtipsBtn").click(function () {
        $.cookie('TipsWasShown', 'false');
        location.reload();
    });
});

$(function () {
    $("#image-container").selectable({
        stop: function () {

            result = [];
            $(".ui-selected", this).each(function () {
                var index = $("#image-container .fl").index(this);

                var num = $(this).attr("data-value");
                var size = $(this).attr("data-filesize");
                if (num) {
                    result.push(num + '/' + size);
                }

            });
            console.log("actual result: ", result);
        }
    });
});

function cleanArray(actual) {
    var newArray = new Array();
    for (var i = 0; i < actual.length; i++) {
        if (actual[i]) {
            newArray.push(actual[i]);
        }
    }
    return newArray;
}


