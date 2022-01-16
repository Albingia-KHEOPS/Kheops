
$(document).ready(function () {
    
    RedirectPage();
});
//------------- Annule la form ------------------------
function RedirectPage() {
    $.ajax({
        type: "POST",
        url: "/Home/RedirectToHomeDev",
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}