var IsEntete = function () {

    $(document).ready(function () {
        infoSpecEntete.BonificationFocus();
        infoSpecEntete.AfficherInfoBonification();
        infoSpecEntete.FormatTaux();
        //infoSpecEntete.AffectChangeBonification();
        infoSpecEntete.Suivant();
        infoSpecEntete.MapElementPage();

    });
    var Taux = "";
    //----------------------Met le focus sur le premier controle au démarrage---------------------
    this.BonificationFocus = function () {
        $("#Bonification").focus();
    }
    //----------------Redirection------------------
    this.Redirection = function (cible, job) {
        ShowLoading();

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var modeNavig = $("#ModeNavig").val();
        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesBranche/Redirection/",
            data: {
                cible: cible, job: job, codeOffre: codeOffre, version: version, 
                type: type, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue
            },
            success: function () { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //----------------Map les éléments de la page------------------
    this.MapElementPage = function () {
        $("#btnAnnuler").live('click', function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnConfirmOk").live('click', function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    infoSpecEntete.Cancel();
                    break;
                case "osMessageConfirm":
                    ShowLoading();
                    $("#nouvelleVersion").val("1");
                    infoSpecEntete.Enregistrer();
                    break;
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancel").live('click', function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "osMessageConfirm":
                    $("#nouvelleVersion").val("-1");
                    break;
            }
            $("#hiddenAction").val('');
        });

        if (window.isReadonly) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        if (window.isReadonly || $("#Offre_Type").val() == "P") {
            $("#dvOffreSimplifiee").hide();
        }
    }
    //------------- Annule la form ------------------------
    this.Cancel = function () {
        var type = $("#Offre_Type").val();
        if (type == "O")
            infoSpecEntete.Redirection("ModifierOffre", "Index");
        else if (type == "P")
            infoSpecEntete.Redirection("AnInformationsGenerales", "Index");
    }
    //----------------------Affiche ou cache les infos de bonification---------------------
    this.AfficherInfoBonification = function () {
        if ($("#Bonification").is(':checked')) {
            $("#aAfficher").show();
        } else {
            $("#aAfficher").hide();
            $("#Taux").val("");
            $("#Anticipee").removeAttr("checked");
        }
    }
    //----------------------Formate le controle du taux---------------------
    this.FormatTaux = function () {
        $("#Taux").attr("maxlength", 7);
        $("#Taux").live("keyup", function (event) {
            $(this).removeClass('requiredField');
            if (!isNaN($(this).val()) && $(this).val() <= 100) {
                Taux = $.trim($(this).val());
            }
            $(this).val(Taux);

        });
    }

    // Test enregistrement -------------//

    //this.Suivant = function() {
    //    $("#btnSuivant").kclick(function () {
    //        common.page.isLoading = true;

    //        if (!window.isReadonly) {
    //            let x = infosSpe.saveAjax({
    //                branche: $("#Branche").val(),
    //                section: $("#section").val(),
    //                guid: common.tabGuid
    //            });
    //            //}
    //            try {
    //                if (!x) {
    //                    ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 400, 200, true, true);
    //                    return
    //                }

    //                if ($("#osMessageVersion").val().toLocaleLowerCase() == 'true' && $("#offreSimplifie").is(':checked') == true) {
    //                    //ShowCommonFancy("Confirm", "osMessageConfirm", "Voulez-vous créer une autre version ?", 350, 180, true, true);
    //                    ShowCommonFancy("Confirm", "osMessageConfirm", "Attention! Cette action va supprimer tous les risques et les formules de l'offre. Voulez-vous continuer?", 350, 180, true, true);
    //                    return;
    //                } else {
    //                    infoSpecEntete.Enregistrer();
    //                }

    //            }
    //            catch (e) {
    //                common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
    //                CloseLoading();
    //            }
    //        }
    //        else
    //            infoSpecEntete.Enregistrer();

    //    });
    //}
    //--------------------------------//


    //----------Suivant -------------------//
    this.Suivant = function (evt, data) {
        $("#btnSuivant").kclick(function () {
            common.page.isLoading = true;
            let NumeroAvenant = $("#NumAvenantPage").val();
            if (!window.isReadonly && !window.isModifHorsAvenant) {
                let x = infosSpe.saveAjax({
                    branche: $("#Branche").val(),
                    section: $("#section").val(),

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
                if (NumeroAvenant != "0") {
                    infoSpecEntete.Redirection("AnnulationQuittances", "Index", false);
                }
                else {
                    infoSpecEntete.Redirection("ChoixClauses", "Index", false);
                }
            }
        });


    }
    //-----------Fin----------------------//

    this.AfficherInfoBonification = function () {
        $("#Bonification").live('change', function () {
            infoSpecEntete.AfficherInfoBonification();
        });
    }

    this.Enregistrer = function () {
        try {
            var branche = encodeURIComponent($("#Branche").val());
            var section = encodeURIComponent($("#section").val());
            var cible = '';
            var additionalParams = '';
            var dataToSave = encodeURIComponent(getValues());
            var splitChars = encodeURIComponent($("#jsSplitChar").val());
            var strParameters = encodeURIComponent($("#parameters").val());
            //if (!Validate())
            //    return;
            var codeOffre = $("#Offre_CodeOffre").val();
            var version = $("#Offre_Version").val();
            var type = $("#Offre_Type").val();
            var tabGuid = $("#tabGuid").val();
            var offreSimplifie = $("#offreSimplifie").is(':checked');
            var numAvenant = $("#NumAvenantPage").val();
            var argRedirectIS = $("#ISRedirect").val();
            var addParamType = $("#AddParamType").val();
            var addParamValue = $("#AddParamValue").val();

            if ($("#osMessageVersion").val())
                ShowLoading();
            $.ajax({
                type: "POST",
                url: "/InformationsSpecifiquesBranche/Enregistrement/",
                data: {
                    message: $("#nouvelleVersion").val(),
                    branche: branche, offreSimplifie: offreSimplifie, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters, argRedirectIS: argRedirectIS
                    , codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val()
                    , modeNavig: $("#ModeNavig").val(), numAvenant: numAvenant, addParamType: addParamType, addParamValue: addParamValue
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });

        } catch (e) {
            common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
            CloseLoading();
        }
    }
};

var infoSpecEntete = new IsEntete();
