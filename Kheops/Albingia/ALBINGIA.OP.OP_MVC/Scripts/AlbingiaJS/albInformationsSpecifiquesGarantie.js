
var ISGarantie = function () {
    this.mapPageElement = function () {
        $("#btnSuivant").kclick(function (evt, data) {
            if ($(this).attr("disabled")) {
                return false;
            }
            infosSpeGarantie.suivant(data && data.returnHome);
        });
        $("#btnAnnuler").kclick(function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    infosSpeGarantie.redirect("CreationFormuleGarantie", "Index");
                    break;
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").val('');
        });

        if (window.isReadonly) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        $("#btnISRecup").kclick(function () {
            infosSpeGarantie.redirect("InformationsSpecifiquesRecupGaranties", "Index");
        });
    }

    this.suivant = function (returnHome) {
        try {
            common.page.isLoading = true;
            if (!window.isReadonly && !window.isModifHorsAvenant) {
                let x = infosSpe.saveAjax({
                    branche: $("#Branche").val(),
                    section: $("#section").val(),
                    risque: $("#Code").val(),
                    formule: $("#CodeFormule").val(),
                    option: $("#CodeOption").val(),
                    guid: common.tabGuid
                });
                if (!x) {
                    ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 400, 200, true, true);
                    return;
                }
                x.done(function () {
                    doRedirect();
                }).fail(function (x) {
                    common.error.showXhr(x);
                });
            }
            else {
                doRedirect();
            }
            function doRedirect() {
                if (returnHome) {
                    infosSpeGarantie.redirect("RechercheSaisie", "Index", false);
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "/InformationsSpecifiquesGarantie/RedirectionCondition",
                        data: {
                            argCodeOffre: $("#Offre_CodeOffre").val(), argVersion: $("#Offre_Version").val(), argType: $("#Offre_Type").val(), codeRisque: $("#Code").val(),
                            argCodeFormule: $("#CodeFormule").val(), argCodeOption: $("#CodeOption").val(),
                            tabGuid: common.tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: $("#ModeNavig").val(),
                            addParamValue: $("#AddParamValue").val(), isForceReadOnly: $("#IsForceReadOnly").val()
                        },
                        error: function (request) {
                            common.error.showXhr(request);
                        }
                    });
                }
            }
        } catch (e) {
            common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
            CloseLoading();
        }
        //try {
        //    var branche = encodeURIComponent($("#Branche").val());
        //    var section = encodeURIComponent($("#section").val());
        //    var additionalParams = "";
        //    var dataToSave = encodeURIComponent(infosSpe.getValues());
        //    var splitChars = encodeURIComponent($("#jsSplitChar").val());
        //    var strParameters = encodeURIComponent($("#parameters").val());
        //    if (!infosSpe.validate() && !window.isReadonly) {
        //        return;
        //    }
        //    var codeOffre = $("#Offre_CodeOffre").val();
        //    var version = $("#Offre_Version").val();
        //    var type = $("#Offre_Type").val();
        //    var codeFormule = $("#CodeFormule").val();
        //    var codeOption = $("#CodeOption").val();
        //    var libelleFormule = $("#LibelleFormule").val();
        //    var lettreLibelleFormule = $("#LettreLibelleFormule").val();
        //    var codeRisque = $("#CodeRisque").val();

        //    var tabGuid = $("#tabGuid").val();
        //    var modeNavig = $("#ModeNavig").val();

        //    var argRedirectIS = $("#ISRedirect").val();

        //    var addParamType = $("#AddParamType").val();
        //    var addParamValue = $("#AddParamValue").val();

        //    //ZBO : à supprimer
        //    if (!infosSpe.validate() && !window.isReadonly) {
        //        ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 350, 180, true, true);
        //        return;
        //    }

        //    ShowLoading();
        //    if (returnHome) {
        //        infosSpeGarantie.redirect("RechercheSaisie", "Index");
        //    }
        //    else {
        //        $.ajax({
        //            type: "POST",
        //            url: "/InformationsSpecifiquesGarantie/RedirectionVersCondition",
        //            data: {
        //                branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
        //                , splitChars: splitChars, strParameters: strParameters,
        //                argCodeOffre: codeOffre, argVersion: version, argType: type, codeRisque: codeRisque, argCodeFormule: codeFormule, argCodeOption: codeOption, argLibelleFormule: libelleFormule, argLettreLibelleFormule: lettreLibelleFormule, argRedirectIS: argRedirectIS, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig,
        //                addParamType: addParamType, addParamValue: addParamValue, isForceReadOnly: $("#IsForceReadOnly").val()
        //            },
        //            error: function (request) {
        //                common.error.showXhr(request);
        //            }
        //        });
        //    }

        //}
        //catch (e) {
        //    common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
        //    CloseLoading();
        //}
    }

    this.redirect = function (cible, job) {
        ShowLoading();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();

        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();

        var libelle = $("#LibelleFormule").val();
        var lettreLib = $("#LettreLibelleFormule").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesGarantie/Redirection/",
            data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption, libelle: libelle, lettreLib: lettreLib, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function () { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
};
var infosSpeGarantie = new ISGarantie();
$(function () {
    infosSpeGarantie.mapPageElement();
});
