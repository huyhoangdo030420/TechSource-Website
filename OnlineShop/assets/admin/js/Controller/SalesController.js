$(function () {
    $('.datepicker').inputmask("mm/dd/yyyy", { placeholder: '__/__/____' });
    $(".datepicker").datepicker({
        Format: "dd/mm/yy",
        
    autoclose: true,
    keyboardNavigation: false,
    maxViewMode: 2,
    todayBtn: "linked",
    clearBtn: true,
    todayHighlight: true,
        language: "vi",
        timezone: "UTC+07:00" 
    });
});

