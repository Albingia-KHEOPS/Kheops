
var InfosSpe = function () {
    //*******************************Chargement des Infos IS****************
    this.load = function () {
        infosSpe.loadData();
    };

    //*******************************Scroll top Div flottante****************
    this.scrollToTopMenu = function () {
        if (!$("#dvIsMenu").val()) {
            return;
        }
        $("span[name=albISLink]").click(function () {
            $('#dvISGtiesResult').scrollTop(0);
            var scrollDiv = $(this).attr('albISLink');
            var topDiv = $('#dvISGtiesResult').position().top;
            var top = $('#' + scrollDiv).position().top;
            $('#dvISGtiesResult').scrollTop(top - topDiv);
        });
    };

    this.start = function () {
        $(":input[eventBehaviour]").each(function () {
            infosSpe.applyBehaviours(this);
        });
    };

    this.applyBehaviours = function (input) {
        $(":input[linkBehaviour='" + $(input).attr("id").substring(4, $(input).attr('id').length) + "']").each(function () {
            let behaviours = $(this).attr("behaviour").split("||");
            for (var i = 0; i < behaviours.length; i++) {
                switch (behaviours[i]) {
                    case "Disable":
                        if ($("#map_" + $(this).attr("linkBehaviour")).is(":checked")) {
                            $(this).removeAttr("disabled");
                        }
                        else {
                            $(this).attr("disabled", "disabled");
                        }
                        break;
                    case "InvEnable":
                        if ($("#map_" + $(this).attr("linkBehaviour")).is(":checked")) {
                            $(this).attr("disabled", "disabled");
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }
                        break;
                    case "Reset": infosSpe.resetValue(this);
                        break;
                    case "Mandatory":
                        if ($(input).val() != "" && $(input).val() != "0") {
                            $(this).attr("albrequired", "O");
                        }
                        else {
                            $(this).removeAttr("albrequired");
                        }
                        break;
                }
            }
        });
    };

    this.initBehaviours = function () {
        $(":input[eventBehaviour]").each(function () {
            switch ($(this).attr("eventBehaviour")) {
                case "click":
                    $(this).click(function () {
                        infosSpe.applyBehaviours(this);
                    });
                    break;
                case "change":
                    $(this).change(function () {
                        infosSpe.applyBehaviours(this);
                    });
                    break;
                default:
                    break;
            }
        });
    };

    this.loadData = function () {
        function val(selector) {
            const j = $(selector);
            return j.length > 0 ? j.val() : "";
        }
        if ($("#OffreSimpContext").val() == "True") {
            return;
        }
        let etapeIs = $("#etapeIS").val();
        common.page.isLoading = true;
        common.$postJson(
            "/InformationsSpecifiquesRisques/GetISHtml",
            {
                affaireId: {
                    CodeAffaire: $("#Offre_CodeOffre").val(),
                    TypeAffaire: $("#Offre_Type").val(),
                    NumeroAliment: $("#Offre_Version").val(),
                    NumeroAvenant: $("#NumAvenantPage").val()
                },
                risque: val("#Code"), objet: val("#CodeObjet"),
                option: val("#CodeOption"),
                formule: val("#CodeFormule")
            },
            true).done(function (htmlText) {
                infosSpe.displayISHtml(htmlText, etapeIs);
            });
    };

    this.loadDataDbHtml = function () {
        if ($("#OffreSimpContext").val() == "True") {
            return;
        }
        let etapeIs = $("#etapeIS").val();
        let params = $("#Params").val();

        const dataParams = {
            modeNavig: $("#ModeNavig").val(),
            codeObjet: $("#CodeObjet").val() || "",
            codeRisque: $("#Code").val() || "",
            codeFormule: $("#CodeFormule").val() || "",
            codeOption: $("#CodeOption").val() || "",
            etapeIs: $("#etapeIS").val(),
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            branche: infosSpe.getParameterByName(params, "branche"),
            section: infosSpe.getParameterByName(params, "section"),
            cible: '',
            additionalParams: '',
            splitChars: $("#splitChar").val(),
            strParameters: $("#SpecificParams").val()
        };

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DynamicGuiIS/Ajax/DbInteraction.asmx/LoadDBData",
            data: JSON.stringify(dataParams),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d == '' && etapeIs != "Risque") {
                    infosSpe.triggerRedirect();
                } else {
                    if ($("#divBandeauIS").attr("id") != undefined)
                        $("#divBandeauIS").show();
                    $("#dvISGtiesResult").html(data.d);
                    if ($("table[name='albISTable']").attr("id") != undefined) {
                        if ($.trim($("table[name='albISTable']").html()) == "" && etapeIs != "Risque") {
                            infosSpe.triggerRedirect();
                        }
                    }
                    else {
                        //Init Data
                        infosSpe.init();
                        infosSpe.scrollToTopMenu();
                        CloseLoading();
                    }
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    this.displayISHtml = function (html, etapeIs) {
        if (html == "" && etapeIs != "Risque") {
            infosSpe.triggerRedirect();
        }
        else {
            console.log("IS loaded");
        }
        if ($("#divBandeauIS").attr("id") != undefined) {
            $("#divBandeauIS").show();
        }
        $("#dvISGtiesResult").html(html);

        if ($("table[name='albISTable']").attr("id") != undefined) {
            if ($.trim($("table[name='albISTable']").html()) == "" && etapeIs != "Risque") {
                infosSpe.triggerRedirect();
            }
        }
        else {
            //Init Data
            infosSpe.init();
            infosSpe.scrollToTopMenu();
            common.page.isLoading = false;
        }
    };

    // Les IS sont vides, passage à l'écran suivant
    this.triggerRedirect = function () {
        $("#ISRedirect").val("redirect");
        $("#btnSuivant").click();
        $("#btnAnnuler").attr("disabled", "disabled");
        $("#btnSuivant").attr("disabled", "disabled");
    };

    this.getParameterByName = function (query, name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(query);
        if (results == null) {
            return "";
        }
        else {
            return decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    };

    this.init = function () {
        infosSpe.formatDatePicker();
        infosSpe.initValidation();
        infosSpe.formatDecimalNumericValue();
        infosSpe.initBehaviours();
        infosSpe.start();
        //gestion de l'affichage de l'écran en mode readonly
        if (window.isReadonly || window.isModifHorsAvenant) {
            //var isAvenantModificationLocale = true;
            //if ($('#chkModificationAVN').length > 0) {
            //    isAvenantModificationLocale = $('#chkModificationAVN').is(':checked');
            //}
            // Desactivation de tous les input
            if (window.isModifHorsAvenant ) {
                $('input:not([albhorsavn])').attr('disabled', 'disabled');
            }
            else {
                $(":input").attr('disabled', 'disabled');
            }
            var isHorsAvnRegularisable = $("#IsHorsAvnRegularisable").val() === "True";

            if (window.isModifHorsAvenant && isHorsAvnRegularisable) {
                $("#IsRegularisable").attr('disabled', 'disabled');
            }

            //Sauf les boutons suivant/precedent
            $("#btnSuivant").removeAttr('disabled');
            $("#btnAnnuler").removeAttr('disabled');
            $("#btnConfirmOk").removeAttr('readonly');
            $("#btnConfirmOk").removeAttr('disabled');
            $("#btnConfirmCancel").removeAttr('readonly');
            $("#btnConfirmCancel").removeAttr('disabled');
            $("#btnInfoSpeRsq").removeAttr('disabled');
            $("#btnFermerISRsq").removeAttr('disabled');
            $("#btnFermerOppositions").removeAttr("disabled");
            $("#btnOppositions").removeAttr("disabled");

            if (window.isModifHorsAvenant) {
                $("#btnSuivantISRsq").show();
            }
            else {
                $("#btnAnnulerISRsq").html("<u>F</u>ermer").attr("data-accesskey", "f").removeAttr('disabled');
                $("#btnSuivantISRsq").hide();
            }
        }
        AffectDateFormat();
        RemoveReadOnlyAtt();
    };

    this.initValidation = function () {
        //$("#validation").click(function () {
        //    infosSpe.saveData();
        //});
    };

    this.getValues = function () {
        let result = "";
        let values = {};
        const sep = "||";
        $(":input[id^='map']").each(function () {
            const id = $(this).attr('id').substring(4);
            const $this = $(this);
            let val = $this.val();
            switch (this.type) {
                case "select-one":
                    if (val == null) {
                        val = "";
                    }
                    break;
                case "checkbox":
                    val = $this.is(":checked");
                    break;
                case "text":
                    if ($this.hasClass("datepicker")) {
                        const splitTab = val.split("/");
                        val = splitTab[2] + splitTab[1] + splitTab[0];
                    }
                    else {
                        if ($this.hasClass("decimal") || $this.hasClass("numerique")) {
                            if (val == "") {
                                val = 0;
                            } else {
                                val = val == "" ? 0 : $this.autoNumeric('get');
                            }
                        }
                    }
                case "textarea":
                case "hidden":
                default: break;
            }
            switch (this.type) {
                case "select-one":
                case "checkbox":
                case "text":
                case "textarea":
                case "hidden":
                    result += id + sep + val + $("#jsSplitChar").val();
                    let array = values[id];
                    if (array == null) {
                        array = [];
                    }
                    values[id] = array.concat([val]);
                    break;
                default:
                    break;
            }
        });

        return result;
    };

    this.resetValue = function (input) {
        if ($(input).is(":disabled")) {
            switch (input.type) {
                case "select-one":
                    $(input).val("");
                    break;
                case "checkbox":
                    $(input).removeAttr("checked");
                    break;
                case "text":
                    $(input).val("");
                    break;
                case "textarea":
                    $(input).html("");
                    break;
                default:
                    break;
            }
        }
    };

    this.saveData = function () {
        common.page.isLoading = true;
        try {
            if (infosSpe.validate()) {
                var valToSend = infosSpe.getValues();
                $.ajax({
                    type: "POST",
                    url: "/DynamicGuiIS/Ajax/DbInteraction.asmx/SaveData",
                    data: "{'branche':'" + encodeURIComponent($("#Branche").val()) +
                        "', 'section':'" + encodeURIComponent($("#section").val()) +
                        "', 'cible':'', 'additionalParams':'', 'dataToSave':'" + encodeURIComponent(valToSend) + "', 'splitChars':'" + encodeURIComponent($("#jsSplitChar").val()) + "','strParameters':'" + encodeURIComponent($("#parameters").val()) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    asynch: false,
                    success: function (data) {
                        var res = false;
                        if (data.d == "ok") {
                            common.page.isLoading = false;
                            res = true;
                        } else {
                            common.dialog.error("Erreur d'enregistrement de données");
                        }
                        return res;
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }
        } catch (e) {
            common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
            common.page.isLoading = false;
        }

        return false;
    };

    this.saveAjax = function (context) {
        //if (!this.validate()) {
        //    return false;
        //}
        const prefix = "map_";
        const suffixUnite = "_unt";
        let array = $("[id^='" + prefix + "']:not(:disabled)").toArray();
        let affaireId = {
            CodeAffaire: $("#Offre_CodeOffre").val(),
            NumeroAliment: $("#Offre_Version").val(),
            TypeAffaire: $("#Offre_Type").val(),
            NumeroAvenant: $("#NumAvenantPage").val()
        };
        let infosValeurs = array
            .filter(function (e) {
                return e.id.match(/_(unt|d2|h1|h2)$/ig) == null;
            })
            .map(function (e) {
                let a = e.id.split("_");
                a.shift();
                return {
                    AffaireId: affaireId,
                    NumeroRisque: context.risque,
                    NumeroObjet: context.objet || 0,
                    NumeroOption: context.option || 0,
                    NumeroFormule: context.formule || 0,
                    Cle: a.join("_"),
                    Valeur: {
                        Val1: e.type === "checkbox" ? e.checked ? "O" : "N" : common.autonumeric.getNumOrVal(e)
                    }
                };
            });

        infosValeurs.forEach(function (v) {
            let unite = array.first(function (x) { return x.id == prefix + v.Cle + suffixUnite; });
            let date2 = array.first(function (x) { return x.id == prefix + v.Cle + "_d2"; });
            let heure1 = array.first(function (x) { return x.id == prefix + v.Cle + "_h1"; });
            if (unite) {
                v.Valeur.Unite = $(unite).val();
            }
            if (date2) {
                let heure2 = array.first(function (x) { return x.id == prefix + v.Cle + "_h2"; });
                if (heure1) {
                    v.Valeur.Val1 = v.Valeur.Val1 ? (v.Valeur.Val1 + " " + ($(heure1).val() || "00:00:00")) : "";
                    v.Valeur.Val2 = $(date2).hasVal() ? ($(date2).val() + " " + ($(heure2).val() || "00:00:00")) : "";
                }
                else {
                    v.Valeur.Val2 = $(date2).val();
                }
            }
        });

        let infos = {
            Modele: { CodeBranche: context.branche, Section: context.section },
            Infos: infosValeurs
        };
        let controller = "InformationsSpecifiques" + context.section;
        if (context.section === "Garanties") {
            controller = "InformationsSpecifiquesGarantie";
        }

        if (context.section === "Entete") {
            controller = "InformationsSpecifiquesBranche";
        }

        window.mapElementsFieldNamesErrors = [];
        array.forEach(function (e) {
            let a = e.id.split("_");
            // removes 'map_'
            a.shift();
            if (e.id.match(/_(unt|d2|h1|h2)$/ig) != null) {
                // removes suffix for each extra element
                a.pop();
            }
            window.mapElementsFieldNamesErrors.push({ fieldname: a.join("_"), element: e.id });
        });
        return common.$postJson("/" + controller + "/SaveIS", { affaireId: affaireId, infos: infos, tabGuid: (context.guid || common.tabGuid) });
    };

    this.validate = function () {
        $(".requiredField").removeClass("requiredField");
        let result = true;
        $(":input[albrequired='O']").each(function () {
            if (!$(this).is(":disabled")) {
                if ($(this).hasClass("datepicker")) {
                    if (!infosSpe.checkDate($(this).val())) {
                        $(this).addClass("requiredField");
                        result = false;
                    }
                }

                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "0,00") {
                    $(this).addClass("requiredField");
                    result = false;
                }
            }
        });
        return result;
    };

    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };

    this.checkDate = function (date) {
        if (!isDate(date)) {
            return false;
        }
        return true;
    };

    //-------Formate les input/span des valeurs----------
    this.formatDecimalNumericValue = function () {

        $('input[albMask="numeric"]').each(function (e) {
            var maxValue = null;
            if ($(this).attr("albEntier") != undefined) {
                maxValue = Math.pow(10, $(this).attr("albEntier")) - 1;
            }
            common.autonumeric.apply($(this), 'init', $(this).attr("albMask"), null, null, 0, maxValue, null);
        });

        $('input[albMask="decimal"]').each(function (e) {

            var maxValue = null;

            if ($(this).attr("albEntier") != undefined) {
                maxValue = Math.pow(10, $(this).attr("albEntier")) - 1;
            }

            if (($(this).attr("albEntier") != undefined) && ($(this).attr("albDecimal") != undefined)) {
                maxValue += 1 - Math.pow(10, -$(this).attr("albDecimal"));
            }
            common.autonumeric.apply($(this), 'init', $(this).attr("albMask"), null, null, $(this).attr("albDecimal"), maxValue, null);

        });
    };

    this.initBtnISVAL = function () {
        $("#btnInitISVAL").kclick(function () {
            infosSpe.initISVALCs();
        });
    };

    this.initISVALJs = function () {
        try {
            $(this).disable();
            $("<button type=button id=cancelInitISAVL>Stop Init ISVAL</button>").appendTo($(this).parent());
            $("#cancelInitISAVL").kclick(function () {
                setTimeout(function () { infosSpe.cancelISVAL(); });
            });
            infosSpe.initKpisvalFromKPIRS();
        }
        catch (e) {
            console.error(e);
            infosSpe.cancelISVAL();
        }
    };

    this.initISVALCs = function () {
        try {
            const uuid = common.tools.createUUID();
            $("#btnInitISVAL").disable();
            $("#initISValHisto").disable();
            $("<button type=button id=cancelInitISAVL>Stop Init ISVAL</button>").appendTo($("#btnInitISVAL").parent());
            $("#cancelInitISAVL").kclick(function () {
                setTimeout(function () {
                    try {
                        common.page.isLoading = true;
                        common.$postJson("/InformationsSpecifiquesRisques/CancelInitISVAL", { initGuid: uuid }, true).done(function () {
                            console.log("init ended");
                            common.page.isLoading = false;
                            $("#cancelInitISAVL").remove();
                            $("#btnInitISVAL").enable();
                            $("#initISValHisto").enable();
                        }).fail(function (xhr) {
                            common.error.show(xhr);
                        });
                    }
                    catch (e) {
                        common.page.isLoading = false;
                    }
                });
            });
            setTimeout(function () {
                common.$postJson("/InformationsSpecifiquesRisques/InitISVAL", { initGuid: uuid, nbAffaires: 3000, fromHisto: $("#initISValHisto").isChecked() }, true)
                    .done(function (result) {
                        $("#cancelInitISAVL").click();
                        if (Array.isArray(result) && result.length > 0) {
                            console.error(result.join("\n", result));
                        }
                    })
                    .fail(function (xhr) {
                        $("#cancelInitISAVL").click();
                        common.error.showXhr(xhr);
                    });
            }, 10);
        }
        catch (e) {
            console.error(e);
            $("#cancelInitISAVL").remove();
            $("#btnInitISVAL").enable();
            $("#initISValHisto").enable();
        }
    };

    const eventAbort = "KPIRS.abort";
    this.cancelISVAL = function () {
        $(document).trigger(eventAbort);
        $(document).off("click", "#cancelInitISAVL");
        $("#cancelInitISAVL").remove();
        $("#btnInitISVAL").enable();
    };

    this.initKpisvalFromKPIRS = function () {
        const eventStart = "KPIRS.start";
        const eventSaved = "KPIRS.saved";
        const eventFailed = "KPIRS.failed";
        const tempDivId = "tempContentIS";
        common.$postJson("/InformationsSpecifiquesRisques/GetOldISList", {}, true).done(function (list) {
            if (Array.isArray(list) && list.length > 0) {
                $("<div style='display: none;' id=" + tempDivId + "></div>").appendTo(window.document.body);
                $(document).on([eventStart, eventFailed, eventSaved].join(" "), function () {
                    if (list.length === 0) {
                        infosSpe.cancelISVAL();
                        return;
                    }
                    $("#" + tempDivId).clearHtml();
                    const item = list.pop();
                    try {
                        loadOlkdISData({
                            item: item,
                            branche: item.section.Branche,
                            ipb: item.affaire.CodeAffaire,
                            alx: item.affaire.NumeroAliment,
                            typ: item.affaire.TypeAffaire === 1 ? "P" : "O",
                            risque: item.section.NumeroRisque,
                            objet: item.section.NumeroObjet,
                            option: item.section.NumeroOption,
                            formule: item.section.NumeroFormule,
                            etape: item.section.Type == 1 ? "Risque" : item.section.Type == 2 ? "Objet" : "Garantie",
                            section: item.section.Type == 1 ? "Risques" : item.section.Type == 2 ? "Objets" : "Garanties"
                        });
                    }
                    catch (e) {
                        console.error("Load IS Failed");
                        console.error(e);
                    }
                });
                $(document).on(eventAbort, function () {
                    $(document).off([eventStart, eventFailed, eventSaved].join(" "));
                    $("#" + tempDivId).remove();
                    $("#Offre_CodeOffre").clear();
                    $("#Offre_Version").clear();
                    $("#Offre_Type").clear();
                    $("#NumAvenantPage").clear();
                    common.page.isLoading = false;
                });
                setTimeout(function () {
                    $(document).trigger(eventStart);
                });
            }
        });

        function loadOlkdISData(context) {
            let controller = "InformationsSpecifiques" + context.section;
            if (context.section === "Garanties") {
                controller = "InformationsSpecifiquesGarantie";
            }
            if (context.section === "Entete") {
                controller = "InformationsSpecifiquesBranche";
            }
            const sep = "#**#";
            let p = [context.typ, context.ipb, context.alx];
            if (context.section.toLowerCase() === "objets") {
                p.push(context.risque);
                p.push(context.objet);
            }
            else if (context.section.toLowerCase() === "garanties") {
                p.push(context.formule);
                p.push(context.option);
            }
            else {
                p.push(context.risque);
            }
            const dataParams = {
                modeNavig: "S",
                codeObjet: context.objet,
                codeRisque: context.risque,
                codeFormule: context.formule,
                codeOption: context.option,
                etapeIs: context.etape,
                codeOffre: context.ipb,
                version: context.alx,
                type: context.typ,
                branche: context.branche,
                section: context.section,
                cible: "",
                additionalParams: "",
                splitChars: sep,
                strParameters: p.join(sep)
            };
            const modele = context.branche + "-" + context.section;
            context.guid = "{2EF60657-76BD-4730-AA10-66B17E347394}";
            common.$postJson("/DynamicGuiIS/Ajax/DbInteraction.asmx/LoadDBData", dataParams)
                .done(function (data) {
                    try {
                        if (data.d) {
                            console.log(context.ipb + " " + context.alx + " IS loaded (" + context.section + ")");
                            $("#" + tempDivId).html(data.d);
                            infosSpe.formatDecimalNumericValue();
                            infosSpe.formatDatePicker();
                            setTimeout(function () {
                                $("#Offre_CodeOffre").val(context.ipb);
                                $("#Offre_Version").val(context.alx);
                                $("#Offre_Type").val(context.typ);
                                $("#NumAvenantPage").clear();
                                let request = infosSpe.saveAjax(context);
                                if (!request) {
                                    common.$postJson(
                                        "/" + controller + "/LogOldISState",
                                        {
                                            affaireId: context.item.affaire,
                                            section: context.item.section,
                                            commentaires: modele + "-ERROR(formulaire invalide)"
                                        }).done();
                                    setTimeout(function () { $(document).trigger(eventFailed); }, 7);
                                    return;
                                }
                                request
                                    .done(function () {
                                        setTimeout(function () {
                                            common.$postJson(
                                                "/" + controller + "/LogOldISState",
                                                {
                                                    affaireId: context.item.affaire,
                                                    section: context.item.section,
                                                    commentaires: modele + "-OK"
                                                }).done();
                                            $(document).trigger(eventSaved);
                                        }, 7);
                                    })
                                    .fail(function (x, s, m) {
                                        let msg = null;
                                        if (x.responseText) {
                                            try {
                                                msg = JSON.parse(x.responseText);
                                            } catch (e) { }
                                        }
                                        if (msg instanceof Object && kheops.errors.isCustomError(msg)) {
                                            msg = kheops.errors.display(msg, true, true);
                                        }
                                        else {
                                            msg = m;
                                        }
                                        common.$postJson(
                                            "/" + controller + "/LogOldISState",
                                            {
                                                affaireId: context.item.affaire,
                                                section: context.item.section,
                                                commentaires: modele + "-ERROR(" + msg + ")"
                                            }).done();
                                        setTimeout(function () { $(document).trigger(eventFailed); }, 7);
                                    });
                            }, 10);
                        }
                        else {
                            console.log("No IS for " + context.ipb + " " + context.alx + " (" + context.section + ")");
                            common.$postJson(
                                "/" + controller + "/LogOldISState",
                                {
                                    affaireId: context.item.affaire,
                                    section: context.item.section,
                                    commentaires: modele + "-NO_IS"
                                }).done();
                            setTimeout(function () { $(document).trigger(eventSaved); }, 7);
                        }
                    }
                    catch (er) {
                        console.error(m);
                        console.error(JSON.stringify(context));
                        $(document).trigger(eventFailed);
                    }
                })
                .fail(function (x, s, m) {
                    console.error(m);
                    console.error(JSON.stringify(context));
                    $(document).trigger(eventFailed);
                });
        }
    };
};

var infosSpe = new InfosSpe();

$(function () {
    if (common.page.controller.toLowerCase() === "recherchesaisie") {
        infosSpe.initBtnISVAL();
        return;
    }
    infosSpe.formatDecimalNumericValue();
    if ($("#executeLoadIs").hasTrueVal()) {
        infosSpe.load();
    }
});
