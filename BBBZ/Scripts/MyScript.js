    $('#datetimepicker1').datepicker({    
        onSelect: function() { 
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




function showThis(x)
{
    var divs = ['div0', 'div1', 'div2', 'div3'];
    for (var i = 0; i < divs.length; i++) {
        document.getElementById(divs[i]).style.display = "none";
    }
    document.getElementById(divs[x]).style.display = "block";
}

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


