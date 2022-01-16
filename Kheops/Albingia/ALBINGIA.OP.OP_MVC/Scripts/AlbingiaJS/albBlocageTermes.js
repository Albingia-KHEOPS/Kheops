/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
});

//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $('#btnAnnuler').kclick(function (evt) {
        Annuler();
    });
    
    $("#btnSuivant").kclick(function (evt, data) {
        Valider(data && data.returnHome);
    });
}

//----------Map les élément de la div flottante déblocage des termes
function MapElementDeblocage() {
    $("#btnValiderDeblocage").kclick(function () {
        ValiderDeblocage();
    });
}

//---------Retourne à l'écran recherche-------
function Annuler() {
    Redirection("RechercheSaisie", "Index", common.tabGuid);
}
//---------Valider---------
function Valider(returnHome) {
    ShowLoading();
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var selectedZoneStop = $("#ZoneStop").val();
    var codeZoneStop = $("#zoneStopCode").val();
    var niveauDroit = $("#niveauDroit").val();
    var acteGestion = $("#ActeGestion").val();
    var modeNavig = $("#ModeNavig").val()
    var codeAvn = $("#NumAvenantPage").val();

    $.ajax({
        type: "POST",
        url: "/BlocageTermes/Valider",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, type: type, tabGuid: tabGuid, codeZoneStop: codeZoneStop, selectedZoneStop: selectedZoneStop, niveauDroit: niveauDroit, acteGestion: acteGestion, modeNavig: modeNavig, codeAvn: codeAvn },
        success: function (data) {            
            if (!(data.indexOf("window.location")>= 0))
            {
                $("#divDataDeblocage").html(data);
                MapElementDeblocage();
                $("#divFullScreen").show();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//---------Valider déblocage des termes--------
function ValiderDeblocage() {
    var isChecked = $("#chkIsEmissionImmediate").is(":checked");
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var acteGestion = $("#ActeGestion").val();
    var modeNavig = $("#ModeNavig").val()
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();
    var niveauDroit = $("#niveauDroit").val();
    $("#divFullScreen").hide();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/BlocageTermes/DeblocageDesTermes/",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, type: type, emission: isChecked, mode: "Loop", acteGestion: acteGestion,niveauDroit:niveauDroit, modeNavig: modeNavig, tabGuid: tabGuid, codeAvn: codeAvn },
        success: function (data) {
            if (!(data.indexOf("window.location") >= 0)) {
                $("#divDataDeblocage").html(data);
                MapElementDeblocage();
                $("#divFullScreen").show();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------Redirection------------------
function Redirection(cible, job, param) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/BlocageTermes/Redirection/",
        data: { cible: cible, job: job, param: param },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}