/// <reference path="../Jquery/jquery-1.4.4-vsdoc.js" />
$(document).ready(function () {
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {

    $('#btnAnnuler').die();
    $('#btnAnnuler').live('click', function (evt) {
        Redirection("MatriceRisque", "Index");
    })

    $("#btnSuivant").kclick(function () {
        var option = $("#drlOptions").val();
        switch (option) {
            case "Option": /*Redirection("Option", "Index");*/ break;
            case "Option2":/* Redirection("Option2", "Index");*/ break;
            case "Option3": Redirection("Option3", "Index"); break;   
        }      
    })


}

//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var versionOffre = $("#Offre_Version").val();
    var typeOffre = $("#Offre_Type").val();
    $.ajax({
        type: "POST",
        url: "/GestionOptions/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: versionOffre, type: typeOffre },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}