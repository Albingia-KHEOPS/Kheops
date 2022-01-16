$(document).ready(function () {
    MapPageElement();
});

function MapPageElement() {
    $("#btnSuivant").live('click', function () {
        if ($(this).attr("disabled")) {
            return false;
        }
        Suivant();
    });
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Redirection("CreationFormuleGarantie", "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
}

function Suivant(evt) {
    try {

        var branche = encodeURIComponent($("#Branche").val());
        var section = encodeURIComponent($("#section").val());
        var cible = '';
        var additionalParams = '';
        var dataToSave = encodeURIComponent(GetValues());
        var splitChars = encodeURIComponent($("#jsSplitChar").val());
        var strParameters = encodeURIComponent($("#parameters").val());
        if (!Validate() && !window.isReadonly)
            return;
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        var libelleFormule = $("#LibelleFormule").val();
        var lettreLibelleFormule = $("#LettreLibelleFormule").val();

        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        //if (!($('#frameIS')[0].contentWindow.Validate())) {
        //ZBO : à supprimer
        if (!Validate() && !window.isReadonly) {
            ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 350, 180, true, true);
            return;
        }
        //if (!SaveData()) {
        //    return;
        //}
        ShowLoading();
        if ($("#txtSaveCancel").val() == "1") {
            Redirection("RechercheSaisie", "Index");
        }
        else {
            $.ajax({
                type: "POST",
                url: "/InformationsSpecifiquesRecupGaranties/RedirectionVersCondition",
                data: {
                    branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters,
                    argCodeOffre: codeOffre, argVersion: version, argType: type, argCodeFormule: codeFormule, argCodeOption: codeOption, argLibelleFormule: libelleFormule, argLettreLibelleFormule: lettreLibelleFormule, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig,
                    addParamType: addParamType, addParamValue: addParamValue
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }

    } catch (e) {
        common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
        CloseLoading();
    }


}
//-----------------Redirection----------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();

    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/InformationsSpecifiquesRecupGaranties/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption, tabGuid: tabGuid, modeNavig: modeNavig,
            addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
