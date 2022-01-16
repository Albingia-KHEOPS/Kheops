
var ISObjet = function () {
    //---------------------Affecte les fonctions au boutons-------------
    this.mapPageElement = function () {
        $("#btnErrorOk").kclick(function () {
            CloseCommonFancy();
        });
        $("#btnAnnuler").kclick(function () {
            ShowCommonFancy("Confirm", window.isModifHorsAvenant ? "CancelReadonly" : "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    infosSpeObjet.cancelForm();
                    break;
            }
            $("#hiddenAction").clear();
        });
        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });
        if (window.isReadonly || window.isModifHorsAvenant) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        $("#btnISRecup").kclick(function () {
            infosSpeObjet.redirect("InformationsSpecifiquesRecupObjets", "Index", true);
        });
    }
    //----------------Redirection------------------
    this.redirect = function (cible, job, withSave) {
        try {
            common.page.isLoading = true;

            let branche = encodeURIComponent($("#Branche").val());
            let section = encodeURIComponent($("#section").val());
            let additionalParams = '';
            let dataToSave = encodeURIComponent(infosSpe.getValues());
            let splitChars = encodeURIComponent($("#jsSplitChar").val());
            let strParameters = encodeURIComponent($("#parameters").val());
            let codeOffre = $("#Offre_CodeOffre").val();
            let version = $("#Offre_Version").val();
            let type = $("#Offre_Type").val();
            let codeRisque = $("#CodeRisque").val();
            let codeObjet = $("#CodeObjet").val();
            let tabGuid = $("#tabGuid").val();
            let modeNavig = $("#ModeNavig").val();
            let libelleRisque = $("#LibRisque").val();
            let libelleObjet = $("#LibObjet").val();
            let addParamType = $("#AddParamType").val();
            let addParamValue = $("#AddParamValue").val();
            let isForceReadOnly = $("#IsForceReadOnly").val();

            $.ajax({
                type: "POST",
                url: "/InformationsSpecifiquesObjets/Redirection/",
                data: {
                    branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters
                    , cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet,
                    libelleRisque: libelleRisque, libelleObjet: libelleObjet
                    , tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), withSave: !!withSave, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue
                    , isForceReadOnly: isForceReadOnly
                },
                success: function (data) { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        } catch (e) {
            common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
            common.page.isLoading = false;
        }
    }
    //----------------------Affiche les controles des paramètres de risque---------------------
    this.displayParamRisque = function () {
        $("#RisqueIndexe").live('change', function () {
            if ($(this).is(':checked')) {
                $("#LCI").attr('checked', 'checked');
                $("#Assiette").attr('checked', 'checked');
                $("#paramRisqueIndexe").show();
            }
            else {
                $("#LCI").removeAttr('checked');
                $("#Franchise").removeAttr('checked');
                $("#Assiette").removeAttr('checked');
                $("#paramRisqueIndexe").hide();
            }

        });
    }

    //----------------------Affiche les infos détaillées---------------------
    this.checkFields = function () {
        if ($("#IndiceRef").val() == "") {
            $("#RisqueIndexe").removeAttr('checked');
            $("#RisqueIndexe").attr('disabled', 'disabled');
        }

        if ($("#RisqueIndexe").is(':checked')) $("#paramRisqueIndexe").show();
        else $("#paramRisqueIndexe").hide();
    }
    //----------------------Reset de la page---------------------
    this.cancelForm = function () {
        infosSpeObjet.redirect("DetailsObjetRisque", "Index", false);
    }
    //----------------------Suivant---------------------
    this.initSuivant = function (evt, data) {
        $("#btnSuivant").kclick(function () {
            common.page.isLoading = true;
            if (!window.isReadonly && !window.isModifHorsAvenant) {
                let x = infosSpe.saveAjax({
                    branche: $("#Branche").val(),
                    section: $("#section").val(),
                    risque: $("#CodeRisque").val(),
                    objet: $("#CodeObjet").val(),
                    guid: common.tabGuid
                });
                if (!x) {
                    ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 400, 200, true, true);
                    return;
                }
                x.done(function () {
                    doRedirect();
                }).fail(function (x, s, m) {
                    let msg = null;
                    if (x.responseText) {
                        try {
                            msg = JSON.parse(x.responseText);
                        } catch (e) { }
                    }
                    if (msg instanceof Object && kheops.errors.isCustomError(msg)) {
                        msg = kheops.errors.display(msg);
                    }
                    else {
                        msg = m;
                    }
                });
            }
            else {
                doRedirect();
            }
            function doRedirect() {
                if (data && data.returnHome) {
                    infosSpeObjet.redirect("RechercheSaisie", "Index", false);
                }
                else {
                    infosSpeObjet.redirect("DetailsRisque", "Index", false);
                }
            }
        });
    }
};

var infosSpeObjet = new ISObjet();
$(function () {
    infosSpeObjet.displayParamRisque();
    infosSpeObjet.checkFields();
    infosSpeObjet.initSuivant();
    infosSpeObjet.mapPageElement();
});
