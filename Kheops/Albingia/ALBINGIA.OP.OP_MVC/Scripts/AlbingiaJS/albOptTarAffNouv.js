$(document).ready(function () {
    MapPageElement();
});

function MapPageElement() {

    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
    });

    $("#btnSuivant").live('click', function () {
        if (!$(this).attr('disabled')) {
            ValidForm();
        }
    });

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("input[type=radio][name^=checkTar_]").each(function () {
        $(this).change(function () {
            UpdateTarif($(this));
        });
    });
}

//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var infoContrat = $("#CodeContrat").val() + "_" + $("#VersionContrat").val() + "_P";
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/OptTarAffNouv/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, infoContrat: infoContrat, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------- Annule la form ------------------------
function CancelForm() {
    Redirection("FormVolAffNouv", "Index");
}

//------------ Valide la form --------------
function ValidForm() {
    var codeContrat = $("#CodeContrat").val();
    var versionContrat = $("#VersionContrat").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var acteGestion = $("#ActeGestion").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/OptTarAffNouv/ValidContrat",
        data: { codeOffre: codeOffre, version: version, type: type, codeContrat: codeContrat, versionContrat: versionContrat, splitHtmlChar: $("#SplitHtmlChar").val(), acteGestion: acteGestion, guid: $("#tabGuid").val(), codeAvn: $("#NumAvenantPage").val() },
        success: function (data) {
            //            $("#divDataNouveauContrat").html(data);
            AlbScrollTop();
            $("#divNouveauContrat").show();
            MapElementRecap();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------- MAJ BDD Tarif --------
function UpdateTarif(elem) {

    var codeContrat = $("#CodeContrat").val();
    var versionContrat = $("#VersionContrat").val();
    var guidTarif = elem.attr("id").split("_")[1];

    $.ajax({
        type: "POST",
        url: "/OptTarAffNouv/UpdateOptTarif",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, guidTarif: guidTarif },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------- Map les éléments de la récapitulation -----------
function MapElementRecap() {
    $("#btnConfHome").die().live('click', function () {
        Redirection("RechercheSaisie", "Index");
    });

    $("#btnConfCreate").die().live('click', function () {
        Redirection("CreationAffaireNouvelle", "Index");
    });

    $("#btnConfValider").die().live('click', function () {
        Redirection("AnCreationContrat", "Index");
    });
}