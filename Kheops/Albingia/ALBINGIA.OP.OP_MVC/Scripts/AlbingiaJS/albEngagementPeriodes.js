/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
function LoadEngagementsJS() {
    MapElementPage();
}

//-------------------------Action des Buttons
function MapElementPage() {
    AlternanceLigne("EngagementPeriodesBody", "", false, null);

    $("#btnSuivant").kclick(function (evt, data) {
        terminer(data && data.returnHome);
    });
    $("#btnAnnuler").kclick(function () {
        var iframe = $("#accessMode").val() != "";
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true, true, iframe);
    });

    $("#btnFermer").kclick(function () {
        ClosePeriodeEng();
    });

    MapElementTabEngagementPeriodes();

    $("#ListeModesActif").offOn("change", function () {
        if ($(this).val() == "A") {
            $("tr[name='lineEngagementPeriode'][albPeriodeActive!='A']").hide();
        }
        else {
            $("tr[name='lineEngagementPeriode']").show();
        }
    });

    $("#ListeModesActif").change();
}

function MapElementTabEngagementPeriodes() {
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly) {
        $("img[name=ajoutEngagementPeriode]").hide();
        $("img[name=deleteEngagementPeriode]").hide();
    }

    $("#ajouterEngagementPeriode").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            $("#actionParameter").val("-9999");
            $("#codePeriodeCourant").val("-9999");
            $("#dateDebutNewPeriode").removeAttr("disabled").val("");
            $("#dateFinNewPeriode").removeAttr("disabled").val("");
            $('#typeOperation').val('Insert');
            $("#OffreReadOnly").val('False');
            $("#divAjoutPeriode").show();
        }
    });

    $("td[name=selectableCol]").kclick(function () {
        var id = $(this).attr('id').split('_')[2];
        if ($("#actif_" + id).val() == "A") {
            //TODO ouverture de la div avec les dates

            //if ($("#ajouterEngagementPeriode").hasClass("CursorPointer")) {
            //    $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            //}
            //$("div[name=colReadOnly_" + id + "]").hide();
            //$("div[name=colEdition_" + id + "]").show();
            //$("tr[name=lineEngagementPeriode]").die();
        }
    });

    $("img[name=consultEngagementPeriode]").kclick(function () {
        var idPeriode = $(this).attr('id').split('_')[1];
        $("#actionParameter").val(idPeriode);
        $("#codePeriodeCourant").val(idPeriode);
        $("#typeOperation").val("Consult");
        var dateDeb = $("#lblDateDeb_" + idPeriode).html();
        var dateFin = $("#lblDateFin_" + idPeriode).html();
        $("#dateDebutNewPeriode").val(dateDeb);
        $("#dateFinNewPeriode").val(dateFin);
        $("#divAjoutPeriode").show();
        $("#dateDebutNewPeriode, #dateFinNewPeriode").disable();
        $("#Msg_Calcul").attr('style', 'visibility:hidden');
    });

    $("img[name=updateEngagementPeriode]").kclick(function () {
        if (window.isReadonly) {
            return;
        }
        let idPeriode = this.id.split('_')[1];
        let INhpeng = this.id.split('_')[2];
        $("#actionParameter").val(idPeriode);
        $("#codePeriodeCourant").val(idPeriode);


        $("#typeOperation").val("Update");
        let dateDeb = $("#lblDateDeb_" + idPeriode).html();
        let dateFin = $("#lblDateFin_" + idPeriode).html();
        $("#dateDebutNewPeriode").val(dateDeb);
        $("#dateFinNewPeriode").val(dateFin);
        $("#divAjoutPeriode").show();

        if (window.isModifHorsAvenant) {
            $("#dateDebutNewPeriode, #dateFinNewPeriode").disable();
        }
        else {
            $("#dateFinNewPeriode").disable();
            if ($("input[id='utilise_" + idPeriode + "']").val() === "U") {
                $("#dateDebutNewPeriode").disable();
                $("#dateFinNewPeriode").enable();
            }
            else {
                if (INhpeng == "O") {
                    $("#dateDebutNewPeriode").disable();
                    $("#typeOperation").val("Consult");
                    $("#Msg_Calcul").attr('style', 'visibility:visible;color:red');

                } else {
                    $("#Msg_Calcul").attr('style', 'visibility:hidden');
                    $("#dateDebutNewPeriode").enable();
                }

                $("#dateFinNewPeriode").enable();

            }
        }
    });
    $("img[name=deleteEngagementPeriode]").kclick(function () {
        var iframe = $("#accessMode").val() != "";
        $("#actionParameter").val($(this).attr('id').split('_')[1]);
        $('#typeOperation').val('Delete');
        $("#OffreReadOnly").val('False');
        ShowCommonFancy("Confirm", "Delete", "Etes-vous sûr de vouloir supprimer cette période?", 320, 150, true, true, true, iframe);
    });
    $("img[name=disableEngagementPeriode]").kclick(function () {
        var iframe = $("#accessMode").val() != "";
        $("#actionParameter").val($(this).attr('id').split('_')[1]);
        $('#typeOperation').val('Disable');
        $("#OffreReadOnly").val('False');
        ShowCommonFancy("Confirm", "Disable", "Attention vous allez désactiver cette période.<BR>La réassurrance va être annulée et recalculée.", 320, 150, true, true, true, iframe);
    });

    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "RedirectionTerminer":
                RedirectionTerminer();
                break;
            case "RedirectionTraite":
                RedirectionCdPeriode("Engagements", "Index", $("#actionParameter").val());
                break;
            case "Cancel":
                $("#actionParameter").val('');
                RedirectionTerminer("MatriceRisque");
                break;
            case "Disable":
                DisableLineEngagementPeriode();
                break;
            case "Delete":
                DeleteLineEngagementPeriode();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    $("#btnAnnulerNewPeriode").kclick(function () {
        $("#divAjoutPeriode").hide();
        $("#dateDebutNewPeriode").removeClass("requiredField");
        $("#dateFinNewPeriode").removeClass("requiredField");
        $("#codePeriodeCourant").val("");
        $("#dateDebutNewPeriode").val("");
        $("#dateFinNewPeriode").val("");
    });

    $("#btnSuivantNewPeriode").kclick(function () {
        let dateDeb = $("#dateDebutNewPeriode").val();
        let dateFin = $("#dateFinNewPeriode").val();
        let a = dateDeb.split('/');
        dateDeb = parseInt(a[2] + a[1] + a[0]);
        a = dateFin.split('/');
        dateFin = parseInt(a[2] + a[1] + a[0]);
        if (dateDeb > dateFin) {
            $("#dateFinNewPeriode").addClass('requiredField');
            $("#dateDebutNewPeriode").addClass('requiredField');
            alert("Incohérence entre la date de début et la date de fin");
        } else
            SaveLineEngagementPeriode($("#codePeriodeCourant").val());
    });

    FormatDecimalValue();
}
function FormatDecimalValue() {
    common.autonumeric.applyAll("init", "decimal", " ", null, null, "99999999999.99", null);
    common.autonumeric.applyAll("init", "pourcentdecimal", "");
};

