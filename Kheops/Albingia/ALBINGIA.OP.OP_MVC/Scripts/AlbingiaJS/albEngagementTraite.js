
var EngagementsTraites = function () {
    const lciGen = "LCIGenerale";
    this.initPage = function () {

        this.ISchange = function (e) {
            e = e || window.event;
            if ((e.keyCode > 95 && e.keyCode < 104) || (e.keyCode == 8) || (e.keyCode == 46)) {
                engagementsTraites.lockInputsLCI();
                engagementsTraites.toggleComputeButton();
                if ($("#Nb").val() == 0) {
                    $("#Nb").val("1");
                }
                else {
                    engagementsTraites.SMPRetenue();
                }
            }

        }
        if (!($("#Valeur_LCIGenerale").isEmpty()) && !($("#Unite_LCIGenerale").isEmpty()) && !($("#Type_LCIGenerale").isEmpty())
            || $("#Unite_LCIGenerale").val() == "CPX") {
            var tableau = document.getElementById("tblTraiteEngagementInfo");
            for (var i = 1; i < tableau.rows[2].cells.length; i++) {

                tableau.rows[2].cells[i].querySelector('input').disabled = true;

            }

        }
        toggleDescription($("#CommentForce"), true);
        $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));

        $("#Valeur_LCIGenerale").addClass("numerique");

        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

        $("#btnSuivant:not(:disabled)").kclick(function (evt, data) {
            let errorMsg = "";

            if (!window.isReadonly) {
                //Contrôle des SMP forcés avec les engagements calculés et la LCI générale
                let engCalcTotal = 0;
                $("input[name='engagementCalc']").each(function () {
                    engCalcTotal += parseInt($(this).val());
                });
                let engForceTotal = parseFloat($("#EngagementTotal").autoNumeric('get'));
                let lciGenerale = parseFloat($("#Valeur_LCIGenerale").val() != "" ? $("#Valeur_LCIGenerale").autoNumeric('get') : 0);
                if (engForceTotal > engCalcTotal || (engForceTotal > lciGenerale && lciGenerale > 0 && $("#Unite_LCIGenerale").val() == "D")) {
                    errorMsg = "Attention, SMP supérieur aux capitaux / à la LCI, veuillez vérifier votre saisie.<br/>Voulez-vous continuer ?<br/>";
                }

                if (errorMsg == "") {
                    //Contrôle de la saisie de tous les SMP
                    $("#SMPRetenue").each(function () {
                        let smpForce = $(this).val() != "" ? parseInt($(this).val()) : 0;
                        if (smpForce == 0) {
                            errorMsg = "Attention, vous n’avez pas effectué la saisie du SMP.<br/>Voulez-vous continuer ?<br/>";
                        }
                    });
                }
            }

            if (errorMsg != "") {
                ShowCommonFancy("Confirm", "Suivant",
                    errorMsg,
                    350, 130, true, true, true, !$("#accessMode").isEmpty());
            }
            else {
                common.page.isLoading = true;
                engagementsTraites.redirect(data && data.returnHome ? "RechercheSaisie" : "Engagements", "Index");
            }
        });

        $("#btnAnnuler").kclick(function () {
            if (window.isReadonly) {
                engagementsTraites.cancel();
            }
            else {
                ShowCommonFancy("Confirm", "Cancel",
                    "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                    350, 130, true, true, true, !$("#accessMode").isEmpty());
            }
        });

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    engagementsTraites.cancel();
                    break;
                case "Suivant":
                    common.page.isLoading = true;
                    engagementsTraites.redirect("Engagements", "Index");
                    //engagementsTraites.redirect(data && data.returnHome ? "RechercheSaisie" : "Engagements", "Index");
                    break;
            }
            $("#hiddenAction").clear();
        });

        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });

        if (window.isReadonly) {
            $("#btnSuivant").removeAttr("data-accesskey").hide();
            $("#btnAnnuler").html("<u>P</u>récédent").assignAccessKey("p");
        }
        else {
            $("#btnRefresh").kclick(function () {

                if (!($("#Valeur_LCIGenerale").isEmpty()) && ($("#Unite_LCIGenerale").val() == 'D') && ($("#fieldInput").val() == 'MAJ')) {
                    engagementsTraites.computeGareatEng();
                }
                if ($("#divRefresh").hasClass("CursorPointer")) {
                    //engagementsTraites.ModifModelTraite();
                    common.page.isLoading = false;
                    engagementsTraites.reloadPage();
                }
                else {
                    if ($("#txtSaveCancel").val() == "1") {
                        engagementsTraites.redirect("RechercheSaisie", "Index");
                    }
                }
            });

            $("#btnReset").kclick(function () {
                if ($("#divReset").hasClass("CursorPointer")) {
                    engagementsTraites.redirect("EngagementTraite", "Index");
                }
            });

            $("#IsIndexe_LCIGenerale, #Type_LCIGenerale, #idLienCpx_LCIGenerale").offOn("change", function () {
                engagementsTraites.toggleComputeButton();
                engagementsTraites.lockInputsNonLCI();
            });

            $("#tblTraiteInfoList input[name='smp'], #SMPRetenue").offOn("change", function () {
                engagementsTraites.lockInputsLCI();
                engagementsTraites.toggleComputeButton();
            });
        }

        AlternanceLigne("TraiteInfoList", "", false, null);
        AlternanceLigne("TraiteEngagementInfo", "", false, null);
        engagementsTraites.formatDecimalNumeric();
        MapElementLCIFranchise("LCI", "Generale", $("#NomEcran").val());

    };
    //ach
    this.formatDecimalNumeric = function () {
        common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '9999999999999', '0');
        common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
        common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    };

    this.cancel = function () {
        engagementsTraites.redirect("Engagements", "Index", "", true);
    };

    this.redirect = function (cible, job, codeSMP, cancelling) {
        common.page.isLoading = true;
        $("#zoneTxtArea").removeClass('requiredField');
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let tabGuid = common.tabGuid;
        let modeNavig = $("#ModeNavig").val();
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let codePeriodeEng = $("#CodePeriodeEng").val();

        //contrôle des champs forcés
        let isForce = false;
        $("input[name^='EngagementVoletForce']").each(function () {
            if ($(this).val() != "" && $(this).val() != "0") {
                isForce = true;
            }
        });

        let argModelCommentForce = $("#CommentForce").html() || $("#CommentForce").val();
        if (argModelCommentForce == "" && isForce && cancelling == undefined) {
            var iframe = $("#accessMode").val() != "";
            $("#zoneTxtArea").addClass('requiredField');
            ShowCommonFancy("Error", "", "Commentaire obligatoire", 300, 80, true, true, true, iframe);
            common.page.isLoading = false;
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/EngagementTraite/Redirection",
            data: {
                cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, argModelCommentForce: argModelCommentForce,
                codeSMP: codeSMP, codeTraite: $("#CodeTraite").val(), tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig,
                addParamType: addParamType, addParamValue: addParamValue, codePeriodeEng: codePeriodeEng, accessMode: $("#accessMode").val(), actionEngagement: $("#CurrentAccessMode").val()
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    this.lockInputsLCI = function () {
        $("input[type=text][id*=LCIGenerale]").makeReadonly(true);
        $("input[type=checkbox][id*=LCIGenerale]").disable(true);
        $("#Unite_LCIGenerale").disable(true);
        $("#Type_LCIGenerale").disable(true);

    };
    this.SMPRetenue = function () {
        var tableau = document.getElementById("tblTraiteEngagementInfo");
        for (var i = 1; i < tableau.rows[2].cells.length; i++) {
            if (tableau.rows[2].cells[i].querySelector('input').value.replace(/ /g, "") == tableau.rows[2].cells[i].querySelector('input').name.split("-")[1]) {
                tableau.rows[2].cells[i].querySelector('input').classList.add("readonly");
                tableau.rows[2].cells[i].querySelector('input').disabled = true;
            }
            else {
                tableau.rows[2].cells[i].querySelector('input').classList.remove("readonly");
                tableau.rows[2].cells[i].querySelector('input').disabled = false;
            }

        }

    };
    this.lockInputsNonLCI = function () {
        $("input[type=text]").not("input[id*=LCIGenerale]").makeReadonly(true);
        $("input[type=checkbox]").not("input[id*=LCIGenerale]").disable(true);
    };

    this.toggleComputeButton = function () {
        $("#divRefresh").addClass("CursorPointer");
        $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
        $("#btnSuivant").attr('disabled', 'disabled');
        $("#fieldInput").val("MAJ");
    };

    this.reloadPage = function () {


        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let field = $("#fieldInput").val();
        let tabGuid = common.tabGuid;
        let modeNavig = $("#ModeNavig").val();
        let codeAvn = $("#NumAvenantPage").val();
        let argEngagementTraite = "[";

        // LCI générale
        argEngagementTraite += "{ ";
        if ($("#Valeur_LCIGenerale").isEmpty()) {
            argEngagementTraite += 'LCIValeur:"",';
        }
        else {
            argEngagementTraite += 'LCIValeur:"' + $("#Valeur_LCIGenerale").val() + '",';
        }
        if ($("#Unite_LCIGenerale").isEmpty()) {
            argEngagementTraite += 'LCIUnite:"",';
        }
        else {
            argEngagementTraite += 'LCIUnite:"' + $("#Unite_LCIGenerale").val() + '",';
        }
        if ($("#Type_LCIGenerale").isEmpty()) {
            argEngagementTraite += 'LCIType:"",';
        }
        else {
            argEngagementTraite += 'LCIType:"' + $("#Type_LCIGenerale").val() + '",';
        }
        if ($("#IsIndexe_LCIGenerale").isEmpty()) {
            argEngagementTraite += 'LCIIndexee:""';
        }
        else {
            argEngagementTraite += 'LCIIndexee:' + $("#IsIndexe_LCIGenerale").is(':checked') + '';
        }
        argEngagementTraite += " },";

        $("#divTraiteInfoList :input[type=hidden][id^=hidden]").each(function () {
            if ($(this).val() == "1") {
                var endId = $(this).attr('id').replace('hidden', '');
                argEngagementTraite += "{ ";

                if ($("#Valeur_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIValeur:"",';
                }
                else {
                    argEngagementTraite += 'LCIValeur:"' + $("#Valeur_LCIGenerale").autoNumeric('get') + '",';
                }
                if ($("#Unite_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIUnite:"",';
                }
                else {
                    argEngagementTraite += 'LCIUnite:"' + $("#Unite_LCIGenerale").val() + '",';
                }
                if ($("#Type_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIType:"",';
                }
                else {
                    argEngagementTraite += 'LCIType:"' + $("#Type_LCIGenerale").val() + '",';
                }
                if ($("#IsIndexe_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIIndexee:"",';
                }
                else {
                    argEngagementTraite += 'LCIIndexee:' + $("#IsIndexe_LCIGenerale").is(':checked') + ',';
                }

                argEngagementTraite += 'CodeRisque:"' + $("#codeRisque" + endId).val() + '",';
                argEngagementTraite += 'IdVentilation:"' + $("#codeVolet" + endId).val() + '",';
                argEngagementTraite += 'ValueRsq:"' + $("#force" + endId).autoNumeric('get') + '",';
                argEngagementTraite += 'SelectedRsq:' + $("#rsqSel" + endId).is(':checked') + ', CodeVolet:"", ValueVol:"", SelectedVol:""';

                argEngagementTraite += " },";
            }

        });

        $("#divTraiteEngagementInfo :input[type=hidden][id^=hidden]").each(function () {
            if ($(this).val() == "1") {
                var endId = $(this).attr('id').replace('hidden', '');
                argEngagementTraite += "{ ";

                if ($("#Valeur_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIValeur:"",';
                }
                else {
                    argEngagementTraite += 'LCIValeur:"' + $("#Valeur_LCIGenerale").autoNumeric('get') + '",';
                }
                if ($("#Unite_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIUnite:"",';
                }
                else {
                    argEngagementTraite += 'LCIUnite:"' + $("#Unite_LCIGenerale").val() + '",';
                }
                if ($("#Type_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIType:"",';
                }
                else {
                    argEngagementTraite += 'LCIType:"' + $("#Type_LCIGenerale").val() + '",';
                }
                if ($("#IsIndexe_LCIGenerale").isEmpty()) {
                    argEngagementTraite += 'LCIIndexee:"",';
                }
                else {
                    argEngagementTraite += 'LCIIndexee:"' + $("#IsIndexe_LCIGenerale").val() + '",';
                }

                argEngagementTraite += 'CodeRisque:"", CodeRsqVentilation:"", ValueRsq:"", SelectedRsq:"",';

                argEngagementTraite += 'IdVentilation:"' + $("#codeVolet" + endId).val() + '",';
                argEngagementTraite += 'ValueVen:"' + $("#EngagementVoletForce" + endId).autoNumeric('get') + '",';
                argEngagementTraite += 'SelectedVen:' + $("#volSel" + endId).is(':checked');

                argEngagementTraite += " },";
            }
        });

        argEngagementTraite = argEngagementTraite.substr(0, argEngagementTraite.length - 1) + "]";
        if (argEngagementTraite == "]") {
            argEngagementTraite = "";
        }
        let argModelCommentForce = $("#CommentForce").val();
        let error = false;
        $(".requiredField").removeClass('requiredField');
        if (!ValidateLCIFranchise("LCI", "Generale")) {
            error = true;
        }

        if (error) {
            return false;
        }
        common.page.isLoading = true;
        if ($("#txtParamRedirect").val() != "" && $("#txtParamRedirect").val() != undefined) {
            $.ajax({
                type: "POST",
                url: "/EngagementTraite/UpdateRedirect",
                context: $("#divEngagementTraite"),
                data: {
                    codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, argEngagementTraite: argEngagementTraite, argModelCommentForce: argModelCommentForce,
                    field: field, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), lienCpxLCI: $("#idLienCpx_LCIGenerale").val(), modeNavig: modeNavig,
                    codeTraite: $("#CodeTraite").val(), codePeriode: $("#CodePeriodeEng").val(), accessMode: $("#accessMode").val()
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            let postedData = {
                affaireId: {
                    CodeAffaire: codeOffre,
                    NumeroAliment: version,
                    TypeAffaire: type,
                    NumeroAvenant: codeAvn,
                    IsHisto: $("#ModeNavig").val() === "H"
                },
                tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(),
                engagementTraite: {
                    codeTraite: $("#CodeTraite").val(),
                    CodePeriode: $("#CodePeriodeEng").val(),
                    Traite: engagementsTraites.buildModelTraite()
                },
                accessMode: $("#accessMode").val(),
                actionEngagement: $("#CurrentAccessMode").val()
            };
            common.$postJson("/EngagementTraite/UpdateTraites", postedData, true).done(function (html) {
                $("#divEngagementTraite").html(html);
                MapElementLCIFranchise("LCI", "Generale", $("#NomEcran").val());
                engagementsTraites.initPage();
                common.page.isLoading = false;
            });
        }
    };

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

    this.buildModelTraite = function () {
        var idVentilation = 0; totalSMP = 0;
        var tableau = document.getElementById("tblTraiteEngagementInfo");
        for (var i = 1; i < tableau.rows[2].cells.length; i++) {
            if (tableau.rows[2].cells[i].querySelector('input').value.replace(/ /g, "") != tableau.rows[2].cells[i].querySelector('input').name.split("-")[1]) {
                idVentilation = tableau.rows[2].cells[i].querySelector('input').name.split("-")[0];
                totalSMP = tableau.rows[2].cells[i].querySelector('input').value.replace(/ /g, "");

            }
        }
        return {
            LCIValeur: $("#Valeur_LCIGenerale").val(),
            LCIUnite: $("#Unite_LCIGenerale").val(),
            LCIType: $("#Type_LCIGenerale").val(),
            LCIIndexee: $("#IsIndexe_LCIGenerale").isChecked(),
            LienComplexeLCI: $("#idLienCpx_LCIGenerale").val(),
            CommentForce: $("#CommentForce").val(),
            IdVentilation: idVentilation,
            TotalSMP: totalSMP

        };
    };
};

var engagementsTraites = new EngagementsTraites();

function LoadEngagementsJS() {
    engagementsTraites.initPage();
}
