
var Engagements = function () {
    const lciGen = "LCIGenerale";
    //----------------Map les éléments de la page------------------
    this.initPage = function () {
        toggleDescription($("#CommentForce"), true);
        $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));

        $("#Valeur_LCIGenerale").addClass("numerique");

        $("#Type_LCIGenerale, #IsIndexe_LCIGenerale").change(function () {
            engagements.toggleComputeButtons();
        });
        $("#PartAlb").change(function () {
            engagements.toggleComputeButtons();
            $("#fieldInput").val("PART");
        });

        if ($("#Nature").val() === "") {
            $("#Nature").val("100%");
        }
        $("#btnAnnuler").kclick(function () {
            var iframe = $("#accessMode").val() !== "";
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true, true, iframe);
        });

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "DelExpression":
                    DeleteExpression();
                    CloseCommonFancy();
                    $("#hiddenInputId").val("");
                    break;
                case "Cancel":
                    if ($("#codePeriode").val() === "") {
                        engagements.redirect("MatriceRisque", "Index");
                    }
                    else {
                        engagements.redirect("EngagementPeriodes", "Index");
                    }
                    break;
            }
            $("#hiddenAction").clear();
        });

        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });

        $("#LCIGenerale").offOn("change", function () {
            engagements.lockUnlockLCI($(this).isChecked());
            common.autonumeric.apply($("#Valeur_LCIGenerale"), "destroy");
            common.autonumeric.apply($("#Valeur_LCIGenerale"), "init", "decimal", " ", null, null, "99999999999.99", null);
        });

        AlternanceLigne("Traite", "noInput", true, null);
        $("#btnRefresh").kclick(function () {
            if ($("#divRefresh").hasClass("CursorPointer")) {
                if (!($("#Valeur_LCIGenerale").isEmpty()) && ($("#Unite_LCIGenerale").val() == 'D') ) {
                    engagements.computeGareatEng();
                }
                engagements.reloadPage();
            
            }
            else {
                if ($("#txtSaveCancel").val() === "1") {
                    engagements.redirect("RechercheSaisie", "Index");
                }
            }
        });
        $("#btnReset").kclick(function () {
            if ($("#divReset").hasClass("CursorPointer")) {
                engagements.redirect("Engagements", "Index");
            }
        });

        engagements.initLinksTraites();
        engagements.formatDecimalValue();
        var modeAcess = $("#accessMode").val();
        MapElementLCIFranchise("LCI", "Generale", $("#NomEcran").val(), modeAcess);

        if (window.isReadonly) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        if ($("#ActeGestion").val() === "AVNMD" || $("#ActeGestion").val() === "AVNRS") {
            $("#Valeur_LCIGenerale").attr("readonly", "readonly").addClass("readonly");
            $("#Unite_LCIGenerale").attr("disabled", "disabled");
            $("#Type_LCIGenerale").attr("disabled", "disabled");
            $("#PartAlb").attr("readonly", "readonly");
        }

        engagements.initSuivant();
        formatDatePicker();
    };

    //-------Formate les input/span des valeurs----------
    this.formatDecimalValue = function () {
        common.autonumeric.applyAll("init", "decimal", " ", null, null, "99999999999.99", null);
        common.autonumeric.applyAll("init", "pourcentdecimal", "");
    };

    //----------------Redirection------------------
    this.redirect = function (cible, job, traite) {
        if (traite == undefined) {
            traite = "";
        }

        common.page.isLoading = true;
        const postedData = {
            cible: cible, job: job,
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), numAvn: $("#NumAvenantPage").val(),
            argModelCommentForce: $("#CommentForce").val(), tabGuid: common.tabGuid, modeNavig: $("#ModeNavig").val(),
            addParamType: $("#AddParamType").val(), addParamValue: $("#AddParamValue").val(),
            traite: traite, codePeriode: $("#codePeriode").val(), accessMode: $("#accessMode").val(), actionEngagement: $("#CurrentAccessMode").val()
        };
        common.$postJson("/Engagements/Redirection", postedData, true).done();
    };

    //-------------Ouvre le traite---------------
    this.initLinksTraites = function () {
        $("tr[name^=traite]").kclick(function () {
            engagements.redirect("EngagementTraite", "Index", this.id.split("_")[1]);
        });
    };

    //--------------Change l'état des boutons------------------
    this.toggleComputeButtons = function () {
        $("#divRefresh").addClass("CursorPointer");
        $("#btnRefresh").attr("src", "/Content/Images/boutonRefresh_3232.png");
        $("#btnSuivant").disable();
        $("#fieldInput").val("MAJ");
    };

    //-------------Rafraichit la page avec les nouvelles données----------
    this.reloadPage = function () {
        if ($("#fieldInput").val() !== "") {
            $(".requiredField").removeClass("requiredField");
            let codeOffre = $("#Offre_CodeOffre").val();
            let version = $("#Offre_Version").val();
            let type = $("#Offre_Type").val();
            let field = $("#fieldInput").val();
            let tabGuid = common.tabGuid;
            let modeNavig = $("#ModeNavig").val();
            let argEngagement = JSON.stringify($("#dataEngagement").find(":input").serializeObject());
            let acteGestion = $("#ActeGestion").val();
            let codeAvn = $("#NumAvenantPage").val();
            let codePeriode = $("#codePeriode").val();
            let argModelCommentForce = $("#CommentForce").val();//JSON.stringify(modelCommentForce);
            let valeurLCI = $("#Valeur_LCIGenerale").val();
            let uniteLCI = $("#Unite_LCIGenerale").val();
            let typeLCI = $("#Type_LCIGenerale").val();
            let isIndexeeLCI = $("#IsIndexe_LCIGenerale").is(":checked");
            let lienCpxLCI = $("#idLienCpx_LCIGenerale").val();
            let error = false;

            if (!ValidateLCIFranchise("LCI", "Generale")) {
                error = true;
            }
            if (!checkDate($("#DateDeb"), $("#DateFin")) && !$("#DateDeb").is(":disabled") && !$("#DateFin").is(":disabled")) {
                $("#DateDeb").addClass("requiredField");
                $("#DateFin").addClass("requiredField");
                error = true;
            }
            else {
                $("#DateDeb").removeClass("requiredField");
                $("#DateFin").removeClass("requiredField");
            }

            if (error) {
                return;
            }

            common.page.isLoading = true;
            if ($("#txtParamRedirect").val() !== "" && $("#txtParamRedirect").val() != undefined) {
                $.ajax({
                    type: "POST",
                    url: "/Engagements/UpdateRedirect/",
                    data: {
                        codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, argEngagement: argEngagement, argModelCommentForce: argModelCommentForce, field: field, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(),
                        valeurLCI: valeurLCI, uniteLCI: uniteLCI, typeLCI: typeLCI, isIndexeeLCI: isIndexeeLCI, lienCpxLCI: lienCpxLCI, modeNavig: modeNavig, acteGestion: acteGestion, codePeriode: codePeriode, accessMode: $("#accessMode").val()
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });

            } else {
                $.ajax({
                    type: "POST",
                    url: "/Engagements/UpdateInPage/",
                    context: $("#divEngagement"),
                    data: {
                        codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, argEngagement: argEngagement, argModelCommentForce: argModelCommentForce, field: field, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(),
                        valeurLCI: valeurLCI, uniteLCI: uniteLCI, typeLCI: typeLCI, isIndexeeLCI: isIndexeeLCI, lienCpxLCI: lienCpxLCI, modeNavig: modeNavig, acteGestion: acteGestion, codePeriode: codePeriode, accessMode: $("#accessMode").val()
                    },
                    success: function (data) {
                        $(this).html(data);
                        AlternanceLigne("Traite", "", false, null);
                        engagements.initLinksTraites();
                        toggleDescription($("#CommentForce"), true);
                        engagements.formatDecimalValue();
                        if ($("#Nature").val() === "")
                            $("#Nature").val("100%");

                        common.page.isLoading = false;
                        formatDatePicker();

                        MapElementLCIFranchise("LCI", "Generale", $("#NomEcran").val());
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }

        }
    };
    
    //-------------Bloque/Débloque les contrôles LCI--------------
    this.lockUnlockLCI = function (val) {
        if (val) {
            $("#Valeur_LCIGenerale").removeClass("readonly").removeAttr("readonly");
            $("#Unite_LCIGenerale").removeClass("readonly").removeAttr("disabled");
            $("#Type_LCIGenerale").removeClass("readonly").removeAttr("disabled");
            $("#IsIndexe_LCIGenerale").removeClass("readonly").removeAttr("disabled");
        }
        else {
            $("#Valeur_LCIGenerale").addClass("readonly").attr("readonly", "readonly");
            $("#Unite_LCIGenerale").addClass("readonly").attr("disabled", "disabled");
            $("#Type_LCIGenerale").addClass("readonly").attr("disabled", "disabled");
            $("#IsIndexe_LCIGenerale").addClass("readonly").attr("disabled", "disabled");
        }
    };
    //------------GAREAT---------------------------------------------------------
    this.computeGareatEng = function () {

        let unitLci = $("#Unite_" + lciGen).val();
        let lci = unitLci === "D" ? getValue($("#Valeur_" + lciGen)) : 0;
        let lignesGaranties = $("#tblConditions tr[data-gareat='True']");
        let model = {
            CodePolicePage: $("#CodePolicePage").val(),
            TypePolicePage: $("#TypePolicePage").val(),
            VersionPolicePage: $("#VersionPolicePage").val(),
            NumAvenantPage: $("#NumAvenantPage").val(),
            CodeRisque: $("#CodeRisque").val(),
            ModeNavig: $("#ModeNavig").val(),
            InformationsContrat: {
                LCIGenerale: {
                    Valeur: lci ? $("#Valeur_LCIGenerale").val() : ""
                }
            },
            InformationsCondition: {
                ListGaranties: lignesGaranties.toArray().map(function (e) {
                    let $unite = $(e).find("select[id^='TauxForfaitHTUnite']");
                    if (!$unite.hasVal()) {
                        return { PrimeGareat: null, IsAttentatGareat: true };
                    }

                    let $v = $(e).find("[id^='TauxForfaitHTValeur']");
                    let $assiette = $(e).find("[id^='AssietteValeur']");
                    let $assietteUnite = $(e).find("[id^='AssietteUnite']");
                    let valeur = parseFloat($(e).find("[id^='TauxForfaitHTMinimum']").val());
                    if (isNaN(valeur)) {
                        valeur = null;
                    }
                    if ($unite.val() === "D") {
                        let x = getValue($v);
                        valeur = valeur > x ? valeur : x;
                    }
                    else if ($assiette.val() && $assietteUnite.val() === "D") {
                        valeur = (getValue($assiette) || 0) * (((getValue($v) / ($unite.val() === "%0" ? 1000 : 100))) || 0);
                    }
                    return { PrimeGareat: valeur, IsAttentatGareat: true };
                }),

                LstRisque: {

                }
            },
            InfosGareat: {
                TauxTranche: 0,
                TauxFraisGeneraux: 0,
                TauxCommissions: 0,
                TauxRetenu: 0,
                Prime: 0
            }
        };

        $("[id$='_gareat']").toArray().forEach(function (e) {
            let prop = e.id.replace("_gareat", "");
            model.InfosGareat[prop] = $(e).val();
        });

        let inputsToDisable = $("[id$='_gareat']:not(:disabled)");
        inputsToDisable.disable();
        common.page.isLoading = true;
        try {
            common.$postJson("/ConditionsGarantie/ComputeGareatEng", model, true)
                .done(function (result) {
                    $("#Tranche_gareat").val(result.Prime === 0 ? "" : result.Tranche);
                    $("#TauxRetenu_gareat").autoNumeric("set", result.Prime === 0 ? "" : result.TauxRetenu);
                    $("#PrimeTheorique_gareat").autoNumeric("set", result.Prime === 0 ? "" : result.Prime);
                    if (!$("#PrimeTheorique_gareat").hasVal()) {
                        $("#Prime_gareat").disable();
                        if ($("#Prime_gareat").val() != 0) {
                            $("#Prime_gareat").autoNumeric("set", "");
                        }
                    }
                    else {
                        $("#Prime_gareat").enable();
                    }

                    common.page.isLoading = false;
                }).always(function () {
                    inputsToDisable.filter(":not(#Prime_gareat)").enable();
                });
        }
        catch (e) {
            inputsToDisable.enable();
            common.page.isLoading = false;
            throw e;
        }

        function getValue($e) {
            let val = parseFloat($e.data("autoNumeric").rawValue);
            return isNaN(val) || val == 0 ? null : val;
        }
    };
    //------------Suivant--------------
    this.initSuivant = function () {
        $("#btnSuivant").kclick(function (evt, data) {
            if (!$(this).attr("disabled")) {
                if (!ValidateLCIFranchise("LCI", "Generale")) {
                    return;
                }

                common.page.isLoading = true;
                const postedData = {
                    codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), tabGuid: common.tabGuid,
                    argModelCommentForce: $("#CommentForce").val(),
                    paramRedirect: $("#txtParamRedirect").val(), modeNavig: $("#ModeNavig").val(),
                    addParamType: $("#AddParamType").val(), addParamValue: $("#AddParamValue").val(),
                    codePeriode: $("#codePeriode").val(), accessMode: $("#accessMode").val(), actionEngagement: $("#CurrentAccessMode").val()
                };
                common.$postJson("/Engagements/RedirectionSuivant" + (data && data.returnHome ? "?returnHome=1" : ""), postedData, true).done();
            }
        });
    };
    
    //----------------------Formate tous les controles datepicker---------------------
    this.formatDatePicker = function () {
        $("#DateDeb").datepicker({ dateFormat: "dd/mm/yy" });
        $("#DateFin").datepicker({ dateFormat: "dd/mm/yy" });
    };
}

var engagements = new Engagements();

var LoadEngagementsJS = function () {
    engagements.initPage();
};
