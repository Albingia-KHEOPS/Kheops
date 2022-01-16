$(document).ready(function () {
    MapElementPage();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    $("#btnAnnuler").die().live('click', function () {
        Redirection("DocumentGestion", "Index");
    });
    $("#btnSuivant").kclick(function (evt, data) {
        Redirection(data && data.returnHome ? "RechercheSaisie" : "ValidationOffre", "Index");
    });
    $("td[albIdDest]").each(function () {
        $(this).mouseover(function () {
            ShowInfoDestinataire($(this));
        });
        $(this).mouseout(function () {
            $("#divDestinataire").html("").hide();
        });
    });

    $("#btnImprimer").die().live("click", function () {
        common.dialog.bigError("En cours de développement", true);
    });
}
//--------Affiche les informations du destinataire---------
function ShowInfoDestinataire(elem) {
    var idDest = elem.attr("albIdDest");
    var typeDest = elem.attr("albTypeDest");
    var typeEnvoi = elem.attr("albTypeEnvoi");

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/ShowInfoDest",
        data: { idDest: idDest, typeDest: typeDest, typeEnvoi: typeEnvoi },
        success: function (data) {
            $("#divDestinataire").html(data);
            var pos = elem.position();
            $("#divDestinataire").css({ "position": "absolute", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
            $("#divDestinataire").show();

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
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/SyntheseDocuments/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
