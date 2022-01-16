$(document).ready(function () {
    SetTitleToButtonPage();
});
//------------------------Text d'aide au touches de raccourci--------
function SetTitleToButtonPage() {
    $("button").each(function () {
        var id = $(this).attr("id");
        SetTitleToButton(id);

    });
}
//----------------Text aide bouton-------------    
function SetTitleToButton(id) {
    if ($("#" + id).attr("data-accesskey") != undefined) {
        var letter = $("#" + id).attr("data-accesskey").toUpperCase();
        if (letter != null) {
            var value = $("#spnShortCutJs").html();
            $("#" + id).attr("title", value.replace("X",letter));
        }
    }
}