var isConditionsLoaded = false;
var isElementPageMapped = false;
var isConditionFaulted = false;
var lastSelectedGarantieId;
var isAvnDisabled = false;

var ConditionsGaranties = function () {
    const lciGen = "LCIGenerale";

    //---------------Affiche les details de la garantie--------------
    this.initDisplayDetailsGarantie = function () {
        $("#tblConditions img[name=linkDetail]").offOn("click", function () {

            common.knockout.components.includeInDialog(
                "Détails garantie",
                { width: 800, height: 550 },
                affaire.formule.detailsGarantie,
                "isReadonly: true"
                + ", id: " + $(this).data("sequence")
                + ", codeBloc: '" + $(this).data("bloc")
                + "', numFormule: " + $(this).data("formule")
                + ", branche: '" + $(this).data("branche")
                + "', cible: '" + $(this).data("cible")
                + "', dateAvenant: ''"
                + ", numeroAvenant: " + $("#NumAvenantPage").val());

        });
    };
    //----------------Map les éléments de la page------------------
    this.initPage = function () {

        conditionsGaranties.formatDecimals();
        $("#Condition_Niveau").removeAttr("disabled");
        $("#Condition_VoletBloc").removeAttr("disabled");
        $("input[name=radioGarantie]").removeAttr("disabled");

        $("#Valeur_LCIGenerale").addClass("numerique");
        $("#Valeur_LCIRisque").addClass("numerique");
        $("#Valeur_FranchiseGenerale").addClass("numerique");
        $("#Valeur_FranchiseRisque").addClass("numerique");

        $("#btnInfoOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "ErrorCache":
                    conditionsGaranties.redirect("ConditionsGarantie", "Index");
                    break;
                default:
                    break;
            }
            $("#hiddenAction").clear();
        });

        $("#btnSuivant").kclick(function (evt, data) {
            if (!$(this).attr('disabled')) {
                conditionsGaranties.validate(data && data.returnHome);
            }
        });

        $("#btnAnnuler").kclick(function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });

        $("input[name=radioGarantie]").offOn("change", function () {
            var modifGarantie = $(this).val();
            var niveau = $("#Condition_Niveau").val();
            var voletbloc = $("#Condition_VoletBloc").val();

            conditionsGaranties.reloadFiltreCondition("VoletBloc", modifGarantie, voletbloc, niveau);
            conditionsGaranties.reloadFiltreCondition("Niveau", modifGarantie, voletbloc, niveau);

            conditionsGaranties.displayCondition(modifGarantie, voletbloc, niveau);
        });

        $("#Condition_Garantie").offOn("change", function () {
            var codeGarantie = $(this).val();
            var niveau = $("#Condition_Niveau").val();
            var voletbloc = $("#Condition_VoletBloc").val();

            conditionsGaranties.reloadFiltreCondition("VoletBloc", codeGarantie, voletbloc, niveau);
            conditionsGaranties.reloadFiltreCondition("Niveau", codeGarantie, voletbloc, niveau);

            conditionsGaranties.displayCondition(codeGarantie, voletbloc, niveau);
        });

        $("#Condition_Niveau").offOn("change", function () {
            CopySelectedTitleToElem(this);
            var modifGarantie = $("input[name=radioGarantie]").die().val();
            var niveau = $(this).val();
            var voletbloc = $("#Condition_VoletBloc").val();

            conditionsGaranties.reloadFiltreCondition("VoletBloc", modifGarantie, voletbloc, niveau);

            conditionsGaranties.displayCondition(modifGarantie, voletbloc, niveau);
        });

        $("#Condition_VoletBloc").offOn("change", function () {
            CopySelectedTitleToElem(this);
            var modifGarantie = $("input[name=radioGarantie]").die().val();
            var niveau = $("#Condition_Niveau").val();
            var voletbloc = $(this).val();

            conditionsGaranties.reloadFiltreCondition("Niveau", modifGarantie, voletbloc, niveau);

            conditionsGaranties.displayCondition(modifGarantie, voletbloc, niveau);
        });

        conditionsGaranties.initDisplayDetailsGarantie();

        $("#FullScreen").kclick(function () {
            conditionsGaranties.openFullScreen();
        });

        $("#dvLinkClose").kclick(function () {
            conditionsGaranties.closeFullScreen();
        });

        $("#imgLCIComplexe").kclick(function () {
            conditionsGaranties.openExpComplexe('LCI');
        });
        $("#imgFranchiseComplexe").kclick(function () {
            conditionsGaranties.openExpComplexe('Franchise');
        });

        $(document).off("change", "select[id^='TauxForfaitHTUnite'], [id^='TauxForfaitHTValeur'], [id^='AssietteValeur'], [id^='TauxForfaitHTMinimum']");

        conditionsGaranties.initLineClick();
        conditionsGaranties.initDelete();
        conditionsGaranties.initLinkExprComplexe();

        $("#tblConditions img[name=ajout]").kclick(function () {
            conditionsGaranties.add($(this));
        });

        $("#tblConditions tr[name=garantie]").kclick(function () {
            if (!isConditionFaulted) {
                conditionsGaranties.openLine($(this));
            }
        });
        conditionsGaranties.initTauxForfaitUnite();

        $("#tblConditions img[name=svgde]").kclick(function () {
            var codeGarantie = $("#EditGarantieId").val();
            $("#EditGarantieId").clear();
            if (conditionsGaranties.checkLine(codeGarantie)) {
                $("#EditGarantieId").val(codeGarantie);
                return false;
            }
            conditionsGaranties.saveLine(null, codeGarantie);
        });

        $("#tblConditions img[name='cancel']").kclick(function () {
            var codeGarantie = $("#EditGarantieId").val();
            $("#EditGarantieId").clear();
            if (codeGarantie) {
                conditionsGaranties.resetLine(codeGarantie);
            }
        });

        $("#largeTauxMini").offOn("blur", function () {
            $("input[id=TauxForfaitHTMinimum¤" + $("#idTauxMini").val() + "]").val($(this).val());
            $("#divTauxMini").hide();
            $(this).clear();
        });

        $("#imgInfoApplique").kclick(function () {
            let height = $(this).outerHeight();
            let width = $(this).outerWidth();
            let widthDiv = $("#divInfoApplique").outerWidth();
            $("#divInfoApplique").css({ position: "absolute", top: height + "px", left: "-" + (widthDiv - width) + "px" }).toggle();
        });

        conditionsGaranties.mapAppel("LCI", "Generale", $("#NomEcran").val());
        conditionsGaranties.mapAppel("LCI", "Risque", $("#NomEcran").val());
        conditionsGaranties.mapAppel("Franchise", "Generale", $("#NomEcran").val());
        conditionsGaranties.mapAppel("Franchise", "Risque", $("#NomEcran").val());

        $(document).on("change", "select[id^='TauxForfaitHTUnite'], [id^='TauxForfaitHTValeur'], [id^='AssietteValeur'], [id^='TauxForfaitHTMinimum']", function () {
            conditionsGaranties.computeGareat();
        });

        $("[id^='TauxForfaitHTValeur'],[id^='AssietteValeur']").offOn("keyup", function (e) {
            if (e.key === "Enter") {
                let x = this;
                setTimeout(function () { $(x).change(); });
            }
        });

        if (conditionsGaranties.isReadonly()) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
            $("#btnFSAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        if ($("#Condition_AppliqueA").val() == "") {
            conditionsGaranties.redirect("CreationFormuleGarantie", "Index");
        }
    };

    //-------Formate les input/span des valeurs----------
    this.formatDecimalValue = function (codeCondition, isInit) {
        var mode = isInit ? 'init' : 'update';
        common.autonumeric.apply($("#Valeur_LCIGenerale"), mode, 'decimal', ' ', null, null, '99999999999.99', null);
        common.autonumeric.apply($("#Valeur_LCIRisque"), mode, 'decimal', ' ', null, null, '99999999999.99', null);
        common.autonumeric.apply($("#Valeur_FranchiseGenerale"), mode, 'decimal', ' ', null, null, '99999999999.99', null);
        common.autonumeric.apply($("#Valeur_FranchiseRisque"), mode, 'decimal', ' ', null, null, '99999999999.99', null);

        conditionsGaranties.formatDecimals(codeCondition);
    };
    //-------Formate la franchise-------------
    this.formatFranchise = function (codeCondition) {
        $("#FranchiseValeur¤" + codeCondition).attr('albMask', 'decimal');
        common.autonumeric.apply($("#FranchiseValeur¤" + codeCondition), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
    };
    //-------Formate la LCI-------------
    this.formatLCI = function (codeCondition) {
        $("#LCIValeur¤" + codeCondition).attr('albMask', 'decimal');
        common.autonumeric.apply($("#LCIValeur¤" + codeCondition), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
    };
    //-------Formate l'assiette-------------
    this.formatAssiette = function (codeCondition) {
        var assUnite = $("#AssietteUnite¤" + codeCondition).val();
        if (assUnite == '%') {
            var assVal = $("#AssietteValeur¤" + codeCondition).autoNumeric('get');
            if (parseFloat(assVal) < 0 || parseFloat(assVal) > 100) {
                $("#AssietteValeur¤" + codeCondition).clear();
            }
            $("#AssietteValeur¤" + codeCondition).attr('albMask', 'pourcentdecimal');
            common.autonumeric.apply($("#AssietteValeur¤" + codeCondition), 'update', 'pourcentdecimal', ' ');
        }
        else {
            $("#AssietteValeur¤" + codeCondition).attr('albMask', 'decimal');
            common.autonumeric.apply($("#AssietteValeur¤" + codeCondition), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
        }
    };
    //-------Formate le taux-------------
    this.formatTauxHT = function (codeCondition) {
        var txUnite = $("#TauxForfaitHTUnite¤" + codeCondition).val();
        var txVal = $("#TauxForfaitHTValeur¤" + codeCondition).val() != undefined ? $("#TauxForfaitHTValeur¤" + codeCondition).autoNumeric('get') : 0;
        if (txUnite == "%") {
            if (parseFloat(txVal) < 0 || parseFloat(txVal) > 100) {
                $("#TauxForfaitHTValeur¤" + codeCondition).clear();
            }
            $("#TauxForfaitHTValeur¤" + codeCondition).attr('albMask', 'pourcentdecimal');
            common.autonumeric.apply($("#TauxForfaitHTValeur¤" + codeCondition), 'update', 'pourcentdecimal', ' ', null, 3);
        }
        else if (txUnite == "%0") {
            if (parseFloat(txVal) < 0 || parseFloat(txVal) > 1000) {
                $("#TauxForfaitHTValeur¤" + codeCondition).clear();
            }
            $("#TauxForfaitHTValeur¤" + codeCondition).attr('albMask', 'pourmilledecimal');
            common.autonumeric.apply($("#TauxForfaitHTValeur¤" + codeCondition), 'update', 'pourmilledecimal', ' ', null, 3);
        }
        else {
            $("#TauxForfaitHTValeur¤" + codeCondition).attr('albMask', 'decimal');
            common.autonumeric.apply($("#TauxForfaitHTValeur¤" + codeCondition), 'update', 'decimal', ' ', null, 3, '99999999999.999');
        }
    };
    //-------Formate la franchise CPX-------------
    this.formatFranchiseCPX = function (codeCondition) {
        var $frh = $("#FranchiseValeurCPX¤" + codeCondition);
        $frh.attr('albMask', 'decimal');
        common.autonumeric.apply($frh, "update", "decimal", " ", null, null, "99999999999.99", null);
    };
    //-------Formate la LCI CPX-------------
    this.formatLCICPX = function (codeCondition) {
        $("#LCIValeurCPX¤" + codeCondition).attr('albMask', 'decimal');
        common.autonumeric.apply($("#LCIValeurCPX¤" + codeCondition), "update", "decimal", " ", null, null, '99999999999.99', null);
    };
    //-------Formate la Concurrence CPX-------------
    this.formatConcurrenceCPX = function (codeCondition) {
        var lciUnite = $("#ConcurrenceUniteCPX¤" + codeCondition).val();
        if (lciUnite == '%') {
            var lciVal = $("#ConcurrenceValeurCPX¤" + codeCondition).val();
            if (parseFloat(lciVal) < 0 || parseFloat(lciVal) > 100) {
                $("#ConcurrenceValeurCPX¤" + codeCondition).clear();
            }
            $("#ConcurrenceValeurCPX¤" + codeCondition).attr('albMask', 'pourcentdecimal');
            common.autonumeric.apply($("#ConcurrenceValeurCPX¤" + codeCondition), 'update', 'pourcentdecimal', ' ');
        }
        else {
            $("#ConcurrenceValeurCPX¤" + codeCondition).attr('albMask', 'decimal');
            common.autonumeric.apply($("#ConcurrenceValeurCPX¤" + codeCondition), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
        }
    }
    //----------------------------Export Conditions tarifaires en CSV
    this.ExportToCSV = function () {
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/CheckConditionInCache",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), modeNavig: $("#ModeNavig").val(), isReadOnly: conditionsGaranties.isReadonly()
            },
            success: function (data) {
                if (data != "") {
                    ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                    return false;
                }
                else {
                    var splitChar = "_";
                    window.location.href = "/ConditionsGarantie/ExportFile/" + encodeURIComponent($("#Offre_CodeOffre").val() + splitChar + $("#Offre_Version").val() + splitChar + $("#Offre_Type").val() + splitChar + $("#CodeFormule").val() + splitChar + $("#CodeOption").val() + splitChar + "Conditions" + splitChar + conditionsGaranties.isReadonly() + splitChar + $("#ModeNavig").val() + splitChar + $("#NumAvenantPage").val());
                }
                return true;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //--------------Changement de l'unité du taux forfaitaire------------
    this.initTauxForfaitUnite = function () {
        $("select[id^=TauxForfaitHTUnite]").offOn("change", function () {
            let id = $(this).attr('id').split('¤')[1];
            let TauxUnite = $(this).val();
            if (TauxUnite == "UM" || TauxUnite == "D") {
                $("#TauxForfaitHTMinimum¤" + id).val("0").attr('readonly', 'readonly').addClass('readonly');
            }
            else {
                $("#TauxForfaitHTMinimum¤" + id).removeAttr('readonly', 'readonly').removeClass('readonly');
            }
            conditionsGaranties.formatTauxHT(id);
        });
        $(document).off("focus", "input[id^=TauxForfaitHTMinimum]");
        $(document).on("focus", "input[id^=TauxForfaitHTMinimum]", function () {
            if (!$(this).hasClass("readonly")) {
                var pos = $(this).offset();

                var topPos = pos.top;
                var leftPos = pos.left;

                if ($("#divFullScreen").is(":visible")) {
                    topPos = topPos + 50;
                    leftPos = leftPos + 15;
                }

                $("#divTauxMini").show().css({ 'position': 'absolute', 'top': topPos + 20 + 'px', 'left': leftPos - 80 + 'px', 'z-index': 50 });
                $("#idTauxMini").val($(this).attr('id').split('¤')[1]);
                $("#largeTauxMini").val($(this).val()).focus();
            }
        });
    };

    //------------Rechargement des filtres-------------------
    this.reloadFiltreCondition = function (type, modifGarantie, voletbloc, niveau) {
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/ReloadFiltre",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(),
                typeFiltre: type, garantie: modifGarantie, voletbloc: voletbloc, niveau: niveau,
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                switch (type) {
                    case "VoletBloc":
                        $("#divFiltreVoletsBlocs").html(data);
                        $("#Condition_VoletBloc").val(voletbloc);
                        break;
                    case "Niveau":
                        $("#divFiltreNiveau").html(data);
                        $("#Condition_Niveau").val(niveau);
                        break;
                    default:
                        break;
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //-----------Affichage des conditions suivant les filtres------------
    this.displayCondition = function (codeGarantie, voletbloc, niveau) {
        $("#tblConditions tr").hide();
        if (niveau != "") {
            if (codeGarantie != "") {
                if (voletbloc != "") {
                    $("#tblConditions tr[albVoletBloc^=" + voletbloc + "][albGarantie=" + codeGarantie + "][albNiveau=" + niveau + "]").show();
                }
                else {
                    $("#tblConditions tr[albGarantie=" + codeGarantie + "][albNiveau=" + niveau + "]").show();
                }
            }
            else {
                if (voletbloc != "") {
                    $("#tblConditions tr[albVoletBloc^=" + voletbloc + "][albNiveau=" + niveau + "]").show();
                }
                else {
                    $("#tblConditions tr[albNiveau=" + niveau + "]").show();
                }
            }
        }
        else {
            if (codeGarantie != "") {
                if (voletbloc != "") {
                    $("#tblConditions tr[albGarantie=" + codeGarantie + "][albVoletBloc^=" + voletbloc + "]").show();
                }
                else {
                    $("#tblConditions tr[albGarantie=" + codeGarantie + "]").show();
                }
            }
            else {
                if (voletbloc != "") {
                    $("#tblConditions tr[albVoletBloc^=" + voletbloc + "]").show();
                }
                else {
                    $("#tblConditions tr").show();
                }
            }
        }
    };
    //-----------------Redirection----------------------
    this.redirect = function (cible, job) {
        ShowLoading();
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let codeFormule = $("#CodeFormule").val();
        let codeOption = $("#CodeOption").val();
        let tabGuid = $("#tabGuid").val();
        let modeNavig = $("#ModeNavig").val();
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();

        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/Redirection/",
            data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function (data) { },
            error: function (request) {
                var responseError;
                try {
                    responseError = jQuery.parseJSON(request.responseText);
                }
                catch (e) {
                    responseError = request.statusText;
                }
                common.dialog.bigError(responseError.ErrorMessage, true);
                CloseLoading();
            }
        });
    };
    //-------------Affecte l'ouverture des expressions complexes sur les liens------------
    this.initLinkExprComplexe = function () {
        $("span[name='LienComplexe']").kclick(function () {
            var codeCondition = $(this).attr('id').split('¤')[1];
            var codeExpression = $(this).attr('albhref').split('#')[1].split('¤')[0];
            var isModif = "";
            if ($(this).attr('id').indexOf('LCI') != -1) {
                isModif = $("input[id='LCILectureSeule¤" + codeCondition + "']").val();
                conditionsGaranties.openExpComplexe('LCI', codeCondition, codeExpression, isModif);
            }
            if ($(this).attr('id').indexOf('FRH') != -1) {
                isModif = $("input[id='FranchiseLectureSeule¤" + codeCondition + "']").val();
                conditionsGaranties.openExpComplexe('Franchise', codeCondition, codeExpression, isModif);
            }
        });
    };
    //-------------Affecte la fonction d'ouverture des conditions garantie---------------
    this.initLineClick = function () {
        $("#tblConditions td[name=lock]").kclick(function () {
            if (!conditionsGaranties.isReadonly()) {
                var lineCondition = $("tr[id=" + $(this).attr('id').split('¤')[1] + "]");
                if ($("#EditGarantieId").val() != lineCondition.attr('id')) {
                    if (!isConditionFaulted)
                        conditionsGaranties.openLine(lineCondition);
                }
            }
        });
        $("#tblConditions select[id^=LCIUnite]").offOn("change", function () {
            var valLCIUnite = $(this).val();
            var codeCondition = $(this).attr('id').split('¤')[1];
            if (valLCIUnite == 'CPX') {
                conditionsGaranties.openExpComplexe("LCI", codeCondition);
                $("#LCIValeur¤" + codeCondition).attr('readonly', 'readonly').addClass('readonly').clear();
            }
            else {
                $("#LienLCIComplexe¤" + codeCondition).val("0¤");
                $("div[name=OffLCIComplexe¤" + codeCondition + "]").removeClass("None");
                $("div[name=OnLCIComplexe¤" + codeCondition + "]").addClass("None");
                $("#LockLienLCIComplexe¤" + codeCondition).text("0¤").attr("albhref", "#");
                $("#EditLienLCIComplexe¤" + codeCondition).text("0¤").attr("albhref", "#");
                $("#LCIValeur¤" + codeCondition).removeAttr('readonly', 'readonly').removeClass('readonly');
            }
            conditionsGaranties.formatLCI(codeCondition);
        });
        $("#tblConditions select[id^=FranchiseUnite]").offOn("change", function () {
            var FranchiseUnite = $(this).val();
            var codeCondition = $(this).attr('id').split('¤')[1];
            if (FranchiseUnite == 'CPX') {
                conditionsGaranties.openExpComplexe("Franchise", codeCondition);
                $("#FranchiseValeur¤" + codeCondition).attr('readonly', 'readonly').addClass('readonly').clear();
            }
            else {
                $("#LienFRHComplexe¤" + codeCondition).val("0¤");
                $("div[name=OffFRHComplexe¤" + codeCondition + "]").removeClass("None");
                $("div[name=OnFRHComplexe¤" + codeCondition + "]").addClass("None");
                $("#LockLienFRHComplexe¤" + codeCondition).text("0¤").attr("albhref", "#");
                $("#EditLienFRHComplexe¤" + codeCondition).text("0¤").attr("albhref", "#");
                $("#FranchiseValeur¤" + codeCondition).removeAttr('readonly', 'readonly').removeClass('readonly');
            }
            conditionsGaranties.formatFranchise(codeCondition);
        });
        $("#tblConditions").on("change", "select[id^=AssietteUnite]", function () {
            conditionsGaranties.formatAssiette($(this).attr('id').split('¤')[1]);
        });

        conditionsGaranties.initSelectTitles();
    };

    this.initSelectTitles = function () {
        var selects = [
            "select[albCFList]",
            "select[name='ligne.FranchiseUnite']",
            "select[name='ligne.FranchiseType']",
            "select[name='ligne.LCIUnite']",
            "select[name='ligne.LCIType']",
            "select[name='ligne.AssietteUnite']",
            "select[name='ligne.AssietteType']",
            "select[name='ligne.TauxForfaitHTUnite']"
        ];

        $(document).on("mouseover", selects.join(","), function () {
            CopySelectedTitleToElem(this);
        });
    };

    //-------------Affecte la fonction de suppression des conditions de garantie---------
    this.initDelete = function () {
        $("#tblConditions").on("click", "img[name=poubelle]", function () {
            conditionsGaranties.delete($(this));
            event.stopPropagation();
        });
    };
    //-------------Ajout d'une ligne de condition de garantie--------------
    this.add = function (elem) {

        var garantieId = elem.attr('id').split('_')[1];
        var countCondition = $("tr[id$=_" + garantieId + "]").length;

        if (elem.hasClass("CursorPointer") && countCondition < 3) {
            ShowLoading();

            $("img[id$=_" + garantieId + "_img]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            var copyLine = $("tr[id$=_" + garantieId + "]").first().clone();
            var lastLine = $("tr[id$=_" + garantieId + "]").last();

            $.ajax({
                type: "POST",
                url: "/ConditionsGarantie/DuplicateCondition",
                data: {
                    codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                    codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeGarantie: copyLine.attr('id').split('_')[1], codeCondition: copyLine.attr('id').split('_')[0],
                    modeNavig: $("#ModeNavig").val(), isReadOnly: conditionsGaranties.isReadonly()
                },
                success: function (data) {
                    if (data == "") {
                        ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                        return false;
                    }

                    var newCodeCondition = data;

                    if (newCodeCondition != "") {
                        var reg = new RegExp(copyLine.attr('id').split('_')[0] + "_", "g");
                        var newLine = $("<table></table>").html(copyLine);
                        var htmlLine = newLine.html().replace(reg, newCodeCondition + "_");
                        copyLine = $(htmlLine).find("tr[id=" + newCodeCondition + "_" + garantieId + "]");
                        copyLine.insertAfter(lastLine);
                        conditionsGaranties.initLineClick();
                        conditionsGaranties.initLinkExprComplexe();
                        conditionsGaranties.formatDecimalValue(newCodeCondition + "_" + garantieId, false);
                        conditionsGaranties.initTauxForfaitUnite();
                        $("input[id=AssietteValeur¤" + newCodeCondition + "_" + garantieId + "]").attr("readonly", "readonly").addClass("readonly");
                        $("select[id=AssietteUnite¤" + newCodeCondition + "_" + garantieId + "]").attr("disabled", "disabled").addClass("readonly");
                        $("select[id=AssietteType¤" + newCodeCondition + "_" + garantieId + "]").attr("disabled", "disabled").addClass("readonly");
                        //Déclenche l'ouverture de la ligne de condition de garantie
                        $("td[id=divLCIValeur¤" + newCodeCondition + "_" + garantieId + "]").trigger('click');
                        //Met le champ de MAJ à "I"
                        $("#MAJ¤" + newCodeCondition + "_" + garantieId).val("I");
                        //Met le nouveau numéro de tarif
                        $("input[id=AssietteNumeroTarif¤" + newCodeCondition + "_" + garantieId + "]").val(newCodeCondition);
                        //Bind le bouton sauvegarde
                        $("img[id=svgde¤" + newCodeCondition + "_" + garantieId + "]").bind('click', function () {
                            var codeGarantie = $("#EditGarantieId").val();
                            $("#EditGarantieId").clear();
                            if (conditionsGaranties.checkLine(codeGarantie)) {
                                $("#EditGarantieId").val(codeGarantie);
                                return false;
                            }
                            conditionsGaranties.saveLine(null, codeGarantie);
                            //change le src de l'img
                            //ChangeAddImgGarantie(garantieId.split('_')[1]);
                        });
                        $("img[id='cancel¤" + newCodeCondition + "_" + garantieId + "']").bind('click', function () {
                            $("#EditGarantieId").clear();
                            $("td[name='edit']").hide();
                            $("td[name='lock']").show();
                            $("img[name=svgde]").hide();
                            //ChangeAddImgGarantie(garantieId.split('_')[1]);
                            $("#btnSuivant").removeAttr("disabled");
                        });

                    }
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };
    //-------------Supprimer une ligne de condition de garantie---------------
    this.delete = function (elem) {
        var lineId = elem.attr('id').split('¤')[1];
        if (elem.hasClass("CursorPointer")) {
            ShowLoading();
            var garantieId = lineId.split('_')[1];
            $.ajax({
                type: "POST",
                url: "/ConditionsGarantie/DeleteCondition",
                data: {
                    codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                    codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeGarantie: lineId.split('_')[1], codeCondition: lineId.split('_')[0],
                    modeNavig: $("#ModeNavig").val(), isReadOnly: conditionsGaranties.isReadonly()
                },
                success: function (data) {
                    if (data == "KO") {
                        ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                        return false;
                    }
                    $("tr[id$=" + lineId + "]").remove();
                    //ChangeAddImgGarantie(garantieId);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };

    //-------------Ouverture du plein écran--------------------
    this.openFullScreen = function () {
        if ($("#EditGarantieId").val() != "") {
            var codeGarantie = $("#EditGarantieId").val();
            $("#EditGarantieId").clear();
            if (conditionsGaranties.checkLine(codeGarantie)) {
                $("#EditGarantieId").val(codeGarantie);
                return false;
            }
            var saveError = conditionsGaranties.saveLine(null, codeGarantie);
            if (saveError)
                return false;
        }
        $("#btnFSAnnuler").kclick(function () {
            $("#btnAnnuler").trigger('click');
        });
        $("#btnFSSuivant").kclick(function () {
            $("#btnSuivant").click();
        });
        if (conditionsGaranties.isReadonly()) {
            $("#btnFSAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }
        conditionsGaranties.load(true);
    };
    //-------------Fermeture du plein écran---------------------
    this.closeFullScreen = function () {
        if ($("#EditGarantieId").val() != "") {
            var codeGarantie = $("#EditGarantieId").val();
            $("#EditGarantieId").clear();
            if (conditionsGaranties.checkLine(codeGarantie)) {
                $("#EditGarantieId").val(codeGarantie);
                return false;
            }
            var saveError = conditionsGaranties.saveLine(null, codeGarantie);
            if (saveError)
                return false;
        }
        conditionsGaranties.load(false);
    };
    //-------------Charge les données des conditions de garantie dans la div plein écran ou vue normale----------
    this.load = function (fullScreen) {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvn = $("#NumAvenantPage").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/LoadConditions",
            data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption, fullScreen: fullScreen, isReadOnly: conditionsGaranties.isReadonly(), modeNavig: $("#ModeNavig").val() },
            success: function (data) {
                if (data == "") {
                    ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                    return false;
                }

                if (fullScreen) {
                    $("#divDataFullScreen").html(data);
                    $("#divDataConditions").clearHtml();
                    AlbScrollTop();
                    $("#divFullScreen").show();
                }
                else {
                    $("#divDataConditions").html(data);
                    $("#divDataFullScreen").clearHtml();
                    $("#divFullScreen").hide();
                }
                conditionsGaranties.initPage();
                if (!fullScreen) {
                    conditionsGaranties.computeGareat();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //-------------Ouverture de la ligne de garantie pour modification--------------
    this.openLine = function (elem) {
        $("input").removeClass("requiredField");
        $("select").removeClass("requiredField");

        var garantieId = elem.attr('id').split('_')[1];
        lastSelectedGarantieId = elem.attr('id');

        elem.find("img[name=poubelle]").hide();
        elem.find("img[name=svgde]").hide();
        $("#btnSuivant").attr('disabled', 'disabled');
        $("#btnFSSuivant").attr('disabled', 'disabled');
        if ($("#EditGarantieId").val() != "") {
            var codeGarantie = $("#EditGarantieId").val();
            $("#EditGarantieId").clear();
            if (conditionsGaranties.checkLine(codeGarantie)) {
                $("#EditGarantieId").val(codeGarantie);
                return false;
            }
            if (conditionsGaranties.saveLine(elem, codeGarantie)) {
                return false;
            }
            elem.find("img[name=svgde]").show();
        }
        else {
            $("td[name=edit]").hide();
            $("td[name=lock]").show();
            elem.find("td[name=lock]").hide();
            elem.find("td[name=edit]").show();
            if (elem.attr('id').indexOf('garantie_') == -1) {
                $("#EditGarantieId").val(elem.attr('id'));
                $("img[id$=_" + garantieId + "_img]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            }
            else {
                $("#btnSuivant").removeAttr('disabled');
                $("#btnFSSuivant").removeAttr('disabled');
            }

            conditionsGaranties.formatLCI(elem.attr('id'));
            conditionsGaranties.formatFranchise(elem.attr('id'));
            conditionsGaranties.formatAssiette(elem.attr('id'));
            conditionsGaranties.formatTauxHT(elem.attr('id'));
            elem.find("img[name=svgde]").show();
        }
    };
    //---------------Vérifie la conformité des informations de la condition-------------
    this.checkLine = function (codeGarantie) {

        var saveError = false;
        if ($("#LCIObligatoire¤" + codeGarantie).val() == "O") {
            saveError = conditionsGaranties.checkLCI(codeGarantie) ? true : saveError;
        }
        if ($("#FranchiseObligatoire¤" + codeGarantie).val() == "O" || ($("#FranchiseValeur¤" + codeGarantie).val() != "" || $("#FranchiseUnite¤" + codeGarantie).val() != "" || $("#FranchiseType¤" + codeGarantie).val() != "" && $("#FranchiseObligatoire¤" + codeGarantie).val() == "N")) {
            saveError = conditionsGaranties.checkFranchise(codeGarantie) ? true : saveError;
        }
        if ($("#AssietteObligatoire¤" + codeGarantie).val() == "O") {
            saveError = conditionsGaranties.checkAssiette(codeGarantie) ? true : saveError;
        }
        if ($("#TauxForfaitHTObligatoire¤" + codeGarantie).val() == "O") {
            saveError = conditionsGaranties.checkTaux(codeGarantie) ? true : saveError;
        }
        return saveError;

    };
    //--------------Vérifie les valeurs de la LCI de la condition------------
    this.checkLCI = function (codeGarantie) {
        var saveError = false;

        var lciVal = $("#LCIValeur¤" + codeGarantie).autoNumeric('get');
        var lciUnite = $("#LCIUnite¤" + codeGarantie).val();
        var lciType = $("#LCIType¤" + codeGarantie).val();
        var codeLCI = $("#EditLienLCIComplexe¤" + codeGarantie).attr('albhref').split('¤')[0];

        if (lciUnite == "" || (lciType == "" && lciUnite != "CPX") || (lciVal == "" && lciUnite != "CPX") || (lciUnite == "CPX" && codeLCI == "#")) {
            $("#LCIValeur¤" + codeGarantie).addClass("requiredField");
            $("#LCIUnite¤" + codeGarantie).addClass("requiredField");
            $("#LCIType¤" + codeGarantie).addClass("requiredField");
            saveError = true;
        }
        return saveError;
    };
    //-------------Vérifie les valeurs de la Franchise de la condition----------
    this.checkFranchise = function (codeGarantie) {
        var saveError = false;

        var frhVal = $("#FranchiseValeur¤" + codeGarantie).autoNumeric('get');
        var frhUnite = $("#FranchiseUnite¤" + codeGarantie).val();
        var frhType = $("#FranchiseType¤" + codeGarantie).val();
        var codeFRH = $("#EditLienFRHComplexe¤" + codeGarantie).attr('albHref').split('¤')[0];
        if ($("#FranchiseObligatoire¤" + codeGarantie).val() == "O") {
            if (frhUnite == "" || (frhType == "" && frhUnite != "CPX") || (frhVal == "" && frhUnite != "CPX") || (frhUnite == "CPX" && codeFRH == "#")) {
                $("#FranchiseValeur¤" + codeGarantie
                    + ",#FranchiseUnite¤" + codeGarantie
                    + ",#FranchiseType¤" + codeGarantie).addClass("requiredField");
                saveError = true;
            }
        }
        else {
            if ((frhUnite == "CPX" && codeFRH != "#") || (frhType != "" && frhUnite != "" && frhUnite != "CPX") || (frhType != "" && frhUnite != "" && $("#FranchiseValeur¤" + codeGarantie).val() != "" && frhUnite != "CPX")) {
                saveError = false;
            }
            else {
                $("#FranchiseValeur¤" + codeGarantie
                    + ",#FranchiseUnite¤" + codeGarantie
                    + ",#FranchiseType¤" + codeGarantie).addClass("requiredField");
                saveError = true;
            }
        }
        return saveError;
    };
    //-------------Vérifie les valeurs de l'Assiette de la condition----------
    this.checkAssiette = function (codeGarantie) {
        var saveError = false;

        var assVal = $("#AssietteValeur¤" + codeGarantie).autoNumeric('get');
        var assUnite = $("#AssietteUnite¤" + codeGarantie).val();
        var assType = $("#AssietteType¤" + codeGarantie).val();

        if (assVal == "" || assUnite == "" || assType == "" || assVal == 0) {
            $("#AssietteValeur¤" + codeGarantie + ",#AssietteUnite¤" + codeGarantie + ",#AssietteType¤" + codeGarantie).addClass("requiredField");
            saveError = true;
        }
        if (assUnite == "%" && (parseFloat(assVal) < 0 || parseFloat(assVal) > 100)) {
            $("#AssietteValeur¤" + codeGarantie + ",#AssietteUnite¤" + codeGarantie).addClass("requiredField");
            saveError = true;
        }

        return saveError;
    };
    //-------------Vérifie les valeurs du taux/forfait de la condition----------
    this.checkTaux = function (codeGarantie) {
        var saveError = false;

        const assiette = $("#AssietteUnite¤" + codeGarantie);
        const tauxVal = $("#TauxForfaitHTValeur¤" + codeGarantie);
        const tauxUnit = $("#TauxForfaitHTUnite¤" + codeGarantie);
        const tauxMin = $("#TauxForfaitHTMinimum¤" + codeGarantie);

        var assUnite = assiette.val();
        var tfhtVal = tauxVal.autoNumeric('get');
        var tfhtUnite = tauxUnit.val();
        var tfhtMini = tauxMin.val();

        if (tfhtVal == "" || tfhtUnite == "") {
            tauxVal.addClass("requiredField");
            tauxUnit.addClass("requiredField");
            tauxMin.addClass("requiredField");
            saveError = true;
        }
        if (tfhtUnite == "%" && (parseFloat(tfhtVal) < 0 || parseFloat(tfhtVal) > 100)) {
            tauxVal.addClass("requiredField");
            tauxUnit.addClass("requiredField");
            saveError = true;
        }
        if ((assUnite != "D" && assUnite != "") && tfhtVal != 0 && tfhtUnite != "D") {
            assiette.addClass("requiredField");
            tauxVal.addClass("requiredField");
            tauxUnit.addClass("requiredField");
            tauxMin.addClass("requiredField");
            saveError = true;
        }
        return saveError;
    };
    //----------------Sauvegarde de la ligne de garantie-----------------
    this.saveLine = function (elem, codeGarantieCondition) {
        let saveError = false;
        isConditionFaulted = true;
        let splitted = codeGarantieCondition.split('_');
        let idGarantie = splitted[1];
        let codeCondition = splitted[0];
        const codeGarantie = $("#" + codeGarantieCondition).data("codegar");

        let dataRow = [{
            Code: codeGarantieCondition,
            CodeGarantie: codeGarantie,
            IndiceLigne: getValue("Index"),
            NumeroTarif: getValue("AssietteNumeroTarif"),
            LCIValeur: getValueOrDefault("LCI"),
            LCIUnite: getValue("LCIUnite"),
            LCIType: getValue("LCIUnite") == "CPX" ? getValue("LienLCIComplexe").split("¤")[1] : getValue("LCIType"),
            LCILectureSeule: getValue("LCILectureSeule"),
            LCIObligatoire: getValue("LCIObligatoire"),
            LibLCIComplexe: getWithCode("LockLienLCIComplexe").attr('title'),
            LienLCIComplexe: getValue("LienLCIComplexe"),

            FranchiseValeur: getValueOrDefault("Franchise"),
            FranchiseUnite: getValue("FranchiseUnite"),
            FranchiseType: getValue("FranchiseUnite") == "CPX" ? getValue("LienFRHComplexe").split("¤")[1] : getValue("FranchiseType"),
            FranchiseLectureSeule: getValue("FranchiseLectureSeule"),
            FranchiseObligatoire: getValue("FranchiseObligatoire"),
            LibFRHComplexe: getWithCode("LockLienFRHComplexe").attr('title'),
            LienFRHComplexe: getValue("LienFRHComplexe"),

            AssietteValeur: getAutoNumeric("AssietteValeur"),
            AssietteUnite: getValue("AssietteUnite"),
            AssietteType: getValue("AssietteType"),
            AssietteLectureSeule: getValue("AssietteLectureSeule"),
            AssietteObligatoire: getValue("AssietteObligatoire"),

            TauxForfaitHTValeur: getAutoNumeric("TauxForfaitHTValeur"),
            TauxForfaitHTUnite: getValue("TauxForfaitHTUnite"),
            TauxForfaitHTMinimum: getValue("TauxForfaitHTMinimum"),
            TauxForfaitHTLectureSeule: getValue("TauxForfaitHTLectureSeule"),
            TauxForfaitHTObligatoire: getValue("TauxForfaitHTObligatoire"),
            MAJ: getValue("MAJ")
        }];



        let data = {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(),
            codeGarantie: idGarantie,
            codeCondition: codeCondition,
            objRow: JSON.stringify(dataRow),
            modeNavig: $("#ModeNavig").val(),
            isReadOnly: conditionsGaranties.isReadonly()
        };

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/SaveGarantie",
            data: data,
            success: function (data) {
                conditionsGaranties.reloadLine(data, elem, codeGarantieCondition);
                CloseLoading();
                isConditionFaulted = false;
                if (!conditionsGaranties.isReadonly()) {

                    $("#CotisationMenuArbreLI").parent().remove();
                    $("#ControleFinMenuArbreLI").parent().remove();

                    $("#MenuRacineGeneral").append('<li class="MenuArbreLI Disabled"><span class="">&nbsp;&nbsp;&nbsp;</span><span class="MenuArbreText" id="CotisationMenuArbreLI">Cotisations</span></li>');
                    $("#MenuRacineGeneral").append('<li class="MenuArbreLI Disabled"><span class="">&nbsp;&nbsp;&nbsp;</span><span class="MenuArbreText" id="ControleFinMenuArbreLI">Contrôle & Fin</span></li>');
                }
            },
            error: function (request) {
                saveError = true;
                common.error.show(request);
                isConditionFaulted = true;
                if (codeGarantieCondition !== lastSelectedGarantieId) {
                    $("#" + lastSelectedGarantieId).find("img[name=svgde]").hide();
                }

                $("#EditGarantieId").val(codeGarantieCondition);
                CloseLoading();
            }
        });
        return saveError;

        function getValueOrDefault(typeVal) {
            var val = getWithCode(typeVal + "Valeur");
            var unite = getWithCode(typeVal + "Unite");
            var type = getWithCode(typeVal + "Type");
            return val.val() ? val.autoNumeric("get") : unite.val() && type.val() ? "0" : "";
        }
        function getJquery(typeVal, code) {
            return $("#" + typeVal + "¤" + code);
        }
        function getWithCode(typeVal) {
            return getJquery(typeVal, codeGarantieCondition);
        }
        function getAutoNumeric(typeVal) {
            return getWithCode(typeVal).autoNumeric('get');
        }
        function getValue(typeVal) {
            return getWithCode(typeVal).val();
        }

    };
    //------------Recharge les données de la ligne de condition--------------
    this.reloadLine = function (data, elem, codeGarantie) {
        if (data == "") {
            ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true, true);
            return false;
        }

        var htmlData = $(data);
        var newCodeGarantie = htmlData.find("input[id=IdLigne]").val();
        conditionsGaranties.copyAssietteValue(data, newCodeGarantie);
        $("tr[id=" + codeGarantie + "]").attr("id", newCodeGarantie).html(htmlData.html());
        var lstCondition = $("tr[id$=_" + codeGarantie.split('_')[1] + "]");
        var index = lstCondition.index($("tr[id=" + newCodeGarantie + "]"));

        if (index > 0) {
            var newGarbage = $("<img id='pbl¤" + newCodeGarantie + "' width='16' height='16' class='CursorPointer' src='/Content/Images/poubelle1616.png' alt='' name='poubelle'>");
            $("tr[id=" + newCodeGarantie + "]").find("td[id^=tdIndex]").append(newGarbage);
            $("tr[id=" + newCodeGarantie + "]").find("img[name=svgde]").hide();

            $("img[id=pbl¤" + newCodeGarantie + "]").bind('click', function () {
                conditionsGaranties.delete($(this));
            });
        }

        conditionsGaranties.initLineClick();
        conditionsGaranties.initLinkExprComplexe();
        CloseLoading();

        $("td[name=edit]").hide();
        $("td[name=lock]").show();

        if (elem != "undefined" && elem != null) {
            elem.find("td[name=lock]").hide();
            elem.find("td[name=edit]").show();
            if (elem.attr('id').indexOf('garantie_') == -1) {
                $("#EditGarantieId").val(elem.attr('id'));
            }
            else {
                $("#btnSuivant").removeAttr('disabled');
                $("#btnFSSuivant").removeAttr('disabled');
            }
        }
        else {
            $("#btnSuivant").removeAttr('disabled');
            $("#btnFSSuivant").removeAttr('disabled');
        }
        conditionsGaranties.formatDecimalValue(codeGarantie, true);
        conditionsGaranties.computeGareat();
    };
    //-------------Copie les nouvelles valeurs de l'assiette------------------
    this.copyAssietteValue = function (data, codeGarantie) {
        var htmlData = $(data);
        var code = codeGarantie.split('_')[1];
        var AssVal = htmlData.find("input[id=AssietteValeur¤" + codeGarantie + "]").val();
        var AssUnit = htmlData.find("select[id=AssietteUnite¤" + codeGarantie + "]").val();
        var AssType = htmlData.find("select[id=AssietteType¤" + codeGarantie + "]").val();

        $("input[id^=AssietteValeur¤][id$=" + code + "]").val(AssVal);
        $("span[id^=spanAssietteValeur¤][id$=" + code + "]").text(AssVal).each(function () {
            common.autonumeric.apply($(this), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
        });
        $("select[id^=AssietteUnite¤][id$=" + code + "]").val(AssUnit);
        $("td[id^=divAssietteUnite¤][id$=" + code + "]").text(AssUnit);
        $("select[id^=AssietteType¤][id$=" + code + "]").val(AssType);
        $("td[id^=divAssietteType¤][id$=" + code + "]").text(AssType);
    };
    //-------------Ouverture de la div flottante des expressions complexes-----------
    this.openExpComplexe = function (type, codeCondition, codeExpression, isModif, actionExpr) {
        if (isModif == undefined || isModif == "")
            isModif = "O";
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var typeOffre = $("#Offre_Type").val();
        var isReadOnly = isModif == "N" || conditionsGaranties.isReadonly();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/OpenExprComplexe",
            data: { codeOffre: codeOffre, version: version, type: typeOffre, typeExpr: type, isReadOnly: isReadOnly, isGenRsq: false, typeAppel: "", modeNavig: $("#ModeNavig").val() },
            context: $("#divDataExprComp"),
            success: function (data) {
                DesactivateShortCut();
                $(this).html(data);

                conditionsGaranties.initExpressionComplexe();
                if (isReadOnly || codeCondition == "" || codeCondition == null) {
                    $("#btnValideExpr").hide();
                }
                else {
                    $("#CurrentConditionExprComp").val(codeCondition);
                }

                AlbScrollTop();

                switch (actionExpr) {
                    case "DuplicateExpr":
                        if ($("img[id='editExpr¤" + codeExpression + "']") != undefined)
                            $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                        if ($("img[id='readExpr¤" + codeExpression + "']") != undefined)
                            $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                        $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                        break;
                    case "SaveExpr":
                        $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                        break;
                    case "ValidExpr":
                        $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                        $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                        $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                        break;
                    default:
                        if (codeExpression != null && codeExpression != "") {
                            $("#setExpr¤" + codeExpression).attr("checked", "checked");
                            if ($("img[id='editExpr¤" + codeExpression + "']") != undefined)
                                $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                            if ($("img[id='readExpr¤" + codeExpression + "']") != undefined)
                                $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                            $("#CurrentCodeExprComp").val(codeExpression);
                        }
                        break;
                }

                $("#divExprComplexe").show();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //---------------Map les éléments de la div flottante Expr Complexe--------------
    this.initExpressionComplexe = function () {
        $("#btnValideExpr").hide();

        $("input[type='radio'][name='modeAjout']").offOn("change", function () {
            if ($(this).val() == "ref" || $(this).val() == "mdl") {
                OpenReferentiel($(this).val());
            }
        });

        AlternanceLigne("InfoExpr", "", false, null);

        $("#addExpr").kclick(function () {
            $(this).attr("disabled", "disabled");
            OpenAddExpr();
        });

        $("#imgAddExpr").kclick(function () {
            if ($(this).hasClass("CursorPointer")) {
                $("input[type='radio'][name='radAddBtn']").removeAttr("checked");
                $(this).attr("src", "/Content/Images/plusajouter_gris1616.jpg");
                var pos = $(this).position();
                $("#dvAddMode").attr("style", "position:absolute;top:" + (pos.top - 6) + "px;left:" + (pos.left + 25) + "px; display: block;");
            }
        });

        $("#dvCloseExpr").kclick(function () {
            $("#dvAddMode").hide();
            $("#imgAddExpr").attr("src", "/Content/Images/plusajouter1616.png");
        });

        $("#btnValideExpr").kclick(function () {
            var idExprComplexe = $("input[type=radio][name=setExpr]:checked").val();
            var codeExprComplexe = $("#hidExpr¤" + idExprComplexe).val();
            var typeExpr = $("#TypeExpressionComplexe").val();
            var libExpr = $("#libExp¤" + idExprComplexe).val();
            if (typeof (idExprComplexe) != "undefined" && idExprComplexe != "") {
                $("#divDataExprComp").html("");
                $("#divExprComplexe").hide();
                conditionsGaranties.assignExprComplexe(typeExpr, idExprComplexe, codeExprComplexe, libExpr);
                $("#CurrentConditionExprComp").clear();
                ReactivateShortCut();
            }
            else {
                ShowCommonFancy("Warning", "", "Veuillez sélectionner une expression complexe", 300, 80, true, true);
            }
        });

        $("#btnCloseExpr").kclick(function () {
            if ($("#CurrentConditionExprComp").val() != "") {
                var typeExpr = $("#TypeExpressionComplexe").val();
                var codeCondition = $("#CurrentConditionExprComp").val();
                if ($("#CurrentCodeExprComp").val() == "") {
                    if (typeExpr == "LCI") {
                        $("#LCIUnite¤" + codeCondition).clear();
                        $("#LCIValeur¤" + codeCondition).removeAttr("readonly").removeClass("readonly");
                    }
                    if (typeExpr == "Franchise") {
                        $("#FranchiseUnite¤" + codeCondition).clear();
                        $("#FranchiseValeur¤" + codeCondition).removeAttr("readonly").removeClass("readonly");
                    }
                }
                $("#CurrentConditionExprComp").clear();
                $("#CurrentCodeExprComp").clear();
            }
            $("#divDataExprComp").html("");
            $("#divExprComplexe").hide();
            ReactivateShortCut();
        });

        $("input[type=radio][name=setExpr]").change(function () {
            if ($("#CurrentConditionExprComp").val() != "")
                $("#btnValideExpr").show();
        });
        $("img[name='editExpr']").click(function () {
            if ($(this).hasClass("CursorPointer")) {
                var val = $(this).attr('id').split('¤')[1];
                $("#setExpr¤" + val).attr("checked", "checked");

                DisplayDetailsExpr(val);
            }
        });
        $("img[name='readExpr']").click(function () {
            if ($(this).hasClass("CursorPointer")) {
                var val = $(this).attr("id").split("¤")[1];
                if ($("#ExprReadOnly").val() != "True")
                    $("#setExpr¤" + val).attr("checked", "checked");

                DisplayDetailsExpr(val, true);
            }
        });
        $("img[name='copyExpr']").click(function () {
            if ($(this).hasClass("CursorPointer")) {
                var val = $(this).attr("id").split("¤")[1];
                DuplicateExpr(val);
            }
        });
        $("img[name='delExpr']").click(function () {
            if ($(this).hasClass("CursorPointer")) {
                conditionsGaranties.confirmDeleteExpression($(this));
            }
        });

        $("input[type='radio'][name='radAddBtn']").offOn("change", function () {
            $("#dvAddMode").hide();
            $("#imgAddExpr").attr("src", "/Content/Images/plusajouter1616.png");
            if ($(this).val() == "ref" || $(this).val() == "mdl") {
                OpenReferentiel($(this).val());
            }
            else {
                OpenAddExpr();
            }
        });

        $("th[name='headerTriExpr']").kclick(function () {
            var colTri = $(this).attr('albcontext');
            var img = $("img[albcontext='" + colTri + "']").attr('src').substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
            img = img.substr(0, img.lastIndexOf('.'));
            TrierExprComplexe(colTri, img);
        });
    };
    //------------Affecte l'expression complexe à la condition sélectionnée-------
    this.assignExprComplexe = function (typeExpr, idExprComplexe, codeExprComplexe, libExpr) {
        var codeCondition = $("#CurrentConditionExprComp").val();
        var exprComplexe = idExprComplexe + "¤" + codeExprComplexe;
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/SaveExprComplexe",
            data: { typeExpr: typeExpr, codeCondition: codeCondition.split('_')[0], codeExpr: idExprComplexe, codeFormule: codeFormule, codeOption: codeOption },
            success: function (data) {
                if (typeExpr == "LCI") {
                    $("#LCIType¤" + codeCondition).clear();
                    $("#divLCIType¤" + codeCondition).text("");
                    $("#LienLCIComplexe¤" + codeCondition).val(exprComplexe);
                    $("div[name=OffLCIComplexe¤" + codeCondition + "]").addClass("None");
                    $("div[name=OnLCIComplexe¤" + codeCondition + "]").removeClass("None");
                    $("#LockLienLCIComplexe¤" + codeCondition).text(codeExprComplexe);
                    $("#LockLienLCIComplexe¤" + codeCondition).attr("albhref", "#" + exprComplexe);
                    $("#LockLienLCIComplexe¤" + codeCondition).attr("title", libExpr);
                    $("#EditLienLCIComplexe¤" + codeCondition).text(codeExprComplexe);
                    $("#EditLienLCIComplexe¤" + codeCondition).attr("albhref", "#" + exprComplexe);
                    $("#EditLienLCIComplexe¤" + codeCondition).attr("title", libExpr);
                }
                if (typeExpr == "Franchise") {
                    $("#FranchiseType¤" + codeCondition).clear();
                    $("#divFranchiseType¤" + codeCondition).text("");
                    $("#LienFRHComplexe¤" + codeCondition).val(exprComplexe);
                    $("div[name=OffFRHComplexe¤" + codeCondition + "]").addClass("None");
                    $("div[name=OnFRHComplexe¤" + codeCondition + "]").removeClass("None");
                    $("#LockLienFRHComplexe¤" + codeCondition).text(codeExprComplexe);
                    $("#LockLienFRHComplexe¤" + codeCondition).attr("albhref", "#" + exprComplexe);
                    $("#LockLienFRHComplexe¤" + codeCondition).attr("title", libExpr);
                    $("#EditLienFRHComplexe¤" + codeCondition).text(codeExprComplexe);
                    $("#EditLienFRHComplexe¤" + codeCondition).attr("albhref", "#" + exprComplexe);
                    $("#EditLienFRHComplexe¤" + codeCondition).attr("title", libExpr);
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request, true);

            }
        });
    };
    //--------------Demande confirmation pour la suppression d'une expression----------
    this.confirmDeleteExpression = function (elem) {
        var elemId = elem.attr("id");
        var utiliseExpr = $("#useExpr¤" + elemId.split("¤")[1]).val();
        if (utiliseExpr == "1") {
            common.dialog.error("Expression utilisée, impossible de la supprimer");
        }
        else {
            $("#CurrentExpression").val(elemId.split("¤")[1]);
            ShowCommonFancy("Confirm", "DelExpression", "Veuillez confirmer la suppression de l'expression", 350, 130, true, true);
        }
    };
    //-----------------Vérifie les données de la LCI-----------------------
    this.checkFranchiseValue = function (idRow) {
        var error = false;

        var fraVal = $("#FranchiseValeurCPX¤" + idRow).autoNumeric('get');
        var fraUnite = $("#FranchiseUniteCPX¤" + idRow).val();
        var fraType = $("#FranchiseType¤" + idRow).val();
        if (fraVal == "" || fraUnite == "" || fraType == "" || fraVal == 0) {
            $("#FranchiseValeurCPX¤" + idRow).addClass("requiredField");
            $("#FranchiseUniteCPX¤" + idRow).addClass("requiredField");
            $("#FranchiseType¤" + idRow).addClass("requiredField");
            error = true;
        }
        if (fraUnite == "%" && (parseFloat(fraVal) < 0 || parseFloat(fraVal) > 100)) {
            $("#FranchiseValeurCPX¤" + idRow).addClass("requiredField");
            $("#FranchiseUniteCPX¤" + idRow).addClass("requiredField");
            error = true;
        }
        var fraMini = $("#FranchiseMini¤" + idRow).autoNumeric('get');
        var fraMaxi = $("#FranchiseMaxi¤" + idRow).autoNumeric('get');
        if (fraMini != "" && fraMaxi != "") {
            if (parseFloat(fraMini) > parseFloat(fraMaxi)) {
                $("#FranchiseMini¤" + idRow).addClass("requiredField");
                $("#FranchiseMaxi¤" + idRow).addClass("requiredField");
                error = true;
            }
        }
        var fraDebut = $("#FranchiseDebut¤" + idRow).val();
        var fraFin = $("#FranchiseFin¤" + idRow).val();
        if (fraDebut != "" && fraFin != "") {
            if (!checkDateById("FranchiseDebut¤" + idRow, "FranchiseFin¤" + idRow)) {
                $("#FranchiseDebut¤" + idRow).addClass("requiredField");
                $("#FranchiseFin¤" + idRow).addClass("requiredField");
                error = true;
            }
        }
        return error;
    };

    window.mapElementsFieldNamesErrors = [
        { fieldname: "LCI", element: function (id) { return buildLineSelectors("LCI", id); }, paramError: "TargetId", className: "invalid-input-no-borders" },
        { fieldname: "FRH", element: function (id) { return buildLineSelectors("Franchise", id); }, paramError: "TargetId", className: "invalid-input-no-borders" },
        { fieldname: "ASSIETTE", element: function (id) { return buildLineSelectors("Assiette", id); }, paramError: "TargetId", className: "invalid-input-no-borders" },
        { fieldname: "PRIME", element: function (id) { return buildLineSelectors("TauxForfaitHT", id); }, paramError: "TargetId", className: "invalid-input-no-borders" }
    ];

    function buildLineSelectors(type, id) {
        let isLciOrFrh = type === "LCI" || type === "Franchise";
        let a = [
            "#tblConditions [name='lock'][id$='_" + id + "'] [id^='div" + type + "Valeur']",
            "#tblConditions [name='lock'][id$='_" + id + "'] [id^='div" + type + "Unite']",
            "#tblConditions [name='lock'][id$='_" + id + "'] [id^='" + (isLciOrFrh ? "tdLock" : "div") + type + "Type']"
        ];
        if (type === "TauxForfaitHT") {
            a.pop();
        }
        return a;
    }

    //---------------------Validation de la page--------------------
    this.validate = function (returnHome) {
        var conditionForm = $("#Condition_Formule").val().split(' - ');
        var lettre = conditionForm[0];
        var libelle = conditionForm[1];

        $(".requiredField").removeClass('requiredField');

        let error = !ValidateLCIFranchise("LCI", "Generale");
        error = !ValidateLCIFranchise("Franchise", "Generale") || error;
        error = !ValidateLCIFranchise("LCI", "Risque") || error;
        error = !ValidateLCIFranchise("Franchise", "Risque") || error;
        if (error) {
            return false;
        }

        ShowLoading();

        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/ValidationConditions",
            data: {
                argCodeOffre: $("#Offre_CodeOffre").val(),
                argType: $("#Offre_Type").val(),
                argVersion: $("#Offre_Version").val(),
                codeAvn: $("#NumAvenantPage").val(),
                argCodeFormule: $("#CodeFormule").val(),
                argCodeOption: $("#CodeOption").val(),

                argValeurLCI: $("#Valeur_LCIGenerale").val().replace('.', ','),
                argUniteLCI: $("#Unite_LCIGenerale").val(),
                argTypeLCI: $("#Type_LCIGenerale").val(),
                argIndexeLCI: $("#IsIndexe_LCIGenerale").is(':checked'),
                argLienCpxLCI: $("#idLienCpx_LCIGenerale").val(),

                argValeurFranchise: $("#Valeur_FranchiseGenerale").val().replace('.', ','),
                argUniteFranchise: $("#Unite_FranchiseGenerale").val(),
                argTypeFranchise: $("#Type_FranchiseGenerale").val(),
                argIndexeFranchise: $("#IsIndexe_FranchiseGenerale").is(':checked'),
                argLienCpxFranchise: $("#idLienCpx_FranchiseGenerale").val(),

                argValeurLCIRisque: $("#Valeur_LCIRisque").val().replace('.', ','),
                argUniteLCIRisque: $("#Unite_LCIRisque").val(),
                argTypeLCIRisque: $("#Type_LCIRisque").val(),
                argLienCpxLCIRisque: $("#idLienCpx_LCIRisque").val(),

                argValeurFranchiseRisque: $("#Valeur_FranchiseRisque").val().replace('.', ','),
                argUniteFranchiseRisque: $("#Unite_FranchiseRisque").val(),
                argTypeFranchiseRisque: $("#Type_FranchiseRisque").val(),
                argLienCpxFranchiseRisque: $("#idLienCpx_FranchiseRisque").val(),

                argExpAssiette: "H",
                argLettre: lettre,
                argLibelle: libelle,
                tabGuid: $("#tabGuid").val(),
                saveCancel: returnHome ? 1 : 0,
                paramRedirect: $("#txtParamRedirect").val(),
                modeNavig: $("#ModeNavig").val(),
                primeGareatTheorique: $("#PrimeTheorique_gareat").hasVal() ? $("#PrimeTheorique_gareat").val() : null,
                primeGareat: $("#Prime_gareat").hasVal() ? $("#Prime_gareat").val() : null,
                isReadOnly: conditionsGaranties.isReadonly()
            },
            success: function (data) {
                if (data == "KO") {
                    ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                    CloseLoading();
                    return false;
                }
                if (data.split(";").length > 0) {
                    var tErrorMsg = data.split(";");
                    if (tErrorMsg[0] == "ERRORMSG") {
                        common.dialog.error(tErrorMsg[1]);
                        CloseLoading();
                        return false;
                    }
                }
                conditionsGaranties.redirectValidationConditions(returnHome);
            },
            error: function (request) {
                let msg = common.error.show(request);
                //$("#btnDialogOk").click(function () {
                //    $("#divDialogInFancy").hide();
                //});
            }
        });
    };
    //redirection après validation
    this.redirectValidationConditions = function (returnHome) {
        var conditionForm = $("#Condition_Formule").val().split(' - ');
        var lettre = conditionForm[0];
        var libelle = conditionForm[1];
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/RedirectValidationConditions",
            data: {
                paramRedirect: $("#txtParamRedirect").val(),
                argCodeOffre: $("#Offre_CodeOffre").val(),
                argType: $("#Offre_Type").val(),
                argVersion: $("#Offre_Version").val(),
                argCodeFormule: $("#CodeFormule").val(),
                argCodeOption: $("#CodeOption").val(),
                argCodeRisque: $("#CodeRisque").val(),
                argLettre: lettre,
                argLibelle: libelle,
                tabGuid: $("#tabGuid").val(),
                saveCancel: returnHome ? 1 : 0,
                modeNavig: $("#ModeNavig").val(),
                addParamType: addParamType, addParamValue: addParamValue,
                isForceReadOnly: $("#IsForceReadOnly").val()
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
                $("#btnDialogOk").click(function () {
                    $("#divDialogInFancy").hide();
                });
            }
        });
    }
    //----------Reset de la ligne de conditions-----------------
    this.resetLine = function (codeGarantie) {

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ConditionsGarantie/CancelGarantie",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeGarantie: codeGarantie.split("_")[1], codeCondition: codeGarantie.split('_')[0],
                oldFRHExpr: $("input[id='oldLienFRHComplexe¤" + codeGarantie + "']").val().split('¤')[0], oldLCIExpr: $("input[id='oldLienLCIComplexe¤" + codeGarantie + "']").val().split('¤')[0],
                modeNavig: $("#ModeNavig").val(), isReadOnly: conditionsGaranties.isReadonly()
            },
            success: function (data) {
                $.when(
                    conditionsGaranties.reloadLine(data, null, codeGarantie)
                ).done(
                    function () {
                        isConditionFaulted = false;
                        saveError = false;
                        CloseLoading();
                    });
            },
            error: function (request) {
                common.error.showXhr(request);
                saveError = true;
            }
        });
    };

    this.initDetailsPopup = function () {
        //$(document).off([customEvents.detailsGarantie.loaded, customEvents.inventaireGarantie.loaded].join(" "));
        $(document).on([customEvents.detailsGarantie.loaded, customEvents.inventaireGarantie.loaded].join(" "), function () {
            var hiddenZones = $(".ko-cloak.conceal");
            hiddenZones.removeClass("conceal");
        });
        //$(document).off(customEvents.detailsGarantie.cancelledEdit);
        $(document).on(customEvents.detailsGarantie.cancelledEdit, function () {
            common.knockout.components.disposeFromDialog(affaire.formule.detailsGarantie.componentName);
        });
    };

    this.formatDecimals = function (codeCondition, autoNumericMode) {
        autoNumericMode = autoNumericMode || "init";
        if (typeof codeCondition === "string" && codeCondition) {
            common.autonumeric.apply($("input[id$=" + codeCondition + "][albMask=decimal]"), autoNumericMode, "decimal", null, null, 3, '99999999999.999', null);
            common.autonumeric.apply($("span[id$=" + codeCondition + "][albMask=decimal]"), autoNumericMode, "decimal", null, null, 3, '99999999999.999', null);
            $("[id$=" + codeCondition + "][albmask='pourcentdecimal'], [id$=" + codeCondition + "][albmask='pourmilledecimal']").each(function () {
                if (this.id.indexOf("TauxForfaitHTValeur") >= 0) {
                    let unit = $(this).parents("td[id*='TauxForfaitHTValeur']").siblings("[id^='divTauxForfaitHTUnite'][name='lock']").text().trim();
                    //common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"), null, null, (unit == "%" || unit == "%0" ? 3 : null));
                    common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"), null, null, 3);
                }
                else {
                    common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"));
                }
            });
            common.autonumeric.apply($("input[id$=" + codeCondition + "][albMask=pourcentdecimal]"), autoNumericMode, "pourcentdecimal", "");
            common.autonumeric.apply($("span[id$=" + codeCondition + "][albMask=pourcentdecimal]"), autoNumericMode, "pourcentdecimal", "");
        }
        else {
            common.autonumeric.applyAll(autoNumericMode, "decimal", " ", null, 3, '99999999999.999', null);
            $("[albmask='pourcentdecimal'], [albmask='pourmilledecimal']").each(function () {
                if (this.id.indexOf("TauxForfaitHTValeur") >= 0) {
                    let unit = $(this).parents("[id*='TauxForfaitHTValeur']").siblings("[id^='divTauxForfaitHTUnite'][name='lock']").text().trim();
                //common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"), null, null, (unit == "%" || unit == "%0" ? 3 : null));
                    common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"), null, null, 3);
                }
                else {
                    common.autonumeric.apply($(this), autoNumericMode, $(this).attr("albmask"));
                }
            });
        }

        if (!$("#TauxRetenu_gareat").data("autoNumeric")) {
            common.autonumeric.apply($("#TauxRetenu_gareat"), "init", "pourcentdecimal", "", null, 2, 100.00, 0);
            common.autonumeric.apply($("#PrimeTheorique_gareat, #Prime_gareat"), "init", "decimal", " ", null, 2, 9999999999999999.99, -9999999999999999.99);
        }
    };
    this.mapAppel = function (typeVue, typeAppel, nomEcran, modeAcess) {
        if (modeAcess == undefined) {
            modeAcess = "";
        }
        let vue_appel = typeVue + typeAppel;
        const pageName = typeof nomEcran === "string" ? nomEcran.toLowerCase().trim() : "";
        $("#MenuLayout select[id=Unite_" + vue_appel + "]").offOn("change", function () {
            if (nomEcran == "EngagementParTraite") {
                ToggleButton();
            }
            if (nomEcran == "DetailsEngagement") {
                ToggleButton();
                LCIControlReadOnly();
            }
            var valLCIUnite = $(this).val();

            $("#Valeur_" + vue_appel).removeAttr('readonly', 'readonly').removeClass('readonly');
            if ($("#Unite_" + vue_appel).val() == "") {
                $("#Valeur_" + vue_appel).clear();
                $("#Type_" + vue_appel).clear();
                $("#idLienCpx_" + vue_appel).val('0');
                $("#LienCpx_" + vue_appel).addClass("None");
                $("#divType_" + vue_appel).removeClass("None");
                $("#divLienCpx_" + vue_appel).addClass("None");
                $("input[id=IsIndexe_" + vue_appel + "]").uncheck();
            }
            if ($("#Unite_" + vue_appel).val() == "CPX") {
                $("#LienCpx_" + vue_appel).removeClass("None");
                $("#divType_" + vue_appel).addClass("None");
                $("#Valeur_" + vue_appel).attr('readonly', 'readonly').addClass('readonly').clear();
                $("#divLienCpx_" + vue_appel).removeClass("None");
                $("#divLienCpx_" + vue_appel).addClass("DivLienCpx");
                $("#TypeOperation").val("Ajout");
            }
            else {
                $("#idLienCpx_" + vue_appel).val('0');
                $("#LienCpx_" + vue_appel).addClass("None");
                $("#divType_" + vue_appel).removeClass("None");
                $("#divLienCpx_" + vue_appel).addClass("None");
            }
            ChangeFormatInfoOffre($(this), typeVue, typeAppel);
            if (valLCIUnite == "CPX") {
                if (typeAppel == "Risque" || typeAppel == "Generale") {
                    OpenExpComplexeGenRsq(typeVue, typeAppel);
                }
                else {
                    conditionsGaranties.openExpComplexe(typeVue);
                }
            }

            if (vue_appel === lciGen && pageName === "conditionsgaranties") {
                setTimeout(conditionsGaranties.computeGareat, 10);
            }
        });
        $("#Valeur_" + vue_appel).offOn("change", function () {
            if (pageName === "engagementpartraite") {
                ToggleButton();
            }
            else if (pageName === "detailsengagement") {
                ToggleButton();
                LCIControlReadOnly();
            }
            if ($("#Valeur_" + vue_appel).val() == "") {
                $("#Unite_" + vue_appel).clear();
                $("#Type_" + vue_appel).clear();
                $("input[id=IsIndexe_" + vue_appel + "]").attr('checked', false);
            }

            if (vue_appel === lciGen && pageName === "conditionsgaranties") {
                setTimeout(conditionsGaranties.computeGareat, 10);
            }
        });
        MapLinkExprComplexeGenRsq(typeVue, typeAppel);
        if (typeVue.trim().toUpperCase() == "LCI" && typeAppel.trim().toUpperCase() == "GENERALE") {
            $("#Valeur_" + vue_appel).offOn("keyup", function (e) {
                if (e.key === "Enter") {
                    let x = this;
                    setTimeout(function () { $(x).change(); });
                }
            });
            if (modeAcess) {
                let a = ["#Valeur_" + vue_appel, "#Unite_" + vue_appel, "#Type_" + vue_appel, "#IsIndexe_" + vue_appel];
                $(a.join(",")).makeReadonly(true);
            }
        }
    };
    this.isReadonly = function () {
        return window.isReadonly || window.isModifHorsAvenant || isAvnDisabled;
    };

    this.computeGareat = function () {
        if (!$(".bloc-gareat").exists()) {
            return;
        }
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
                })
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
            common.$postJson("/ConditionsGarantie/ComputeGareat", model, true)
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
};

var conditionsGaranties = new ConditionsGaranties();

$(document).ready(function () {
    isAvnDisabled = $("#InformationsCondition_IsAvnDisabled").hasTrueVal();
    conditionsGaranties.initPage();
    $("#overlayCG").hide();
    conditionsGaranties.initDetailsPopup();
});
