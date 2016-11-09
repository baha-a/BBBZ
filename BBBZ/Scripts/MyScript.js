$(function () {
    $('#datetimepicker1').datepicker({
                
        onSelect: function() { 
            x = $(this).datepicker('getDate').toLocaleDateString();
            console.log(x);
        }});
            
    $('#datetimepicker2').datepicker(); 
});
