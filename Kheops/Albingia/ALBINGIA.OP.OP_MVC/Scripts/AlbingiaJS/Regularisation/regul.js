
var regul = {
    regulKey: "regul_context",
    buildId: function () {
        return $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumInterne").val()
            + $("#tabGuid").val()
            + "addParamAVN|||" + $("#AddParamValue").val()
            + (window.isReadonly ? "" : "||IGNOREREADONLY|1")
            + "addParam";
    },
    tryGetContext: function (stepName, rsqId, grId, rgGrId) {
        try {
            window.context = JSON.parse(window.sessionStorage.getItem(regul.regulKey));
            if (window.context == null) {
                return null;
            }

            window.context.Step = stepName;
            window.context.RsqId = rsqId ? rsqId : 0;
            window.context.GrId = grId ? grId : 0;
            window.context.RgGrId = rgGrId ? rgGrId : 0;

            return window.context;
        }
        catch (e) {
            console.error(e);
            return null;
        }
    },
    resetContext: function () {
        window.sessionStorage.removeItem(regul.regulKey);
        return null;
    },
    tryCreateContext: function (next) {
        try {
            var accessMode = $("#ModeAvt").val();
            var rg = common.getRegul();
            var keys = [$("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), ($("#NumInterne").val() + $("#tabGuid").val() + "addParamAVN|||" + $("#AddParamValue").val())];
            if (window.isReadonly) {
                accessMode = "";
                keys[keys.length - 1] += "addParam";
            }
            else {
                keys[keys.length - 1] += "||IGNOREREADONLY|1addParam";
            }
            if (window.context == undefined) {
                window.context = {
                    ComputeDone: false,
                    ValidationDone: false,
                    DateDebut: common.dateStrToInt($("#PeriodeDeb").val() || ""),
                    DateFin: common.dateStrToInt($("#PeriodeFin").val() || ""),
                    Gestionnaire: $("#GestionnaireCode").val() || "",
                    Souscripteur: $("#SouscripteurCode").val() || "",
                    IdContrat: {
                        CodeOffre: $("#Offre_CodeOffre").val(),
                        Type: $("#Offre_Type").val(),
                        Version: $("#Offre_Version").val(),
                        TypeContrat: $("#Type").length == 0 ? "" : $("#Type").val().split(" - ")[0],
                        LibTypeContrat: $("#Type").length == 0 ? "" : $("#Type").val().split(" - ")[1],
                        IsHisto: $("#ModeNavig").val() == "H"
                    },
                    IsReadOnlyMode: accessMode != "CREATE" && accessMode != "UPDATE",
                    LotId: $("#lotId").val() || "",
                    Mode: rg.mode,
                    ModeleAvtRegul: {
                        TypeAvt: $.trim($("#TypeAvt").val() || ""),
                        NumInterneAvt: $("#NumInterne").val(),
                        NumAvt: $("#NumInterne").val(),
                        MotifAvt: $("#MotifAvt").val(),
                        DescriptionAvt: $("#Description").val(),
                        ObservationsAvt: $.trim(($("#Observation").html() || "").replace(/<br>/ig, "\n"))
                    },
                    RgId: rg.id,
                    Scope: rg.scope,
                    Step: "ChoixPeriodeCourtier",
                    TypeAvt: $("#TypeAvt").val() || "",
                    User: "",
                    RgHisto: rg.histo,
                    Type: rg.type,
                    Exercice: $("#ExerciceRegule").val() || "",
                    CodeEnc: $("#inQuittancement").length == 0 ? "" : $("#inQuittancement").attr("albcode"),
                    CodeICC: $("#inCourtierCom").length > 0 ? $.trim($("#inCourtierCom").val().split("-")[0]) : "",
                    CodeICT: $("#inCourtier").length > 0 ? $.trim($("#inCourtier").val().split("-")[0]) : "",
                    TauxCom: $("#inHorsCATNAT").val() || "",
                    TauxComCATNAT: $("#inCATNAT").val() || "",
                    KeyValues: keys,
                    RegimeTaxe: $("#RegimeTaxe").val() || "",
                    IsSaveAndQuit: false
                };
            }
            else {
                window.context.Step = "ChoixPeriodeCourtier";
                window.context.KeyValues = keys;
                window.context.User = "";
            }

            common.$postJson("/Regularisation/EnsureContext", window.context)
                .done(function (data) {
                    window.sessionStorage.setItem(regul.regulKey, JSON.stringify(data));
                    window.context = data;
                    if ($.isFunction(next)) next(data);
                }).fail(function (x, s, e) {
                    window.sessionStorage.setItem(regul.regulKey, JSON.stringify(window.context));
                });
        }
        catch (e) {
            console.error(e);
            return null;
        }
    },
    tryMakeContext: function (next) {
        if (window.context != null) {
            if ($.isFunction(next)) {
                next();
            }
            else {
                return window.context;
            }
        }

        return regul.tryCreateContext(next);
    },
    nextStep: function (context) {
        if (!context) {
            common.error.showMessage("Erreur: l'étape suivante n'a pas pu être identifiée");
        }

        ShowLoading();
        return common.$postJson("/Regularisation/NextStep", context)
            .fail(function (x, s, e) {
                common.error.show(x);
            })
            .done(function (data) {
                if (!data || !data.Step) {
                    common.error.showMessage("Erreur: impossible d'accéder à l'étape suivante");
                }
                else {
                    regul.goToStep(data);
                }
            });
    },
    previousStep: function (context, notRedirecting, next) {
        if (!context) {
            common.error.showMessage("Erreur: l'étape précédente n'a pas pu être identifiée");
        }

        ShowLoading();
        //return common.$postJson("/Regularisation/" + (notRedirecting ? "" : "Route") + "PreviousStep", context)
        return common.$postJson("/Regularisation/PreviousStep", context)
            .done(function (data) {
                if ($.isFunction(next)) {
                    next(data);
                }
                else if (notRedirecting || notRedirecting === undefined) {
                    regul.goToStep(data);
                }
            })
            .fail(function (x, s, e) {
                common.error.showMessage("Erreur: l'étape précédente n'a pas pu être identifiée");
            });
    },
    reachStep: function (context, step) {
        if (!context) {
            common.error.showMessage("Erreur: l'étape suivante n'a pas pu être identifiée");
        }

        ShowLoading();

        if (common.getRegul() != undefined) {
            if (common.getRegul().id != NaN && common.getRegul().id != 0) {
                context.KeyValues[context.KeyValues.length - 1] = common.addOrReplaceParam(context.KeyValues[context.KeyValues.length - 1], 'REGULEID', common.getRegul().id);
                context.KeyValues[context.KeyValues.length - 1] = common.addOrReplaceParam(context.KeyValues[context.KeyValues.length - 1], 'AVNMODE', "UPDATE");
            }
        }


        return common.$postJson("/Regularisation/ReachStep", { context: context, stepToRich: step })
            .fail(function (x, s, e) {
                common.error.show(x);
            })
            .done(function (data) {
                if (!data || !data.Step) {
                    common.error.showMessage("Erreur: impossible d'accéder à l'étape demandée");
                }
                else {
                    regul.goToStep(data);
                }
            });
    },
    goToStep: function (context) {
        if (!context) {
            common.error.showMessage("Erreur: l'étape n'a pas pu être identifiée");
        }
        
        regul.buildRedirectForm(context);
        context.IsSaveAndQuit = $("#txtSaveCancel").val() == "1";
        return common.$postJson("/Regularisation/RouteFromAjaxCall", context);
    },
    buildRedirectForm: function (context) {
        if ($("#redirectForm").length > 0) {
            $("#redirectForm").remove();
        }

        $("body").append("<form id='redirectForm' method='POST'></form>");
        var names = common.razor.getFormNames(null, context);
        for (var x = 0; x < names.length; x++) {
            if (names[x].id) {
                $("#redirectForm").append("<input id='redirectForm" + names[x].id + "' name='" + names[x].name + "' type='hidden' value='' />");
                $("#redirectForm" + names[x].id).val(names[x].value);
            }
            else {
                $("#redirectForm").append("<input name='" + names[x].name + "' type='hidden' value='' />");
                $('#redirectForm input[name="' + names[x].name +'"]').val(names[x].value);
            }

        }
    },
    navPane: {
        isReady: false,
        prepare: function () {
            if (!regul.navPane.isReady) {
                $("span[name='stepStateNavLink']").click(function (ev) {
                    regul.navPane.stepClick(ev.target);
                });

                regul.navPane.isReady = true;
            }
        },
        standardRedirection: function (url) {
            let tabGuid = "";
            let modeNavig = "";
            let addParamType = $("#AddParamType").val();
            let addParamValue = $("#AddParamValue").val();
            let addParamString = "addParam" + addParamType + "|||" + addParamValue + (window.isReadonly ? "" : "||IGNOREREADONLY|1") + "addParam";
            if ($("#tabGuid").val() != undefined && $("#tabGuid").val() !== "") {
                tabGuid = $("#tabGuid").val();
            }

            if ($("#ModeNavig").val() != undefined && $("#ModeNavig").val() !== "") {
                modeNavig = GetFormatModeNavig($("#ModeNavig").val());
            }
            else {
                modeNavig = GetFormatModeNavig("S");
            }

            CommonSaveRedirectRegul(url + tabGuid + addParamString + modeNavig);
        },
        stepClick: function (link) {
            if (!link || $(link).length == 0) {
                return;
            }

            var $lnk = $(link);
            var url = $lnk.data("url");
            if (url.indexOf("CreationRegularisation/") == 0 || url.indexOf("Quittance/") == 0) {
                if ($lnk.data("rsq")) {
                    let p = common.albParam.buildObject();
                    if (!p.RSQID) {
                        p.RSQID = $lnk.data("rsq");
                        let apv = common.albParam.objectToString(p);
                        common.albParam.setString(apv);
                    }
                }
                regul.navPane.standardRedirection(url);
            }
            else {
                if (!window.context || $.isEmptyObject(window.context)) {
                    regul.tryMakeContext(function (result) {
                        if (!window.context) {
                            common.error.showMessage("Impossible d'accéder à l'étape demandée");
                            return;
                        }

                        regul.reachStep(window.context, $lnk.data("step"));
                    });
                }
                else {
                    regul.reachStep(window.context, $lnk.data("step"));
                }
            }
        },
        refreshDisplay: function () {
            if (window.context && !window.context.ValidationDone) {
                $("span[name^='stepStateNav', data-step='Cotisation']").parent().remove();
                $("#ControleFinMenuArbreLI").remove();
                //$("span[name='stepStateNavLink']").off();
                //regul.navPane.isReady = false;
                //regul.navPane.prepare();
            }
        }
    }
};

$(function () {
    regul.navPane.prepare();
    $("#linkAccueil").kclick(function () {
        DeverouillerUserOffres($("#tabGuid").val());
        RedirectToAction("Index", "RechercheSaisie");
    });
});