//----------------Sauvegarde une période d'engagement--------------------
function SaveLineEngagementPeriode(code) {
    let engTotal = $("#EngagementTotal_" + code).val() != "" ? $("#EngagementTotal_" + code).val() : "0";
    let engAlbingia = $("#EngagementAlbingia_" + code).val() != "" ? $("#EngagementAlbingia_" + code).val() : "0";
    let utiliseStatut = $("#utilise_" + code).val();
    let dateDeb = $("#dateDebutNewPeriode").val();
    let dateFin = $("#dateFinNewPeriode").val();
    let typeOperation = $("#typeOperation").val();
    let a = dateDeb.split('/');
    dateDeb = parseInt(a[2] + a[1] + a[0]);
    a = dateFin.split('/');
    dateFin = parseInt(a[2] + a[1] + a[0]);

    common.page.isLoading = true;

    $("#dateDebutNewPeriode, #dateFinNewPeriode, #EngagementTotal_" + code + ", #EngagementAlbingia_" + code).removeClass("requiredField");

    if (window.isReadonly) {
        RedirectionCdPeriode("Engagements", "Index", code);
        return;
    }
    let passTest = true;
    if (dateDeb > dateFin) {
        passTest = false;
        $("#dateFinNewPeriode").addClass('requiredField');
        $("#dateDebutNewPeriode").addClass('requiredField');
    }

    if (engTotal === "") {
        $("#EngagementTotal_" + code).addClass('requiredField');
    }
    if (engAlbingia === "") {
        $("#EngagementAlbingia_" + code).addClass('requiredField');
    }
    if (passTest) {
        common.page.isLoading = true;
        let dataValid = CheckData(code);
        if (dataValid) {
            const postedData = {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                code: code, dateDeb: dateDeb, dateFin: dateFin,
                engTotal: engTotal, engAlbingia: engAlbingia, partAlbingia: $("#Part_" + code).val() != "" ? $("#Part_" + code).val() : "0",
                actifStatut: $("#actif_" + code).val(), utiliseStatut: $("#utilise_" + code).val(), typeOperation: typeOperation,
                isReadOnly: false, tabGuid: common.tabGuid, modenavig: $("#ModeNavig").val(), addParamType: $("#AddParamType").val(), addParamValue: $("#AddParamValue").val(),
                dateDebContrat: $("#debutEffetContrat").val(), dateFinContrat: $("#finEffetContrat").val(),
                accessMode: $("#accessMode").val(),
            };
            common.$postJson("/EngagementPeriodes/SaveLineEngagementPeriode", postedData, true).done(function (data, s, jqXhr) {
                if (!jqXhr.getResponseHeader("Content-Type").indexOf("text/html") == 0) {
                    return;
                }

                $("#divEngagementPeriodesBodyLines").html(data);
                AlternanceLigne("EngagementPeriodesBody", "noInput", true, null);
                MapElementTabEngagementPeriodes();
                if ($("#newEngagementPeriode").isVisible()) {
                    $("#newEngagementPeriode").hide();
                }
                if (typeOperation == "Insert") {
                    ClearEmptyLine(code);
                }
                $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");

                if (typeOperation.toUpperCase() === 'UPDATE' && utiliseStatut === 'U') {
                    $('#divAjoutPeriode').hide();
                }
            });
        }
        else {
            common.page.isLoading = false;
        }
    }
}
//--------------Supprime la ligne de la période d'engagement---------------
function DeleteLineEngagementPeriode() {
    common.page.isLoading = true;
    var code = $("#actionParameter").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var dateDeb = $("#dateDeb_" + code).val();
    var dateFin = $("#dateFin_" + code).val();
    var engTotal = $("#EngagementTotal_" + code).val();
    var engAlbingia = $("#EngagementAlbingia_" + code).val();
    var partAlbingia = $("#Part_" + code).val();
    var actifStatut = $("#actif_" + code).val();
    var utiliseStatut = $("#utilise_" + code).val();
    var typeOperation = "Delete";
    var tabGuid = common.tabGuid;
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/EngagementPeriodes/SaveLineEngagementPeriode/",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), code: code, dateDeb: dateDeb, dateFin: dateFin,
            engTotal: engTotal, engAlbingia: engAlbingia, partAlbingia: partAlbingia, actifStatut: actifStatut, utiliseStatut: utiliseStatut, typeOperation: typeOperation,
            tabGuid: tabGuid, modenavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, isReadOnly: $("#OffreReadOnly").val(),
            dateDebContrat: $("#debutEffetContrat").val(), dateFinContrat: $("#finEffetContrat").val()
        },
        success: function (data) {
            $("#divEngagementPeriodesBodyLines").html(data);
            AlternanceLigne("EngagementPeriodesBody", "noInput", true, null);
            MapElementTabEngagementPeriodes();
            if ($("#newEngagementPeriode").is(":visible"))
                $("#newEngagementPeriode").hide();
            ClearEmptyLine(code);
            $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
            common.page.isLoading = false;

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    $("#actionParameter").val('');
}
//------------------desactive la ligne de la période d'engagement--------------
function DisableLineEngagementPeriode() {
    common.page.isLoading = true;
    var code = $("#actionParameter").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var dateDeb = $("#dateDeb_" + code).val();
    var dateFin = $("#dateFin_" + code).val();
    var engTotal = $("#EngagementTotal_" + code).val();
    var engAlbingia = $("#EngagementAlbingia_" + code).val();
    var partAlbingia = $("#Part_" + code).val();
    var actifStatut = "I";
    var utiliseStatut = $("#utilise_" + code).val();
    var typeOperation = "Disable";
    var tabGuid = common.tabGuid;
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var checkResult = true;//CheckData(code);//pas de verif, on desactive la ligne
    if (checkResult) {
        $.ajax({
            type: "POST",
            url: "/EngagementPeriodes/SaveLineEngagementPeriode/",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), code: code, dateDeb: dateDeb, dateFin: dateFin,
                engTotal: engTotal, engAlbingia: engAlbingia, partAlbingia: partAlbingia, actifStatut: actifStatut, utiliseStatut: utiliseStatut, typeOperation: typeOperation,
                tabGuid: tabGuid, modenavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, isReadOnly: $("#OffreReadOnly").val(),
                dateDebContrat: $("#debutEffetContrat").val(), dateFinContrat: $("#finEffetContrat").val()
            },
            success: function (data) {
                $("#divEngagementPeriodesBodyLines").html(data);
                AlternanceLigne("EngagementPeriodesBody", "noInput", true, null);
                MapElementTabEngagementPeriodes();
                if ($("#newEngagementPeriode").is(":visible"))
                    $("#newEngagementPeriode").hide();
                ClearEmptyLine(code);
                $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                common.page.isLoading = false;

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    $("#actionParameter").val('');
}
//----------------Nettoie la ligne d'insertion--------------
function ClearEmptyLine(code) {
    $("#dateDeb_" + code).val('').removeClass('requiredField');
    $("#dateFin_" + code).val('').removeClass('requiredField');
    $("#EngagementTotal_" + code).val('').removeClass('requiredField');
    $("#EngagementAlbingia_" + code).val('').removeClass('requiredField');
}
//------------- Terminer-------
function terminer(returnHome) {
    if (CheckBeforeRedirect("RedirectionTerminer") || $("OffreReadOnly").val() != "True")
        RedirectionTerminer(null, returnHome);
}
//------------- Redirection-----
function Redirection(cible, job) {
    common.page.isLoading = true;
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = common.tabGuid;
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/EngagementPeriodes/Redirection/",
        data: { cible: cible, job: job, codeContrat: codeContrat, versionContrat: versionContrat, type: type, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------- Redirection du button Terminer-----
function RedirectionTerminer(controller, returnHome) {
    if (controller == undefined)
        controller = "";

    common.page.isLoading = true;
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = common.tabGuid;
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/EngagementPeriodes/RedirectionTerminer" + (returnHome ? "?returnHome=1" : ""),
        data: {
            codeContrat: codeContrat, versionContrat: versionContrat, type: type, codeAvn: $("#NumAvenantPage").val(),
            tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig,
            addParamType: addParamType, addParamValue: addParamValue, controller: controller
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//------------Redirection avec cdPeriode
function RedirectionCdPeriode(cible, job, code) {
    common.page.isLoading = true;
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = common.tabGuid;
    var accessMode = $("#accessMode").val();

    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    const data = {
        cible: cible,
        job: job,
        codeContrat: codeContrat,
        versionContrat: versionContrat,
        type: type,
        cdPeriode: "cdPeriode" + code + "cdPeriode",
        tabGuid: tabGuid,
        modeNavig: modeNavig,
        addParamType: addParamType,
        addParamValue: addParamValue,
        modeConsult: "ConsultOnly",
        accessMode: accessMode
    };

    $.ajax({
        type: "POST",
        url: "/EngagementPeriodes/Redirection/",
        data: data,
        success: function (data) {
            // do nothing
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Vérifie les données de la période d'engagement---------------
function CheckData(code) {
    var toReturn = true;
    var msgError = "";

    //Vérification de la date de début
    msgError = CheckStartDate(code, msgError);
    if (msgError != "") {
        $("#dateDeb_" + code).addClass('requiredField');
    }
    //Vérification de la date de fin
    msgError = CheckEndDate(code, msgError);
    if (msgError != "")
        $("#DateFin_" + code).addClass('requiredField');

    //Vérification des empiètements
    if (msgError == "") {
        msgError = CheckEmpietement(code, msgError);
    }

    //Affichage du message d'erreur
    if (msgError != "") {
        var iframe = $("#accessMode").val() != "";
        toReturn = false;
        ShowCommonFancy("Error", "", msgError, 700, 400, true, true, true, iframe);
    }
    return toReturn;
}
//------------Vérifie la date de début-----------------------
function CheckStartDate(code, msgError) {
    // Si la date de début est celle de la premiere periode et que son format est incorrect, on ignore les tests
    let codePremierePeriode = $("#codePremierePeriode").val();
    let dateDeb = parseInt($("#dateDebutNewPeriode").val().split('/').reverse().join(""));
    if (codePremierePeriode == "0") {
        return msgError;
    }
    // Vérification si une date est présente
    if (isNaN(dateDeb)) {
        msgError += "- Aucune date de début de période saisie<BR>";
        return msgError;
    }

    // Comparaison à la date d'effet du contrat
    if (dateDeb < parseInt($("#debutEffetContrat").val())) {
        msgError += "- La date de début doit être supérieure ou égale à la date d'effet du contrat<BR>";
    }
    return msgError;
}
//---------------Vérifie la date de fin--------------------
function CheckEndDate(code, msgError) {

    //Verification de l'obligation de la date de fin ou non
    var isMandatory = true;
    // var dateDeb = $("#DateDeb_" + code).val();
    var iDateDeb = $("#dateDebutNewPeriode").val();
    iDateDeb = iDateDeb.split('/')[2] + iDateDeb.split('/')[1] + iDateDeb.split('/')[0];
    if (iDateDeb >= $("#debutDernierePeriode").val())
        isMandatory = false;

    //Si il s'agit de la premiere periode, on ignore la date de début
    var codePremierePeriode = $("#codePremierePeriode").val();
    if (codePremierePeriode == code)
        iDateDeb = 0;

    ////Vérification de la validité du format
    //var dateFin = $("#DateFin_" + code).val();
    //if ((dateFin.split('/').length != 3) && (isMandatory == false))
    //    return msgError;
    //else if ((dateFin.split('/').length != 3) && (isMandatory == true)) {
    //    msgError += "- Le format de la date de fin est incorrect<BR>";
    //    return msgError;
    //}
    //Conversion de la date en chiffre pour comparaison
    var iDateFin = $("#dateFinNewPeriode").val();
    iDateFin = iDateFin.split('/')[2] + iDateFin.split('/')[1] + iDateFin.split('/')[0];
    //Comparaison de la date debut et la date fin
    if (iDateFin < iDateDeb) {
        msgError += "- La date de fin doit être postérieure à la date de début de la période<BR>";
        return msgError;
    }


    //Comparaison à la date d'effet du contrat si elle est renseignée
    if ($("#finEffetContrat").val() != "00000") {
        if (iDateFin != "NaN" && iDateFin > $("#finEffetContrat").val())
            msgError += "- La date de fin doit être inférieure ou égale à la date de fin d'effet du contrat<BR>";
    }
    return msgError;
}
//--------------Vérifie si la plage de saisie empiète sur une période active-------------
function CheckEmpietement(code, msgError) {
    //Récupération de la plage saisie et conversion en chiffre pour comparaison
    var dateDeb = $("#dateDebutNewPeriode").val();
    var iDateDeb = dateDeb;
    iDateDeb = iDateDeb.split('/')[2] + iDateDeb.split('/')[1] + iDateDeb.split('/')[0];

    var dateFin = $("#dateFinNewPeriode").val();
    var iDateFin = dateFin;
    iDateFin = iDateFin.split('/')[2] + iDateFin.split('/')[1] + iDateFin.split('/')[0];

    var codePremierePeriode = $("#codePremierePeriode").val();
    var codeDernierePeriode = $("#codeDernierePeriode").val();

    var isCorrect = true;
    //Récupération des périodes actives dans le tableau
    $('input[name=statutActif][value=A]').each(function () {
        var id = $(this).attr("id").split('_')[1];
        var dateDebActif = $("#dateDeb_" + id).val();
        var dateFinActif = $("#dateFin_" + id).val();

        if ((id != code) && (id != codePremierePeriode) && (id != codeDernierePeriode) && (dateFinActif != 0) && (dateDebActif != 0)) {

            if (iDateDeb == dateDebActif || iDateDeb == dateFinActif || iDateFin == dateDebActif || iDateFin == dateFinActif)
                isCorrect = false;

            if (iDateDeb < dateDebActif && iDateFin > dateFinActif)
                isCorrect = false;

            if (iDateDeb > dateDebActif && iDateDeb < dateFinActif)
                isCorrect = false;

            if (iDateFin > dateDebActif && iDateFin < dateFinActif)
                isCorrect = false;

            if (isCorrect == false) {
                var jourD = dateDebActif.substring(6, 8);
                var moisD = dateDebActif.substring(4, 6);
                var anneeD = dateDebActif.substring(0, 4);
                var jourF = dateFinActif.substring(6, 8);
                var moisF = dateFinActif.substring(4, 6);
                var anneeF = dateFinActif.substring(0, 4);
                msgError += "- La plage saisie empiète sur la période active du " + jourD + "/" + moisD + "/" + anneeD + " au " + jourF + "/" + moisF + "/" + anneeF + "<BR>";
                isCorrect = true;
            }
        }
    });
    return msgError;
}
//-----------------Vérifie s'il existe des périodes discontinues-----------------
function CheckPeriodesDiscontinues(msgError) {
    var dateEffetContrat = $("#debutEffetContrat").val();
    var datePrecedente = 1;
    var codePrecedent = -1;
    var dateDebutPrecedente = 1;
    $('input[name=statutActif][value=A]').each(function () {
        var id = $(this).attr("id").split('_')[1];
        var dateDebActif = $("#dateDeb_" + id).val();
        var dateFinActif = $("#dateFin_" + id).val();

        //Desactivé car la première date de début active est toujours vide
        //if (datePrecedente == 1) {
        //    if (dateDebActif < dateEffetContrat) {
        //        msgError = "La première période commence avant la date d'effet du contrat*" + dateEffetContrat + "*" + dateFinActif;
        //        return msgError;
        //    }
        //    else
        //        datePrecedente = dateEffetContrat;
        //}

        if ((datePrecedente != 1) && (msgError == "")) {
            if (dateDebActif != parseInt(datePrecedente) + 1) {
                if ($("#dateCreationContrat") >= $("#dateControle")) {
                    $("#colRequired_" + id).show();
                    $("#colRequired_" + codePrecedent).show();
                    // $("#tr_" + id).addClass('requiredRow');
                    // $("#tr_" + codePrecedent).addClass('requiredRow');
                    msgError = "Il ne peut pas y avoir de périodes discontinues_" + dateDebutPrecedente + "_" + dateDebActif;
                    return msgError;
                }
                else {
                    msgError = "Périodes discontinues¤" + dateDebActif + "¤" + dateFinActif;
                    return msgError;
                }
            }
        }
        datePrecedente = dateFinActif;
        codePrecedent = id;
        dateDebutPrecedente = dateDebActif;
    });
    return msgError;
}
//-----------------Lance les vérifications avant de quitter l'écran et affiche les éventuels messages d'erreur--------------
function CheckBeforeRedirect(action) {
    var msgError = "";
    // msgError = CheckPeriodesDiscontinues(msgError);
    var iframe = $("#accessMode").val() != "";
    if (msgError != "") {
        if (msgError.split("_").length > 1) {

            var jourD = msgError.split("_")[1].substring(6, 8);
            var moisD = msgError.split("_")[1].substring(4, 6);
            var anneeD = msgError.split("_")[1].substring(0, 4);
            var jourF = msgError.split("_")[2].substring(6, 8);
            var moisF = msgError.split("_")[2].substring(4, 6);
            var anneeF = msgError.split("_")[2].substring(0, 4);


            var alertTxt = "Attention, il y a une discontinuité des dates entre le " + jourD + "/" + moisD + "/" + anneeD;
            alertTxt += " et le " + jourF + "/" + moisF + "/" + anneeF + ". ";
            //$("#divTexteAlerteDiscontinuite").html(alertTxt);
            //$("#divCliquezIci").attr("albDates", msgError.split("_")[1] + "_" + msgError.split("_")[2]);
            //$("#alerteDiscontinuiteDiv").show();
            ShowCommonFancy("Error", "", alertTxt, 1212, 700, true, true, true, iframe);
            return false;
        }
        else if (msgError.split("¤").length > 1) {

            var jourD = msgError.split("¤")[1].substring(6, 8);
            var moisD = msgError.split("¤")[1].substring(4, 6);
            var anneeD = msgError.split("¤")[1].substring(0, 4);
            var jourF = msgError.split("¤")[2].substring(6, 8);
            var moisF = msgError.split("¤")[2].substring(4, 6);
            var anneeF = msgError.split("¤")[2].substring(0, 4);


            var alertTxt = "Attention, il y a une discontinuité des dates entre le " + jourD + "/" + moisD + "/" + anneeD;
            alertTxt += " et le " + jourF + "/" + moisF + "/" + "/" + anneeF + ". ";
            //$("#divTexteAlerteDiscontinuite").html(alertTxt);
            //$("#divCliquezIci").attr("albDates", msgError.split("¤")[1] + "_" + msgError.split("¤")[2]);
            //$("#alerteDiscontinuiteDiv").show();
            ShowCommonFancy("Confirm", action, msgError.split("¤")[0], 320, 150, true, true, true, iframe);
            return false;
        }
        else if (msgError.split("*").length > 1) {

            var jourD = msgError.split("*")[1].substring(6, 8);
            var moisD = msgError.split("*")[1].substring(4, 6);
            var anneeD = msgError.split("*")[1].substring(0, 4);
            var jourF = msgError.split("*")[2].substring(6, 8);
            var moisF = msgError.split("*")[2].substring(4, 6);
            var anneeF = msgError.split("*")[2].substring(0, 4);


            var alertTxt = msgError.split("*")[0];
            //$("#divTexteAlerteDiscontinuite").html(alertTxt);
            //$("#divCliquezIci").attr("albDates", msgError.split("*")[1] + "_" + msgError.split("*")[2]);
            //$("#alerteDiscontinuiteDiv").show();
            return false;
        }
    }
    return true;
}
//----------Ferme l'iframe de période d'engagement-----------
function ClosePeriodeEng() {
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/EngagementPeriodes/CloseEngPer",
        data: { codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val() }
    }
    ).then(
        function (data) {
            $("#EngagementIFrame", window.parent.document).attr("src", "about:blank");
            $("#divRechEngagementPeriode", window.parent.document).hide();
            if (window.parent != window.top) {
                return window.top.UnlockSessionUserFolder();
            }
        }
    ).then(CloseLoading
    ).fail(
        function (request) {
            common.error.showXhr(request);
        }
    );

}