$(document).ready(function () {
    MapElementPage();
});


function MapElementPage() {
    MapSaisiePeriodGarElement();
}


function MapSaisiePeriodGarElement() {
    $("#btnReguleSuivant").removeAttr('disabled');
    $("#TextRed").css({ "display": "none" });


    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_gris3232.png');
    $("#btnRefresh").removeClass("CursorPointer");



    $("#btnReguleSuivant").kclick(function (evt, data) {
        SuivantSaisie(data && data.returnHome);
    });

    $("#btnRegulePrec").kclick(function () {

        ShowCommonFancy("Confirm", "Cancel",
            "Attention, les informations d'assiette vont être réinitialisées.<br/><br/>Confirmez-vous votre action ?<br/>",
            350, 130, true, true);
    });

    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":


                var addParam = $('#AddParamValue').val();

                var url = '/CreationRegularisation/IsSimplifiedRegule/';

                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                        reguleGarId: $("#idregulgar").val(), addParamValue: addParam
                    },
                    success: function (data) {
                        if (data.IsSimplifiedRegule) {
                            RedirectionRegul('Step1_ChoixPeriode', 'CreationRegularisation', false);
                        }
                        else {
                            $('#AddParamValue').val(addParam.substring(0, addParam.lastIndexOf('||REGULGARID')));
                            RedirectionRegul('Step4_ChoixPeriodeGarantie', 'CreationRegularisation', false, 'Previous');
                        }
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });


                break;
        }
        $("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").clear();
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

    $("#unitedef").die().live("change", function () {
        AffectTitleList($(this));
        ToggleButton();
        if ($(this).val() == "D") {
            $("#TauxMontantGarDef").attr("albmask", "numeric");
            common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'numeric', '', null, 2, '99999999999.99', '-99999999999.99');
        }
        else {
            if ($(this).val() == "%" && parseInt($("#TauxMontantGarDef").autoNumeric('get')) > 100) {
                $("#TauxMontantGarDef").val("");
                $("#TauxMontantGarDef").attr("albmask", "decimal");
                common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'decimal', '', null, 4, '999.9999', '-999.9999');
            }
            else if ($(this).val() == "%0" && parseInt($("#TauxMontantGarDef").autoNumeric('get')) > 1000) {
                $("#TauxMontantGarDef").val("");
                common.autonumeric.apply($("#TauxMontantGarDef"), 'update', 'decimal', '', null, 4, '9999.9999', '-9999.9999');
            }
        }

        //if ($("#unitedef").val() == "D") {
        //    common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'numeric', '', null, 2, '99999999999.99', '-99999999999.99');
        //}
        //else {
        //    common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'decimal', '', null, 4, '999.9999', '-999.9999');
        //}

    });
    $("#codetaxedef").die().live("change", function () {
        AffectTitleList($(this));
        ToggleButton();
    });
    $("#AssietteGarantieDef").die().live('change', function () {
        ToggleButton();
    });
    $("#TauxMontantGarDef").die().live('change', function () {
        ToggleButton();
    });
    $("#codeprev").die().live("change", function () {
        AffectTitleList($(this));
        ToggleButton();
    });

    $("#codetaxedef").die().live("change", function () {
        AffectTitleList($(this));
        ToggleButton();
    });
    $("#AssietteGarantie").die().live('change', function () {
        ToggleButton();
    });

    $("#TauxMontantGar").die().live('change', function () {
        ToggleButton();
    });
    $("#MontantCotisEmiseForce").die().live('change', function () {
        ToggleButton();
    });
    $("#MntTaxePrimeEmise").die().live('change', function () {
        ToggleButton();
    });
    $("#coefficient").die().live('change', function () {
        ToggleButton();
    });


    $("#IsCheckForcer").die().live('change', function () {
        if ($("#IsCheckForcer").is(":checked")) {
            //ZBO 12092016 : Correction suite au point du 12092016 bug 2143  
            //$("#MontantForcePrimecalcul").attr('readonly', 'readonly').addClass('readonly');
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
        $("#AssietteGarantieDef").attr("readOnly", "readonly").addClass("readonly");
        $("#TauxMontantGarDef").attr("readOnly", "readonly").addClass("readonly");
        $("#unitedef").attr("disabled", "disabled").addClass("readonly");
        $("#codetaxedef").attr("disabled", "disabled").addClass("readonly");
        $("#MontantCotisEmiseForce").attr("readOnly", "readonly").addClass("readonly");
        $("#MntTaxePrimeEmise").attr("readOnly", "readonly").addClass("readonly");
        $("#AttentatGareat").attr("readOnly", "readonly").addClass("readonly");
        $("#MontantForcePrimecalcul").attr("readOnly", "readonly").addClass("readonly");
        $("#IsCheckForcer").attr("disabled", "disabled").addClass("readonly");
        if ($("#IsSimplifiedRegul").val() !== "True") {
            $("#btnReguleSuivant").addClass("None");
        }
        //
        //$("#btnSaisieCancel").html("Fermer");
    }
    else {        
            $("#AssietteGarantieDef").attr("readOnly", "readonly").addClass("readonly");
            $("#TauxMontantGarDef").attr("readOnly", "readonly").addClass("readonly");
            $("#unitedef").attr("disabled", "disabled").addClass("readonly");
            $("#codetaxedef").attr("disabled", "disabled").addClass("readonly");
       
            $("#AssietteGarantieDef").removeAttr("readOnly").removeClass("readonly");
            $("#TauxMontantGarDef").removeAttr("readOnly").removeClass("readonly");
            $("#unitedef").removeAttr("disabled").removeClass("readonly");
            $("#codetaxedef").removeAttr("disabled").removeClass("readonly");
       

        if ($("#motifInf").val() != "1") {
            $("#MontantCotisEmiseForce").removeAttr("readOnly").removeClass("readonly");
            $("#MntTaxePrimeEmise").removeAttr("readOnly").removeClass("readonly");
            $("#AttentatGareat").removeAttr("readOnly").removeClass("readonly");
            $("#MontantForcePrimecalcul").removeAttr("readOnly").removeClass("readonly");
            $("#IsCheckForcer").removeAttr("disabled").removeClass("readonly");
        }
        $("#btnSaisieSuivant").removeClass("None");
    }

    if ($("#grisSuivant").val() == "True") {
        if ($("#motifInf").val() != "1")
            $("#btnReguleSuivant").attr('disabled', 'disabled')
        $("#MontantRegularisationHT").val(0);
        $("#MontantForcePrimecalcul").attr('disabled', 'disabled');
        $("#IsCheckForcer").attr("disabled", "disabled").addClass("readonly");
    }

    $("#grisSuivant").val('False');
}

function FormatDecimalNumricValue() {

    common.autonumeric.apply($("#AssietteGarantie"), 'init', 'decimal', '', null, null, '99999999999.99', '-99999999999.99');
    common.autonumeric.apply($("#AssietteGarantieDef"), 'init', 'decimal', '', null, null, '99999999999.99', '-99999999999.99');

    if ($("#uniteprev").val() == "D") {
        common.autonumeric.apply($("#TauxMontantGar"), 'init', 'numeric', ' ', null, 2, '99999999999.99', '-99999999999.99');
    }
    else {
        common.autonumeric.apply($("#TauxMontantGar"), 'init', 'decimal', ' ', null, 4, '999.9999', '-999.9999');
    }
    if ($("#unitedef").val() == "D") {
        common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'numeric', ' ', null, 2, '99999999999.99', '-99999999999.99');
    }
    else {
        common.autonumeric.apply($("#TauxMontantGarDef"), 'init', 'decimal', ' ', null, 4, '999.9999', '-999.9999');
    }

    common.autonumeric.apply($("#coefficient"), 'init', 'decimal', '', null, 3, '99.999', '-99.999');
    common.autonumeric.apply($("#TauxAppel"), 'init', 'decimal', '', null, 3, '99.999', '-99.999');

    common.autonumeric.apply($("#NbAnnee"), 'init', 'numeric', '', null, null, '99', '-99');

    common.autonumeric.apply($("#TxCotisRetenues"), 'init', 'decimal', '', null, null, '999.99', '-999.99');
    common.autonumeric.apply($("#Ristourne"), 'init', 'decimal', '', null, null, '999.99', '-999.99');

    common.autonumeric.apply($(".inNumericRegule"), 'init', 'decimal', '', null, null, '999999999.99', '-999999999.99');
}

