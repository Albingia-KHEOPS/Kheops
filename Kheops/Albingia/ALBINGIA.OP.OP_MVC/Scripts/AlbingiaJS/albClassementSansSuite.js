$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page---------
function MapElementPage() {

    toggleDescription();
    

    $("#btnAnnuler").die().live('click', function () {
        Cancel();
    });
    $("#btnSuivant").click(function () {
        ClasserSansSuite();
    });
}

//----------Annule et retourne sur l'écran de recherche----------
function Cancel() {
    var tabGuid = $("#tabGuid").val();
    DeverouillerUserOffres(tabGuid);
    Redirection("RechercheSaisie", "Index");
}

//----------Classe le contrat sans suite--------------
function ClasserSansSuite() {
    ShowLoading();
    var codeAffaire = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var listeAnnulQuitt = "";
    $("table#tblVisuQuittancesBody tr:not(.NotAffecte)").each(function () {
        listeAnnulQuitt += $(this).find("td.tdQuittNum").attr("id").split("_")[1] + "|";
    });

    $.ajax({
        type: "POST",
        url: "/ClassementSansSuite/ClasserSansSuite/",
        data: {
            codeAffaire: codeAffaire, version: version, type: type, listeAnnulQuitt: listeAnnulQuitt
        },
        success: function (data) {
            Redirection("RechercheSaisie", "Index");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/ClassementSansSuite/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid,
            addParamType: addParamType, addParamValue: addParamValue, modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
