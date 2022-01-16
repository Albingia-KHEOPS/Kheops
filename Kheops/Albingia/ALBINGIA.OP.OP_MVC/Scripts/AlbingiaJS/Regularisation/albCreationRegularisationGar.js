$(document).ready(function () {

    MapElementPage();

});
function MapElementPage() {

    Annuler();

    $("#btnRegulePrecedent").live('click', function () {
        Suivant();
    });
    FormatAutoNumeric('init', 'decimal');
    FormatAutoNumeric('init', 'pourcentdecimal');
    AlternanceLigne("PeriodRegulBody", "", false, null);
    AlternanceLigne("MouvementBody", "", false, null);
    $("#dvAddPeriodRegule").hide();
    $("#btnAjouterPeriodregul").die();
    $("#btnAjouterPeriodregul").live('click', function () {
        $("#dvAddPeriodRegule").show();
    });
    $("#btnCancelAddPeriodRegule").die();
    $("#btnCancelAddPeriodRegule").live('click', function () {
        Initialiser();
        $("#dvAddPeriodRegule").hide();
    });
    $("#btnValidAddPeriodRegule").die();
    $("#btnValidAddPeriodRegule").live('click', function () {
        EnregistrerLigneMouvementPeriode();
    });
    $("td[id^=DateDeb_]").die();
    $("td[id^=DateDeb_]").live('click', function () {
        var idregulgar = $(this).attr("id").split('_')[1];
        Redirection("SaisieRegulGarantie", "Index", idregulgar, $(this));
    });
    $("#btnAppliquer").die();
    $("#btnAppliquer").click(function () {
        var position = $(this).offset();
        $("#divApplique").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 410 + 'px' }).toggle();
    });
    $("#btnFermerpopup").click(function () {
        $("#divApplique").hide();
    });
    MapListBas();
}
/***Map element grille Mouvt regularisé  **/
function MapListBas() {

    AlternanceLigne("PeriodRegulBody", "", false, null);
    Supprimer();
}
function Supprimer() {
    $("img[id='SuppPeriodRegul']").die();
    $("img[id='SuppPeriodRegul']").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr("albregulecode"));
            ShowCommonFancy("ConfirmTrans", "Suppr",
                "Vous allez supprimer la connexité de ce contrat avec les autres contrats. Voulez-vous continuer ?",
                350, 130, true, true);
        });
    });
    $("#btnConfirmTransOk").die();
    $("#btnConfirmTransOk").live('click', function () {
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                var code = $("#hiddenInputId").val();
                SupprimerPeriodRegul(code);
                CloseCommonFancy();
                break;
        }
        $("#hiddenInputId").val('');
    });
    $("#btnConfirmTransCancel").die();
    $("#btnConfirmTransCancel").live('click', function () {
        CloseCommonFancy();
    });
}


// ANNULER
function Annuler() {
    $("#btnReguleCancel").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Redirection("RechercheSaisie", "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
}

// SUIVANT
function Suivant() {
    RetourEcran();
}
//----------------Redirection------------------
function Redirection(cible, job, idregulgar, elem) {
    ShowLoading();
    var codeContrat = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var codeGar = "";
    var codeRsq = $("#codersq").val();
    var codeFor = $("#codefor").val();
    var codeAvenant = $("#NumAvenantPage").val();
    var lotID = "";
    var codeOpt = "";
    var idGar = "";
    var libGar = $("#libgar").val();
    var reguleId = "";
    if (elem != undefined && elem != null) {
        codeOpt = elem.attr("albcodeopt");
        idGar = elem.attr("albidgar");
        reguleId = elem.attr("albidregul");
        lotID = elem.attr("albidlot");
        codeGar = elem.attr("albcodgar");
    }

    $.ajax({
        type: "POST",
        url: "/CreationRegularisationGarantie/Redirection/",
        data: {
            cible: cible, job: job, codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
            codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotID: lotID, reguleId: reguleId, codeGar: codeGar, idregulgar: idregulgar, libGar: libGar,
            addParamType: addParamType, addParamValue: addParamValue, modeNavig: $("#ModeNavig").val()
        },
        success: function (data) { },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            ShowCommonFancy("Error", "", responseError.ErrorMessage, 1212, 700, true, true);
            CloseLoading();
        }
    });
}
//----------------Sauvegarde une période de mouvement--------------------
function EnregistrerLigneMouvementPeriode() {

    if (!CheckDate()) {
        return;
    }
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var dateDeb = $("#dateDebutNewPeriode").val();
    var dateFin = $("#dateFinNewPeriode").val();
    var dateDebMin = $("#DateDebMin").val();
    var dateFinMax = $("#DateFinMax").val();
    var codersq = $("#codersq").val();
    var codefor = $("#codefor").val();
    var codegar = $("#codegar").val();
    var idlot = $("#idlot").val();
    var idregul = $("#idregul").val();


    dateDeb = dateDeb.split('/')[2] + dateDeb.split('/')[1] + dateDeb.split('/')[0];
    dateFin = dateFin.split('/')[2] + dateFin.split('/')[1] + dateFin.split('/')[0];
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationRegularisationGarantie/SaveLineMouvtPeriode",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), dateDeb: dateDeb, dateFin: dateFin, dateDebMin: dateDebMin, dateFinMax: dateFinMax,
            codersq: codersq, codefor: codefor, codegar: codegar, idregul: idregul
        },
        success: function (data) {
            $("#ListePeriodeRegularise").html(data);
            MapListBas();
            Initialiser();
            $("#dvAddPeriodRegule").hide();
            CloseLoading();
        },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            ShowCommonFancy("Error", "", responseError.ErrorMessage, 300, 100, true, true, true, false);
            CloseLoading();
        }
    });
}
//----------------Suprimer une période de mouvement--------------------
function SupprimerPeriodRegul(code) {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codersq = $("#codersq").val();
    var codefor = $("#codefor").val();
    var codegar = $("#codegar").val();

    var code = code;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationRegularisationGarantie/SupprimerMouvtPeriode",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), codersq: codersq, codefor: codefor, codegar: codegar, code: code
        },
        success: function (data) {
            $("#ListePeriodeRegularise").html(data);
            MapListBas();
            CloseLoading();
        },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            ShowCommonFancy("Error", "", responseError.ErrorMessage, 300, 100, true, true, true, false);
            CloseLoading();
        }
    });


}
function CheckDate() {
    toReturn = true;

    if ($("#dateDebutNewPeriode").val() == "") {
        $("#dateDebutNewPeriode").addClass('requiredField');
        toReturn = false;
    }
    if ($("#dateFinNewPeriode").val() == "") {
        $("#dateFinNewPeriode").addClass('requiredField');
        toReturn = false;
    }
    return toReturn;
}
function Initialiser() {
    $("#dateDebutNewPeriode").val("");
    $("#dateFinNewPeriode").val("");

}
function RetourEcran() {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codersq = $("#codersq").val();
    var codefor = $("#codefor").val();
    var codegar = $("#codegar").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationRegularisationGarantie/RetourEcran",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), codersq: codersq, codefor: codefor, codegar: codegar, tabGuid: tabGuid,
            addParamType: addParamType, addParamValue: addParamValue, modeNavig: $("#ModeNavig").val()
        },
        success: function () {

            CloseLoading();
        },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            ShowCommonFancy("Error", "", responseError.ErrorMessage, 300, 100, true, true, true, false);
            CloseLoading();
        }
    });

}