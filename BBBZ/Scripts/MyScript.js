    


function ToggelPostButton(txt) {
    if ($(txt).text().trim().length == 0)
        $("#postSunmit").attr('disabled', 'disabled');
    else
        $("#postSunmit").removeAttr('disabled');
}




//$(document).ready(function () {
//    var video = document.querySelector('video');
//    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
//    window.URL = window.URL || window.webkitURL || window.mozURL || window.msURL;
//    if (navigator.getUserMedia) {
//        navigator.getUserMedia({ video: true }, successCallback, errorCallback);
//    } else {
//        console.log('Native device media streaming (getUserMedia) not supported in this browser.');
//        // Display a friendly "sorry" message to the user.
//    }
//});


$("#addMenuBtn").click(function () {
    var txt = $('#txtNewMenuName').val();

    $.ajax({
        url: "/Admin/AddMenu",
        method: 'POST',
        data: { Name : txt},
        beforeSend: function (){},
        success: function () {
            $('#menuDropList').append("<option value='5'>" + txt + "</option>");
            $('#txtNewMenuName').val('');
            Toast('done');
        },
        error: ShowError,
    });
});

function Toast(msg) {
    var x = document.getElementById("snackbar")
    x.innerText = msg;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 2500);
}

function ShowError(x) {
    try{ Toast("ERROR : " + jQuery.parseJSON(x.responseText).Message); }
    catch (e) { Toast("ERROR : " + x); }
}


$(document).ready(function () {
    $('.navbar a.dropdown-toggle').on('click', function (e) {
        var $el = $(this);
        var $parent = $(this).offsetParent(".dropdown-menu");
        $(this).parent("li").toggleClass('open');

        if (!$parent.parent().hasClass('nav')) {
            $el.next().css({ "top": $el[0].offsetTop, "left": $parent.outerWidth() - 4 });
        }

        $('.nav li.open').not($(this).parents("li")).removeClass("open");



        $('#datetimepicker1').datepicker({
            onSelect: function () {
                x = $(this).datepicker('getDate').toLocaleDateString();
                console.log(x);
            }
        });

        $('#datetimepicker2').datepicker({
            onSelect: function () {
                x = $(this).datepicker('getDate').toLocaleDateString();
                console.log(x);
            }
        });

        return false;
    });
});
