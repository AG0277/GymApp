$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
    document.addEventListener('keydown', function (event) {
        if (event.keyCode === 13 && (document.activeElement === null || document.activeElement === document.body))
        { 
            $('#submitButton').click();
        }
    });
});