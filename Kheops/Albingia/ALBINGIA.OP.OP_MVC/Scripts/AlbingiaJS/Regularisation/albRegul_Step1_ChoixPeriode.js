$(document).ready(function () {
    MapElementPage();
});


function MapElementPage() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    toggleDescription();
    MapElementCourtier();

    if ($("#ActeGestion").val() == "AVNRG" || ($("#ActeGestion").val() == "REGUL" && ($('#ModeAvt').val() === 'UPDATE' || $('#ModeAvt').val() === 'CREATE' || $('#ModeAvt').val() === 'CONSULT'))) {
        MapElementRegularisation($("#ModeAvt").val());
    }

    if (window.isReadonly) {
        $("#MotifAvt").attr('disabled', 'disabled');
        $("#ExerciceRegule").attr('disabled', 'disabled').addClass("readonly");
        $("#PeriodeDeb").attr('disabled', 'disabled').addClass("readonly");
        $("#PeriodeFin").attr('disabled', 'disabled').addClass("readonly");
    }

    var addParam = $('#AddParamValue').val();

    if (addParam.indexOf('||LOTID|') === -1)
        $('#AddParamValue').val(addParam + '||LOTID|' + $("#lotId").val());

    $("td[id='linkAlerte']").each(function () {
        $(this).click(function () {
            OpenAlerte($(this));
        });
    });

    $("#btnRegulePrec").kclick(function () {
        let p = common.albParam.buildObject();
        delete p.LOTID;
        delete p.REGULEID;
        delete p.REGULMOD;
        delete p.REGULTYP;
        delete p.REGULNIV;
        delete p.REGULAVN;
        p.AVNMODE = "CREATE";
        $("#AddParamValue").val(common.albParam.objectToString(p));
        RedirectionRegul("Index", "CreationRegularisation", false, 'Previous');
    });

    $("#btnReguleSuivant").kclick(function (evt, data) {
        $("#MotifAvt").removeClass("requiredField");
        var codeAvn = $("#NumInterne").val();
        var typeAvt = $("#TypeAvt").val();
        var modeAvt = $("#ModeAvt").val();
        var exercice = $("#ExerciceRegule").val();
        var dateDeb = $("#PeriodeDeb").val();
        var dateFin = $("#PeriodeFin").val();
        var lotId = $("#lotId").val();
        var motifAvt = $("#MotifAvt").val();
        if (motifAvt == "" && !window.isReadonly) {
            $("#MotifAvt").addClass("requiredField");
            ShowCommonFancy("Error", "", "Veuillez remplir les champs vides.", true, true, true);
            return false;
        }
        if (!checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
            common.dialog.error("Incohérence au niveau des dates");
            return false;
        }

        $("#ExerciceRegule").attr("disabled", "disabled").addClass("readonly");
        $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
        $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");

        Next(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, modeAvt, data && data.returnHome);
    });

    $("#MotifAvt").off("change").change(function () {
        let menuParent = $("#CotisationMenuArbreLI").parent();
        if ($(this).val() != $("#MotifAvtInitial").val() && ($(this).val() == "INFERIEURE" || $("#MotifAvtInitial").val() == "INFERIEURE")) {
            menuParent.addClass("hide-it");
            while ((menuParent = menuParent.next()).length > 0) {
                menuParent.addClass("hide-it");
            }
        }
        else {
            menuParent.removeClass("hide-it");
            while ((menuParent = menuParent.next()).length > 0) {
                menuParent.removeClass("hide-it");
            }
        }
    });
}

