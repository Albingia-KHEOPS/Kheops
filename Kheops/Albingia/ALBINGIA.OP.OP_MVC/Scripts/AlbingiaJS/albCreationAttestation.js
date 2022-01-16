$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page---------
function MapElementPage() {
    $("#btnSuivant").attr("disabled", "disabled");

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    common.autonumeric.applyAll('init', 'year', '', null, 0, 9999, 0);

    $("#btnSuivant").kclick(function (evt, data) {
        if (!$(this).is(":disabled")) {
            Suivant(data && data.returnHome);
        }
    });

    $("#btnAnnuler").die().live('click', function () {
        CancelTab();
    });

    $("#ExerciceAttes").die().live('change', function () {
        ChangeExercice();
    });

    $("#PeriodeDeb").die().live('change', function () {
        ChangePeriode();
    });

    $("#PeriodeFin").die().live('change', function () {
        ChangePeriode();
    });

    $("#IntegraliteContrat").die().live('click', function () {
        ChangeIntegralite($(this));
    });

    $("#tabPeriode").die().live('click', function () {
        var tabActif = $(".onglet-actif").attr("id");
        if (tabActif != "tabPeriode") {
            ShowCommonFancy("Confirm", "tabPeriode",
               "Attention, en changeant d'onglet vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l'action ?<br/>",
                 350, 130, true, true, true);
        }
        else {
            OpenTabPeriode();
        }
    });

    $("#tabRisqueObjet").die().live('click', function () {
        //if ($("#PeriodeValide").val() != "1") {
        //    common.dialog.error("La plage de dates est invalide");
        //    return;
        //}
        var tabActif = $(".onglet-actif").attr("id");
        if (tabActif != "tabPeriode") {
            ShowCommonFancy("Confirm", "tabRisqueObjet",
               "Attention, en changeant d'onglet vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l'action ?<br/>",
                 350, 130, true, true, true);
        }
        else {
            OpenTabRisqueObjet();
        }
    });

    $("#tabGarantie").die().live('click', function () {
        //if ($("#PeriodeValide").val() != "1") {
        //    common.dialog.error("La plage de dates est invalide");
        //    return;
        //}
        var tabActif = $(".onglet-actif").attr("id");
        if (tabActif != "tabPeriode") {
            ShowCommonFancy("Confirm", "tabGarantie",
                "Attention, en changeant d'onglet vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l'action ?<br/>",
                350, 130, true, true, true);
        }
        else {
            OpenTabGarantie();
        }
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "tabPeriode":
                OpenTabPeriode();
                break;
            case "tabRisqueObjet":
                OpenTabRisqueObjet();
                break;
            case "tabGarantie":
                OpenTabGarantie();
                break;
            case "CancelRsq":
                OpenTabPeriode();
                break;
            case "CancelGar":
                OpenTabPeriode();
                break;
            case "Cancel":
                Cancel();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
    });

}
//--------Annule les actions de l'onglet sélectionné------------
function CancelTab() {
    var tabOpened = $("#tabOpen").val();
    var operation = "";
    switch (tabOpened) {
        case "RsqObj":
            operation = "CancelRsq";
            break;
        case "Garantie":
            operation = "CancelGar";
            break;
        default:
            operation = "Cancel";
            break;
    }
    ShowCommonFancy("Confirm", operation,
                        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                        350, 130, true, true, true);
}
//--------Change l'exercice de l'attestation--------
function ChangeExercice() {
    $("#PeriodeValide").val("0");
    $(".requiredField").removeClass("requiredField");
    $("#PeriodeDeb").val("");
    $("#PeriodeFin").val("");

    if ($("#ExerciceAttes").val() == "") {
        $("#btnSuivant").attr("disabled", "disabled");
        return;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationAttestation/ChangeExercice",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            exercice: $("#ExerciceAttes").val()
        },
        success: function (data) {
            if (data != "") {
                $("#PeriodeValide").val("1");
                $("#PeriodeDeb").val(data.split("_")[0]).removeClass("requiredField");
                $("#PeriodeFin").val(data.split("_")[1]).removeClass("requiredField");
                $("#btnSuivant").removeAttr("disabled");
            }
            CloseLoading();
        },
        error: function (request) {
            $("#ExerciceAttes").addClass("requiredField");

            common.error.showXhr(request);
        }
    });
}
//--------Change la période de l'attestation--------
function ChangePeriode() {
    $("#PeriodeValide").val("0");
    $(".requiredField").removeClass("requiredField");
    if ($("#PeriodeDeb").val() != "" && $("#PeriodeFin").val() != "") {
        $("#ExerciceAttes").val("");
        if (checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/CreationAttestation/ChangePeriode",
                data: {
                    codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                    periodeDeb: $("#PeriodeDeb").val(), periodeFin: $("#PeriodeFin").val()
                },
                success: function (data) {
                    if (data != "") {
                        $("#PeriodeValide").val("1");
                        $("#PeriodeDeb").val(data.split("_")[0]).removeClass("requiredField");
                        $("#PeriodeFin").val(data.split("_")[1]).removeClass("requiredField");
                        $("#btnSuivant").removeAttr("disabled");
                    }
                    CloseLoading();
                },
                error: function (request) {
                    $("#PeriodeDeb").addClass("requiredField");
                    $("#PeriodeFin").addClass("requiredField");

                    common.error.showXhr(request);
                }
            });
        }
    }
}
//---------Change la sélection de l'intégralité du contrat------------
function ChangeIntegralite(elem) {
    if (elem.is(":checked")) {
        $("#tabRisqueObjet").hide();
        $("#tabGarantie").hide();
    }
    else {
        $("#tabRisqueObjet").show();
        $("#tabGarantie").show();
    }
}
//---------Enumération Tab attestation------------
var AttestationTab = {
    RisqueObjet: 1,
    Garantie: 2,
    Periodee: 3
};
//---------Changement du style du contenu tab attestation lorsque on cache ou affiche les détails contrat ------------
function ChangeAttestationTabStyle(tab) {
    var img = $("#imgExpandInfo");
    var garantieTab = $(".divGarantie");
    var rsqObjTab = $(".divRsqObj");
    var isCollapsed = img.attr("src") == "/Content/Images/collapse.png" ? false : true;
    if (isCollapsed == false) {
        switch (tab) {
            case AttestationTab.RisqueObjet:
                rsqObjTab.css("max-height", rsqObjTab.data("min-height") + "px");
                break;
            case AttestationTab.Garantie:
                garantieTab.css("max-height", garantieTab.data("min-height") + "px");
                break;
        }
    }
}

