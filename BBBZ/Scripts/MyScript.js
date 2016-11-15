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

tinymce.init({
    selector: "#editor",
    height: 200,
    plugins: [
    'advlist autolink lists link image charmap print preview hr anchor pagebreak',
    'searchreplace wordcount visualblocks visualchars code',
    'insertdatetime media nonbreaking save table contextmenu directionality',
    'emoticons paste textcolor colorpicker textpattern imagetools',
    'autoresize'
    ],

    toolbar1: 'styleselect | fontselect | fontsizeselect | hr charmap emoticons | link image',
    toolbar2: 'forecolor backcolor | bold italic underline strikethrough | ltr rtl | alignleft aligncenter alignright outdent indent | bullist numlist',
    menu: {},

    setup: function (ed) {

        ed.on('keyup', function () {
            ToggelPostButton(ed.getContent());
        });

        ed.on('change', function () {
            ToggelPostButton(ed.getContent());
        });
    }
});

function ToggelPostButton(txt) {
    if ($(txt).text().trim().length == 0)
        $("#postSunmit").attr('disabled', 'disabled');
    else
        $("#postSunmit").removeAttr('disabled');
}




$(document).ready(function () {
    var video = document.querySelector('video');
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
    window.URL = window.URL || window.webkitURL || window.mozURL || window.msURL;
    if (navigator.getUserMedia) {
        navigator.getUserMedia({ video: true }, successCallback, errorCallback);
    } else {
        console.log('Native device media streaming (getUserMedia) not supported in this browser.');
        // Display a friendly "sorry" message to the user.
    }
});