function MapElementCourtier() {
    $("#ddlCourtiers, #ddlCourtiersCom, #ComHCATNAT, #ComCATNAT").die().live('change', function () {
        $("#btnCancelCourtier").show();
        $("#btnCloseCourtier").hide();
        $("#btnValidCourtier").removeAttr("disabled");
    });

    $("#ddlQuittancements").die().live('change', function () {
        $("#btnCancelCourtier").show();
        $("#btnCloseCourtier").hide();
        $("#btnValidCourtier").removeAttr("disabled");


        if ($("#ddlQuittancements").val() == "D") {
            $("#ComHCATNAT").val("0");
            $("#ComCATNAT").val("0");
            $("#ComHCATNAT").attr("readonly", "readonly").addClass("readonly");
            $("#ComCATNAT").attr("readonly", "readonly").addClass("readonly");
        }
        else {
            $("#ComHCATNAT").val($("#inHorsCATNAT").val());
            $("#ComCATNAT").val($("#inCATNAT").val());
            $("#ComHCATNAT").removeAttr("readonly").removeClass("readonly");
            $("#ComCATNAT").removeAttr("readonly").removeClass("readonly");
        }
    });

    $("#btnModifCourtier").kclick(function () {
        $("#dvChoixCourtier").show();
        $("#btnReguleSuivant").attr("disabled", "disabled");
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

    $("#btnValidCourtier").kclick(function () {
        if (!$(this).is(":disabled")) {
            ValidCourtierRegule();
        }
    });


}

function MapElementRegularisation(modeAvt) {
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
    if ($("#PeriodeDeb").hasVal() && $("#PeriodeFin").hasVal() && $("#HasSelections").hasTrueVal()) {
        $("#btnReguleSuivant").removeAttr("disabled");
    }
    if (!$("#PeriodeDeb").hasVal() && !$("#PeriodeFin").hasVal()) {
        $("#btnDeleteRegule").removeClass("CursorPointer");
    }
    $("#btnReguleBack").hide();

    $("#btnReguleCancel").kclick(function () {
        if (window.isReadonly) {
            RedirectionReguleOuter("RechercheSaisie", "Index");
        }
        else {
            var tabGuid = $("#tabGuid").val();
            DeverouillerUserOffres(tabGuid);
            ReloadListeRegularisation();
        }
    });

    $("#btnReguleSuivant").kclick(function () {
        $("#MotifAvt").removeClass("requiredField");
        var codeAvn = $("#NumInterne").val();
        var typeAvt = $("#TypeAvt").val();
        var modeAvt = $("#ModeAvt").val();
        var exercice = $("#ExerciceRegule").val();
        var dateDeb = $("#PeriodeDeb").val();
        var dateFin = $("#PeriodeFin").val();
        var lotId = $("#lotId").val();
        var motifAvt = $("#MotifAvt").val();

        if (motifAvt == "" && !window.isReadonly) {
            $("#MotifAvt").addClass("requiredField");
            ShowCommonFancy("Error", "", "Veuillez remplir les champs vides.", true, true, true);
            return false;
        }
        if (!checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
            common.dialog.error("Incohérence au niveau des dates");
            return false;
        }

        $("#ExerciceRegule").attr("disabled", "disabled").addClass("readonly");
        $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
        $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");

        OpenSelectionRsqRegule(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, modeAvt);
    });

    $(document).off("change", "#ExerciceRegule, #PeriodeDeb, #PeriodeFin").on("change", "#ExerciceRegule, #PeriodeDeb, #PeriodeFin", function (evt) {
        if (!evt.isDefaultPrevented()) {
            changePeriod(this.id);
        }
        evt.stopPropagation();
        evt.preventDefault();
    });

    if (modeAvt == "UPDATE" || modeAvt == "CONSULT") {
        $("#ExerciceRegule").attr("readonly", "readonly").addClass("readonly");
        $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
        $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");
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
        $("#PeriodeDeb").removeAttr("disabled").removeClass("readonly");
        $("#PeriodeFin").removeAttr("disabled").removeClass("readonly");
        $("#ExerciceRegule").removeAttr("readonly").removeClass("readonly");
        $("#cancelMod").val('C');
    }

    if (window.isReadonly) {
        $("#MotifAvt").attr('disabled', 'disabled');
        $("#btnSaveCancel").removeClass('SaveInfo');
        $("#btnSaveCancel").addClass('NoSaveInfo');
        $("#PeriodeDeb").attr('disabled', 'disabled').addClass("readonly");
        $("#PeriodeFin").attr('disabled', 'disabled').addClass("readonly");
    }

    $("td[id='linkAlerte']").each(function () {
        $(this).click(function () {
            OpenAlerte($(this));
        });
    });
}

function Next(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, mode, returnHome) {
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
        //ObservationsAvt: $.trim($("#Observation").html().replace(/<br>/ig, "\n"))
    };

    argModeleAvtRegule = JSON.stringify(modeleAvtRegule);

    if (mode === 'CREATE') {
        var addParamValue = $("#AddParamValue").val();
        var oldAvnId = $("#NumAvenantPage").val();
        var avnId = $("#NumInterne").val();
        addParamValue = addParamValue.replace("|TYPEAVT|S|", "|TYPEAVT|REGUL|");
        if (addParamValue.indexOf("AVNID|" + oldAvnId + "||") !== -1) {
            addParamValue = addParamValue.replace("AVNID|" + oldAvnId + "||", "AVNID|" + avnId + "||");
            addParamValue = addParamValue.replace("AVNIDEXTERNE|" + oldAvnId + "||", "AVNIDEXTERNE|" + avnId + "||");

            $("#AddParamValue").val(addParamValue);
        }
    }

    var keys = [$("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), ($("#NumInterne").val() + $("#tabGuid").val() + "addParamAVN|||" + $("#AddParamValue").val())];
    if (window.isReadonly) {
        mode = "";
        keys[keys.length - 1] += "addParam";
    }
    else {
        keys[keys.length - 1] += "||IGNOREREADONLY|1addParam";
    }

    var id = keys.join("_");
    var rg = common.getRegul();

    if (rg.mode != "STAND") {
        regul.tryCreateContext(regul.nextStep);
        return;
    }
    else {
        var params = {
            lotId: lotId, reguleId: reguleId, codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeAvn: codeAvn, typeAvt: typeAvt, exercice: exercice, dateDeb: dateDeb, dateFin: dateFin,
            codeICT: codeICT, codeICC: codeICC, tauxCom: tauxCom, tauxComCATNAT: tauxComCATNAT, codeEnc: codeEnc, mode: mode,
            souscripteur: $("#SouscripteurCode").val(), gestionnaire: $("#GestionnaireCode").val(),
            argModeleAvtRegule: argModeleAvtRegule,
            id: id, addParamValue: $('#AddParamValue').val()
        };
        regul.tryCreateContext(function () { Step1_ChoixPeriode_Next(params, returnHome); });
    }
}

var Step1_ChoixPeriode_Next = function (params, returnHome) {
    $.ajax({
        type: "POST",
        url: "/CreationRegularisation/Step1_ChoixPeriode_Next",
        data: params,
        success: function (data) {
            if (data.Result.indexOf("SUCCESS") === -1) {
                CloseLoading();
                common.dialog.error("Ce contrat est verrouillé, vous ne pouvez y accéder qu'en consultation");
                return;
            }

            var addParamValue = $("#AddParamValue").val();

            var oldRegulId = addParamValue.split('||').filter(function (element) {
                return element.indexOf("REGULEID|") === 0 ? element : '';
            })[0];
            addParamValue = addParamValue.replace(oldRegulId, "REGULEID|" + data.IdRegule);

            var oldModeAvt = addParamValue.split('||').filter(function (element) {
                return element.indexOf("AVNMODE|") === 0 ? element : '';
            })[0];

            if (oldModeAvt !== "AVNMODE|CONSULT") {
                addParamValue = addParamValue.replace(oldModeAvt, "AVNMODE|UPDATE");
            }


            var oldAvnId = addParamValue.split('||').filter(function (element) {
                return element.indexOf("AVNID|") === 0 ? element : '';
            })[0];
            addParamValue = addParamValue.replace(oldAvnId, "AVNID|" + data.NumAvn);

            // add lotid cause it might change
            addParamValue = addParamValue.replace("||LOTID|0", "||LOTID|" + $("#lotId").val());

            $("#AddParamValue").val(addParamValue);

            var redirectFromMenu = $('#txtParamRedirect').val();
            if (redirectFromMenu && redirectFromMenu.trim() !== '') {
                RedirectionRegul(redirectFromMenu.split('/')[1], redirectFromMenu.split('/')[0], returnHome, params);
            }

            leapStepsOrGoNext(data, returnHome);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

$("#btnConfirmCancel").kclick(function () {
    CloseCommonFancy();
    $("#hiddenAction").val("");
    $("#hiddenInputId").val('');
});

$("#btnConfirmOk").kclick(function () {
    CloseCommonFancy();
    switch ($("#hiddenAction").val()) {
        case "CancelCourtierRegule":
            CancelCourtierRegule();
            break;
        case "DelRegul":
            DeleteRegule($("#hiddenInputId").val());
            break;
        case "DelPeriode":
            //DeleteRegule($("#hiddenInputId").val());
            ResetPeriodes();
            break;
        case "Cancel":
            $("#divInfoSaisieGar").hide();
            $("#divDataInfoSaisieGar").html('');
            break;
        case "ValidUpdateRegule":
            ValidUpdateRegule();
            break;
    }
    $("#hiddenInputId").val('');
    $("#hiddenAction").val("");
});

function ResetPeriodes() {
    let reguleId = parseInt($("#ReguleId").val());
    if (isNaN(reguleId) || reguleId < 1) {
        $("#ExerciceRegule, #PeriodeDeb, #PeriodeFin").clear();
        $("#btnReguleSuivant").attr("title", "").disable();
        return;
    }
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/CreationRegularisation/SupressionDatesRegularisation",
        data: {
            reguleId: $("#ReguleId").val()
        },
        success: function (data) {
            if ($("#isContratTempo").val() !== "True") {
                $("#ExerciceRegule").removeAttr("disabled").removeAttr("readonly").removeClass("readonly");
            }
            $("#PeriodeDeb").removeAttr("disabled").removeClass("readonly");
            $("#PeriodeFin").removeAttr("disabled").removeClass("readonly");
            $("#deleteMod").val('D');
            $("#ExerciceRegule").clear();
            $("#PeriodeDeb").clear();
            $("#PeriodeFin").clear();
            $("#btnReguleSuivant").attr("title", "").disable();
            $("#inCourtier").clear();
            $("#inHorsCATNAT").val(0);
            $("#inCATNAT").val(0);
            $("#inQuittancement").clear();
            $("#btnDeleteRegule").removeClass("CursorPointer");
            let params = common.albParam.buildObject();
            if (params.REGULMOD !== "BNS" && params.REGULMOD !== "BURNER") {
                $("select[id='MotifAvt']").clear();
            }
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function DeleteRegule(reguleId) {
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
            if ($("#isContratTempo").val() !== "True") {
                $("#ExerciceRegule").removeAttr("disabled").removeAttr("readonly").removeClass("readonly");
            }

            MapListRegularisation();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function ValidUpdateRegule() {
    var typeTraitement = $("#TypeTraitement").val();
    var datedebutavn = $("#DateDebutAvenant").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    //alert("type = " + type + " - date = " + datedebutavn);

    var datefin = $("#btnUpdateTypeRegule").attr("albdatefinavn");
    var codeAvn = $("#btnUpdateTypeRegule").attr("albcodeavn");
    var numReg = $("#btnUpdateTypeRegule").attr("albreguleid");

    datefin = datefin.split(' ')[0];
    var anneeFinReg = datefin.split('/')[2];
    var moisFinReg = datefin.split('/')[1];
    var jourFinReg = datefin.split('/')[0];

    //alert("datefinregule = " + anneeFinReg + moisFinReg + jourFinReg);

    $(".requiredField").removeClass("requiredField");
    var error = false;
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
            $("#divDataUpdateRegul").html('');

            //MapListRegularisation();
            ReloadListeRegularisation();
            DeverouillerUserOffres($("#tabGuid").val());

            CloseLoading();
        },
        error: function (request) {
            DeverouillerUserOffres($("#tabGuid").val());
            common.error.showXhr(request);
        }
    });

}

function ValidCourtierRegule() {
    $("#oldCourtier").val($("#ddlCourtiers").val());
    $("#oldCourtierCom").val($("#ddlCourtiersCom").val());
    $("#oldQuittancement").val($("#ddlQuittancements").val());
    $("#oldHCATNAT").val($("#ComHCATNAT").val());
    $("#oldCATNAT").val($("#ComCATNAT").val());

    $("#inCourtier").val($("#ddlCourtiers option:selected").text());
    //$("#inCourtier").val($("#ddlCourtiers").val());
    $("#inCourtierCom").val($("#ddlCourtiersCom option:selected").text());
    //$("#inCourtierCom").val($("#ddlCourtiersCom").val());
    $("#inHorsCATNAT").val($("#ComHCATNAT").val());
    $("#inCATNAT").val($("#ComCATNAT").val());
    $("#inQuittancement").val($("#ddlQuittancements").find("option:selected").attr("title")).attr("albcode", $("#ddlQuittancements").val());

    $("#dvChoixCourtier").hide();
    $("#btnReguleSuivant").removeAttr("disabled");

    $("#btnCancelCourtier").hide();
    $("#btnCloseCourtier").show();
    $("#btnValidCourtier").attr("disabled", "disabled");
}

function changePeriod(id) {
    $("#PeriodeValide").val("0");
    $(".requiredField").removeClass("requiredField");
    let isExerciceChanging = id === "ExerciceRegule";
    let emptyVal = !$("#" + id).hasVal();
    if (isExerciceChanging) {
        $("#PeriodeDeb").clear();
        $("#PeriodeFin").clear();
    }
    else {
        $("#ExerciceRegule").clear();
        emptyVal = !$("#PeriodeFin").hasVal() || !$("#PeriodeDeb").hasVal();
    }
    if (emptyVal || !isExerciceChanging && !checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
        $("#btnReguleSuivant").attr("title", "").disable();
        return false;
    }

    let url = "/CreationRegularisation/Change" + (isExerciceChanging ? "Exercice" : "Periode");
    common.page.isLoading = true;
    let postData = {
        codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
        codeAvn: $("#NumInterne").val(), typeAvt: $("#TypeAvt").val(), periodeDeb: $("#PeriodeDeb").val(),
        periodeFin: $("#PeriodeFin").val(), lotId: $("#lotId").val(), reguleId: $("#ReguleId").val(), regulMode: common.getRegul().mode,
        deleteMod: $("#deleteMod").val(), cancelMod: $("#cancelMod").val()
    };
    if (isExerciceChanging) {
        postData.exercice = $("#ExerciceRegule").val();
    }
    common.$postJson(url, postData)
        .done(function (data) {
            $("#dvReguleCourtier").html(data);
            setTimeout(function () {
                let hasSelections = $("#HasSelections").hasTrueVal();
                if (hasSelections) {
                    $("#btnReguleSuivant").show().enable();
                }
                else {
                    $("#btnReguleSuivant").attr("title", "Pas de régularisation possible pour la période sélectionnée");
                    $("#btnReguleSuivant").disable();
                }

                common.autonumeric.applyAll('init', 'decimal');
                common.autonumeric.applyAll('init', 'pourcentdecimal');
                if (isExerciceChanging) {
                    common.autonumeric.applyAll('init', 'year', '', null, 0, 9999, 0);
                }

                MapElementCourtier();

                var retourPGM = $("#inRetourPGM").val();
                if (retourPGM != "") {
                    $("#PeriodeValide").val(hasSelections ? "1" : "0");
                    $("#PeriodeDeb").val(retourPGM.split("_")[0]).removeClass("requiredField");
                    $("#PeriodeFin").val(retourPGM.split("_")[1]).removeClass("requiredField");
                }
                $("#btnDeleteRegule").addClass("CursorPointer");
            }, 5);
            requestAnimationFrame(function () {
                common.page.isLoading = false;
            });
        })
        .fail(function (request) {
            if (isExerciceChanging) {
                $("#ExerciceRegule").addClass("requiredField");
            }
            else {
                $("#PeriodeDeb").addClass("requiredField");
                $("#PeriodeFin").addClass("requiredField");
            }
            $("#inCourtier").clear();
            $("#inHorsCATNAT").clear();
            $("#inCATNAT").clear();
            $("#inQuittancement").clear();
            $("#btnModifCourtier").hide();
            $("#btnReguleSuivant").disable();
            common.error.showXhr(request);
        });
}

function leapStepsOrGoNext(data, returnHome) {
    let action = "Step2_ChoixRisque";
    let controller = "CreationRegularisation";

    let params = common.albParam.buildObject();
    if (data.IsSimplified) {
        params.RSQID = data.IdRsq;
        params.REGULGARID = data.RgGrId;
        params.GARID = data.IdGar;
        $("#AddParamValue").val(common.albParam.objectToString(params));
        if (data.CodeGar.indexOf("RCFR") !== -1 && data.IsMultiRC) {
            action = "CalculGarantiesRC";
            controller = "Regularisation";
        }
        else {
            controller = "CreationRegularisation";
            action = "Step5_RegulContrat";
        }
    }
    else if (data.NbRisques == 1) {
        params.RSQID = data.IdRsq;
        params.GARID = data.NbGaranties == 1 || data.IsMultiRC ? data.IdGar : 0;
        $("#AddParamValue").val(common.albParam.objectToString(params));
        action = data.NbGaranties == 1 || data.IsMultiRC ? "Step4_ChoixPeriodeGarantie" : "Step3_ChoixGarantie";
    }

    RedirectionRegul(action, controller, returnHome);
};

//--------Annule les modifications effectuées dans la div flottante de gestion des courtiers---------
function CancelCourtierRegule() {
    $("#ddlCourtiers").val($("#oldCourtier").val());
    $("#ddlCourtiersCom").val($("#oldCourtierCom").val());
    $("#ddlQuittancements").val($("#oldQuittancement").val());
    $("#ComHCATNAT").val($("#oldHCATNAT").val());
    $("#ComCATNAT").val($("#oldCATNAT").val());

    $("#dvChoixCourtier").hide();

    $("#btnCancelCourtier").hide();
    $("#btnCloseCourtier").show();
    $("#btnValidCourtier").attr("disabled", "disabled");

    $("#btnReguleSuivant").removeAttr("disabled");
}
