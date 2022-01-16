
var ListeRegularisations = function () {
    //--------Map les éléments de la page---------
    this.initPage = function () {

        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

        listeReguls.loadTypeRegul();
        toggleDescription();

        $("#btnFermer").kclick(function () {
            var tabGuid = $("#tabGuid").val();
            DeverouillerUserOffres(tabGuid);
            listeReguls.redirect("RechercheSaisie", "Index");
        });
        $("#CreateReg").addClass("notDisplayed");

        $("#btnValidAdd").kclick(function () {

            if ($('input:radio[name=typeRegule]').is(":checked") === false) {
                ShowCommonFancy("Error", "", "Veuillez sélectionner un mode de régularisation.", true, true, true);
                return;
            }

            if ($('input:radio[name=niveauRegule]').is(":checked") === false) {
                ShowCommonFancy("Error", "", "Veuillez sélectionner un niveau de régularisation.", true, true, true);
                return;
            }

            listeReguls.consultOrEdit($("#OffreReadOnly").val(), "CREATE", $("input[type='radio']:checked").val(), "0");
        });

        $("#btnCancelAdd").kclick(function () {

            $("#CodeTypeRegul").val($("#CodeTypeRegul option:first").val());
            $("#CodeTypeRegul").change();

            DeverouillerUserOffres($("#tabGuid").val());
            $("input[type='radio']").uncheck();
            $("#dvAddRegule").hide();
        });

        listeReguls.initCourtier();
        listeReguls.initListeRegularisation();

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "CancelCourtierRegule":
                    listeReguls.cancelCourtierRegul();
                    break;
                case "DelRegul":
                    listeReguls.deleteRegul($("#hiddenInputId").val());
                    break;
                case "DelPeriode":
                    listeReguls.resetPeriodes();
                    break;
                case "Cancel":
                    $("#divInfoSaisieGar").hide();
                    $("#divDataInfoSaisieGar").clearHtml();
                    break;
                case "ChangeRegulType":
                    listeReguls.changeRegulType();
                    break;
            }
            $("#hiddenInputId").clear();
            $("#hiddenAction").clear();
        });

        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
            $("#hiddenInputId").clear();
        });

        $("#btnAnnulUpdate").kclick(function () {
            DeverouillerUserOffres($("#tabGuid").val());
            $("#divUpdateRegul").hide();
            $("#divDataUpdateRegul").clearHtml();
        });

        $("#btnValidUpdate").kclick(function () {
            ShowCommonFancy("Confirm", "ChangeRegulType", "Vous allez modifier le type de traitement de cet acte de gestion. Confirmez-vous cette modification?", 300, 80, true, true, true);
        });

        if (window.isReadonly) {
            $("#MotifAvt").disable();
            $("#ExerciceRegule").disable(true);
            $("#PeriodeDeb").disable(true);
            $("#PeriodeFin").disable(true);
        }
    };

    this.loadTypeRegul = function () {
        $.ajax({
            type: "GET",
            url: "/CreationRegularisation/GetTypeReguls",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeAvn: $("#NumInterne").val()
            },
            success: function (data) {

                $.each(data, function (index, item) {
                    $('#CodeTypeRegul').append($("<option></option>")
                        .attr("value", item.Code)
                        .attr("alb-codetpcn1", item.CodeTpcn1 > 0 ? "O" : "N")
                        .attr("alb-codetpca1", item.CodeTpca1)
                        .attr("alb-codetpca2", item.CodeTpca2)
                        .text(item.Libelle));
                });

                $('#CodeTypeRegul').change();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

        $('#CodeTypeRegul').offOn("change", function () {

            var element = $(this).find('option:selected');
            var tpca1 = element.attr("alb-codetpca1");
            var tpca2 = element.attr("alb-codetpca2");

            $('#rdReguleModif').prop('checked', false);
            $('#rdReguleSeule').prop('checked', false);
            $('#rdNiveauReguleEntete').prop('checked', false);
            $('#rdNiveauReguleRisque').prop('checked', false);
            $('#rdNiveauReguleManuel').prop('checked', false);

            listeReguls.initTypeRegule(tpca1)
            listeReguls.initNiveauRegule(tpca2);
        });
    };

    this.resetPeriodes = function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/SupressionDatesRegularisation",
            data: {
                reguleId: $("#ReguleId").val()
            },
            success: function (data) {
                $("#ExerciceRegule").enable();
                $("#PeriodeDeb").enable();
                $("#PeriodeFin").enable();
                $("#deleteMod").val('D');
                $("#ExerciceRegule").clear();
                $("#PeriodeDeb").clear();
                $("#PeriodeFin").clear();
                $("#btnReguleSuivant").attr("disabled", "disabled");
                $("#inCourtier").clear();
                $("#inHorsCATNAT").val(0);
                $("#inCATNAT").val(0);
                $("#inQuittancement").clear();
                $("#btnDeleteRegule").removeClass("CursorPointer");
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    };

    this.changeRegulType = function () {
        let typeTraitement = $("#TypeTraitement").val();
        let datedebutavn = $("#DateDebutAvenant").val();
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let datefin = $("#btnUpdateTypeRegule").attr("albdatefinavn");
        let codeAvn = $("#btnUpdateTypeRegule").attr("albcodeavn");
        let numReg = $("#btnUpdateTypeRegule").attr("albreguleid");

        datefin = datefin != undefined ? datefin.split(' ')[0] : "";
        let anneeFinReg = datefin != "" ? datefin.split('/')[2] : "";
        let moisFinReg = datefin != "" ? datefin.split('/')[1] : "";
        let jourFinReg = datefin != "" ? datefin.split('/')[0] : "";

        $(".requiredField").removeClass("requiredField");
        let error = false;
        if (typeTraitement == "") {
            $("#TypeTraitement").addClass("requiredField");
            error = true;
        }
        if (datedebutavn == "") {
            $("#DateDebutAvenant").addClass("requiredField");
            error = true;
        }
        if (error == true) {
            common.dialog.bigError("Merci de renseigner les champs.", true);
            return false;
        }

        if (datedebutavn != "") {
            var anneeFinAvn = datedebutavn.split('/')[2];
            var moisFinAvn = datedebutavn.split('/')[1];
            var jourFinAvn = datedebutavn.split('/')[0];
            //alert("datefinavn = " + anneeFinAvn + moisFinAvn + jourFinAvn + " --- datefinregule = " + anneeFinReg + moisFinReg + jourFinReg);
        }

        if ((anneeFinAvn + moisFinAvn + jourFinAvn) <= (anneeFinReg + moisFinReg + jourFinReg)) {
            $("#DateDebutAvenant").addClass("requiredField");
            common.dialog.bigError("La date d’effet de l’avenant de modification doit être postérieure à la dernière période de régularisation.", true);
            return false;
        }

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/DoUpdateType",
            data: {
                codeOffre: codeOffre, version: version, type: type, typeTraitement: typeTraitement, dateDebutAvn: datedebutavn, codeAvn: codeAvn, numReg: numReg
            },
            success: function (data) {
                $("#divUpdateRegul").hide();
                $("#divDataUpdateRegul").clearHtml();

                listeReguls.reloadListeRegularisation();
                DeverouillerUserOffres($("#tabGuid").val());

                CloseLoading();
            },
            error: function (request) {
                DeverouillerUserOffres($("#tabGuid").val());
                common.error.showXhr(request);
            }
        });
    };

    //--------Change l'exercice de l'attestation--------
    this.changeExercice = function () {
        $("#PeriodeValide").val("0");
        $(".requiredField").removeClass("requiredField");
        $("#PeriodeDeb").clear();
        $("#PeriodeFin").clear();
        var deleteMod = $("#deleteMod").val();
        var cancelMod = $("#cancelMod").val();

        if ($("#ExerciceRegule").val() == "") {
            $("#btnSuivant").attr("disabled", "disabled");
            return;
        }

        var regulMode = common.getRegul().mode;

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/ChangeExercice",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeAvn: $("#NumInterne").val(), typeAvt: $("#TypeAvt").val(), exercice: $("#ExerciceRegule").val(), lotId: $("#lotId").val(),
                reguleId: $("#ReguleId").val(), regulMode: regulMode, deleteMod: deleteMod, cancelMod: cancelMod
            },
            success: function (data) {
                $("#dvReguleCourtier").html(data);

                common.autonumeric.applyAll('init', 'decimal');
                common.autonumeric.applyAll('init', 'pourcentdecimal');
                common.autonumeric.applyAll('init', 'year', '', null, 0, 9999, 0);

                listeReguls.initCourtier();

                var retourPGM = $("#inRetourPGM").val();
                if (retourPGM != "") {
                    $("#PeriodeValide").val("1");
                    $("#PeriodeDeb").val(retourPGM.split("_")[0]).removeClass("requiredField");
                    $("#PeriodeFin").val(retourPGM.split("_")[1]).removeClass("requiredField");
                    $("#btnReguleSuivant").removeAttr("disabled");
                }
                CloseLoading();
            },
            error: function (request) {
                $("#ExerciceRegule").addClass("requiredField");

                //RAZ des données
                $("#inCourtier").clear();
                $("#inHorsCATNAT").clear();
                $("#inCATNAT").clear();
                $("#inQuittancement").clear();
                $("#btnModifCourtier").hide();
                $("#btnReguleSuivant").attr("disabled", "disabled");

                common.error.showXhr(request);
            }
        });
    };

    //--------Change la période de l'attestation--------
    this.modifyPeriode = function () {
        $("#PeriodeValide").val("0");
        $(".requiredField").removeClass("requiredField");
        var deleteMod = $("#deleteMod").val();
        var cancelMod = $("#cancelMod").val();
        var regulMode = common.getRegul().mode;

        if ($("#PeriodeDeb").val() != "" && $("#PeriodeFin").val() != "") {
            $("#ExerciceRegule").clear();
            if (checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
                ShowLoading();
                $.ajax({
                    type: "POST",
                    url: "/CreationRegularisation/ChangePeriode",
                    data: {
                        codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                        codeAvn: $("#NumInterne").val(), typeAvt: $("#TypeAvt").val(), periodeDeb: $("#PeriodeDeb").val(),
                        periodeFin: $("#PeriodeFin").val(), lotId: $("#lotId").val(), reguleId: $("#ReguleId").val(), regulMode: regulMode,
                        deleteMod: deleteMod, cancelMod: cancelMod
                    },
                    success: function (data) {
                        $("#dvReguleCourtier").html(data);

                        common.autonumeric.applyAll('init', 'decimal');
                        common.autonumeric.applyAll('init', 'pourcentdecimal');

                        listeReguls.initCourtier();

                        var retourPGM = $("#inRetourPGM").val();
                        if (retourPGM != "") {
                            $("#PeriodeValide").val("1");
                            $("#PeriodeDeb").val(retourPGM.split("_")[0]).removeClass("requiredField");
                            $("#PeriodeFin").val(retourPGM.split("_")[1]).removeClass("requiredField");
                            $("#btnReguleSuivant").removeAttr("disabled");
                        }
                        $("#btnDeleteRegule").addClass("CursorPointer");
                        CloseLoading();
                    },
                    error: function (request) {
                        $("#PeriodeDeb").addClass("requiredField");
                        $("#PeriodeFin").addClass("requiredField");

                        //RAZ des données
                        $("#inCourtier").clear();
                        $("#inHorsCATNAT").clear();
                        $("#inCATNAT").clear();
                        $("#inQuittancement").clear();
                        $("#btnModifCourtier").hide();
                        $("#btnReguleSuivant").attr("disabled", "disabled");

                        common.error.showXhr(request);
                    }
                });
            }
        }
    };

    //----------------Redirection------------------
    this.redirect = function (cible, job, elem) {
        ShowLoading();
        var modeAvt = $("#ModeAvt").val();
        var typAvt = $("#TypeAvt").val();
        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvenant = $("#NumInterne").val();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var codeAvenantExterne = $("#NumAvt").val();
        var lotID = $("#lotId").val();
        var reguleId = $("#ReguleId").val();
        var codeRsq = $("#CodeRsq").val();
        var dateDebReg = $("#inDateDebRsqRegule").val();
        var dateFinReg = $("#inDateFinRsqRegule").val();
        var codeFor = "";
        var codeOpt = "";
        var idGar = "";
        var codeGAr = "";
        var libgar = "";
        var isReadonly = window.isReadonly ? true : false;
        if (elem != undefined && elem != null) {
            codeFor = elem.attr("albcodefor");
            codeOpt = elem.attr("albcodeopt");
            idGar = elem.attr("albidgar");
            codeGAr = elem.attr("albcodgar");
            libgar = elem.attr("alblibgar");
        }
        var isSaveAndQuit = $("#txtSaveCancel").val() == "1";

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/Redirection/",
            data: {
                cible: cible, job: job, codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
                codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotID: lotID, reguleId: reguleId, codeGAr: codeGAr, libgar: libgar,
                addParamType: addParamType, addParamValue: addParamValue, codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(), typAvt: typAvt,
                acteGestionRegule: $("#ActeGestionRegule").val(), dateDebReg: dateDebReg, dateFinReg: dateFinReg, isReadonly: isReadonly, isSaveAndQuit: isSaveAndQuit
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //------------------- Initialise les radios buttons Niveau de Regularisation -------
    this.initNiveauRegule = function (tpca2) {
        if (tpca2 == "E") {
            $('#rdNiveauReguleEntete').show();
            $('#rdNiveauReguleEntete').prop('checked', true);
            $('#rdNiveauLabelEntete').show();
            $('#rdNiveauReguleRisque').hide();
            $('#rdNiveauLabelRisque').hide();
            $('#rdNiveauReguleManuel').hide();
            $('#rdNiveauLabelManuel').hide();
        }
        else if (tpca2 == "R") {
            $('#rdNiveauReguleEntete').hide();
            $('#rdNiveauLabelEntete').hide();
            $('#rdNiveauReguleRisque').show();
            $('#rdNiveauReguleRisque').prop('checked', true);
            $('#rdNiveauLabelRisque').show();
            $('#rdNiveauReguleManuel').hide();
            $('#rdNiveauLabelManuel').hide();
        }
        else if (tpca2 == "M") {
            $('#rdNiveauReguleEntete').hide();
            $('#rdNiveauLabelEntete').hide();
            $('#rdNiveauReguleRisque').hide();
            $('#rdNiveauLabelRisque').hide();
            $('#rdNiveauReguleManuel').show();
            $('#rdNiveauReguleManuel').prop('checked', true);
            $('#rdNiveauLabelManuel').show();
        }
        else {
            $('#rdNiveauReguleEntete').show();
            $('#rdNiveauLabelEntete').show();
            $('#rdNiveauReguleRisque').show();
            $('#rdNiveauLabelRisque').show();
            $('#rdNiveauReguleManuel').show();
            $('#rdNiveauLabelManuel').show();
        }
    };

    //------------------- Initialise les radios buttons Type de Regularisation -------
    this.initTypeRegule = function (tpca1) {
        if (tpca1 == "S") {
            $('#rdReguleModif').hide();
            $('#rdReguleModifLabel').hide();
            $('#rdReguleSeule').show();
            $('#rdReguleSeule').prop('checked', true);
            $('#rdReguleSeuleLabel').show();
        }
        else if (tpca1 == "M") {
            $('#rdReguleSeule').hide();
            $('#rdReguleSeuleLabel').hide();
            $('#rdReguleModif').show();
            $('#rdReguleModif').prop('checked', true);
            $('#rdReguleModifLabel').show();
        }
        else {
            $('#rdReguleModif').show();
            $('#rdReguleModifLabel').show();
            $('#rdReguleSeule').show();
            $('#rdReguleSeuleLabel').show();
        }
    };

    this.consultOrEdit = function (isReadonly, modeAvt, typeAvt, codeAvn, reguleId, regulMode, regulType, regulNiv, regulAvn) {
        let params = common.albParam.buildObject();
        if (["CONSULT", "UPDATE"].indexOf(modeAvt) >= 0 && params.AVNMODE == "CREATE") {
            params.AVNMODE = modeAvt;
        }
        else if (modeAvt === "CREATE") {
            params.AVNMODE = modeAvt;
            params.REGULEID = 0;
        }
        if (!params.AVNMODE) {
            params.AVNMODE = modeAvt;
        }

        reguleId = reguleId != null ? reguleId : $("#ReguleId").val();
        if (reguleId) {
            params.REGULEID = reguleId;
        }
        params.REGULMOD = regulMode ? regulMode : $('#CodeTypeRegul').val();
        params.REGULTYP = regulType ? regulType : $('input[name=typeRegule]:checked').val();
        params.REGULNIV = regulNiv ? regulNiv : $('input[name=niveauRegule]:checked').val();
        params.REGULAVN = regulAvn ? regulAvn : $('#CodeTypeRegul').find('option:selected').attr('alb-codetpcn1');
        params.AVNID = (codeAvn || "").toString() || $("#NumAvenantPage").val();
        params.AVNIDEXTERNE = params.AVNID;
        if (typeAvt == "M" || typeAvt == "AVNRG") {
            params.AVNTYPE = "AVNRG";
        }
        else if (typeAvt == "S" || typeAvt == "REGUL") {
            params.AVNTYPE = "REGUL";
        }

        //let newValue = common.albParam.objectToString(params);
        //let folderParts = [$("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), codeAvn];
        //let idParts = [folderParts.join("_"), $("#tabGuid").val(), common.albParam.objectToString(params, true, true), (isReadonly == "True" ? "ConsultOnly" : "")];
        let isConsulting = params.AVNMODE === "CONSULT";
        common.page.isLoading = true;
        try {
            let stepContext = {
                TabGuid: infosTab.tabGuid,
                ModeNavig: params.AVNMODE === "UPDATE" ? "S" : $("#ModeNavig").val(),
                Folder: { CodeOffre: $("#Offre_CodeOffre").val(), Version: $("#Offre_Version").val(), Type: "P", NumeroAvenant: codeAvn },
                Target: isConsulting ? "ConsulterRegule" : params.REGULMOD === "BNS" ? "EditionBNS" : "EditionRegularisation",
                IsReadonlyTarget: isConsulting,
                IsModifHorsAvenant: false,
                NewProcessCode: params.REGULEID == 0 ? params.AVNTYPE : "",
                AvnParams: params.REGULEID == 0 ? Object.getOwnPropertyNames(params).map(function (x) { return { Key: x, Value: params[x] }; }) : []
            };
            
            common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                if (result.url) {
                    if (!!stepContext.IsReadonlyTarget !== !!result.context.IsReadonlyTarget) {
                        common.page.isLoading = false;
                        let message = "Impossible de modifier le contrat numéro <b>" + $("#Offre_CodeOffre").val() + "</b>"
                            + " car il est actuellement en cours de modification par l'utilisateur <b>" + result.context.LockingUser + "</b>"
                            + ". Voulez-vous l'ouvrir en consultation ?";

                        $("#centerOffreVerrouillee").html(message);
                        $("#MessageOffreVerrouillee").show();

                        // Traitement du bouton "Consulter"
                        $("#btnOVReadOnly").kclick(function () {
                            $("#OffreReadOnly").val("True");
                            listeReguls.consult();
                        });
                        return;
                    }
                    else {
                        document.location.replace(result.url);
                    }
                }
                else {
                    common.page.isLoading = false;
                    switch (result.context.DenyReason.toLowerCase()) {
                        case "citrix":
                            common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                            break;
                        default:
                            common.dialog.error(result.context.DenyReason);
                    }
                }
            });
        }
        catch (e) {
            console.error(e);
            common.error.showMessage(e);
        }
    };

    //------------Ouvre la régularisation en mode consultation-----------
    this.consult = function () {
        var reguleId = $("[name='consulter-regule']").last().attr("albreguleid");
        var typeRegule = $("[name='consulter-regule']").last().attr("albreguletype");
        var codeAvn = $("[name='consulter-regule']").last().attr("albcodeavn");
        var regulMode = $("[name='consulter-regule']").last().data("regulmode");
        var regulType = $("[name='consulter-regule']").last().data("regultype");
        var regulNiv = $("[name='consulter-regule']").last().data("regulniv");
        var regulAvn = $("[name='consulter-regule']").last().data("regulavn");
        listeReguls.consultOrEdit($("#OffreReadOnly").val(), "CONSULT", typeRegule, codeAvn, reguleId, regulMode, regulType, regulNiv, regulAvn);
    };

    //----------Map les éléments de la régularisation---------
    this.initRegulPage = function (modeAvt) {

        $("#dvInfoContrat").removeClass("divInfoContrat").addClass("divInfoContratObs");
        toggleDescription();
        AlternanceLigne("ReguleBody", "", false, null);
        AlternanceLigne("AlerteAvenant", "", false, null);

        common.autonumeric.applyAll('init', 'decimal');
        common.autonumeric.applyAll('init', 'pourcentdecimal');
        common.autonumeric.applyAll('init', 'year', '', null, 0, 9999, 0);
        $("#PeriodeDeb").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#PeriodeFin").datepicker({ dateFormat: 'dd/mm/yy' });

        AffectDateFormat();

        $("#btnFermer").hide();
        $("#btnReguleCancel").show();
        if ($("#PeriodeDeb").val() != "" && $("#PeriodeFin").val() != "") {
            $("#btnReguleSuivant").removeAttr("disabled");
        }
        if ($("#PeriodeDeb").val() == "" && $("#PeriodeFin").val() == "") {
            $("#btnDeleteRegule").removeClass("CursorPointer");
        }
        $("#btnReguleBack").hide();
        $("#btnRegulePrec").hide();

        $("#btnReguleCancel").kclick(function () {
            if ($("#btnReguleCancel").text().toLowerCase() === "annuler") {
                common.page.forceDirectClose();
                return;
            }
            if (window.isReadonly) {
                listeReguls.redirect("RechercheSaisie", "Index");
            }
            else {
                var tabGuid = $("#tabGuid").val();
                DeverouillerUserOffres(tabGuid);
                listeReguls.reloadListeRegularisation();
            }
        });

        $("#btnReguleSuivant").kclick(function () {
            $("#MotifAvt").removeClass("requiredFieOpenSelectionRsqReguleld");
            let codeAvn = $("#NumInterne").val();
            let typeAvt = $("#TypeAvt").val();
            let modeAvt = $("#ModeAvt").val();
            let exercice = $("#ExerciceRegule").val();
            let dateDeb = $("#PeriodeDeb").val();
            let dateFin = $("#PeriodeFin").val();
            let lotId = $("#lotId").val();
            let motifAvt = $("#MotifAvt").val();

            if (motifAvt == "" && !window.isReadonly) {
                $("#MotifAvt").addClass("requiredField");
                ShowCommonFancy("Error", "", "Veuillez remplir les champs vides.", true, true, true);
                return false;
            }
            if (!checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
                common.dialog.error("Incohérence au niveau des dates");
                return false;
            }

            $("#ExerciceRegule").disable(true);
            $("#PeriodeDeb").disable(true);
            $("#PeriodeFin").disable(true);

            listeReguls.OpenSelectionRsqRegule(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, modeAvt);
        });

        $("#ExerciceRegule").offOn("change", function () {
            listeReguls.changeExercice();
        });

        $("#PeriodeDeb, #PeriodeFin").offOn("change", function () {
            listeReguls.modifyPeriode();
        });

        if (modeAvt == "UPDATE" || modeAvt == "CONSULT") {
            $("#ExerciceRegule").makeReadonly(true);
            $("#PeriodeDeb").disable(true);
            $("#PeriodeFin").disable(true);
        }
        else {
            if (($("#erreurPGM").val() != "") && ($("#erreurPGM").val() != undefined)) {
                ShowCommonFancy("Error", "", $("#erreurPGM").val(), 300, 80, true, true, true);
                $("#PeriodeDeb").addClass("requiredField");
                $("#PeriodeFin").addClass("requiredField");
                $("#btnReguleSuivant").attr("disabled", "disabled");
            }
        }

        $("#btnDeleteRegule.CursorPointer").kclick(function () {
            ShowCommonFancy("Confirm", "DelPeriode", "Etes-vous sûr de vouloir supprimer ces dates de régularisation ?", 300, 80, true, true, true);
        });

        if ($("#PeriodeDeb").val() == "" || $("#PeriodeFin").val() == "") {
            $("#PeriodeDeb").enable();
            $("#PeriodeFin").enable();
            $("#ExerciceRegule").enable();
            $("#cancelMod").val('C');
        }

        if (window.isReadonly) {
            $("#MotifAvt").disable();
            $("#btnSaveCancel").removeClass('SaveInfo');
            $("#btnSaveCancel").addClass('NoSaveInfo');
            $("#PeriodeDeb").disable(true);
            $("#PeriodeFin").disable(true);
        }

        $("td[id='linkAlerte']").kclick(function () {
            OpenAlerte($(this));
        });
    };

    //---------Recharge la liste des régularisations---------
    this.reloadListeRegularisation = function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/ReloadListRegule",
            data: { codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val() }, //, id: id
            success: function (data) {
                $("#divInfoRegul").html(data).show();
                $("#dvObservations").hide();

                $("#btnFermer").show();
                $("#btnReguleCancel").hide();
                $("#btnReguleSuivant").hide();

                $("#dvInfoContrat").removeClass("divInfoContratObs").addClass("divInfoContrat");
                listeReguls.initListeRegularisation();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //---------Map les éléments de la liste des régularisation----------
    this.initListeRegularisation = function () {
        AlternanceLigne("BodyRegule", "", false, null);

        $("td[name=linkVisuRegule].navig").kclick(function () {
            let num = $(this).attr("id");
            if (num != undefined) {
                num = num.split("_")[1];
            }
            let numAvn = $(this).attr("albnumavn");
            let reguleId = $(this).attr("albreguleid");
            if (num != undefined) {
                VisualisationDetailsQuittance(num, numAvn, "true", reguleId);
            }
        });

        $("#btnAddRegule").kclick(function () {
            var pos = $(this).offset();
            $.ajax({
                type: "POST",
                url: "/CreationRegularisation/Step1_ChoixPeriode_Lock/",
                data: {
                    id: $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumAvenantPage").val() + $("#tabGuid").val() + "addParamAVN|||" + $("#AddParamValue").val() + "||AVNID|" + $("#NumAvenantPage").val() + "||IGNOREREADONLY|1addParam"
                },
                success: function (data) {
                    if (data !== "True") {
                        CloseLoading();
                        common.dialog.error("Ce contrat est verrouillé, vous ne pouvez pas créer de nouvelle régularisation");
                        return;
                    }

                    //BUG 2044
                    var element = $('#CodeTypeRegul').find('option:selected');
                    var tpca2 = element.attr("alb-codetpca2");

                    listeReguls.initNiveauRegule(tpca2);
                    // END 2044


                    $("#dvAddRegule").show().css({ 'position': 'absolute', 'top': (pos.top - 22) + 'px', 'left': (pos.left - 537) + 'px', 'z-index': 5 });
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });


        });

        $("[name='consulter-regule']").kclick(function () {
            $("#OffreReadOnly").val("True");
            let reguleId = $(this).attr("albreguleid");
            let typeRegule = $(this).attr("albreguletype");
            let codeAvn = $(this).attr("albcodeavn");
            let regulMode = $(this).data("regulmode");
            let regulType = $(this).data("regultype");
            let regulNiv = $(this).data("regulniv");
            let regulAvn = $(this).data("regulavn");
            listeReguls.consultOrEdit($("#OffreReadOnly").val(), "CONSULT", typeRegule, codeAvn, reguleId, regulMode, regulType, regulNiv, regulAvn);
        });

        $("img[id='btnUpdateRegule']").kclick(function () {
            let reguleId = $(this).attr("albreguleid");
            let typeRegule = $(this).attr("albreguletype");
            let codeAvn = $(this).attr("albcodeavn");
            let regulMode = $(this).data("regulmode");
            let regulType = $(this).data("regultype");
            let regulNiv = $(this).data("regulniv");
            let regulAvn = $(this).data("regulavn");

            listeReguls.consultOrEdit($("#OffreReadOnly").val(), "UPDATE", typeRegule, codeAvn, reguleId, regulMode, regulType, regulNiv, regulAvn);
        });

        $("img[id='btnDeleteRegule']").kclick(function () {
            //TODO ECM : ATTENTE SPEC TECH
            var reguleId = $(this).attr('albreguleid');
            $("#hiddenInputId").val(reguleId);
            ShowCommonFancy("Confirm", "DelRegul", "Etes-vous sûr de vouloir supprimer la régularisation n°" + $(this).attr('albreguleid') + " ?", 300, 80, true, true, true);
        });

        $("img[id='btnUpdateTypeRegule']").kclick(function () {
            var traitement = $("td[class='tdTraitementRegule']").attr("title");
            var codeOffre = $("#Offre_CodeOffre").val();
            var version = $("#Offre_Version").val();
            var type = $("#Offre_Type").val();
            var datefin = $(this).attr("albdatefinavn");
            var periodicite = $("#periodicite").val();

            $.ajax({
                type: "POST",
                url: "/CreationRegularisation/UpdateTypeRegul/",
                data: {
                    traitement: traitement, codeOffre: codeOffre, version: version, type: type, datefin: datefin, periodicite: periodicite,
                    //id: $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumAvenantPage").val() + $("#tabGuid").val()
                    id: $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumAvenantPage").val() + "addParamAVN|||" + $("#AddParamValue").val() + "||IGNOREREADONLY|1addParam" + $("#tabGuid").val()
                },
                success: function (data) {
                    if (data.indexOf("TraitementActuel") === -1) {
                        CloseLoading();
                        common.dialog.error("Ce contrat est verrouillé, vous ne pouvez pas modifier le type de traitement");
                        return;
                    }
                    $("#divUpdateRegul").show();
                    $("#divDataUpdateRegul").html(data);
                    $("#DateDebutAvenant").datepicker({ dateFormat: 'dd/mm/yy' });
                    AffectDateFormat();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        });
    };

    //----------Ouvre la div flottante de gestion des courtiers-------
    this.initCourtier = function () {
        $("#ddlCourtiers, #ddlCourtiersCom, #ComHCATNAT, #ComCATNAT").offOn("change", function () {
            $("#btnCancelCourtier").show();
            $("#btnCloseCourtier").hide();
            $("#btnValidCourtier").removeAttr("disabled");
        });

        $("#ddlQuittancements").offOn("change", function () {
            $("#btnCancelCourtier").show();
            $("#btnCloseCourtier").hide();
            $("#btnValidCourtier").removeAttr("disabled");


            if ($("#ddlQuittancements").val() == "D") {
                $("#ComHCATNAT").val("0");
                $("#ComCATNAT").val("0");
                $("#ComHCATNAT").makeReadonly(true);
                $("#ComCATNAT").makeReadonly(true);
            }
            else {
                $("#ComHCATNAT").val($("#inHorsCATNAT").val());
                $("#ComCATNAT").val($("#inCATNAT").val());
                $("#ComHCATNAT").enable();
                $("#ComCATNAT").enable();
            }
        });

        $("#btnModifCourtier").kclick(function () {
            $("#dvChoixCourtier").show();
            $("#btnReguleSuivant").disable();
        });

        $("#btnCancelCourtier").kclick(function () {
            ShowCommonFancy("Confirm", "CancelCourtierRegule",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true, true);
        });

        $("#btnCloseCourtier").kclick(function () {
            $("#dvChoixCourtier").hide();
            $("#btnReguleSuivant").removeAttr("disabled");
        });

        $("#btnValidCourtier:not(:disabled)").kclick(function () {
            listeReguls.assignCourtier();
        });
    };

    //--------Annule les modifications effectuées dans la div flottante de gestion des courtiers---------
    this.cancelCourtierRegul = function () {
        $("#ddlCourtiers").val($("#oldCourtier").val());
        $("#ddlCourtiersCom").val($("#oldCourtierCom").val());
        $("#ddlQuittancements").val($("#oldQuittancement").val());
        $("#ComHCATNAT").val($("#oldHCATNAT").val());
        $("#ComCATNAT").val($("#oldCATNAT").val());

        $("#dvChoixCourtier").hide();

        $("#btnCancelCourtier").hide();
        $("#btnCloseCourtier").show();
        $("#btnValidCourtier").disable();
    };

    //-------Valide les modifications effectuées dans la div flottante de gestion des courtiers-----
    this.assignCourtier = function () {
        $("#oldCourtier").val($("#ddlCourtiers").val());
        $("#oldCourtierCom").val($("#ddlCourtiersCom").val());
        $("#oldQuittancement").val($("#ddlQuittancements").val());
        $("#oldHCATNAT").val($("#ComHCATNAT").val());
        $("#oldCATNAT").val($("#ComCATNAT").val());

        $("#inCourtier").val($("#ddlCourtiers").val());
        $("#inCourtierCom").val($("#ddlCourtiersCom").val());
        $("#inHorsCATNAT").val($("#ComHCATNAT").val());
        $("#inCATNAT").val($("#ComCATNAT").val());
        $("#inQuittancement").val($("#ddlQuittancements").find("option:selected").attr("title")).attr("albcode", $("#ddlQuittancements").val());

        $("#dvChoixCourtier").hide();
        $("#btnReguleSuivant").removeAttr("disabled");

        $("#btnCancelCourtier").hide();
        $("#btnCloseCourtier").show();
        $("#btnValidCourtier").disable();
    };

    //-------Supprime une régularisation-------
    this.deleteRegul = function (reguleId) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/DeleteRegule",
            data: { codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), reguleId: reguleId },
            success: function (data) {
                $("#divInfoRegul").html(data).show();
                $("#dvObservations").hide();
                $("#btnFermer").show();
                $("#btnReguleCancel").hide();
                $("#btnReguleSuivant").hide();
                $("#dvInfoContrat").removeClass("divInfoContratObs").addClass("divInfoContrat");
                listeReguls.initListeRegularisation();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //#region Risque
    ////////////////////////////////////////////////////////////////////
    /////////////          ZONE DU RISQUE          /////////////////////
    ////////////////////////////////////////////////////////////////////
    //----------Ouvre la sélection des risques disponibles pour la régularisation------
    this.OpenSelectionRsqRegule = function (codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, mode) {
        ShowLoading();
        let reguleId = $("#ReguleId").val();
        let codeICT = "";
        let codeICC = "";
        let tauxCom = "";
        let tauxComCATNAT = "";
        let codeEnc = "";
        let modeleAvtRegule = "";
        let argModeleAvtRegule = "";

        codeICT = $.trim($("#inCourtier").val().split("-")[0]);
        codeICC = $.trim($("#inCourtierCom").val().split("-")[0]);
        tauxCom = $("#inHorsCATNAT").val();
        tauxComCATNAT = $("#inCATNAT").val();
        codeEnc = $("#inQuittancement").attr("albcode");

        modeleAvtRegule = {
            TypeAvt: $.trim(typeAvt),
            NumInterneAvt: codeAvn,
            NumAvt: codeAvn,
            MotifAvt: $("#MotifAvt").val(),
            DescriptionAvt: $("#Description").val(),
            ObservationsAvt: $.trim($("#Observation").html().replace(/<br>/ig, "\n"))
        };
        argModeleAvtRegule = JSON.stringify(modeleAvtRegule);

        if (mode === 'CREATE') {
            var addParamValue = $("#AddParamValue").val();
            var oldAvnId = $("#NumAvenantPage").val();
            var avnId = $("#NumInterne").val();
            if (addParamValue.indexOf("AVNID|" + oldAvnId + "||") !== -1) {
                addParamValue = addParamValue.replace("AVNID|" + oldAvnId + "||", "AVNID|" + avnId + "||");
                addParamValue = addParamValue.replace("AVNIDEXTERNE|" + oldAvnId + "||", "AVNIDEXTERNE|" + avnId + "||");
                $("#AddParamValue").val(addParamValue);
            }
        }
        let id = "";
        if (window.isReadonly) {
            mode = "";
            id = $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumInterne").val() + "addParamAVN|||" + $("#AddParamValue").val() + "addParam" + $("#tabGuid").val();
        }
        else {
            id = $("#Offre_CodeOffre").val() + '_' + $("#Offre_Version").val() + '_' + $("#Offre_Type").val() + '_' + $("#NumInterne").val() + "addParamAVN|||" + $("#AddParamValue").val() + "||IGNOREREADONLY|1addParam" + $("#tabGuid").val();
        }

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenRsqRegule",
            data: {
                lotId: lotId, reguleId: reguleId, codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeAvn: codeAvn, typeAvt: typeAvt, exercice: exercice, dateDeb: dateDeb, dateFin: dateFin,
                codeICT: codeICT, codeICC: codeICC, tauxCom: tauxCom, tauxComCATNAT: tauxComCATNAT, codeEnc: codeEnc, mode: mode,
                souscripteur: $("#SouscripteurCode").val(), gestionnaire: $("#GestionnaireCode").val(),
                argModeleAvtRegule: argModeleAvtRegule, tabGuid: $("#tabGuid").val(),
                id: id
            },
            success: function (data) {
                if (data.indexOf("dvInfoRsqRegul") === -1) {
                    CloseLoading();
                    common.dialog.error("Ce contrat est verrouillé, vous ne pouvez y accéder qu'en consultation");
                    return;
                }

                if ($("#txtSaveCancel").val() == "1") {
                    DeverouillerUserOffres($("#tabGuid").val());
                    listeReguls.redirect("RechercheSaisie", "Index");
                }
                else {
                    $("#ExerciceRegule").makeReadonly(true);
                    $("#PeriodeDeb").disable(true);
                    $("#PeriodeFin").disable(true);
                    $("#divInfoRegul").hide();
                    $("#divInfoRegul2").html(data).show();
                    $("#ReguleId").val($("#ReguleIdRsq").val());


                    var addParamValue = $("#AddParamValue").val();

                    var oldRegulId = addParamValue.split('||').filter(function (element) {
                        return element.indexOf("REGULEID|") === 0 ? element : '';
                    })[0];

                    var oldModeAvt = addParamValue.split('||').filter(function (element) {
                        return element.indexOf("AVNMODE|") === 0 ? element : '';
                    })[0];

                    addParamValue = addParamValue.replace(oldRegulId, "REGULEID|" + $("#ReguleIdRsq").val());

                    if (oldModeAvt !== "AVNMODE|CONSULT") {
                        addParamValue = addParamValue.replace(oldModeAvt, "AVNMODE|UPDATE");
                    }

                    $("#AddParamValue").val(addParamValue);

                    listeReguls.MapListRsqElement();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //-------------Map les éléments de la div flottante des rsq-----------
    this.MapListRsqElement = function () {
        $("#btnReguleCancel").hide();
        $("#btnRegulePrec").show();
        $("#btnReguleSuivant").enable().show();

        $("#btnRegulePrec").kclick(function () {
            var modeAvt = window.isReadonly ? "CONSULT" : "UPDATE";
            listeReguls.consultOrEdit($("#OffreReadOnly").val(), modeAvt, $("#TypeAvtRsq").val(), $("#CodeAvtRsq").val());
            $("#divInfoRegul").show();
            $("#divInfoRegul2").hide();
        });

        $("#btnReguleSuivant").kclick(function () {
            if ($("#txtSaveCancel").val() == "1") {
                DeverouillerUserOffres($("#tabGuid").val());
                listeReguls.redirect("RechercheSaisie", "Index");
            }
            else {
                if (!$(this).is(":disabled")) {
                    listeReguls.redirect("Quittance", "Index");
                }
            }
        });

        $("img[name='ExpandRsq']").each(function () {
            $(this).click(function () {
                var codeRsq = $(this).attr("id").split("_")[1];
                var imgExpCol = $(this).attr("albexpcol");
                switch (imgExpCol) {
                    case "expand":
                        $(this).attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
                        $("tr[id^='trObj_" + codeRsq + "']").show();
                        break;
                    case "collapse":
                        $(this).attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
                        $("tr[id^='trObj_" + codeRsq + "']").hide();
                        break;
                }
            });
        });

        $("td[id='tdRsqRegule']").each(function () {
            $(this).die();
            $(this).click(function () {
                var codeRsq = $(this).attr('albcodersq');
                listeReguls.OpenSelectionGarRegule(codeRsq);
            });
        });

        // Si aucun risque n'est régularisé, blocage du passage à l'écran Cotisations
        if ($("td[id='tdRsqRegule'][albreguleused='O']").length <= 0) {
            $("#btnReguleSuivant").disable();
        }
    };

    //#endregion

    //#region Garantie
    ////////////////////////////////////////////////////////////////////
    /////////////          ZONE DE LA GARANTIE        //////////////////
    //-----------Ouvre la sélection des garanties disponibles pour la régularisation-------
    this.OpenSelectionGarRegule = function (codeRsq) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenGarRegule",
            data: {
                lotId: $("#lotId").val(), reguleId: $("#ReguleId").val(), codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), tyepe: $("#Offre_Type").val(),
                codeAvn: $("#CodeAvtRsq").val(), typeAvt: $("#TypeAvtRsq").val(), codeRsq: codeRsq,
                dateDeb: $("#inDateDebRsqRegule").val(), dateFin: $("#inDateFinRsqRegule").val(), isReadonly: $("#OffreReadOnly").val()
            },
            success: function (data) {
                $("#divDataInfoGarRsq").html(data);
                $("#divInfoGarRsq").show();
                listeReguls.MapListGarElement();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //--------Map les éléments de la div flottante des garanties------
    this.MapListGarElement = function () {
        $("#btnReguleBack").kclick(function () {
            var lotId = $("#lotId").val();
            listeReguls.ReloadListRsqRegule(lotId);
        });

        $("#btnReguleSuivant").disable();

        document.body.addEventListener('click', function () {
            // Masque tous les popup
            $("div[id^='divApplique']").each(function () {
                $(this).hide();
            });

        }, true);

        $("td[name='tdFormuleRegule']").each(function () {
            var divName = + $(this).parent().get(0).querySelector("td[id='tdGarantieRegule']").getAttribute('albcodgar');
            $(this).mouseover(function () {
                if ($('#dvBodyApplique' + divName).val() == undefined) {
                    var div = document.createElement('div');
                    div.setAttribute("id", "dvBodyApplique" + divName);
                    document.getElementsByTagName('body')[0].appendChild(div);
                    listeReguls.OpenAppliqueFormule($(this));
                }
                else {
                    var pos = $(this).position();
                    $('#dvBodyApplique' + divName).css({ "position": "absolute", "z-index": "6", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
                    $('#divApplique' + divName).show();
                }
            });
            $(this).mouseout(function () {
                $("#divApplique" + divName).hide();
            });
            $(this).mouseleave(function () {
                $("#divApplique" + divName).hide();
            });
        });

        $("td[name='tdImgGar']").kclick(function () {
            listeReguls.OpenDetailGarantie($(this));
        });

        $("td[name='tdGarantieRegule']").kclick(function () {
            listeReguls.OpenSelectionPeriodGar($(this));
        });
    };

    //--------Recharge la liste des risques de régularisation----------
    this.ReloadListRsqRegule = function (lotId) {
        var reguleId = $("#ReguleId").val();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/ReloadListRsqRegule",
            data: { lotId: lotId, reguleId: reguleId, isReadonly: $("#OffreReadOnly").val() },
            success: function (data) {
                $("#dvBodyRsqRegularisation").html(data);

                $("#divInfoGarRsq").hide();
                $("#divDataInfoGarRsq").clearHtml();
                $("#btnReguleSuivant").removeAttr('disabled');

                listeReguls.MapListRsqElement();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //--------Ouvre le s'applique à de la formule-------
    this.OpenAppliqueFormule = function (elem, divname) {
        var idFormule = elem.attr("albcodefor");
        //ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenApplique",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#CodeAvtGar").val(),
                codeFor: idFormule
            },
            success: function (data) {
                // Masquer tous les popup récalcitrant
                $("div[id^='divApplique']").each(function () {
                    $(this).hide();
                });

                var divName = elem.parent().get(0).querySelector("td[id='tdGarantieRegule']").getAttribute('albcodgar');
                $('#dvBodyApplique' + divName).html(data);
                var div = $('#dvBodyApplique' + divName).get(0).querySelector("div[id='divApplique']");
                div.id = 'divApplique' + divName;
                $("#divApplique" + divName).show();
                var pos = elem.position();
                $('#dvBodyApplique' + divName).css({ "position": "absolute", "z-index": "6", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
                $("#divApplique" + divName).show();
                //CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //--------Ouvre le détail de la garantie----------
    this.OpenDetailGarantie = function (elem) {
        var codeObjetRisque = $("#ObjetRisqueCode").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/LoadDetailsGarantie",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeFormule: elem.attr("albcodefor"), codeOption: elem.attr("albcodeopt"), codeGarantie: elem.attr("albcodegar"), codeObjetRisque: codeObjetRisque, modeNavig: $("#ModeNavig").val(),
                codeAvn: $("#CodeAvtGar").val(), isReadonly: true
            },
            success: function (data) {
                $("#dvBodyDetail").html(data);
                $("#dvDetailGarantie").show();
                listeReguls.MapElementDetailGarantie();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //----------Map les éléments du détail de garantie-------
    this.MapElementDetailGarantie = function() {
        $("#infoDetail").kclick(function () {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/CreationFormuleGarantie/OpenInfoDetail",
                data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeGarantie: $("#CodeGarantie").val() },
                success: function (data) {
                    $("#divInfoDetail").show();
                    $("#divDataInfoDetail").html(data);
                    AlternanceLigne("InfoDetail1", "", false, null);
                    AlternanceLigne("InfoDetail2", "", false, null);
                    common.autonumeric.applyAll('init', 'decimal', '', null, null, '99999999999.99', null);
                    common.autonumeric.applyAll('init', 'pourcentdecimal', '');
                    common.autonumeric.applyAll('init', 'pourmilledecimal', '');
                    $("#btnCloseInfoDetail").kclick(function () {
                        $("#divInfoDetail").hide();
                    });
                    CloseLoading();
                },
                error: function (data) {
                    common.error.showXhr(request);
                }
            });
        });

        $("#btnFancyAnnuler").kclick(function () {
            $("#dvBodyDetail").clearHtml();
            $("#dvDetailGarantie").hide();
        });
    }
    //-------Ouvre l'éran de régularisation d'une garantie-------
    this.OpenGarantieRegule = function(elem) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenGarantieRegule",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#CodeAvtGar").val(),
                codeRsq: $("#CodeRsq").val(), codeFor: elem.attr("albcodefor"), codeOpt: elem.attr("albcodeopt"), codeGar: elem.attr("albcodegar")
            },
            success: function (data) {
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //#endregion

    //#region période de garantie

    ////////////////////////////////////////////////////////////////////
    ////////          ZONE DE LA PERIODE GARANTIE        ///////////////
    //-----------Ouvre la sélection des periodes de garanties -------
    this.OpenSelectionPeriodGar = function(elem) {
        ShowLoading();
        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRsq = $("#CodeRsq").val();
        var codeFor = "";
        var codeOpt = "";
        var idGar = "";
        var codeGar = "";
        var libgar = "";
        var lotId = $("#lotId").val();
        var reguleId = $("#ReguleId").val();
        var codeAvenant = $("#NumInterne").val();
        var isReadonly = $("#OffreReadOnly").val();

        if (elem != undefined && elem != null) {
            codeFor = elem.attr("albcodefor");
            codeOpt = elem.attr("albcodeopt");
            idGar = elem.attr("albidgar");
            codeGar = elem.attr("albcodgar");
            libgar = elem.attr("alblibgar");
        }

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenPeriodGar",
            data: {
                codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant,
                codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotId: lotId, reguleId: reguleId, codeGar: codeGar, libgar: libgar, isReadonly: isReadonly
            },
            sync: false,
            success: function (data) {
                $("#divInfoPeriodGar").show();
                $("#divDataInfoPeriodGar").html(data);
                listeReguls.MapPeriodGarElement();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    }
    //--------Map les éléments de la div flottante des periodes de garanties------
    this.MapPeriodGarElement = function() {
        $("#dateDebutNewPeriode").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#dateFinNewPeriode").datepicker({ dateFormat: 'dd/mm/yy' });

        $("#dateDebutNewPeriode").offOn("change", function () {
            if ($(this).val() != "")
                FormatAutoDate($(this));
        });

        $("#dateFinNewPeriode").offOn("change", function () {
            if ($(this).val() != "")
                FormatAutoDate($(this));
        });

        $("#btnReguleBack").disable();
        AlternanceLigne("PeriodRegulBody", "", false, null);
        AlternanceLigne("MouvementBody", "", false, null);

        /* A verifer */
        $("#btnAppliquer").kclick(function () {
            var position = $(this).offset();
            //$("#divApplique").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 410 + 'px' }).toggle();
            $("#divApplique").css({ 'position': 'absolute', 'top': (position.top - 16) + 'px', 'left': position.left - 110 + 'px' }).toggle();
        });

        $("#btnFermerpopup").kclick(function () {
            $("#divApplique").hide();
        });

        $("#dvAddPeriodRegule").hide();
        $("#btnAjouterPeriodregul").kclick(function () {
            $("#dvAddPeriodRegule").show();
        });
        listeReguls.MapListBas();

        $("#btnCancelAddPeriodRegule").kclick(function () {
            Initialiser();
            $("#dvAddPeriodRegule").hide();
        });

        $("#btnValidAddPeriodRegule").kclick(function () {
            EnregistrerLigneMouvementPeriode();
        });

        $("#btnRegulePrecedent").kclick(function () {
            listeReguls.RetourEcran();
        });

        /* REDIRECTION SAISIE PERIOD REGUL  */

        $("td[id^=DateDeb_]").kclick(function () {
            var idregulgar = $(this).attr("id").split('_')[1];
            var param = $(this).attr("albprov");
            listeReguls.RetourEcran(param, $(this), idregulgar);
        });

        if (window.isReadonly) {
            //$("#btnAjouterPeriodregul").parent().addClass("None");
            $("#btnAjouterPeriodregul").addClass("None");
            $("img[name='SuppPeriodRegul']").parent().addClass("None");
            $("img[name='SuppPeriodRegul']").addClass("None");
        }
        else {
            $("#btnAjouterPeriodregul").removeClass("None");
            $("#SuppPeriodRegul").parent().removeClass("None");
            $("#SuppPeriodRegul").removeClass("None");
        }
    }
    this.RetourEcran = function(param, elem, idregulgar) {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codersq = $("#codersq").val();
        var codefor = $("#codefor").val();
        var codegar = $("#codegar").val();
        var dateDebReg = $("#DateDebMin").val();
        var dateFinReg = $("#DateFinMax").val();
        var controle = "";
        //TODO check periode
        if (param == "" || param == undefined) {
            controle = "true";
        }
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/RetourEcran",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), reguleId: $("#ReguleId").val(), codersq: codersq, codefor: codefor, codegar: codegar, dateDebReg: dateDebReg,
                dateFinReg: dateFinReg, controle: controle
            },
            success: function (data) {
                if (param == "" || param == undefined) {
                    if (data == null || data == '') {
                        listeReguls.ReloadListGarRegule($("#lotId").val(), $("#codersq").val());
                    }
                }
                else {
                    if (data == null || data == '') {

                        OpenPeriodRegul(idregulgar, elem);
                    }

                }
                CloseLoading();

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    }
    //---------Recharge la liste des garanties de régularisations----------
    this.ReloadListGarRegule = function(lotId, codeRsq) {
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/ReloadListGarRegule",
            data: { lotId: lotId, reguleId: $("#ReguleId").val(), codeRsq: codeRsq, isReadonly: $("#OffreReadOnly").val() },
            success: function (data) {
                $("#dvBodyGarRegularisation").html(data);
                $("#divInfoPeriodGar").hide();
                $("#divDataInfoPeriodGar").clearHtml();
                $("#btnReguleBack").removeAttr('disabled');
                listeReguls.MapListGarElement();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    /***Map element grille Mouvt regularisé  **/
    this.MapListBas = function() {

        common.autonumeric.applyAll('init', 'decimal', null, null, null, "99999999999.99", "-99999999999.99");
        common.autonumeric.applyAll('init', 'pourcentdecimal', null, null, 4, "999.9999", "-999.9999");

        AlternanceLigne("PeriodRegulBody", "", false, null);
        listeReguls.Supprimer();
    }
    this.Supprimer = function() {
        $("img[id='SuppPeriodRegul']").die();
        $("img[id='SuppPeriodRegul']").each(function () {
            $(this).die();
            $(this).click(function () {
                $("#hiddenInputId").val($(this).attr("albregulecode"));
                ShowCommonFancy("ConfirmTrans", "Suppr",
                    "Vous allez supprimer une période de régularisation. Voulez-vous continuer ?",
                    350, 130, true, true);
            });
        });
        $("#btnConfirmTransOk").kclick(function () {
            switch ($("#hiddenAction").val()) {
                case "Suppr":
                    var code = $("#hiddenInputId").val();
                    SupprimerPeriodRegul(code);
                    CloseCommonFancy();
                    break;
            }
            $("#hiddenInputId").clear();
        });
        $("#btnConfirmTransCancel").kclick(function () {
            CloseCommonFancy();
        });
    }
    //----------------Sauvegarde une période de mouvement--------------------

    this.EnregistrerLigneMouvementPeriode = function() {
        if (!CheckDate()) {
            return;
        }
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var dateDeb = $("#dateDebutNewPeriode").val();
        var dateFin = $("#dateFinNewPeriode").val();
        var dateDebMin = $("#DateDebMin").val();
        var dateFinMax = $("#DateFinMax").val();
        var codersq = $("#codersq").val();
        var codefor = $("#codefor").val();
        var codegar = $("#codegar").val();
        var idlot = $("#idlot").val();
        var idregul = $("#idregul").val();

        dateDeb = dateDeb.split('/')[2] + dateDeb.split('/')[1] + dateDeb.split('/')[0];
        dateFin = dateFin.split('/')[2] + dateFin.split('/')[1] + dateFin.split('/')[0];
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/SaveLineMouvtPeriode",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), dateDeb: dateDeb, dateFin: dateFin, dateDebMin: dateDebMin, dateFinMax: dateFinMax,
                codersq: codersq, codefor: codefor, codegar: codegar, idregul: idregul
            },
            success: function (data) {
                $("#ListePeriodeRegularise").html(data);
                listeReguls.MapListBas();
                Initialiser();
                $("#dvAddPeriodRegule").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //----------------Suprimer une période de mouvement--------------------
    this.SupprimerPeriodRegul = function(code) {

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codersq = $("#codersq").val();
        var codefor = $("#codefor").val();
        var codegar = $("#codegar").val();

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/SupprimerMouvtPeriode",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), codersq: codersq, codefor: codefor, codegar: codegar, coderegul: $("#idregul").val(), code: code
            },
            success: function (data) {
                $("#ListePeriodeRegularise").html(data);
                listeReguls.MapListBas();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });


    }
    this.CheckDate = function() {
        toReturn = true;

        if ($("#dateDebutNewPeriode").val() == "") {
            $("#dateDebutNewPeriode").addClass('requiredField');
            toReturn = false;
        }
        if ($("#dateFinNewPeriode").val() == "") {
            $("#dateFinNewPeriode").addClass('requiredField');
            toReturn = false;
        }

        if (!checkDate($("#dateDebutNewPeriode"), $("#dateFinNewPeriode"))) {
            toReturn = false;
        }

        return toReturn;
    }
    this.Initialiser = function() {
        $("#dateDebutNewPeriode").clear();
        $("#dateFinNewPeriode").clear();

    }
    //#endregion

    //#region saisie période de garantieOP

    ////////////////////////////////////////////////////////////////////
    ////////          ZONE DE LA SAISIE PERIOD  Regul        ///////////////
    //-----------Ouvre la saisie des donnees  regul -------

    this.OpenPeriodRegul = function(idregulgar, elem) {

        ShowLoading();

        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvenant = $("#NumAvenantPage").val();
        var codeRsq = $("#codersq").val();
        var codeGar = "";
        var codeFor = $("#codefor").val();
        var lotId = "";
        var codeOpt = "";
        var idGar = "";
        var libGar = $("#libgar").val();
        var reguleId = "";
        if (elem != undefined && elem != null) {
            codeOpt = elem.attr("albcodeopt");
            idGar = elem.attr("albidgar");
            reguleId = elem.attr("albidregul");
            lotId = elem.attr("albidlot");
            codeGar = elem.attr("albcodgar");
        }
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/OpenSaisiePeriodRegul",
            data: {
                codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant,
                codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotId: lotId, reguleId: reguleId, codeGar: codeGar, libGar: libGar, idregulgar: idregulgar
            },
            success: function (data) {

                $("#divInfoSaisieGar").show();
                $("#divDataInfoSaisieGar").html(data);
                MapSaisiePeriodGarElement();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    this.MapSaisiePeriodGarElement = function() {
        $("#btnSaisieSuivant").removeAttr('disabled');
        $("#TextRed").css({ "display": "none" });

        //if ($("#AssietteGarantieDef").val() == undefined) { // && ($("#MotifAvt").val() == "INFERIEURE" || $("#MotifAvt").val() == "CONTRACTUELLE")
        //    $("#MontantForcePrimecalcul").disable();
        //    $("#IsCheckForcer").disable();
        //}
        $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_gris3232.png');
        $("#btnRefresh").removeClass("CursorPointer");

        AnnulerSaisie();

        $("#btnSaisieSuivant").kclick(function () {

            SuivantSaisie();
        });
        FormatDecimalNumricValue();
        $("#btnAppliquerA").kclick(function () {
            var position = $(this).offset();
            $("#divAppliqueA").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 410 + 'px' }).toggle();
        });

        $("#btnFermerpop").kclick(function () {
            $("#divAppliqueA").hide();
        });

        $("#btnRefresh").kclick(function () {
            if ($(this).hasClass("CursorPointer")) {
                chargement();
            }
        });

        $("#btnValidConfirm").kclick(function () {
            ReloadListPeriodReguleGar();
        });

        $("#btnAnnulConfirm").kclick(function () {
            $("#divConfirmMntRegul").hide();
            $("#divDataConfirmMntRegul").clearHtml();
        });

        $("#unitedef").offOn("change", function () {
            AffectTitleList($(this));
            ToggleButton();
            if ($(this).val() == "D") {
                $("#TauxMontantGarDef").attr("albmask", "numeric");
                common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'numeric', '', null, 2, '99999999999.99', '-99999999999.99');
            }
            else {
                if ($(this).val() == "%" && parseInt($("#TauxMontantGarDef").autoNumeric('get')) > 100) {
                    $("#TauxMontantGarDef").clear();
                    $("#TauxMontantGarDef").attr("albmask", "decimal");
                    common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'decimal', '', null, 4, '999.9999', '-999.9999');
                }
                else if ($(this).val() == "%0" && parseInt($("#TauxMontantGarDef").autoNumeric('get')) > 1000) {
                    $("#TauxMontantGarDef").clear();
                    common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'decimal', '', null, 4, '9999.9999', '-9999.9999');
                }
            }
        });
        $("#codetaxedef").offOn("change", function () {
            AffectTitleList($(this));
            ToggleButton();
        });
        $("#AssietteGarantieDef").offOn("change", function () {
            ToggleButton();
        });
        $("#TauxMontantGarDef").offOn("change", function () {
            ToggleButton();
        });
        $("#codeprev").offOn("change", function () {
            AffectTitleList($(this));
            ToggleButton();
        });

        $("#codetaxedef").offOn("change", function () {
            AffectTitleList($(this));
            ToggleButton();
        });
        $("#AssietteGarantie").offOn("change", function () {
            ToggleButton();
        });

        $("#TauxMontantGar").offOn("change", function () {
            ToggleButton();
        });
        $("#MontantCotisEmiseForce").offOn("change", function () {
            ToggleButton();
        });
        $("#MntTaxePrimeEmise").offOn("change", function () {
            ToggleButton();
        });
        $("#coefficient").offOn("change", function () {
            ToggleButton();
        });


        $("#IsCheckForcer").offOn("change", function () {
            if ($("#IsCheckForcer").is(":checked")) {
                $("#MontantForcePrimecalcul").attr('readonly', 'readonly').addClass('readonly').val("0");

                $("#TextRed").css({ "display": "none" });
            }
            else {

                $("#MontantForcePrimecalcul").removeAttr('readonly', 'readonly').removeClass('readonly');
            }
        });

        if (parseInt($("#MontantRegularisationHT").val()) < 0 && (parseInt($("#MontantForcePrimecalcul").val()) == 0) && ($("#IsCheckForcer").is(':not(:checked)'))) {
            $("#TextRed").css({ "display": "block" });
        }

        if ($("#errorCalculStr").val() != "") {
            ShowCommonFancy("Error", "", $("#errorCalculStr").val(), 300, 80, true, true, true);
        }

        if (window.isReadonly) {
            $("#AssietteGarantieDef").makeReadonly(true);
            $("#TauxMontantGarDef").makeReadonly(true);
            $("#unitedef").disable(true);
            $("#codetaxedef").disable(true);
            $("#MontantCotisEmiseForce").makeReadonly(true);
            $("#MntTaxePrimeEmise").makeReadonly(true);
            $("#AttentatGareat").makeReadonly(true);
            $("#MontantForcePrimecalcul").makeReadonly(true);
            $("#IsCheckForcer").disable(true);
            $("#btnSaisieSuivant").addClass("None");
            $("#btnSaisieCancel").html("Fermer");
        }
        else {

            $("#AssietteGarantieDef").enable();
            $("#TauxMontantGarDef").enable();
            $("#unitedef").enable();
            $("#codetaxedef").enable();
            $("#MontantCotisEmiseForce").enable();
            $("#MntTaxePrimeEmise").enable();
            $("#AttentatGareat").enable();
            $("#MontantForcePrimecalcul").enable();
            $("#IsCheckForcer").enable();
            $("#btnSaisieSuivant").removeClass("None");
            $("#btnSaisieCancel").html("Annuler");
        }

        if ($("#grisSuivant").val() == "True") {
            $("#btnSaisieSuivant").disable();
            $("#MontantRegularisationHT").val(0);
            $("#MontantForcePrimecalcul").disable();
            $("#IsCheckForcer").disable(true);
        }

        $("#grisSuivant").val('False');
    }

    this.ReloadListPeriodReguleGar = function() {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/ReloadListPeriodReguleGar",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeRsq: $("#codersq").val(), codeFor: $("#codefor").val(), codeGar: $("#codegar").val(), codeRegul: $("#idregul").val()
            },
            success: function (data) {
                $("#divConfirmMntRegul").hide();
                $("#divDataConfirmMntRegul").clearHtml();
                $("#divInfoSaisieGar").hide();
                $("#divDataInfoSaisieGar").clearHtml();
                $("#ListePeriodeRegularise").html(data);
                $("#btnReguleBack").removeAttr("disabled");
                listeReguls.MapPeriodGarElement();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    }

    this.FormatDecimalNumricValue = function() {

        common.autonumeric.apply($("#AssietteGarantie"), 'init', 'decimal', '', null, null, '99999999999.99', '-99999999999.99');
        common.autonumeric.apply($("#AssietteGarantieDef"), 'init', 'decimal', '', null, null, '99999999999.99', '-99999999999.99');

        if ($("#uniteprev").val() == "D") {
            common.autonumeric.apply($("#TauxMontantGar"), 'init', 'numeric', '', null, 2, '99999999999.99', '-99999999999.99');
        }
        else {
            common.autonumeric.apply($("#TauxMontantGar"), 'init', 'decimal', '', null, 4, '999.9999', '-999.9999');
        }
        if ($("#unitedef").val() == "D") {
            common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'numeric', '', null, 2, '99999999999.99', '-99999999999.99');
        }
        else {
            common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'decimal', '', null, 4, '999.9999', '-999.9999');
        }

        common.autonumeric.apply($("#coefficient"), 'init', 'decimal', '', null, 3, '99.999', '-99.999');
        common.autonumeric.apply($("#TauxAppel"), 'init', 'decimal', '', null, 3, '99.999', '-99.999');

        common.autonumeric.apply($("#NbAnnee"), 'init', 'numeric', '', null, null, '99', '-99');

        common.autonumeric.apply($("#TxCotisRetenues"), 'init', 'decimal', '', null, null, '999.99', '-999.99');
        common.autonumeric.apply($("#Ristourne"), 'init', 'decimal', '', null, null, '999.99', '-999.99');

        common.autonumeric.apply($(".inNumericRegule"), 'init', 'decimal', '', null, null, '999999999.99', '-999999999.99');
    }

    this.AnnulerSaisie = function() {
        $("#btnSaisieCancel").kclick(function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
    }
    this.SuivantSaisie = function() {
        var typeRegule = $("#TypeGrille").val();
        var accesType = true;
        if (typeRegule != "PB" && typeRegule != "BN") {
            accesType = ChekInput();
        }
        else {
            accesType = ChekInputPBBN();
        }
        if (accesType) {
            ShowLoading();
            var dataRow = GetDataRow();
            $.ajax({
                type: "POST",
                url: "/CreationRegularisation/ValidSaisiePeriodRegule",
                data: {
                    codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                    codeAvn: $("#NumAvenantPage").val(), codeRsq: $("#CodeRsq").val(),
                    reguleGarId: $("#idregulgar").val(), typeRegule: $("#TypeGrille").val(),
                    dataRow: JSON.stringify(dataRow), addParamValue: $('#AddParamValue').val()
                },
                success: function (data) {


                    OpenPopupConfirmMntRegul();

                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            ShowCommonFancy("Error", "", "Veuillez remplir les champs vides.", true, true, true);
        }
    }
    //-------------Récupère la chaine de paramètre------------
    this.GetDataRow = function() {
        var splitCharHtml = $("#splitCharHtml").val();

        var dataRow = "";

        //#region Variables
        var mntReguleHT_STD = "";
        var force0_STD = "";
        var mntForceCalc_STD = "";
        var attentat_STD = "";
        var mntForceEmis_STD = "";
        var mntForceTx_STD = "";
        var coef_STD = "";
        var prevAssiette_STD = "";
        var prevTaux_STD = "";
        var prevUnite_STD = "";
        var prevMntHT_STD = "";
        var prevCodeTaxe_STD = "";
        var defAssiette_STD = "";
        var defTaux_STD = "";
        var defUnite_STD = "";
        var defVmntHT_STD = "";
        var defCodeTaxe_STD = "";

        var cotisEmise_PB = "";
        var txAppelPbns_PB = "";
        var txCotisRet_PB = "";
        var chargeSin_PB = "";
        var txRistRegul_PB = "";
        var ristAnticipee_PB = "";
        var nbYearRegul_PB = "";
        var txCotisRetRsq_PB = "";
        //#endregion

        //#region Récupération des données
        if (typeof ($("input[id='MontantRegularisationHT']").val()) != "undefined" && $("input[id='MontantRegularisationHT']").val() != "")
            mntReguleHT_STD = $("input[id='MontantRegularisationHT']").val();
        else
            mntReguleHT_STD = 0;

        if (typeof ($("input[id='IsCheckForcer']").val()) != "undefined" && $("input[id='IsCheckForcer']").is(':checked'))
            force0_STD = "O";
        else
            force0_STD = "N";

        if (typeof ($("input[id='MontantForcePrimecalcul']").val()) != "undefined" && $("input[id='MontantForcePrimecalcul']").val() != "")
            mntForceCalc_STD = $("input[id='MontantForcePrimecalcul']").val();
        else
            mntForceCalc_STD = 0;

        if (typeof ($("input[id='AttentatGareat']").val()) != "undefined" && $("input[id='AttentatGareat']").val() != "")
            attentat_STD = $("input[id='AttentatGareat']").val();
        else
            attentat_STD = 0;

        if (typeof ($("input[id='MontantCotisEmiseForce']").val()) != "undefined" && $("input[id='MontantCotisEmiseForce']").val() != "")
            mntForceEmis_STD = $("input[id='MontantCotisEmiseForce']").val();
        else
            mntForceEmis_STD = 0;

        if (typeof ($("input[id='MntTaxePrimeEmise']").val()) != "undefined" && $("input[id='MntTaxePrimeEmise']").val() != "")
            mntForceTx_STD = $("input[id='MntTaxePrimeEmise']").val();
        else
            mntForceTx_STD = 0;

        if (typeof ($("input[id='coefficient']").val()) != "undefined" && $("input[id='coefficient']").val() != "")
            coef_STD = $("input[id='coefficient']").val();
        else
            coef_STD = 0;

        if (typeof ($("input[id='AssietteGarantie']").val()) != "undefined" && $("input[id='AssietteGarantie']").val() != "")
            prevAssiette_STD = $("input[id='AssietteGarantie']").val();
        else
            prevAssiette_STD = 0;

        if (typeof ($("input[id='TauxMontantGar']").val()) != "undefined" && $("input[id='TauxMontantGar']").val() != "")
            prevTaux_STD = $("input[id='TauxMontantGar']").val();
        else
            prevTaux_STD = 0;

        if (typeof ($("select[id='uniteprev']").val()) != "undefined" && $("select[id='uniteprev']").val() != "")
            prevUnite_STD = $("select[id='uniteprev']").val();

        if (typeof ($("select[id='codeprev']").val()) != "undefined" && $("select[id='codeprev']").val() != "")
            prevCodeTaxe_STD = $("select[id='codeprev']").val();

        if (typeof ($("input[id='AssietteGarantieDef']").val()) != "undefined" && $("input[id='AssietteGarantieDef']").val() != "")
            defAssiette_STD = $("input[id='AssietteGarantieDef']").val();
        else
            defAssiette_STD = 0;

        if (typeof ($("input[id='TauxMontantGarDef']").val()) != "undefined" && $("input[id='TauxMontantGarDef']").val() != "")
            defTaux_STD = $("input[id='TauxMontantGarDef']").val();
        else
            defTaux_STD = 0;

        if (typeof ($("select[id='unitedef']").val()) != "undefined" && $("select[id='unitedef']").val() != "")
            defUnite_STD = $("select[id='unitedef']").val();

        if (typeof ($("input[id='MontantCotisationsHT']").val()) != "undefined" && $("input[id='MontantCotisationsHT']").val() != "")
            defVmntHT_STD = $("input[id='MontantCotisationsHT']").val();
        else
            defVmntHT_STD = 0;

        if (typeof ($("select[id='codetaxedef']").val()) != "undefined" && $("select[id='codetaxedef']").val() != "")
            defCodeTaxe_STD = $("select[id='codetaxedef']").val();

        if (typeof ($("input[id='CotisEmises']").val()) != "undefined" && $("input[id='CotisEmises']").val() != "")
            cotisEmise_PB = $("#CotisEmises").val();
        else
            cotisEmise_PB = 0;

        if (typeof ($("input[id='TauxAppel']").val()) != "undefined" && $("input[id='TauxAppel']").val() != "")
            txAppelPbns_PB = $("#TauxAppel").val();
        else
            txAppelPbns_PB = 0;

        if (typeof ($("input[id='TxCotisRetenues']").val()) != "undefined" && $("input[id='TxCotisRetenues']").val() != "")
            txCotisRet_PB = $("#TxCotisRetenues").val();
        else
            txCotisRet_PB = 0;

        if (typeof ($("input[id='ChargementSinistres']").val()) != "undefined" && $("input[id='ChargementSinistres']").val() != "")
            chargeSin_PB = $("#ChargementSinistres").val();
        else
            chargeSin_PB = 0;

        if (typeof ($("input[id='Ristourne']").val()) != "undefined" && $("input[id='Ristourne']").val() != "")
            txRistRegul_PB = $("#Ristourne").val();
        else
            txRistRegul_PB = 0;

        if (typeof ($("input[id='RistourneAnticipee']").val()) != "undefined" && $("input[id='RistourneAnticipee']").val() != "")
            ristAnticipee_PB = $("#RistourneAnticipee").val();
        else
            ristAnticipee_PB = 0;

        if (typeof ($("input[id='NbAnnee']").val()) != "undefined" && $("input[id='NbAnnee']").val() != "")
            nbYearRegul_PB = $("#NbAnnee").val();
        else
            nbYearRegul_PB = 0;

        if (typeof ($("input[id='CotisationRetenusID']").val()) != "undefined" && $("input[id='CotisationRetenusID']").val() != "")
            txCotisRetRsq_PB = $("#CotisationRetenusID").val();
        else
            txCotisRetRsq_PB = 0;
        //#endregion

        //#region Alimentation JSON
        dataRow = {
            MntRegulHt_STD: mntReguleHT_STD,
            Force0_STD: force0_STD,
            MntForceCalc_STD: mntForceCalc_STD,
            Attentat_STD: attentat_STD,
            MntForceEmis_STD: mntForceEmis_STD,
            MntForceTx_STD: mntForceTx_STD,
            Coef_STD: coef_STD,
            PrevAssiette_STD: prevAssiette_STD,
            PrevTaux_STD: prevTaux_STD,
            PrevUnite_STD: prevUnite_STD,
            PrevMntHt_STD: prevMntHT_STD,
            PrevCodTaxe_STD: prevCodeTaxe_STD,
            DefAssiette_STD: defAssiette_STD,
            DefTaux_STD: defTaux_STD,
            DefUnite_STD: defUnite_STD,
            DefVmntHt_STD: defVmntHT_STD,
            DefCodTaxe_STD: defCodeTaxe_STD,
            CotisEmise_PB: cotisEmise_PB,
            TxAppelPbns_PB: txAppelPbns_PB,
            TxCotisRet_PB: txCotisRet_PB,
            ChargeSin_PB: chargeSin_PB,
            TxRistRegul_PB: txRistRegul_PB,
            RistAnticipee_PB: ristAnticipee_PB,
            NbYearRegul_PB: nbYearRegul_PB,
            TxCotisRetRsq_PB: txCotisRetRsq_PB
        };
        //#endregion

        return dataRow;
    }
    this.OpenPopupConfirmMntRegul = function() {
        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvenant = $("#NumAvenantPage").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            data: { reguleGarId: $("#idregulgar").val() },
            url: "/CreationRegularisation/GetPopupConfirmMntRegul",
            success: function (data) {
                $("#divDataConfirmMntRegul").html(data);
                $("#divConfirmMntRegul").show();
                common.autonumeric.apply($("#MntRegulHTGar"), 'init', 'decimal', '', null, null, '999999999.99', '-999999999.99');
                common.autonumeric.apply($("#MntRegulHTAttentat"), 'init', 'decimal', '', null, null, '999999999.99', '-999999999.99');
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    this.chargement = function() {
        if (ChekInput()) {
            var codeContrat = $("#Offre_CodeOffre").val();
            var version = $("#Offre_Version").val();
            var codeAvenant = $("#NumAvenantPage").val();
            var type = $("#Offre_Type").val();
            var codeRsq = $("#codersq").val();

            var assiettePrev = $("#AssietteGarantie").val();
            var valeurPrev = $("#TauxMontantGar").val();
            var unitePrev = $("#uniteprev").val();
            var codetaxePrev = $("#codeprev").val();

            var assietteDef = $("#AssietteGarantieDef").val();
            var valeurDef = $("#TauxMontantGarDef").val();
            var uniteDef = $("#unitedef").val();
            var codetaxeDef = $("#codetaxedef").val();

            var cotisEmiseForceHt = $("#MontantCotisEmiseForce").val();
            var cotisEmiseForceTaxe = $("#MntTaxePrimeEmise").val();
            var coefficient = $("#coefficient").val();

            var idregulgar = $("#idregulgar").val();
            /*controles */


            ShowLoading();
            $.ajax({
                type: "POST",
                data: {
                    codeContrat: codeContrat, version: version, codeAvenant: codeAvenant, type: type, codeRsq: codeRsq, assiettePrev: assiettePrev, valeurPrev: valeurPrev, unitePrev: unitePrev, codetaxePrev: codetaxePrev,
                    assietteDef: assietteDef, valeurDef: valeurDef, uniteDef: uniteDef, codetaxeDef: codetaxeDef,
                    cotisForceHT: cotisEmiseForceHt, cotisForceTaxe: cotisEmiseForceTaxe, coeff: coefficient,
                    idregulgar: idregulgar

                },
                url: "/CreationRegularisation/ReloadEcranSaisie",
                success: function (data) {
                    $("#DataRegul").html(data);
                    $("#btnSuivant").disable();

                    MapSaisiePeriodGarElement();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            ShowCommonFancy("Error", "", "Veuillez remplir les champs vides.", true, true, true);
        }

    }
    this.ChekInputPBBN = function() {
        $(".requiredField").removeClass('requiredField');

        toReturn = true;

        if ($("#CotisEmises").val() == "") {
            $("#CotisEmises").addClass('requiredField');
            toReturn = false;
        }
        if ($("#ChargementSinistres").val() == "") {
            $("#ChargementSinistres").addClass('requiredField');
            toReturn = false;
        }
        if ($("#RistourneAnticipee").val() == "") {
            $("#RistourneAnticipee").addClass('requiredField');
            toReturn = false;
        }
        if ($("#TxCotisRetenues").val() == "") {
            $("#TxCotisRetenues").addClass('requiredField');
            toReturn = false;
        }
        return toReturn;
    }

    this.ChekInput = function() {
        $(".requiredField").removeClass('requiredField');

        toReturn = true;

        if ($("#AssietteGarantieDef").val() == "") {
            $("#AssietteGarantieDef").addClass('requiredField');
            toReturn = false;
        }
        if ($("#TauxMontantGarDef").val() == "") {
            $("#TauxMontantGarDef").addClass('requiredField');
            toReturn = false;
        }
        if ($("#unitedef").val() == "") {
            $("#unitedef").addClass('requiredField');
            toReturn = false;
        }
        if ($("#codetaxedef").val() == "") {
            $("#codetaxedef").addClass('requiredField');
            toReturn = false;
        }
        return toReturn;
    }
    this.ToggleButton = function() {
        $("#btnRefresh").addClass("CursorPointer");
        $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png')
        $("#btnSaisieSuivant").disable();
        $("#MontantForcePrimecalcul").removeAttr('disabled');
        $("#IsCheckForcer").removeAttr('disabled').removeClass('readonly');;

    }
    //#endregion
};

var listeReguls = new ListeRegularisations();

$(function () {
    listeReguls.initPage();
});