function chargement() {
    if (ChekInput()) {
        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvenant = $("#NumAvenantPage").val();
        var type = $("#Offre_Type").val();
        var codeRsq = $("#CodeRsq").val() || $("#codeRsq").val();

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
                $("#btnSuivant").attr('disabled', 'disabled');

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

function SuivantSaisie(returnHome) {
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
                codeAvn: $("#NumAvenantPage").val(), codeRsq: $("#CodeRsq").val() || $("#codeRsq").val(), reguleGarId: $("#idregulgar").val(), typeRegule: $("#TypeGrille").val(),
                dataRow: JSON.stringify(dataRow), addParamValue: $('#AddParamValue').val()
            },
            success: function (data) {
                var redirectFromMenu = $('#txtParamRedirect').val();
                if (redirectFromMenu && redirectFromMenu.trim() !== "") {
                    return RedirectionRegul(redirectFromMenu.split('/')[1], redirectFromMenu.split('/')[0], returnHome);
                }

                var p = common.albParam.buildObject();
                delete p.REGULGARID;
                $('#AddParamValue').val(common.albParam.objectToString(p));
                
                if (data.IsSimplifiedRegule) {
                    return RedirectionRegul("Index", "Quittance", returnHome);
                }
                else {
                    return RedirectionRegul("Step4_ChoixPeriodeGarantie", "CreationRegularisation", returnHome);
                }
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

function GetDataRow() {
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
    var prevMntHT_STD = $("input[name='InfoInitregul.PrevMntHt_STD']").val();;
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

    //if (typeof ($("input[id='MontantCotiCalcul']").val()) != "undefined" && $("input[id='MontantCotiCalcul']").val() != "")
    //    prevMntHT_STD = $("input[id='MontantCotiCalcul']").val();
    //else
    //    prevMntHT_STD = 0;

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

function ChekInputPBBN() {
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

function ChekInput() {
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

function ToggleButton() {
    $("#btnRefresh").addClass("CursorPointer");
    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png')
    $("#btnReguleSuivant").attr('disabled', 'disabled');
    if ($("#motifInf").val() != "1") {
        $("#MontantForcePrimecalcul").removeAttr('disabled');
        $("#IsCheckForcer").removeAttr('disabled').removeClass('readonly');
    }
}