//--------Ouvre l'onglet de sélection des périodes-------------
function OpenTabPeriode() {
    $("#btnSuivant").removeAttr("disabled").show();
    $("#btnValider").die().hide();
    $(".onglet-actif").removeClass("onglet-actif").addClass("onglet");
    $(".onglet-actif-orange").removeClass("onglet-actif-orange").addClass("onglet-orange");
    $("#tabPeriode").addClass("onglet-actif");
    $("#dvAttesRsqObj").html("").hide();
    $("#dvAttesGar").html("").hide();
    $("#tabOpen").val("Periode");
    $("#dvAttesPeriode").show();
}
//--------Ouvre l'onglet de sélection des risques/objets----------
function OpenTabRisqueObjet() {
    $(".requiredField").removeClass("requiredField");
    var erreur = CheckPeriode();

    if (!erreur) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationAttestation/OpenTabRsqObj",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumInterne").val(),
                lotId: $("#attestId").val(), exercice: $("#ExerciceAttes").val(), periodeDeb: $("#PeriodeDeb").val(), periodeFin: $("#PeriodeFin").val(),
                typeAttes: $("#TypeAttes").val(), integralite: $("#IntegraliteContrat").is(":checked")
            },
            success: function (data) {
                if (data != "") {
                    $(".onglet-actif").removeClass("onglet-actif").addClass("onglet");
                    $(".onglet-actif-orange").removeClass("onglet-actif-orange").addClass("onglet-orange");
                    if ($("#tabRisqueObjet").hasClass("onglet-orange")) {
                        $("#tabRisqueObjet").addClass("onglet-actif-orange");
                    }
                    else {
                        $("#tabRisqueObjet").addClass("onglet-actif");
                    }
                    $("div[id^='dvAttes']").hide();
                    $("#dvAttesRsqObj").html(data).show();
                    $("#attestId").val($("#rsqobjAttestId").val());
                    $("#tabOpen").val("RsqObj");
                    $("#ExerciceAttes").attr("readonly", "readonly").addClass("readonly");
                    $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
                    $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");
                    MapElementRsqObj();
                    ChangeAttestationTabStyle(AttestationTab.RisqueObjet);
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        $("#PeriodeDeb").addClass("requiredField");
        $("#PeriodeFin").addClass("requiredField");
        common.dialog.error("Veuillez renseigner les champs de périodes");
    }
}
//---------Ouvre l'onglet de sélection des garanties-----------
function OpenTabGarantie() {
    $(".requiredField").removeClass("requiredField");
    var erreur = CheckPeriode();

    if (!erreur) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationAttestation/OpenTabGarantie",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumInterne").val(),
                lotId: $("#attestId").val(), exercice: $("#ExerciceAttes").val(), periodeDeb: $("#PeriodeDeb").val(), periodeFin: $("#PeriodeFin").val(),
                typeAttes: $("#TypeAttes").val(), integralite: $("#IntegraliteContrat").is(":checked")
            },
            success: function (data) {
                if (data != "") {
                    $(".onglet-actif").removeClass("onglet-actif").addClass("onglet");
                    $(".onglet-actif-orange").removeClass("onglet-actif-orange").addClass("onglet-orange");
                    if ($("#tabGarantie").hasClass("onglet-orange")) {
                        $("#tabGarantie").addClass("onglet-actif-orange");
                    }
                    else {
                        $("#tabGarantie").addClass("onglet-actif");
                    }
                    $("div[id^='dvAttes']").hide();
                    $("#dvAttesGar").html(data).show();
                    $("#attestId").val($("#garAttestId").val());
                    $("#tabOpen").val("Garantie");
                    $("#ExerciceAttes").attr("readonly", "readonly").addClass("readonly");
                    $("#PeriodeDeb").attr("readonly", "readonly").addClass("readonly").datepicker("disable");;
                    $("#PeriodeFin").attr("readonly", "readonly").addClass("readonly").datepicker("disable");;
                    MapElementGarantie();
                    ChangeAttestationTabStyle(AttestationTab.Garantie);
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        $("#PeriodeDeb").addClass("requiredField");
        $("#PeriodeFin").addClass("requiredField");
        common.dialog.error("Veuillez renseigner les champs de périodes");
    }
}
//---------Map les éléments de l'onglet "Sélection Rsq/Obj"--------------
function MapElementRsqObj() {
    $("#btnSuivant").attr("disabled", "disabled").hide();
    $("#btnValider").show();

    $("#btnValider").die().live('click', function () {
        ValidSelectionRsqObj();
    });

    $("#checkAllRsqObj").die().live('change', function () {
        if ($(this).is(":checked")) {
            $("input[type='checkbox'][name='checkRsqObj']").attr("checked", "checked");
            $("img[name='ExpandRsq']").attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
            $("tr[id^='trObj']").show();
        }
        else {
            $("input[type='checkbox'][name='checkRsqObj']").removeAttr("checked");
            $("img[name='ExpandRsq']").attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
            $("tr[id^='trObj']").hide();
        }
    });

    $("input[type='checkbox'][id^='checkRsq_']").each(function () {
        $(this).change(function () {
            var codeRsq = $(this).attr('id').split('_')[1];
            if ($(this).is(":checked")) {
                $("input[type='checkbox'][id^='checkObj_" + codeRsq + "']").attr("checked", "checked");
                $("img[id='ExpandRsq_" + codeRsq + "']").attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
                $("tr[id^='trObj_" + codeRsq + "']").show();
            }
            else {
                $("input[type='checkbox'][id^='checkObj_" + codeRsq + "']").removeAttr("checked");
                $("img[id='ExpandRsq_" + codeRsq + "']").attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
                $("tr[id^='trObj_" + codeRsq + "']").hide();
                $("#checkAllRsqObj").removeAttr("checked");
            }
        });
    });

    $("input[type='checkbox'][id^='checkObj_']").each(function () {
        $(this).change(function () {
            var codeRsq = $(this).attr('id').split('_')[1];
            var countObjCheck = $("input[type='checkbox'][id^='checkObj_" + codeRsq + "_']:checked").length;
            if (countObjCheck == 0) {
                $("input[type='checkbox'][id='checkRsq_" + codeRsq + "']").removeAttr("checked");
                $("input[type='checkbox'][id='checkRsq_" + codeRsq + "']").trigger("change");
            }
            if (!$(this).is(":checked")) {
                $("#checkAllRsqObj").removeAttr("checked");
            }
        });
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

    var countUncheck = $("input[type='checkbox'][id^='checkRsq_']:not(:checked)").length + $("input[type='checkbox'][id^='checkObj_']:not(:checked)").length;
    if (countUncheck == 0) {
        $("#checkAllRsqObj").attr("checked", "checked");
    }

    common.autonumeric.applyAll('init', 'numeric');
}
//----------Valide la sélection des risques et objets---------
function ValidSelectionRsqObj() {
    var countSelRsqObj = $("input[type='checkbox'][name='checkRsqObj']:checked").length;
    if (countSelRsqObj == 0) {
        common.dialog.error("Veuillez sélectionner au moins un risque et un objet");
        return;
    }
    else {
        var selcodeobj = "";
        var selcodersq = "";
        var selRsqObj = "";
        $("input[type='checkbox'][id^='checkRsq']:checked").each(function () {

            var codeRsq = $(this).attr("id").split("_")[1];
            selcodersq = codeRsq;

            $("input[type='checkbox'][id^='checkObj_" + codeRsq + "_']:checked").each(function () {

                var codeObj = $(this).attr("id").split("_")[2];
                selcodeobj += "$" + codeObj;
            });

            if (selcodeobj != "") {
                selcodeobj = selcodeobj.substring(1) + "$";
            }
            selRsqObj += ";" + selcodersq + "_" + selcodeobj;

            selcodeobj = "";
        });

        if (selRsqObj != "") {
            selRsqObj = selRsqObj.substring(1) + ";";

        }
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationAttestation/ValidSelectionRsqObj",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            lotId: $("#attestId").val(), selRsqObj: selRsqObj
        },
        success: function (data) {
            if (data == "") {
                $("#tabRisqueObjet").removeClass("onglet-actif").addClass("onglet-actif-orange");
                $("#changeRsqObj").val("1");
                OpenTabPeriode();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les éléments de l'onglet "Sélection des Garanties"------------
function MapElementGarantie() {
    $("#btnSuivant").attr("disabled", "disabled").hide();
    $("#btnValider").show();

    $("#btnValider").die().live('click', function () {
        ValidSelectionGarantie();
    });

    $("#FiltreFormule").die().live('change', function () {
        var filtreFor = $(this).val();
        if (filtreFor == "") {
            $("#tblBodyGarantie tr[albcodefor]").show();
        }
        else {
            $("#tblBodyGarantie tr[albcodefor]").hide();
            $("tr[albcodefor='" + filtreFor + "']").show();
        }
    });

    $("#FiltreRisque").die().live('change', function () {
        var filtreRsq = $(this).val();
        if (filtreRsq == "") {
            $("#tblBodyGarantie tr[albcodersq]").show();
        }
        else {
            $("#tblBodyGarantie tr[albcodersq]").hide();
            $("tr[albcodersq='" + filtreRsq + "']").show();
        }
    });

    $("#checkAllGarantie").die().live('change', function () {
        if ($(this).is(":checked")) {
            $("input[type='checkbox'][name='checkGar']").attr("checked", "checked");
            $("img[name='ExpandGar']").attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
            $("tr[id^='trGar']").show();
        }
        else {
            $("input[type='checkbox'][name='checkGar']").removeAttr("checked");
            $("img[name='ExpandGar']").attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
            $("tr[id^='trGar']").hide();
        }
    });

    $("input[type='checkbox'][id^='checkGar_']").each(function () {
        $(this).change(function () {
            ChangeStatutCheckGar($(this));
        });
    });

    $("img[name='ExpandGar']").each(function () {
        $(this).click(function () {
            var codeGar = $(this).attr('id').replace('ExpandGar', '');
            var imgExpCol = $(this).attr("albexpcol");
            switch (imgExpCol) {
                case "expand":
                    $(this).attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
                    $("tr[id^='trGar" + codeGar + "_']").show();
                    break;
                case "collapse":
                    $(this).attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
                    $("tr[id^='trGar" + codeGar + "_']").hide();
                    break;
            }
        });
    });

    $("#btnSearchGar").die().live('click', function () {
        SearchGarantie();
    });

    $("#btnReinit").die().live('click', function () {
        $("#FiltreFormule").val("");
        $("#FiltreRisque").val("");
        $("#inSearchGarantie").val("");
        $("#btnSearchGar").trigger("click");
    });

    var countUncheck = $("input[type='checkbox'][id^='checkGar_']:not(:checked)").length;
    if (countUncheck == 0) {
        $("#checkAllGarantie").attr("checked", "checked");
    }

    //common.autonumeric.applyAll('init', 'decimal');
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');

}
//----------Coche/Décoche les garanties---------
function ChangeStatutCheckGar(elem) {
    ShowLoading();

    var codeGar = elem.attr('id').replace('checkGar', '');
    var idGar = elem.attr('albidgar');
    var isChecked = elem.is(":checked");
    var charFor = elem.attr('albcharfor');

    $.ajax({
        type: "POST",
        url: "/CreationAttestation/ChangeStatutCheckGar",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            lotId: $("#attestId").val(), codeGar: idGar, isChecked: isChecked
        },
        success: function (data) {
            if (isChecked) {
                $("input[type='checkbox'][id^='checkGar" + codeGar + "_'][albcharfor^='" + charFor + "']").attr("checked", "checked");
                $("img[id^='ExpandGar" + codeGar + "']").attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
                $("tr[id^='trGar" + codeGar + "_']").show();
            }
            else {
                $("input[type='checkbox'][id^='checkGar" + codeGar + "_'][albcharfor^='" + charFor + "']").removeAttr("checked");
                $("img[id^='ExpandGar" + codeGar + "']").attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
                $("tr[id^='trGar" + codeGar + "_']").hide();
                $("#checkAllGarantie").removeAttr("checked");
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//----------Valide la sélection des garanties------------
function ValidSelectionGarantie() {
    var countSelGar = $("input[type='checkbox'][name='checkGar']:checked").length;
    if (countSelGar == 0) {
        common.dialog.error("Veuillez sélectionner au moins une garantie");
        return;
    }
    else {
        var selGarantie = "";
        $("input[type='checkbox'][id^='checkGar']:checked").each(function () {
            var codeGar = $(this).attr("albIdGar");
            selGarantie += ";" + codeGar;
        });
        if (selGarantie != "") {
            selGarantie = selGarantie.substring(1) + ";";
        }
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationAttestation/ValidSelectionGar",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            lotId: $("#attestId").val(), selGarantie: selGarantie
        },
        success: function (data) {
            if (data == "") {
                $("#tabGarantie").removeClass("onglet-actif").addClass("onglet-actif-orange");
                $("#changeGar").val("1");
                OpenTabPeriode();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Recherche une garantie précise dans la période-----------
function SearchGarantie() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationAttestation/SearchGarantie",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeAvn: $("#NumInterne").val(), lotId: $("#attestId").val(), searchGarantie: $("#inSearchGarantie").val()
        },
        success: function (data) {
            if (data != "") {
                $("#dvBodyGarantie").html(data).show();
                $("tr[albisshown='0']").hide();
                MapElementGarantie();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Vérifie les informations de l'onglet "Période"-------
function CheckPeriode() {
    if ($("#PeriodeDeb").val() != "" && $("#PeriodeFin").val() != "") {
        if (!checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
            return true;
        }
    }
    else {
        return true;
    }
    return false;
}
//--------Valide les périodes et passe à l'écran suivant----------
function Suivant(returnHome) {
    if ($("#PeriodeValide").val() != "1") {
        common.dialog.error("La plage de dates est invalide");
        return;
    }

    $(".requiredField").removeClass("requiredField");
    var erreur = CheckPeriode();

    if (erreur) {
        $("#PeriodeDeb").addClass("requiredField");
        $("#PeriodeFin").addClass("requiredField");
        common.dialog.error("Veuillez renseigner les champs de périodes");
    }

    if ($("#TypeAttes").val() == "") {
        $("#TypeAttes").addClass("requiredField");
        erreur = true;
    }

    if (!erreur) {
        ShowLoading();

        var codeAvenant = $("#NumInterne").val();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var codeAvenantExterne = $("#NumAvt").val();

        $.ajax({
            type: "POST",
            url: "/CreationAttestation/Suivant",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeAvenant: codeAvenant, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue,
                codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(),
                lotId: $("#attestId").val(), exercice: $("#ExerciceAttes").val(), periodeDeb: $("#PeriodeDeb").val(), periodeFin: $("#PeriodeFin").val(),
                typeAttes: $("#TypeAttes").val(), integralite: $("#IntegraliteContrat").is(":checked")
            },
            success: function (data) {
                if (data.split(";").length > 0) {
                    var tErrorMsg = data.split(";");
                    if (tErrorMsg[0] == "ERRORMSG") {
                        common.dialog.error(tErrorMsg[1]);
                        CloseLoading();
                    }
                    else {
                        $("#attestId").val(data);
                        Redirection(returnHome ? "RechercheSaisie" : "ChoixClauses", "Index");
                    }
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        common.dialog.error("Veuillez corriger les champs erronés");
    }
}
//---------Annule et retourne sur l'écran de recherche-----------
function Cancel() {
    var tabGuid = $("#tabGuid").val();
    DeverouillerUserOffres(tabGuid);
    Redirection("RechercheSaisie", "Index");
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeContrat = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvenant = $("#NumInterne").val();
    var tabGuid = $("#tabGuid").val();
    var lotId = $("#attestId").val();
    var addParamType = "AVN";
    var addParamValue = $("#AddParamValue").val();
    addParamValue += "||ATTESTID|" + lotId + "||AVNTYPE|ATTES";
    var codeAvenantExterne = $("#NumAvt").val();

    $.ajax({
        type: "POST",
        url: "/CreationAttestation/Redirection/",
        data: {
            cible: cible, job: job, codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
            addParamType: addParamType, addParamValue: addParamValue, codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(),
            lotId: lotId
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
