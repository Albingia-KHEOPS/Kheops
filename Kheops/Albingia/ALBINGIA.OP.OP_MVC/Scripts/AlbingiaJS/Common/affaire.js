
var affaire = {
    formule: {
        applique: {
            initPopup: function () {
                if (common.dom.exists("btnMultiObj") && common.dom.exists("divLstRsqObj")) {
                    $("#btnMultiObj").click(function () {
                        common.page.scrollTop();
                        $("#divLstRsqObj").show();
                        affaire.formule.applique.checkRsqObjets();
                    });
                }

                // sélectionne les garanties dès l'affichage de la page
                if ($("#ObjetRisqueCode").val() != "") {
                    affaire.formule.applique.checkRsqObjets();
                    $("#CodeCibleRsq").val($("input[name='radioRsq']:checked").attr("branche"));
                }
                else if ($("#FormGen").val() == "1") {
                    //todo
                }
                else {
                    if (common.dom.exists("chkModificationAVN") || $("#chkModificationAVN").is(":checked")) {
                        $("#btnMultiObj").trigger("click");
                    }
                }

                if (common.dom.exists("btnValidListApplique")) {
                    $("#btnCancelListApplique").live('click', function () {
                        $("#divLstRsqObj").hide();
                        if ($("#ObjetRisque").val() == "") {
                            //Annuler();
                        }
                    });

                    $("#btnValidListApplique").click(function () {
                        if (!$(this).attr("disabled")) {
                            var newCible = $("input[name='radioRsq']:checked").attr('branche');
                            if (newCible) {
                                if ($("#CodeCibleRsq").val() != "" && $("#CodeCibleRsq").val() != newCible) {
                                    ShowCommonFancy("Confirm", "Applique", "Attention, vous changez de risque, toutes les garanties sélectionnées pour ce risque seront supprimées.<br/>Voulez-vous continuer ?", 400, 150, true, true);
                                }
                                else {
                                    affaire.formule.applique.validListRsqObj();
                                }
                            }
                            else {
                                common.dialog.bigError("Vous devez sélectionner un risque.", true);
                            }
                        }
                    });
                }
            },
            checkRsqObjets: function () {
                $("input[name=checkObj]").removeAttr("checked");
                var elem = $("#ObjetRisqueCode").val().split(';');
                if (elem.length > 1) {
                    var currentRsq = elem[0];

                    $("input[name='radioRsq'][id='" + currentRsq + "']").attr("checked", true);
                    if ($("#OffreReadOnly").val() == "False") $("input[name='radioRsq'][id='" + currentRsq + "']").removeAttr("disabled");
                    $("td[albcontext='rsq_" + currentRsq + "']").removeClass("NotAffecte");

                    var tabObjet = elem[1].split('_');
                    for (i = 0; i < tabObjet.length; i++) {
                        $("input[id='check_" + currentRsq + "_" + tabObjet[i] + "']").attr("checked", true);
                        if ($("#OffreReadOnly").val() == "False") $("input[id='check_" + currentRsq + "_" + tabObjet[i] + "']").removeAttr("disabled");
                        $("td[albcontext='obj_" + currentRsq + "_" + tabObjet[i] + "']").removeClass("NotAffecte");
                    }
                }
            },
            validListRsqObj: function () {
                var rsqSelected = $("input[name='radioRsq']:checked");
                var currentRsq = rsqSelected.attr("id");
                var cibleRsq = rsqSelected.attr("branche");
                var strListObj = "_";

                $("input[id^='check_" + currentRsq + "_'][name='checkObj']").each(function () {
                    if ($(this).is(":checked")) {
                        if (strListObj.indexOf("_" + $(this).val().split('_')[1] + "_") < 0)
                            strListObj += $(this).val().split('_')[1] + "_";
                    }
                    else {
                        strListObj = strListObj.replace("_" + $(this).val().split('_')[1] + "_", "_");
                    }
                });
                strListObj = strListObj.substring(1).substring(0, strListObj.length - 2);

                $("#ObjetRisqueCode").val(currentRsq + ";" + strListObj);
                affaire.formule.applique.displayLibObjetRisque($("input[id^='check_" + currentRsq + "_'][name='checkObj']").length, strListObj.split('_').length);
                $("#divLstRsqObj").hide();

                if ($("#ObjetRisqueCode").val() != "") {
                    $("#CodeCibleRsq").val(cibleRsq);
                    if ($("#CodeFormule").val() != "0") {
                        //SaveAppliqueA();
                    }
                    else {
                        //SaveFormule();
                    }
                }
            },
            displayLibObjetRisque: function () {
                var nbRsq = $("input[type='radio'][name='radioRsq']").length;
                var codeRsqSelected = $("input[name='radioRsq']:checked").attr("id");
                var libRsqSelected = $("tr[id='" + codeRsqSelected + "_0']").attr("name");

                if (nbRsq == 1) {
                    // mono Risque
                    if (countObj == countSelObj) {
                        //Tous les objets sélectionnés
                        $("#ObjetRisque").val("à l'ensemble du risque");
                    }
                    else {
                        //1 à N objet(s) sélectionné(s)
                        $("#ObjetRisque").val("à l'objet(s) du risque");
                    }
                }
                else {
                    if (countObj == countSelObj) {
                        //Tous les objets sélectionnés
                        $("#ObjetRisque").val("au risque " + codeRsqSelected + " '" + libRsqSelected + "'");
                    }
                    else {
                        //1 à N objet(s) sélectionné(s)
                        $("#ObjetRisque").val("à l'objet(s) du risque" + codeRsqSelected + " '" + libRsqSelected + "'");
                    }
                }
            }
        },
        avenant: {
            init: function () {
                if (common.dom.exists("chkModificationAVN") && !$("#chkModificationAVN").is(":disabled")) {
                    $("#chkModificationAVN").live("change", function () {
                        affaire.formule.redirect("FormuleGarantie", "Index", !$("#chkModificationAVN").is(":checked"));
                    });
                }
            }
        },
        redirect: function (cible, job, readonlyDisplay, sessionLost) {
            common.page.isLoading = true;
            var codeOffre = $("#Offre_CodeOffre").val();
            var version = $("#Offre_Version").val();
            var type = $("#Offre_Type").val();
            var tabGuid = $("#tabGuid").val();
            var modeNavig = $("#ModeNavig").val();
            var libelle = $("#Libelle").val();
            var lettreLib = $("#LettreLib").val();
            var addParamType = $("#AddParamType").val();
            var addParamValue = $("#AddParamValue").val();
            var codeRisque = $("#ObjetRisqueCode").val();
            var codeObjet = "";
            var branche = $("#HiddenBranche").val();
            var codeObjetRisque = $("#ObjetRisqueCode").val();

            if (codeObjetRisque != undefined) {
                codeObjet = codeObjetRisque.split(";")[1];
            }

            if (codeRisque != undefined) {
                codeRisque = codeRisque.split(";")[0];
            }

            if (sessionLost == undefined) {
                sessionLost = false;
            }

            $.ajax({
                type: "POST",
                url: "/FormuleGarantie/Redirection/",
                data: {
                    cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeRisque: codeRisque, codeObjet: codeObjet, branche: branche,
                    tabGuid: tabGuid, modeNavig: modeNavig,
                    libelle: libelle, lettreLib: lettreLib, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(),
                    addParamType: addParamType, addParamValue: addParamValue, readonlyDisplay: readonlyDisplay,
                    isModeConsultationEcran: $("#IsModeConsultationEcran").val(), isForceReadOnly: $("#IsForceReadOnly").val(), sessionLost: sessionLost, isModeConsult: window.isReadonly
                },
                success: function (data) { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    },
    showConnexites: function (title, params) {
        let jsonParams = JSON.stringify(params);
        return common.knockout.components.includeInDialog(
            (title || "Connexités"),
            { width: 1200, height: 500 },
            affaire.connexites,
            jsonParams.trim().substr(1, jsonParams.length - 2));
    },
    showSearchPopup: function (title, params) {
        let jsonParams = JSON.stringify(params);
        return common.knockout.components.includeInDialog(
            (title || "Recherche"),
            { width: 1200, height: 500 },
            affaire.recherche,
            jsonParams.trim().substr(1, jsonParams.length - 2));
    },
    callStandardNext: function (returnHome) {

        $(".requiredField").removeClass("requiredField");

        let cible = "InformationsSpecifiquesGarantie";
        let job = "Index";
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let tabGuid = $("#tabGuid").val();
        let modeNavig = $("#ModeNavig").val();
        let libelle = $("#Libelle").val();
        let lettreLib = $("#LettreLib").val();
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let codeRisque = $("#ObjetRisqueCode").val();
        let branche = $("#HiddenBranche").val();

        if (typeof sessionLost === "undefined") {
            sessionLost = false;
        }
        common.page.isLoading = true;

        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/Redirection/",
            data: {
                cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeRisque: codeRisque, codeObjet: "", branche: branche,
                tabGuid: tabGuid, modeNavig: modeNavig,
                libelle: libelle, lettreLib: lettreLib, saveCancel: returnHome ? 1 : 0, paramRedirect: $("#txtParamRedirect").val(),
                addParamType: addParamType, addParamValue: addParamValue, readonlyDisplay: false,
                isModeConsultationEcran: $("#IsModeConsultationEcran").val(), isForceReadOnly: $("#IsForceReadOnly").val(), sessionLost: sessionLost, isModeConsult: $("#IsModeConsultationEcran").val() == "True"
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    },
    /*showInfosPopup: function (code, version, type, avn, isFullVersion, formatResult, next) {
        $("#isLoadingBandeau").val("1");
        common.page.isLoading = true;
        let list = [code, version, type];
        if (!isNaN(parseInt(avn))) {
            list.push(avn);
        }
        common.$postJson("/SyntheseAffaire/Popup", { codeAffaire: code, numeroAliment: version, typeAffaire: type, numeroAvenant: list.length === 3 ? null : avn }, true).done(function (html) {
            if ($.isFunction(formatResult)) {
                html = formatResult(html);
            }
            common.$ui.showDialog($("<div><div id='partial_synthese'>" + html + "</div></div>"), "", "Synthèse", { width: isFullVersion ? 1210 : 1020, height: 550 }, null, null, "dialog-fix");
            if ($.isFunction(next)) {
                next();
            }
            $("#isLoadingBandeau").val("0");
            common.page.isLoading = false;
        });
    },*/
    showInfosPopup: function (code, version, type, avn, isFullVersion, formatResult, next, isSynthese) {
        if (isSynthese == false) {
            $("#isLoadingBandeau").val("1");
            common.page.isLoading = true;
            let list = [code, version, type];
            if (!isNaN(parseInt(avn))) {
                list.push(avn);
            }
            common.$postJson("/SyntheseAffaire/PopupS", { codeAffaire: code, numeroAliment: version, typeAffaire: type, numeroAvenant: list.length === 3 ? null : avn }, true).done(function (html) {
                if ($.isFunction(formatResult)) {
                    html = formatResult(html);
                }
                common.$ui.showDialog($("<div><div id='partial_synthese'>" + html + "</div></div>"), "", "Synthèse", { width: isFullVersion ? 1210 : 1020, height: 550 }, null, null, "dialog-fix");
                if ($.isFunction(next)) {
                    next();
                }
                $("#isLoadingBandeau").val("0");
                common.page.isLoading = false;
            });

        }
       else {
            $("#isLoadingBandeau").val("1");
            common.page.isLoading = true;
            let list = [code, version, type];
            if (!isNaN(parseInt(avn))) {
                list.push(avn);
            }
            common.$postJson("/SyntheseAffaire/PopupS", { codeAffaire: code, numeroAliment: version, typeAffaire: type, numeroAvenant: list.length === 3 ? null : avn }, true).done(function (html) {
                if ($.isFunction(formatResult)) {
                    html = formatResult(html);
                }
                common.$ui.showDialog($("<div><div id='partial_synthese'>" + html + "</div></div>"), "", "Synthèse", { width: isFullVersion ? 1310 : 1080, height: 605 }, null, null, "dialog-fix");
                if ($.isFunction(next)) {
                    next();
                }
                $("#isLoadingBandeau").val("0");
                common.page.isLoading = false;
            });
        }
        },
    
    initEntete: function () {
        if (affaire.contrat && affaire.contrat.entete) {
            affaire.contrat.entete.init();
        }
        $(document).on(customEvents.entete.initializing, function () {
            common.page.isLoading = true;
        });
        $(document).on([customEvents.entete.loaded, customEvents.entete.error].join(" "), function () {
            window.requestAnimationFrame(function () {
                common.page.isLoading = false;
            });
        });
    }
};

$(function () {
    affaire.initEntete();
});
