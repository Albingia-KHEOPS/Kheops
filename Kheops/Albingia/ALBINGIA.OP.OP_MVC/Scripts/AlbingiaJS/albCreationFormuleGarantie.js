$(document).ready(function () {
    MapPageElement();
    Suivant();
    ChangeHour();
});
//---------------------Affecte les fonctions sur les controles heures-----------------------
function ChangeHour() {
    $("#HeureDebHours").live('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffecteHour($(this));
    });
    $("#HeureDebMinutes").live('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffecteHour($(this));
    });
    $("#HeureFinHours").live('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffecteHour($(this));
    });
    $("#HeureFinMinutes").live('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffecteHour($(this));
    });
}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "");

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }
}
//----------------Redirection------------------
function RedirectionFormuleGarantie(cible, job, readonlyDisplay, sessionLost) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var libelle = $("#Libelle").val();
    var lettreLib = $("#LettreLib").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var codeRisque = $("#ObjetRisqueCode").val();
    var codeObjet = "";
    var branche = $("#HiddenBranche").val();
    var codeObjetRisque = $("#ObjetRisqueCode").val();

    if (codeObjetRisque != undefined) {
        codeObjet = codeObjetRisque.split(";")[1];
    }

    if (codeRisque != undefined) {
        codeRisque = codeRisque.split(";")[0];
    }

    if (sessionLost == undefined)
        sessionLost = false;

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeRisque: codeRisque, codeObjet: codeObjet, branche: branche,
            tabGuid: tabGuid, modeNavig: modeNavig,
            libelle: libelle, lettreLib: lettreLib, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(),
            addParamType: addParamType, addParamValue: addParamValue, readonlyDisplay: readonlyDisplay,
            isModeConsultationEcran: $("#IsModeConsultationEcran").val(), isForceReadOnly: $("#IsForceReadOnly").val(), sessionLost: sessionLost, isModeConsult: window.isReadonly
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------- affecte les fonctions au demarrage----------
function MapPageElement() {

    $("#btnDialogOk").die().live('click', function () {
        $("#divDialogInFancy").hide();
    });
    $("#btnInfoOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "ErrorCache":
                DeleteInventaireGarantieCache();
                break;
            default:
                break;
        }
        $("#hiddenAction").val("");
    });

    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Annuler();
                break;
            case "Applique":
                DeleteFormuleGarantie();
                $("#hiddenAction").val('');
                break;
            case "Portee":
                SavePortee();
                $("#hiddenAction").val('');
                break;
            case "Inventaire":
                SupprimerInventaire();
                $("#hiddenAction").val('');
                break;
        }

    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    if ($.trim($("#dvListObjRsq").html()) == "") {
        ShowCommonFancy("Error", false, "Aucun risque disponible pour l'avenant", 300, 80, true, true, true);
        $("#btnMultiObj").attr("disabled", "disabled");
        $("#btnSuivant").attr("disabled", "disabled");
        $("#btnFSSuivant").attr("disabled", "disabled");
        return;
    }

    if ($("#chkModificationAVN").attr("id") != undefined) {
        if (($("#IsAvenantModificationLocale").val() == "True" && $("#IsTraceAvnExist").val() == "True") || window.isReadonly || $("#IsFormuleSortie").val() == 'True') {
            $("#chkModificationAVN").attr("disabled", "disabled");
            $("#chkModificationAVN").die();
        }
        else {
            $("#chkModificationAVN").removeAttr("disabled");
            $("#chkModificationAVN").die().live("change", function () {
                RedirectionFormuleGarantie("CreationFormuleGarantie", "Index", !$("#chkModificationAVN").is(':checked'));
            });
        }
    }

    $("#btnMultiObj").die().live('click', function () {
        if (!$(this).attr("disabled")) {
            AlbScrollTop();
            DesactivateShortCut("mainFormuleGarantie");
            ReactivateShortCut();//active les raccourcis de la popup
            ShowFancy();
            checkListRsqObj();
        }
    });

    //Sélectionne les garanties dès l'affichage de la page
    if ($("#ObjetRisqueCode").val() != "") {
        checkListRsqObj();
        $("#CodeCibleRsq").val($("input[name='radioRsq']:checked").attr("branche"));
    }
    else if ($("#FormGen").val() == "1") {
    }
    else {
        $("#btnSuivant").attr("disabled", "disabled");
        $("#btnFSSuivant").attr("disabled", "disabled");
        if ($("#chkModificationAVN").attr("id") == undefined || $("#chkModificationAVN").is(":checked"))
            $("#btnMultiObj").trigger("click");
    }

    AlternanceLigne("RsqObj", "ObjetRisqueCode", true, null);

    $("#btnListValid").die().live('click', function () {
        if (!$(this).attr("disabled")) {
            var newCible = $("input[name=radioRsq]:checked").attr('branche');
            if (typeof (newCible) != "undefined" && newCible != "") {
                if ($("#CodeCibleRsq").val() != "" && $("#CodeCibleRsq").val() != newCible) {
                    ShowCommonFancy("Confirm", "Applique", "Attention, vous changez de risque, toutes les garanties sélectionnées pour ce risque seront supprimées.<br/>Voulez-vous continuer ?", 400, 150, true, true);
                }
                else {
                    validListRsqObj();
                }
            }
            else {
                ShowCommonFancy("Error", "", "Vous devez sélectionner un risque.", 1212, 700, true, true);
            }
        }
    });

    $("#btnListCancel").die().live('click', function () {
        $("#divLstRsqObj").hide();
        DesactivateShortCut();
        ReactivateShortCut("mainFormuleGarantie");
        if ($("#ObjetRisque").val() == "") {
            Annuler();
        }
    });

    $("input[name=checkObj]").die().live('click', function () {
        if ($(this).is(':checked')) {
            selectListRsqObj($(this), true);
        }
        else {
            CheckObjChecked();
        }
    });

    $("input[name=radioRsq]").die().live('click', function () {
        selectListRsqObj($(this), false);
    });

    $("input[type=checkbox]").die().live('click', function () {
        CheckVadider();
    });

    $("#FullScreenFormule[albContext='Formule']").die().live('click', function () {
        OpenFullScreenFormule();
    });

    $("#btnFSAnnuler").die().live('click', function () {
        $("#btnAnnuler").trigger('click');
    });
    $("#btnFSSuivant").die().live('click', function () {
        $("#btnSuivant").trigger('click');
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("accesskey", "p");
        $("#btnSuivant").html("<u>S</u>uivant").attr("accesskey", "s");
    }

    $("img[name=expandVolet]").die().live('click', function () {
        ExpandCollapseVoletBloc($(this));
    });
    $("img[name=expandBloc]").die().live('click', function () {
        ExpandCollapseVoletBloc($(this));
    });


    if ($("#divListGaranties").is(":visible")) {
        if ((!$("#divModeAvenant").is(":visible") && $("#divModeAvenant").html() != undefined) || $("#chkModificationAVN").is(":checked"))
            PrepareFormuleGarantie();
        else {
            if ($.trim($("#divListGaranties").html()) != "") {
                PrepareFormuleGarantie();
            }
        }
    }
    else {
        if ($.trim($("#divListGaranties").html()) != "") {
            PrepareFormuleGarantie();
        }
    }

    if ($("#modeDuplication").val() == "True") {
        $("#ObjetRisque").val("");
        $("#btnMultiObj").trigger("click");
        $("input[type='radio'][name='radioRsq']").removeAttr("checked");
        $("input[type='checkbox'][name='checkObj']").removeAttr("checked");
    }
}
//-----------Expand/Collapse Volet/Bloc------------
function ExpandCollapseVoletBloc(elem) {
    var tAttrElem = AlbJsSplitArray(elem.attr('id'), "_");
    var typeElem = tAttrElem[0];
    var codeElem = tAttrElem[1];
    var openElem = false;
    if (elem.attr("src") == "/Content/Images/Op.png") {
        elem.attr("src", "/Content/Images/Cl.png");
        openElem = false;
    }
    else {
        if (elem.attr("src") == "/Content/Images/Cl.png") {
            elem.attr("src", "/Content/Images/Op.png");
            openElem = true;
        }
    }
    var inputElem = $("input[type='checkbox'][id='" + elem.attr("id") + "']")
    if (typeElem == "volet") {
        openChild(inputElem, 1, openElem);
    }
    else {
        if (typeElem == "bloc") {
            openChild(inputElem, 2, openElem);
        }
    }
}
//-------------Ouverture du plein écran--------------------
function OpenFullScreenFormule() {
    RetrieveFormulesGaranties($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeFormule").val(), $("#CodeOption").val(), true);
}
//-------------Fermeture du plein écran---------------------
function CloseFullScreen() {
    RetrieveFormulesGaranties($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeFormule").val(), $("#CodeOption").val(), false);
}
//--------------- Supprime les garanties ----------------
function DeleteFormuleGarantie() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/DeleteFormuleGarantie",
        data: { codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            validListRsqObj();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------- Verifie CheckBox pour btn Valider -------------------------------
function CheckVadider() {
    $("#btnSuivant").attr("disabled", "disabled");
    $("#btnFSSuivant").attr("disabled", "disabled");
    $("input[type=checkbox][name^=volet]:checked").each(function () {
        var idVolet = $(this).attr("id");
        $("tr[name=" + idVolet + "] input[type=checkbox][name^=bloc]:checked").each(function () {
            var idBloc = $(this).attr("id");
            $("tr[name=" + idBloc + "] input[type=checkbox][name^=garantieNiv1]:checked").each(function () {
                $("#btnSuivant").removeAttr("disabled");
                $("#btnFSSuivant").removeAttr("disabled");
                //checkGarantie = true;
            });
        });
    });
}
//--------------- Coche les risques et objets lors de l'ouverture ----------------
function checkListRsqObj() {

    $("input[name=checkObj]").removeAttr('checked');
    var elem = $("#ObjetRisqueCode").val().split(';')
    if (elem.length > 1) {
        var currentRsq = elem[0];

        $("input[name=radioRsq][id=" + currentRsq + "]").attr('checked', true);
        if ($("#OffreReadOnly").val() == 'False') $("input[name=radioRsq][id=" + currentRsq + "]").removeAttr("disabled");
        $("td[albcontext='rsq_" + currentRsq + "']").removeClass("NotAffecte");

        var tabObjet = elem[1].split('_');
        for (i = 0; i < tabObjet.length; i++) {
            $("input[id=check_" + currentRsq + "_" + tabObjet[i] + "]").attr('checked', true);
            if ($("#OffreReadOnly").val() == 'False') $("input[id=check_" + currentRsq + "_" + tabObjet[i] + "]").removeAttr("disabled");
            $("td[albcontext='obj_" + currentRsq + "_" + tabObjet[i] + "']").removeClass("NotAffecte");
        }
    }

}
//--------------- Selectionne un risque ou objet dans la liste---------------
function selectListRsqObj(e, checkbox) {
    $("#btnListValid").removeAttr("disabled");
    if (checkbox) {
        $("input[name=checkObj]:not([id^=check_" + e.val().split('_')[0] + "])").removeAttr('checked');
        $("input[name=radioRsq][id=" + e.val().split('_')[0] + "]").attr('checked', true);
    }
    else {
        $("input[name=checkObj]").removeAttr('checked');
        $("input[name=checkObj][id^=check_" + e.val().split('_')[0] + "]:not(:disabled)").attr('checked', true);
    }
}
//-------------Vérifie qu'au moins un objet est sélectionné----------
function CheckObjChecked() {
    var countObjChecked = $("input[type='checkbox'][name='checkObj']:checked").length;
    if (countObjChecked == 0) {
        $("#btnListValid").attr("disabled", "disabled");
    }
}
//--------------- Sauvegarde la formule ------------------
function SaveFormule() {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var branche = $("#HiddenBranche").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var codeAlpha = $("#LettreLib").val();
    var codeCible = $("#CodeCibleRsq").val();
    var libFormule = $("#Libelle").val();
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    var codeAvn = $("#NumAvenantPage").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/SaveFormuleGarantie",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption, formGen: $("#FormGen").val(),
            codeAlpha: codeAlpha, branche: branche, codeCible: codeCible, libFormule: libFormule, codeObjetRisque: codeObjetRisque, tabGuid: $("#tabGuid").val(), modeNavig: $("#ModeNavig").val(), codeAvn: codeAvn
        },
        success: function (data) {
            if (!window.isReadonly) {
                $("#CodeFormule").val(data.split('_')[0]);
                $("#LettreLib").val(data.split('_')[1]);
                if ($("#CodeOption").val() == "0")
                    $("#CodeOption").val(1);
                //Récupération de la date de modif du rsq pour la création en avenant
                var dateModif = data.split('_')[2];
                if (dateModif != "") {
                    $("#DateEffetAvenantModificationLocale").val(dateModif);
                }
            }
            GetLibCible();
            LoadFormulesGaranties(codeOffre, version, type, $("#CodeFormule").val(), $("#CodeOption").val(), 1);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------- Valide la sélection des objets et risques dans la liste ---------
function validListRsqObj() {
    var rsqSelected = $("input[name=radioRsq]:checked");

    var currentRsq = rsqSelected.attr('id');
    var cibleRsq = rsqSelected.attr('branche');

    var strList = "";
    var strListObj = "_";
    strList += currentRsq;

    $("input[id^=check_" + currentRsq + "_][name=checkObj]").each(function () {
        if ($(this).is(":checked")) {
            if (strListObj.indexOf("_" + $(this).val().split('_')[1] + "_") < 0)
                strListObj += $(this).val().split('_')[1] + "_";
        }
        else {
            strListObj = strListObj.replace("_" + $(this).val().split('_')[1] + "_", "_");
        }
    });
    strListObj = strListObj.substring(1).substring(0, strListObj.length - 2);
    strList += ";" + strListObj;

    $("#ObjetRisqueCode").val(strList);
    DisplayLibObjetRisque($("input[id^=check_" + currentRsq + "_][name=checkObj]").length, strListObj.split('_').length);
    $("#divLstRsqObj").hide();

    if ($("#ObjetRisqueCode").val() != "") {
        $("#CodeCibleRsq").val(cibleRsq);
        if ($("#CodeFormule").val() != "0")
            SaveAppliqueA();
        else
            SaveFormule();
    }
    DesactivateShortCut();
    ReactivateShortCut("mainFormuleGarantie");
}
//--------------- Sauvegarde le s'applique à pour la formule --------------------
function SaveAppliqueA() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var objetRisqueCode = $("#ObjetRisqueCode").val();
    var cible = $("#CodeCibleRsq").val();
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/SaveAppliqueA",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption,
            cible: cible, formGen: $("#FormGen").val(), objetRisqueCode: objetRisqueCode, tabGuid: tabGuid, modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            GetLibCible();
            LoadFormulesGaranties(codeOffre, version, type, codeFormule, codeOption, 1);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------- Affiche le libellé pour le choix des risques/objets appliqué -----------
function DisplayLibObjetRisque(countObj, countSelObj) {
    var nbRsq = $("input[type=radio][name=radioRsq]").length;
    var codeRsqSelected = $("input[name=radioRsq]:checked").attr('id');
    var libRsqSelected = $("tr[id=" + codeRsqSelected + "_0]").attr('name');

    if (nbRsq == 1) {   //Mono Risque
        if (countObj == countSelObj) {  //Tous les objets sélectionnés
            $("#ObjetRisque").val("à l'ensemble du risque");
        }
        else {                          //1 à N objet(s) sélectionné(s)
            $("#ObjetRisque").val("à l'objet(s) du risque");
            //$("#ObjetRisque").val("à " + countSelObj + " objet(s) du risque");
        }
    }
    else {              //Multi Risques
        if (countObj == countSelObj) {  //Tous les objets sélectionnés
            $("#ObjetRisque").val("au risque " + codeRsqSelected + " '" + libRsqSelected + "'");
        }
        else {                          //1 à N objet(s) sélectionné(s)
            $("#ObjetRisque").val("à l'objet(s) du risque" + codeRsqSelected + " '" + libRsqSelected + "'");
            //$("#ObjetRisque").val("à " + countSelObj + " objet(s) du risque" + codeRsqSelected + " '" + libRsqSelected + "'");
        }
    }
}
//------------Charge les infos des formules de garanties------------
function LoadFormulesGaranties(codeOffre, version, type, codeFormule, codeOption, appliqueA) {
    var codeAvn = $("#NumAvenantPage").val();
    var codeCible = $("#CodeCibleRsq").val();
    var codeAlpha = $("#LettreLib").val();
    var branche = $("#HiddenBranche").val();
    var libFormule = $("#Libelle").val();
    var offreReadOnly = $("#OffreReadOnly").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/LoadFormulesGaranties",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption, formGen: $("#FormGen").val(),
            codeAlpha: codeAlpha, branche: branche, codeCible: codeCible, libFormule: libFormule, appliqueA: appliqueA, offreReadOnly: offreReadOnly,
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            if (data == "")
                ShowCommonFancy("Warning", "", "Aucune garantie pour cette cible.", 350, 140, true, true);
            $("#divListGaranties").html(data);
            $("#divListGaranties").show();
            PrepareFormuleGarantie();
            CloseLoading();
        },
        error: function (request) {
            $("#btnSuivant").attr("disabled", "disabled");
            $("#btnFSSuivant").attr("disabled", "disabled");
            common.error.showXhr(request);
        }
    });
}
//------------Met en forme le tableau des formules de garanties-------------
function PrepareFormuleGarantie() {
    AlternanceLigne("ListGaranties", "", false, null);
    ColorVoletBloc();
    AffectClickTree();
    AffectClickIco();

    if ($.trim($("#tblListGaranties").html()) == "") {
        $("#btnSuivant").attr("disabled", "disabled");
        $("#btnFSSuivant").attr("disabled", "disabled");
    }
    else {
        $("#btnSuivant").removeAttr("disabled");
        $("#btnFSSuivant").removeAttr("disabled");
        CheckVadider();
    }
    //if (!window.isReadonly) {
    ChangeParamNatMod();
    CheckObjetRisque();
    //}

    $("input[type='checkbox'][albInfo]").each(function () {
        $(this);
    });
}
//------------Met à jour le paramNatMod lors du changement de paramètrage------
function ChangeParamNatMod() {
    $("select[name=ParamNatMod]").each(function () {
        $(this).change(function () {
            var checkGar = $("input[type=checkbox][id=" + $(this).attr("id").replace("paramNatMod", "garantie") + "]").is(":checked");
            if ($(this).val() == "E" && checkGar) {
                ShowCommonFancy("Error", "", "Impossible d'exclure une garantie sélectionnée.", 300, 80, true, true);
                $(this).val($("input[id=" + $(this).attr("id").replace("paramNatMod", "oldparamNatMod") + "]").val());
                return false;
            }
            if ($(this).val() == "C" && !checkGar) {
                ShowCommonFancy("Error", "", "Impossible de comprendre une garantie non sélectionnée.", 300, 80, true, true);
                $(this).val($("input[id=" + $(this).attr("id").replace("paramNatMod", "oldparamNatMod") + "]").val());
                return false;
            }

            $("input[id=" + $(this).attr("id").replace("paramNatMod", "oldparamNatMod") + "]").val($(this).val());

            updateGarantie($("input[type=checkbox][id=" + $(this).attr("id").replace("paramNatMod", "garantie") + "]"));
        });
    });
}
//--------------Récupère les infos des tables de travail----------
function RetrieveFormulesGaranties(codeOffre, version, type, codeFormule, codeOption, fullScreen) {
    var codeAvn = $("#NumAvenantPage").val();
    var codeCible = $("#CodeCibleRsq").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/RetrieveFormulesGaranties",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption, formGen: $("#FormGen").val(),
            codeCible: codeCible, fullScreen: fullScreen, modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val()
        },
        success: function (data) {
            if (fullScreen) {
                $("#divDataFullScreenFormule").html(data);
                $("#divDataFullScreenFormule").show();
                AlbScrollTop();
                $("#divFullScreenFormule").show();
                $("#divListGaranties").html("");
                $("#dvLinkClose[albContext='Formule']").die().live('click', function () {
                    CloseFullScreen();
                });
                if ($("#btnSuivant").is(':disabled')) {
                    $("#btnFSSuivant").attr('disabled', 'disabled');
                }
                else {
                    $("#btnFSSuivant").removeAttr('disabled');
                }

                if (window.isReadonly) {
                    $("#btnFSAnnuler").html("<u>P</u>récédent").attr("accesskey", "p");
                    $("#btnFSSuivant").html("<u>S</u>uivant").attr("accesskey", "s");
                }

            }
            else {
                $("#divListGaranties").html(data);
                $("#divListGaranties").show();
                $("#divFullScreenFormule").hide();
                $("#divDataFullScreenFormule").html("");
            }

            if ($.trim($("#tblListGaranties").html()) == "") {
                ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                return false;
            }

            AlternanceLigne("ListGaranties", "", false, null);
            ColorVoletBloc();
            AffectClickTree();
            AffectClickIco();
            ChangeParamNatMod();
            CloseLoading();
            CheckObjetRisque();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Colorie les lignes de Volets et Blocs
function ColorVoletBloc() {
    $("tr[name=Volet]").addClass("bgVolet");
    $("tr[name^=volet_]").addClass("bgBloc");
}
//-----------------Charge les détails de la garantie
function ShowDetailsGarantie(idGaran) {
    var codeGaran = $("#currentGuidGarantie").val();
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/LoadDetailsGarantie",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeGarantie: codeGaran, codeObjetRisque: codeObjetRisque, modeNavig: $("#ModeNavig").val(),
            codeAvn: $("#NumAvenantPage").val(), isReadonly: $("#OffreReadOnly").val(),
            dateEffAvnModifLocale: $('#DateEffetAvenantModificationLocale').val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsGarantie").html(data);
            $("#divDetailsGarantie").show();
            InitControlFancy('Details');
            var garInfo = $("img[id=" + idGaran + "]").attr("albGarInfo");
            var bddparamNat = $("input[id=" + garInfo.replace("garInfo1", "bddparamNatMod1") + "]").val();
            var paramNat = $("select[id=" + garInfo.replace("garInfo1", "paramNatMod1") + "]").val();
            if (bddparamNat != paramNat) {
                if (paramNat == "")
                    paramNat = "A";
                $("#Nature").val(paramNat);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Charge les infos de la portée de la garantie------------
function ShowPorteeGarantie(idGaran) {
    var cible = $("#CibleRisque").val().split(" - ")[0];
    var alimAssiette = $("img[name='icoPorteeGar'][id='" + idGaran + "']").attr("albalimassiette");
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/LoadPorteeGarantie",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
            codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeGarantie: idGaran,
            modeNavig: $("#ModeNavig").val(), alimAssiette: alimAssiette, codeBranche: $("#Offre_Branche").val(), codeCible: cible,
            modifAvn: $("#DateEffetAvenantModificationLocale").val() != undefined ? $("#DateEffetAvenantModificationLocale").val() : "", isReadonly: $("#OffreReadOnly").val(),
            codeObjetRisque: codeObjetRisque
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataPorteGarantie").html(data);
            $("#divPorteGarantie").show();
            InitControlFancy('Portee');
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Initialise les contrôles du détails de la formule garantie
function InitControlFancy(win) {
    switch (win) {
        case 'Details':
            MapFancyElementDetail();
            if ($("#isReadOnly").val() != "True")
                ChangeEtatDateDuree();
            break;
        case 'Inventaire':
            MapFancyElementInventaire();
            break;
        case 'Portee':
            MapFancyElementPortee();
            break;
    }
}
//-------Map les actions sur les contrôles de la fancy Portee---------
function MapFancyElementPortee() {

    $("input[name='chkObjPortee']").each(function () {
        $(this).change(function () {
            RecalculMntTotal();
        });
    });
    $("input[name='inValObj']").each(function () {
        $(this).change(function () {
            var codeObj = $(this).attr("id").split("_")[1];
            CalculMontantHT(codeObj);
        });
    });
    $("select[name='UnitePortee']").each(function () {
        $(this).change(function () {
            var codeObj = $(this).attr("id").split("_")[1];
            var unitPortee = $(this).val();
            switch (unitPortee) {
                case "%":
                    if ($("input[id='inValObj_" + codeObj + "']").autoNumeric("get") > 100) {
                        $("input[id='inValObj_" + codeObj + "']").val(0);
                    }
                    $("input[id='inValObj_" + codeObj + "']").attr("albmask", "pourcentdecimal");
                    break;
                case "%0":
                    if ($("input[id='inValObj_" + codeObj + "']").autoNumeric("get") > 1000) {
                        $("input[id='inValObj_" + codeObj + "']").val(0);
                    }
                    $("input[id='inValObj_" + codeObj + "']").attr("albmask", "pourmilledecimal");
                    break;
                default:
                    $("input[id='inValObj_" + codeObj + "']").attr("albmask", "decimal");
                    break;
            }
            CalculMontantHT(codeObj);
        });
    });
    $("select[name='TypePorteeCal']").each(function () {
        $(this).change(function () {
            var codeObj = $(this).attr("id").split("_")[1];
            var typePortee = $(this).val();

            if ($(this).val() == "M") {
                $("select[id='UnitePortee_" + codeObj + "']").val("D").attr("disabled", "disabled");
                $("input[id='inValObj_" + codeObj + "']").attr("albmask", "decimal");
            }
            else {
                $("select[id='UnitePortee_" + codeObj + "']").removeAttr("disabled");
            }

            CalculMontantHT(codeObj);
        });
    });

    $("#btnFancyValider").bind('click', function () {
        CheckPortee();
    });
    $("#btnFancyAnnuler").bind('click', function () {
        $("#divPorteGarantie").hide();
        $("#divDataPorteGarantie").html("");
        ReactivateShortCut();
    });

    common.autonumeric.applyAll('init', 'numeric', null, null, null, '99999999999', null);
    common.autonumeric.applyAll('init', 'pourmilledecimal', '');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '');
    common.autonumeric.applyAll('init', 'decimal', null, null, null, '9999999999999.99', null);
}
//--------------Map les actions sur les contrôles de la fancy Inventaire-------
function MapFancyElementInventaire() {
    $("#btnFancyAnnuler").bind('click', function () {
        $.fancybox.close();
        ReactivateShortCut();
        $("#buttonClicked").val("Ajout");

    });
}
//--------------------Map les actions sur les contrôles de la fancy Détails
function MapFancyElementDetail() {
    $("#btnFancyValider").attr('disabled', 'disabled');

    if ($("#isReadOnly").val() != "True") { //&& $("#isAvnReadOnly").val() != "True") {
        if ($("#DateDeb").val() != "") { //) { && $("#isAvnReadOnly").val() != "True" 
            $("#HeureDebHours").removeClass("readonly").removeAttr("disabled");
            $("#HeureDebMinutes").removeClass("readonly").removeAttr("disabled");
        }
        if ($("#DateFin").val() != "") {
            $("#HeureFinHours").removeClass("readonly").removeAttr("disabled");
            $("#HeureFinMinutes").removeClass("readonly").removeAttr("disabled");
        }
    }
    $("#isFinEffet").bind('change', function () {
        if ($(this).is(':checked'))
            $("#isDuree").removeAttr("checked");
        ChangeEtatDateDuree();
    });
    $("#isDuree").bind('change', function () {
        if ($(this).is(':checked'))
            $("#isFinEffet").removeAttr("checked");
        ChangeEtatDateDuree();
    });
    var Duree = "";
    $("#Duree").bind('keyup', function (event) {
        $("#btnFancyValider").removeAttr('disabled');
        if (!isNaN($(this).val()) && $(this).val() > 0) {
            Duree = $.trim($(this).val());
        }
        $(this).val(Duree);
    });
    $("#Nature").live('change', function () {
        ChangeEtatOption($(this).val());
        $("#btnFancyValider").removeAttr('disabled');
        AffectTitleList($(this));
    });
    $("#btnFancyValider").bind('click', function () {
        if (!$(this).attr('disabled')) {
            $("#Nature").die('change');
            SauvegardeDetails();
            ReactivateShortCut();
        }
    });
    $("#btnFancyAnnuler").bind('click', function () {
        $(".requiredField").removeClass("requiredField");
        $("#Nature").die('change');
        $("#divDetailsGarantie").hide();
        $("#divDataDetailsGarantie").html("");
        ReactivateShortCut();
    });
    $("#DateDeb").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AddRemoveCSSHours($(this));
    });
    $("#HeureDeb").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#DateFin").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AddRemoveCSSHours($(this));
    });
    $("#HeureFin").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#Duree").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#DureeUnite").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#GarantieIndexe").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#LCI").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#Franchise").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#CATNAT").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#InclusMontant").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#Application").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffectTitleList($(this));
    });
    $("#TypeEmission").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffectTitleList($(this));
    });
    $("#Taxe").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffectTitleList($(this));
    });
    $("#Definition").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
    });
    $("#AlimAssiette").bind('change', function () {
        $("#btnFancyValider").removeAttr('disabled');
        AffectTitleList($(this));
    });

    AffectTitleList($("#Application"));
    AffectTitleList($("#TypeEmission"));
    AffectTitleList($("#Taxe"));
    AffectTitleList($("#AlimAssiette"));
    AffectTitleList($("#Nature"));
    AffectTitleList($("#DureeUnite"));

    $("#infoDetail").bind('click', function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/OpenInfoDetail",
            data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeGarantie: $("#CodeGarantie").val(), codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val() },
            success: function (data) {
                $("#divInfoDetail").show();
                $("#divDataInfoDetail").html(data);
                AlternanceLigne("InfoDetail1", "", false, null);
                AlternanceLigne("InfoDetail2", "", false, null);
                common.autonumeric.applyAll('init', 'decimal', '', null, null, '9999999999999.99', '-9999999999999.99');
                common.autonumeric.applyAll('init', 'pourcentdecimal', '');
                common.autonumeric.applyAll('init', 'pourmilledecimal', '');
                $("#btnCloseInfoDetail").bind('click', function () {
                    $("#divInfoDetail").hide();
                });
                CloseLoading();
            },
            error: function (data) {
                common.error.showXhr(request);
            }
        });
    });

    //CONTRÔLE DE LA DATE DE FIN OU DURÉE AVEC LA DATE DE MODIFICATION DANS L'AVENANT
    if ($("#NumAvenantPage").val() != "" && $("#NumAvenantPage").val() != "0") {
        if ($("#DateFin").val() != "" && (getDate($("#DateFin").val()) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0)))) {
            $("#DateFin").attr("disabled", "disabled").addClass("readonly").removeClass("datepicker");
            $("#Duree").attr("disabled", "disabled").addClass("readonly");
            $("#DureeUnite").attr("disabled", "disabled").addClass("readonly");
            $("#isFinEffet").attr("disabled", "disabled");
            $("#isDuree").attr("disabled", "disabled");
            $("#HeureFinHours").attr("disabled", "disabled").addClass("readonly");
            $("#HeureFinMinutes").attr("disabled", "disabled").addClass("readonly");
        }
        if ($("#isDuree").is(':checked') && $("#Duree").val() != "" && $("#DureeUnite").val() != "") {
            var dateFinCalc = "";
            var dateDeb = "";
            if ($("#DateDeb").val() != "")
                dateDeb = $("#DateDeb").val();
            else if ($("#DateDebStd").val() != "" && $("#DateDeb").val() == "")
                dateDeb = $("#DateDebStd").val();

            switch ($("#DureeUnite").val()) {
                case "A":
                    dateFinCalc = incrementDate(dateDeb, 0, 0, $("#Duree").val(), 0, true);
                    break;
                case "M":
                    dateFinCalc = incrementDate(dateDeb, 0, $("#Duree").val(), 0, 0, true);
                    break;
                case "J":
                    dateFinCalc = incrementDate(dateDeb, $("#Duree").val(), 0, 0, 0, true);
                    break;
                case "S":
                    dateFinCalc = incrementDate(dateDeb, 0, 0, 0, $("#Duree").val(), true);
                    break;
            }


            if (getDate(dateFinCalc) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0))) {
                $("#DateFin").attr("disabled", "disabled").addClass("readonly").removeClass("datepicker");
                $("#Duree").attr("disabled", "disabled").addClass("readonly");
                $("#DureeUnite").attr("disabled", "disabled").addClass("readonly");
                $("#isFinEffet").attr("disabled", "disabled");
                $("#isDuree").attr("disabled", "disabled");
            }
        }
    }

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    AffectDateFormat();

}
//--------------Ajoute/Enleve les css disabled
function AddRemoveCSSHours(elem) {
    var inputName = elem.attr("id").replace("Date", "");
    if (elem.val() == "") {
        $("#Heure" + inputName + "Hours").val("");
        $("#Heure" + inputName + "Minutes").val("");
        $("#Heure" + inputName + "Hours").addClass("readonly").attr("disabled", "disabled");
        $("#Heure" + inputName + "Minutes").addClass("readonly").attr("disabled", "disabled");
        $("#Heure" + inputName + "Hours").trigger("change");
    }
    else {
        $("#Heure" + inputName + "Hours").removeClass("readonly").removeAttr("disabled");
        $("#Heure" + inputName + "Minutes").removeClass("readonly").removeAttr("disabled");

        if ($("#Heure" + inputName + "Hours").val() == "") {
            if (inputName == "Deb") {
                $("#Heure" + inputName + "Hours").val("00");
                $("#Heure" + inputName + "Minutes").val("00");
            }
            if (inputName == "Fin") {
                $("#Heure" + inputName + "Hours").val("23");
                $("#Heure" + inputName + "Minutes").val("59");
            }
            $("#Heure" + inputName + "Hours").trigger('change');
        }
    }
}
//--------------Enleve la class Required---------
function RemoveClassDetails() {
    $(".requiredField").removeClass("requiredField");
    $("#DateDebStd").removeClass("requiredField");
    $("#HeureDebStdHours").removeClass("requiredField");
    $("#HeureDebStdMinutes").removeClass("requiredField");
    $("#DateFinStd").removeClass("requiredField");
    $("#HeureFinStdHours").removeClass("requiredField");
    $("#HeureFinStdMinutes").removeClass("requiredField");
    $("#DateDeb").removeClass("requiredField");
    $("#HeureDebHours").removeClass("requiredField");
    $("#HeureDebMinutes").removeClass("requiredField");
    $("#DateFin").removeClass("requiredField");
    $("#HeureFinHours").removeClass("requiredField");
    $("#HeureFinMinutes").removeClass("requiredField");
    $("#Duree").removeClass("requiredField");
    $("#DureeUnite").removeClass("requiredField");
}
//-------------Remise en l'état après erreur--------
function ResetDetailsDate(isDate, isDuree) {
    if (isDate) {
        $("#isFinEffet").attr("disabled", "disabled");
        $("#DateFin").attr("disabled", "disabled").addClass("readonly");
        $("#HeureFinHours").attr("disabled", "disabled").addClass("readonly");
        $("#HeureFinMinutes").attr("disabled", "disabled").addClass("readonly");
    }
    if (isDuree) {
        $("#isDuree").attr("disabled", "disabled");
        $("#Duree").attr("disabled", "disabled").addClass("readonly");
        $("#Duree").attr("disabled", "disabled").addClass("readonly");
    }
}
//--------------Sauvegarde les changements du détails de la garantie


function SauvegardeDetails() {
    //enlever les css obligatoires
    RemoveClassDetails();
    $("#DateFinCalc").val("");
    var errDate = false;
    if ($("#DateDeb").val() != "" && !isDate($("#DateDeb").val())) {
        $("#DateDeb").addClass("requiredField");
        errDate = true;
    }
    if ($("#DateFin").val() != "" && !isDate($("#DateFin").val())) {
        $("#DateFin").addClass("requiredField");
        errDate = true;
    }
    if (errDate) {
        return false;
    }

    var msgdateErreur = "";
    //datedeb
    if ($("#DateDebStd").val() != "" && $("#DateDeb").val() != "" && $("#TypeControleDate").val() != "NDEB" && $("#TypeControleDate").val() != "NONCL") {
        msgdateErreur = CheckValidDate("Deb");
        if (msgdateErreur != "") {
            ShowDialogInFancy("Error", msgdateErreur, 300, 65);
            return false;
        }
    }

    //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
    if ($("#NumAvenantPage").val() != "0" && $("#NumAvenantPage").val() == $("#DetailGarAvnCreation").val() && $("#DateDeb").val() == "" && !$("#DateDeb").is(":disabled") && !$("#DateDeb").is("[readonly]")) {
        ShowDialogInFancy("Error", "La date de début est obligatoire en avenant", 300, 65);
        $("#DateDeb").addClass("requiredField");
        $("#HeureDebHours").addClass("requiredField");
        $("#HeureDebMinutes").addClass("requiredField");
        return false;
    }
    if ($("#NumAvenantPage").val() != "0" && $("#DateDeb").val() != "" && !$("#DateDeb").is(":disabled") && !$("#DateDeb").is("[readonly]")) {
        if (getDate($("#DateDeb").val(), $("#HeureDeb").val()) < getDate($("#DateEffetAvenantModificationLocale").val())) {
            ShowDialogInFancy("Error", "La date de début ne doit pas être inférieure à la date de modification dans l'avenant", 300, 65);
            $("#DateDeb").addClass("requiredField");
            $("#HeureDebHours").addClass("requiredField");
            $("#HeureDebMinutes").addClass("requiredField");
            return false;
        }
    }

    //datefin
    if ($("#DateFinStd").val() != "" && $("#isFinEffet").is(':checked') && $("#TypeControleDate").val() != "NFIN" && $("#TypeControleDate").val() != "NONCL") {
        msgdateErreur = CheckValidDate("Fin");
        if (msgdateErreur != "") {
            ShowDialogInFancy("Error", msgdateErreur, 300, 65);
            return false;
        }
    }
    if ($("#DateFin").val() != "" && isDate($("#DateFin").val()) && $("#isFinEffet").is(':checked') && $("#DateDeb").val() != "" && isDate($("#DateDeb").val())) {
        if (!checkDateHeure($("#DateDeb"), $("#DateFin"), $("#HeureDebHours"), $("#HeureFinHours"), $("#HeureDebMinutes"), $("#HeureFinMinutes"))) {
            ShowDialogInFancy("Error", "Veuillez sélectionner une date de début inférieure<br/>à la date de fin", 300, 65);
            return false;
        }
    }

    if ($("#NumAvenantPage").val() != "0" && $("#DateFin").val() != "" && !$("#DateFin").is(":disabled")) {
        if (getDate($("#DateFin").val(), $("#HeureFin").val()) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0), "23:59")) {
            ShowDialogInFancy("Error", "La date de fin ne doit pas être inférieure à la date de modification dans l'avenant -1 min", 300, 65);
            $("#DateFin").addClass("requiredField");
            $("#HeureFinHours").addClass("requiredField");
            $("#HeureFinMinutes").addClass("requiredField");
            return false;
        }
    }

    if ($("#isDuree").is(':checked')) {
        var dateFinCalc = "";
        var dateDeb = "";
        if ($("#DateDeb").val() != "")
            dateDeb = $("#DateDeb").val();
        else if ($("#DateDebStd").val() != "" && $("#DateDeb").val() == "")
            dateDeb = $("#DateDebStd").val();


        if ($("#Duree").val() == "" || $("#DureeUnite").val() == "") {
            $("#Duree").addClass("requiredField");
            $("#DureeUnite").addClass("requiredField");
            ShowDialogInFancy("Error", "Veuillez renseigner tous les champs de durée.", 300, 65);
            return false;
        }
        else {
            switch ($("#DureeUnite").val()) {
                case "A":
                    dateFinCalc = incrementDate(dateDeb, 0, 0, $("#Duree").val(), 0, true);
                    break;
                case "M":
                    dateFinCalc = incrementDate(dateDeb, 0, $("#Duree").val(), 0, 0, true);
                    break;
                case "J":
                    dateFinCalc = incrementDate(dateDeb, $("#Duree").val(), 0, 0, 0, true);
                    break;
                case "S":
                    dateFinCalc = incrementDate(dateDeb, 0, 0, 0, $("#Duree").val(), true);
                    break;
            }
            $("#DateFinCalc").val(dateFinCalc);
            if ($("#DateFinStd").val() != "") {
                if (!checkDateHeure($("#DateFinCalc"), $("#DateFinStd"), $("#HeureFinStdHours"), $("#HeureFinStdHours"), $("#HeureFinStdHours"), $("#HeureFinStdMinutes"))) {
                    $("#Duree").addClass("requiredField");
                    $("#DureeUnite").addClass("requiredField");
                    ShowDialogInFancy("Error", "La date de fin forcée doit être inférieure ou égale<br/> à la date de fin standard", 300, 65);
                    return false;
                }
            }

            if ($("#NumAvenantPage").val() != "0" && $("#DateFinCalc").val() != "" && !$("#Duree").is(":disabled")) {
                if (getDate($("#DateFinCalc").val()) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0))) {
                    ShowDialogInFancy("Error", "La date de fin ne doit pas être inférieure à la date de modification dans l'avenant -1 min", 300, 65);
                    $("#Duree").addClass("requiredField");
                    $("#DureeUnite").addClass("requiredField");
                    return false;
                }
            }

        }


    }

    var finEffet = false;
    var duree = false;
    if ($("#isFinEffet").is(":disabled") && $("#isFinEffet").is(":checked")) {
        finEffet = true;
        $("#isFinEffet").removeAttr("disabled");
        $("#DateFin").removeAttr("disabled").removeClass("readonly");
        $("#HeureFinHours").removeAttr("disabled").removeClass("readonly");
        $("#HeureFinMinutes").removeAttr("disabled").removeClass("readonly");
    }
    if ($("#isDuree").is(":disabled") && $("#isDuree").is(":checked")) {
        duree = true;
        $("#isDuree").removeAttr("disabled");
        $("#Duree").removeAttr("disabled").removeClass("readonly");
        $("#DureeUnite").removeAttr("disabled").removeClass("readonly");
    }
    var argDetails = JSON.stringify($("#dtlGarantie").find(':input').serializeObject());
    var alimAssiette = $("#AlimAssiette").val();
    var codeInven = $("img[name=icoAddInven][albniv=" + $("#currentGarantieNiv").val() + "]").attr("alborigincodeinven");
    var codeObjetRisque = $("#ObjetRisqueCode").val();

    if (codeInven == undefined)
        codeInven = 0;

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/SaveDetailsGarantie",
        data: {
            argDetails: argDetails, albNiv: $("#currentGarantieNiv").val(), codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeAvenant: $("#NumAvenantPage").val(), codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), codeInven: codeInven, modeNavig: $("#ModeNavig").val(),
            codeObjetRisque: codeObjetRisque
        },
        success: function (data) {
            var codeGarantie = $("#currentGarantieId").val();

            // ZBO-20/01/2016 : MOdification bug 1819 : Validatio suite à  une modification d'une Gtie ==> Ajout du test en cas d'avenants
            //if ($("#DateFin").val() != "" && getDate($("#DateFin").val()) < getDate($("#DateEffetAvenantModificationLocale").val()) || ($("#DateFinCalc").val() != "" && getDate($("#DateFinCalc").val()) < getDate($("#DateEffetAvenantModificationLocale").val()))) {
            if ($("#NumAvenantPage").val() != "0") {

                if ($("#DateFin").val() != "" && getDate($("#DateFin").val()) < getDate($("#DateEffetAvenantModificationLocale").val()) || ($("#DateFinCalc").val() != "" && getDate($("#DateFinCalc").val()) < getDate($("#DateEffetAvenantModificationLocale").val()))) {

                    $("img[name=imgcheck][albcodegar=" + codeGarantie + "]").attr("src", "../../Content/Images/icoDelete.png");
                    CloseChildrenAvn(codeGarantie);
                }
                else {
                    $("img[name=imgcheck][albcodegar=" + codeGarantie + "]").attr("src", "../../Content/Images/Checkmark-16.png");
                    OpenChildrenAvn(codeGarantie);
                }
            }

            $("#divDetailsGarantie").hide();
            $("#divDataDetailsGarantie").html("");
            CloseLoading();
            $("#hiddenAction").val('');
            if (data == "O") {
                $("img[name=icoDetailGaran][id=" + codeGarantie + "]").attr("src", "../../Content/Images/Edit_2_16.png");
                if ($("#NumAvenantPage").val() != "" && $("#NumAvenantPage").val() != "0" && $("#NumAvenantPage").val() != undefined)
                    $("img[name=icoDetailGaran][id=" + codeGarantie + "]").parent().parent().find("td[albModifAvt='libVBM']").find("span[name='spanImg']").find("img").attr("src", "/Content/Images/voyant_rouge_petit.png").attr("title", "Modifié dans l'avenant");
            }
            else
                $("img[name=icoDetailGaran][id=" + codeGarantie + "]").attr("src", "../../Content/Images/Edit_16.png");

            $("img[name='icoPorteeGar'][id='" + codeGarantie + "']").attr("albalimassiette", alimAssiette);
            if (alimAssiette != "B" && alimAssiette != "C") {
                var countObj = $("#ObjetRisqueCode").val().split(";")[1].split("_").length;
                if (countObj <= 1) {
                    $("img[name='icoPorteeGar'][id='" + codeGarantie + "']").hide().parent().removeClass('linkImage');
                }
            }
            else {
                $("img[name='icoPorteeGar'][id='" + codeGarantie + "']").show().parent().addClass('linkImage');
            }

            $("#currentGarantieNiv").val("");
        },
        error: function (data) {
            ResetDetailsDate(finEffet, duree);
            var json = $.parseJSON(data.responseText);
            var msg = "Erreur lors de l'enregistrement des détails.";
            if (json.ErrorMessage == "ERREUR_DATE")
                msg = "Erreur date invalide. Veuillez vérifier les niveaux inférieurs et supérieurs";


            ShowDialogInFancy("Error", msg, 300, 65);
        }
    });
}

//--------------Vérifie la validité des dates
function CheckValidDate(elem) {
    if ($("#DateDebStd").val() != "") {
        if (!checkDateHeure($("#DateDebStd"), $("#Date" + elem), $("#HeureDebStdHours"), $("#Heure" + elem + "Hours"), $("#HeureDebStdMinutes"), $("#Heure" + elem + "Minutes"))) {
            if (elem == "Deb")
                return "La date de début forcée doit être supérieure ou égale<br/> à la date de début standard";
            if (elem == "Fin")
                return "La date de fin forcée doit être supérieure ou égale<br/> à la date de début standard";
        }
        if (elem == "Deb" && $("#NumAvenantPage").val() != "0" && $("#isAvnReadOnly").val() != "True") {
            if (!checkDate($("#DateEffetAvenantModificationLocale"), $("#Date" + elem)))
                return "La date de début forcé doit être supérieure ou égale<br/> à la date de modification de la formule";
        }
    }
    if ($("#DateFinStd").val() != "") {
        if (!checkDateHeure($("#Date" + elem), $("#DateFinStd"), $("#Heure" + elem + "Hours"), $("#HeureFinStdHours"), $("#Heure" + elem + "Minutes"), $("#HeureFinStdMinutes"))) {
            if (elem == "Deb")
                return "La date de début forcée doit être inférieure ou égale<br/> à la date de fin standard";
            if (elem == "Fin")
                return "La date de fin forcée doit être inférieure ou égale<br/> à la date de fin standard";
        }
    }
    return "";
}
//-----------------Mode MAJ/ReadOnly Option
function ChangeEtatOption(val) {
    if (val == "C") {
        $("#GarantieIndexe").attr("disabled", "disabled");
        $("#CATNAT").attr("disabled", "disabled");
    }
    else {
        $("#GarantieIndexe").removeAttr("disabled");
        $("#CATNAT").removeAttr("disabled");
    }
}
//---------------------Mode MAJ/ReadOnly FinEffet & Duree
function ChangeEtatDateDuree() {
    if ($("#isDuree").is(":disabled") || $("#isFinEffet").is(":disabled")) {

        return;
    }


    var isFinEffet = $("#isFinEffet").is(':checked');
    var isDuree = $("#isDuree").is(':checked');
    if (isDuree && isFinEffet) {
        isFinEffet = false;
        $("#isFinEffet").removeAttr("checked");
        $("#DateFin").val("");
        $("#HeureFinHours").val("");
        $("#HeureFinMinutes").val("");
    }
    //if (!isFinEffet) {
    //    $("#DateFin").val("");  
    //}
    if (isFinEffet) {
        Duree = "";
        if ($("#Duree").val() != "")
            $("#Durre").trigger("change");
        $("#Duree").val("");
        $("#DureeUnite").val("");
        $("#Duree").addClass("readonly").attr("readonly", "readonly");
        $("#DureeUnite").addClass("readonly").attr("disabled", "disabled");
        $("#DateFin").removeClass("readonly").removeAttr("disabled");
    }
    else if (isDuree) {
        $("#Duree").removeClass("readonly").removeAttr("readonly");
        $("#DureeUnite").removeClass("readonly").removeAttr("disabled");
        if ($("#DateFin").val() != "")
            $("#DateFin").trigger("change");
        $("#DateFin").val("");
        $("#HeureFinHours").val("");
        $("#HeureFinMinutes").val("");
        $("#DateFin").addClass("readonly").attr("disabled", "disabled");
        $("#HeureFinHours").addClass("readonly").attr("disabled", "disabled");
        $("#HeureFinMinutes").addClass("readonly").attr("disabled", "disabled");
    }
    else {
        if ($("#Duree").val() != "")
            $("#Duree").trigger("change");
        $("#Duree").val("");
        $("#DureeUnite").val("");
        $("#Duree").addClass("readonly").attr("readonly", "readonly");
        $("#DureeUnite").addClass("readonly").attr("disabled", "disabled");
        if ($("#DateFin").val() != "")
            $("#DateFin").trigger("change");
        $("#DateFin").val("");
        $("#HeureFinHours").val("");
        $("#HeureFinMinutes").val("");
        $("#DateFin").addClass("readonly").attr("disabled", "disabled");
        $("#HeureFinHours").addClass("readonly").attr("disabled", "disabled");
        $("#HeureFinMinutes").addClass("readonly").attr("disabled", "disabled");
    }
}
//-------------------- Crée la boite de dialog pour le contexte.
function ShowFancy() {
    $("#divLstRsqObj").show();
}
//------------------- Ouvre/Cache les lignes bloc/garantie ---------------------
function AffectClickTree() {
    $("input[type=checkbox][id^=volet]").each(function () {
        $(this).click(function () {
            updateOption($(this));
            openChild($(this), 1, $(this).is(":checked"));
        });
    });
    $("input[type=checkbox][id^=bloc]").each(function () {
        $(this).click(function () {
            updateOption($(this));
            openChild($(this), 2, $(this).is(":checked"));
        });
    });
    $("input[type=checkbox][id^=garantie1]").each(function () {
        $(this).click(function () {
            if ($(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "E") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            updateGarantie($(this));
            openChild($(this), 3, $(this).is(":checked"));
            var albInfo = $(this).attr("albInfo");
            if ($(this).is(":checked")) {
                $("td[albinfo='" + albInfo + "']").attr("title", "A - Accordée");
            }
            else {
                if ($("select[albinfo='" + albInfo + "']").val() == "E")
                    $("td[albinfo='" + albInfo + "']").attr("title", "E - Exclue");
                else
                    $("td[albinfo='" + albInfo + "']").attr("title", "");
            }
        });
    });
    $("input[type=checkbox][id^=garantie2]").each(function () {
        $(this).click(function () {
            if ($(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "E") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            if (!$(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "C") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            updateGarantie($(this));
            openChild($(this), 4, $(this).is(":checked"));
            var albInfo = $(this).attr("albInfo");
            if ($(this).is(":checked")) {
                $("td[albinfo='" + albInfo + "']").attr("title", "A - Accordée");
            }
            else {
                if ($("select[albinfo='" + albInfo + "']").val() == "E")
                    $("td[albinfo='" + albInfo + "']").attr("title", "E - Exclue");
                else
                    $("td[albinfo='" + albInfo + "']").attr("title", "");
            }
        });
    });
    $("input[type=checkbox][id^=garantie3]").each(function () {
        $(this).click(function () {
            if ($(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "E") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            if (!$(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "C") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            updateGarantie($(this));
            openChild($(this), 5, $(this).is(":checked"));
            var albInfo = $(this).attr("albInfo");
            if ($(this).is(":checked")) {
                $("td[albinfo='" + albInfo + "']").attr("title", "A - Accordée");
            }
            else {
                if ($("select[albinfo='" + albInfo + "']").val() == "E")
                    $("td[albinfo='" + albInfo + "']").attr("title", "E - Exclue");
                else
                    $("td[albinfo='" + albInfo + "']").attr("title", "");
            }
        });
    });
    $("input[type=checkbox][id^=garantie4]").each(function () {
        $(this).click(function () {
            if ($(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "E") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            if (!$(this).is(":checked") && $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val() == "C") {
                $("select[id=" + $(this).attr("id").replace("garantie", "paramNatMod") + "]").val("");
            }
            updateGarantie($(this));
            var albInfo = $(this).attr("albInfo");
            if ($(this).is(":checked")) {
                $("td[albinfo='" + albInfo + "']").attr("title", "A - Accordée");
            }
            else {
                if ($("select[albinfo='" + albInfo + "']").val() == "E")
                    $("td[albinfo='" + albInfo + "']").attr("title", "E - Exclue");
                else
                    $("td[albinfo='" + albInfo + "']").attr("title", "");
            }
        });
    });
}
//-----Affectation des méthodes pour les icônes ajout/exclusion formule garantie-------
function AffectClickIco() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();

    var param = codeOffre + "_" + version + "_" + codeFormule + "_" + codeOption;

    $("img[name=icoDetailGaran]").each(function () {
        $(this).click(function () {
            $("#currentGarantieNiv").val($(this).attr("albNiv"));
            $("#currentGuidGarantie").val($(this).attr("albgarguid"));
            ShowDetailsGarantie($(this).attr("id"));
            $("#currentGarantieId").val($(this).attr("id"));
        });
    });
    $("img[name=icoAddInven]").each(function () {
        $(this).click(function () {
            AfficherInventaire($(this).attr("id"), $(this).attr("albNiv"));
            $("#currentGarantieNiv").val($(this).attr("albNiv"));
            $("#currentGarantieId").val($(this).attr("id"));
            $("#currentOriginCodeInventaire").val($(this).attr("albOriginCodeInven"));
        });
    });
    $("img[name=icoPorteeGar]").each(function () {
        $(this).click(function () {
            $("#currentGarantieNiv").val($(this).attr("albNiv"));
            ShowPorteeGarantie($(this).attr("id"));
            $("#currentGarantieId").val($(this).attr("id"));
        });
    });
}
//------------------- Ouvre les fils -----------------------------
function openChild(elem, level, openElem) {
    if (elem.is(":checked"))
        $("img[id='" + elem.attr("id") + "']").show();
    else
        $("img[id='" + elem.attr("id") + "']").hide();
    $("tr[name=" + elem.attr('id') + "]").each(function () {        // récupère tous les <tr> "fils"
        if (openElem)
            $(this).show();
        else
            $(this).hide();                                         // affiche ou cache les <tr>
        $(this).find("input[type=checkbox]").each(function () {     // cherche tous les checkbox du <tr> "fils"
            if ($(this).is(":checked"))                             // vérifie que ces checkbox sont cochés
            {
                if (level == 1) {
                    $("img[id='" + $(this).attr("id") + "']").attr("src", "/Content/Images/Op.png");        // remet l'image du collapse pour les blocs cochés
                }
                openChild($(this), level + 1, openElem && true); // appelle la fonction pour ouvrir les <tr> "fils" du "fils" ("petit-fils")
            }
        });
    });
}
//--------------MAJ des options-----------------------
function updateOption(elem) {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var guidOption = elem.attr('name').split('_')[1];
    var codeCible = $("#CodeCibleRsq").val();
    var isChecked = elem.is(':checked');
    var nivGarantie = elem.attr('albNiv');
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    var codeGarantie = $(elem).attr("albcodegarantie");

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/UpdateOption",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption,
            formGen: $("#FormGen").val(), guidOption: guidOption, codeCible: codeCible, codeObjetRisque: codeObjetRisque, isChecked: isChecked, albNiv: nivGarantie,
            modeNavig: $("#ModeNavig").val(), dateModifAvt: $("#DateEffetAvenantModificationLocale").val(), codeGarantie: codeGarantie
        },
        success: function (data) {
            if (data == "ERRORCACHE") {
                ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                return false;
            }

            if (elem.is(':checked')) {
                elem.parent().parent().find("img[id=icoAddInven]").show();
                elem.parent().parent().find("img[id=icoDetailGaran]").show();
            }
            else {
                elem.parent().parent().find("img[id=icoAddInven]").hide();
                elem.parent().parent().find("img[id=icoDetailGaran]").hide();
            }
            if (codeAvn != "" && codeAvn != "0" && codeAvn != undefined)
                elem.parent().parent().find("td[albModifAvt='libVBM']").find("span[name='spanImg']").find("img").attr("src", "/Content/Images/voyant_rouge_petit.png").attr("title", "Modifié dans l'avenant");

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------MAJ des garanties--------------------
function updateGarantie(elem) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var guidGarantie = elem.attr('name').split('_')[1];
    var isChecked = elem.is(':checked');
    var caractere = elem.siblings("input[name=Caractere]").val();
    var nature = elem.siblings("input[name=Nature]").val();
    var nivGarantie = elem.attr('albNiv');
    var codeCible = $("#Cible").val();
    var actegestion = $("#ActeGestion").val();
    var codeObjetRisque = $("#ObjetRisqueCode").val();

    var paramNat = $("select[id=" + elem.attr("id").replace("garantie", "paramNatMod") + "]").val();

    var codeGarantie = '';
    var codeInventaire = '';
    if (!isChecked) {
        var id = $("img[albgar='" + elem.attr('albgar') + "']").attr('id');
        var img = $("img[albgar='" + elem.attr('albgar') + "']");
        if (id != undefined && img != undefined) {
            codeGarantie = elem.attr('albgar').split('_')[1];
            codeInventaire = img.attr('id').split('_')[2];
        }
    }
    else {
        var id = $("img[albgar='" + elem.attr('albgar') + "']").attr('id');
        if (id != undefined) {
            var tbId = id.split('_');
            $("img[albgar='" + elem.attr('albgar') + "']").attr('src', '/Content/Images/ajouterInventaire1616.png');
            $("img[albgar='" + elem.attr('albgar') + "']").attr('id', tbId[0] + "_" + tbId[1] + "_0");
            codeGarantie = elem.attr('albgar').split('_')[1];
        }
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/UpdateGarantie",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeOption: codeOption, formGen: $("#FormGen").val(),
            guidGarantie: guidGarantie, isChecked: isChecked, caractere: caractere, nature: nature, codeCible: codeCible, codeObjetRisque: codeObjetRisque, albNiv: nivGarantie, paramNat: paramNat,
            codeGarantie: codeGarantie, codeInventaire: codeInventaire, actegestion: actegestion, modeNavig: $("#ModeNavig").val(), dateModifAvt: $("#DateEffetAvenantModificationLocale").val()
        },
        success: function (data) {
            if (data == "ERRORCACHE") {
                ShowCommonFancy("Info", "ErrorCache", "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.", 200, 65, true, true);
                return false;
            }

            elem.siblings("input[name=NatureParam]").val(data);

            if ((data == "A" || data == "C") && elem.is(':checked')) {
                elem.parent().parent().find("img[name=icoAddInven]").parent().addClass('linkImage');
                elem.parent().parent().find("img[name=icoAddInven]").show();
                elem.parent().parent().find("img[name=icoDetailGaran]").parent().addClass('linkImage');
                elem.parent().parent().find("img[name=icoDetailGaran]").show();
                //elem.parent().parent().find("img[name='icoPorteeGar']").parent().addClass('linkImage');
                //elem.parent().parent().find("img[name='icoPorteeGar']").show();
            }
            else {
                elem.parent().parent().find("img[name=icoAddInven]").parent().removeClass('linkImage');
                elem.parent().parent().find("img[name=icoAddInven]").hide();
                elem.parent().parent().find("img[name=icoDetailGaran]").parent().removeClass('linkImage');
                elem.parent().parent().find("img[name=icoDetailGaran]").hide();
                //elem.parent().parent().find("img[name='icoPorteeGar']").parent().removeClass('linkImage');
                //elem.parent().parent().find("img[name='icoPorteeGar']").hide();
            }
            CheckObjetRisque();
            CheckVadider();
            CloseLoading();

            var codeGarPort = elem.attr('albcodegarantie');
            if (elem.is(':checked')) {
                var alimAssiette = $("img[name='icoPorteeGar'][id='" + codeGarPort + "']").attr("albalimassiette");
                if (alimAssiette != "B" && alimAssiette != "C") {
                    var countObj = $("#ObjetRisqueCode").val().split(";")[1].split("_").length;
                    if (countObj <= 1) {
                        $("img[name='icoPorteeGar'][id='" + codeGarPort + "']").hide().parent().removeClass('linkImage');
                    }
                }
                else {
                    $("img[name='icoPorteeGar'][id='" + codeGarPort + "']").show().parent().addClass('linkImage');
                }
            }
            else
                $("img[name='icoPorteeGar'][id='" + codeGarPort + "']").hide().parent().removeClass('linkImage');

            var albInfo = elem.attr("albInfo");
            var isChecked = $("input[type='checkbox'][albinfo='" + albInfo + "']").is(":checked");
            var title = "";
            switch ($("select[albinfo='" + albInfo + "']").val()) {
                case "":
                    title = "A - Accordée";
                    break;
                case "C":
                    title = "C - Comprise";
                    break;
                case "E":
                    title = "E - Exclue";
                    break;
                default:
                    title = "A - Accordée";
                    break;
            }
            $("select[albinfo='" + albInfo + "']").attr("title", title);
            if (codeAvn != "" && codeAvn != "0" && codeAvn != undefined)
                elem.parent().parent().find("td[albModifAvt='libVBM']").find("span[name='spanImg']").find("img").attr("src", "/Content/Images/voyant_rouge_petit.png").attr("title", "Modifié dans l'avenant");

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Suivant---------------------
function Suivant() {
    $('#btnSuivant').live('click', function (evt) {
        if ($(this).attr("disabled")) {
            return false;
        }
        $(".requiredField").removeClass("requiredField");
        ShowLoading();

        if (!window.isReadonly) {
            //var errBlocRelation = checkBlocRelation();
            //if (errBlocRelation != "") {
            //    CloseLoading();
            //    ShowCommonFancy("Error", "", "Erreur : " + errBlocRelation, 600, 200, true, true, true);
            //    return false;
            //}

            //var errGarRelation = checkGarRelation();
            //if (errGarRelation != "") {
            //    CloseLoading();
            //    ShowCommonFancy("Error", "", "Erreur : " + errGarRelation, 600, 200, true, true, true);
            //    return false;
            //}

            var erreurCoherence = checkGarCoherence();
            if (erreurCoherence != "") {
                CloseLoading();
                ShowCommonFancy("Error", "", "Erreur de paramètrage des garanties, impossible de continuer.<br/>Garantie(s) sélectionnée(s) plus d'une fois : " + erreurCoherence, 600, 200, true, true, true);
                return false;
            }

            if ($("#Libelle").val() == "") {
                $("#Libelle").addClass("requiredField");
                CloseLoading();
                return false;
            }

            if ($("#chkModificationAVN").is(":checked")) {
                //TODO : vérifier la compatibilité des dates sur toutes les garanties cochées.
                var errGarDates = "";//checkGarDates();
                if (errGarDates != "") {
                    CloseLoading();
                    ShowCommonFancy("Error", "", "Erreur : " + errGarDates, 600, 200, true, true, true);
                    return false;
                }
                if ($("#DateEffetAvenantModificationLocale").val() == "") {
                    $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                    CloseLoading();
                    return false;
                }
                if ($("#ProchEch").val() != "") {
                    if (!checkDate($("#DateEffetAvenantModificationLocale"), $("#ProchEch"))) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        ShowCommonFancy("Error", "", "La date de modification de la formule doit être inférieure à la date de prochaine échéance " + $("#ProchEch").val(), 300, 80, true, true, true);
                        CloseLoading();
                        return false;
                    }
                }
                if (!checkDate($("#DateEffetAvenant"), $("#DateEffetAvenantModificationLocale"))) {
                    $("#DateEffetAvenant").addClass("requiredField");
                    $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                    ShowCommonFancy("Error", "", "La date de modification de la formule doit être supérieure à la date d'effet de l'avenant " + $("#DateEffetAvenant").val(), 300, 80, true, true, true);
                    CloseLoading();
                    return false;
                }
                if ($("#FinEffet").val() != "") {
                    if (!checkDate($("#DateEffetAvenantModificationLocale"), $("#FinEffet"))) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        ShowCommonFancy("Error", "", "La date de modification de la formule doit être inférieure à la date de fin d'effet " + $("#FinEffet").val(), 300, 80, true, true, true);
                        CloseLoading();
                        return false;
                    }
                }
            }
        }

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        var libelle = $("#Libelle").val();
        var lettreLib = $("#LettreLib").val();
        var codeObjetRisque = $("#ObjetRisqueCode").val();
        var codeCible = $("#CodeCibleRsq").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var codeAvenant = $("#NumAvenantPage").val();
        var dateAvenant = $("#DateEffetAvenantModificationLocale").val();

        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/FormuleGarantieEnregistrer",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvenant: codeAvenant, codeFormule: codeFormule, codeOption: codeOption,
                formGen: $("#FormGen").val(), libelle: libelle, codeObjetRisque: codeObjetRisque, codeCible: codeCible,
                tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), modeNavig: modeNavig, dateAvenant: dateAvenant
            },
            success: function (data) {
                if (data == "")
                    RedirectionFormuleGarantie("InformationsSpecifiquesGarantie", "Index");
                if (data.split(";").length > 0) {
                    var tErrorMsg = data.split(";");
                    if (tErrorMsg[1] == "ERRORDATE") {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        ShowCommonFancy("Error", "", tErrorMsg[2], 300, 80, true, true, true);
                        CloseLoading();
                        return false;
                    }
                    if (tErrorMsg[1] == "ERRORMSG") {
                        DisplayErrorMsg(tErrorMsg);
                        CloseLoading();
                        return false;
                    }

                    if (tErrorMsg[1] == "ERRORREL") {
                        ShowCommonFancy("Error", "", tErrorMsg[2], 300, 80, true, true, true);
                        CloseLoading();
                        return false;
                    }
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//----------Vérifie la relation des blocs des incompatibilités/association-------
function checkBlocRelation() {
    var errBlocRelation = "";
    //Récupération de tous les blocs cochés
    $("input[type=checkbox][name^=bloc]:checked").each(function () {
        var blocId = $(this).attr("id");
        var voletName = blocId.split("_")[2];

        //Vérification que le volet père est coché
        if ($("input[type=checkbox][id=volet_" + voletName + "]").is(":checked")) {
            var tBlocIncomp = $(this).attr("albblocincompatible").split("_")[1].split("||");
            //Contrôle de chaque bloc incompatible à ce bloc
            $.each(tBlocIncomp, function (i, val) {
                if (errBlocRelation == "" && val != "") {
                    var blocCheck = $("input[type=checkbox][id^=bloc_" + val + "_]");
                    if (blocCheck.is(":checked") && blocCheck.length > 0) {
                        var idBlocCheck = blocCheck.attr("id");
                        var voletCheck = $("input[type=checkbox][id=volet_" + idBlocCheck.split("_")[2] + "]");
                        if (voletCheck.is(":checked") && voletCheck.length > 0) {
                            errBlocRelation += "Les blocs <br/><br/>" + $("td[albblocdesc=" + blocId + "]").text() + "<br/>et<br/>" + $("td[albblocdesc=" + idBlocCheck + "]").text() + "<br/><br/> ne sont pas compatibles.<br/>Vous devez en retirer un des deux pour pouvoir continuer.";
                        }
                    }
                }
            });

            if (errBlocRelation == "") {
                var tBlocAssoc = $(this).attr("albblocassocie").split("_")[1].split("||");
                //Contrôle de chaque bloc associé à ce bloc
                $.each(tBlocAssoc, function (i, val) {
                    if (errBlocRelation == "" && val != "") {
                        var blocCheck = $("input[type=checkbox][id^=bloc_" + val + "_]");
                        if (blocCheck.length > 0) {
                            var idBlocCheck = blocCheck.attr("id");
                            if (!blocCheck.is(":checked")) {
                                errBlocRelation += "Les blocs <br/><br/>" + $("td[albblocdesc=" + blocId + "]").text() + "<br/>et<br/>" + $("td[albblocdesc=" + idBlocCheck + "]").text() + "<br/><br/> sont associés.<br/>Si vous en sélectionner un, vous devez sélectionner l'autre pour pouvoir continuer.";
                            }
                            else {
                                var voletCheck = $("input[type=checkbox][id=volet_" + idBlocCheck.split("_")[2] + "]");
                                if (!voletCheck.is(":checked") && voletCheck.length > 0) {
                                    errBlocRelation += "Les blocs <br/><br/>" + $("td[albblocdesc=" + blocId + "]").text() + "<br/>et<br/>" + $("td[albblocdesc=" + idBlocCheck + "]").text() + "<br/><br/> sont associés.<br/>Si vous en sélectionner un, vous devez sélectionner l'autre pour pouvoir continuer.";
                                }
                            }
                        }
                    }
                });
            }
        }
    });
    return errBlocRelation;
}
//----------Vérifie la relation des garanties des incompatibilités/associations/dépendances/alternance-------
function checkGarRelation() {
    var result = "";
    var garanties = "";
    $("input[type=checkbox][name^=garantieNiv]:checked").each(function () {
        garanties += $(this).attr("id").split("_")[1] + "_";
    });

    $.ajax({
        url: "/CreationFormuleGarantie/GetDatesByGaranries",
        type: "Get",
        data: { idGaranties: garanties },
        success: function (data) {
            result = checkGarRelationWithPeriodes(data);
        },
        async: false,
        cache: false,
        error: function (request) {
            common.error.showXhr(request);
        }

    });

    return result;
}

//----------Vérifie les dates des garanties-------
function checkGarDates() {
    var result = "";
    var garanties = "";
    $("input[type=checkbox][name^=garantieNiv]:checked").each(function () {
        garanties += $(this).attr("id").split("_")[1] + "_";
    });

    $.ajax({
        url: "/CreationFormuleGarantie/GetDatesDebByGaranties",
        type: "Get",
        data: { idGaranties: garanties, codeAvn: $("#NumAvenantPage").val() },
        success: function (data) {
            result = checkGarWithDates(data);
        },
        async: false,
        cache: false,
        error: function (request) {
            common.error.showXhr(request);
        }

    });

    return result;
}

function checkGarWithDates(periodes) {

    var errGarDate = "Incohérence de dates sur les garanties suivantes :<br/><br/>";
    var i = 0;
    var dateModifAvn = getDate($("#DateEffetAvenantModificationLocale").val());
    //Récupération de toutes les garanties cochées
    $("input[type='checkbox'][name^='garantieNiv']:checked").each(function () {
        var path = $(this).attr("albNiv");
        var tPath = path.split("_");

        //Vérification que tous les pères sont cochés
        var checkGar = true;
        switch (tPath[0]) {
            case "niv1":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv2":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv3":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv4":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv3_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "_" + tPath[6] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
        }

        if (checkGar) {
            var garId = $(this).attr("id");
            var natureParam = $("#NatureParam" + garId.replace("garantie1", "").replace("garantie2", "").replace("garantie3", "").replace("garantie4", "")).val();
            if (natureParam !== "E" && natureParam !== "") {
                var garPeriode = $.grep(periodes, function (e) { return e.IdGarantie === parseInt(garId.split("_")[1]); })[0];
                var debutPeriode = ToJavaScriptDate(garPeriode.DateDebut);

                if (debutPeriode != null && dateModifAvn > debutPeriode && (garPeriode.IsCreateAvn || garPeriode.IsUpdateAvn)) {
                    errGarDate += "- " + $("td[albgardesc=" + garId + "]").text() + "<br/>";
                    i++;
                }
            }
        }
    });
    return i === 0 ? "" : errGarDate;
}

function checkGarRelationWithPeriodes(periodes) {

    var errGarRelation = "";
    //Récupération de toutes les garanties cochées
    $("input[type=checkbox][name^=garantieNiv]:checked").each(function () {
        var path = $(this).attr("albNiv");
        var tPath = path.split("_");

        //Vérification que tous les pères sont cochés
        var checkGar = true;
        switch (tPath[0]) {
            case "niv1":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv2":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv3":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
            case "niv4":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv3_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "_" + tPath[6] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
        }

        if (checkGar) {
            var garAlternative = $(this).attr("albgaralternative").split("_")[4];
            var garId = $(this).attr("id");
            var natureParam = $("#NatureParam" + garId.replace("garantie1", "").replace("garantie2", "").replace("garantie3", "").replace("garantie4", "")).val();
            if (natureParam != "E" && natureParam != "") {
                var nameVoletBloc = $(this).attr("albVoletBloc");
                var tGarIncomp = $(this).attr("albgarincompatible").split("_")[4].split("||");
                //Contrôle de chaque garantie incompatible à cette garantie
                $.each(tGarIncomp, function (i, val) {
                    if (errGarRelation == "" && val != "") {
                        if (garAlternative != "0") {
                            var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "][albgaralternative=" + garAlternative + "]");
                            if (garCheck <= 0)
                                var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "]");
                        }
                        else
                            var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "]");
                        if (garCheck.is(":checked") && garCheck.length > 0) {
                            var idGarCheck = garCheck.attr("id");
                            var natureParamGarCheck = $("#NatureParam" + idGarCheck.replace("garantie1", "").replace("garantie2", "").replace("garantie3", "").replace("garantie4", "")).val();
                            if (natureParamGarCheck != "E" && natureParamGarCheck != "") {
                                var garCheckVoletBloc = garCheck.attr("albVoletBloc");
                                var blocCheck = $("input[type=checkbox][id=bloc_" + garCheckVoletBloc.split("_")[0] + "_" + garCheckVoletBloc.split("_")[1] + "]");
                                var voletCheck = $("input[type=checkbox][id=volet_" + garCheckVoletBloc.split("_")[1] + "]");
                                if (voletCheck.is(":checked") && blocCheck.is(":checked") && voletCheck.length > 0 && blocCheck.length > 0) {
                                    if (garAlternative != "0")
                                        errGarRelation += "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + "<br/><br/>sont alternatives.<br/>Vous devez en retirer une des deux pour pouvoir continuer.";
                                    else {
                                        var garPeriode = $.grep(periodes, function (e) { return e.IdGarantie === parseInt(garId.split("_")[1]); })[0];
                                        var garPeriodeToCheck = $.grep(periodes, function (e) { return e.IdGarantie === parseInt(idGarCheck.split("_")[1]); })[0];

                                        var isOverlaps = (garPeriode != null) && (garPeriodeToCheck != null)
                                            && (isDateRangesOverlaps(ToJavaScriptDate(garPeriode.DateDebut),
                                                ToJavaScriptDate(garPeriode.DateFin),
                                                ToJavaScriptDate(garPeriodeToCheck.DateDebut),
                                                ToJavaScriptDate(garPeriodeToCheck.DateFin))) === true;
                                        if (isOverlaps) {
                                            errGarRelation +=
                                                "Les garanties <br/><br/>" +
                                                $("td[albgardesc=" + garId + "]").text() +
                                                "<br/>et<br/>" +
                                                $("td[albgardesc=" + idGarCheck + "]").text() +
                                                ",<br/><br/>doivent avoir des plages de dates qui ne se croisent pas";
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

                if (errGarRelation == "") {
                    var tGarAssoc = $(this).attr("albgarassociee").split("_")[4].split("||");
                    //Contrôle de chaque garantie incompatible à cette garantie
                    $.each(tGarAssoc, function (i, val) {
                        if (errGarRelation == "" && val != "") {
                            if (garAlternative != "0") {
                                var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "][albgaralternative=" + garAlternative + "]");
                                if (garCheck <= 0)
                                    var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "]");
                            }
                            else
                                var garCheck = $("input[type=checkbox][id$=" + nameVoletBloc + "][albcodegarantie=" + val + "]");
                            if (garCheck.length > 0) {
                                var idGarCheck = garCheck.attr("id");
                                var natureParamGarCheck = $("#NatureParam" + idGarCheck.replace("garantie1", "").replace("garantie2", "").replace("garantie3", "").replace("garantie4", "")).val();
                                if (natureParamGarCheck != "E" && natureParamGarCheck != "") {
                                    var garCheckVoletBloc = garCheck.attr("albVoletBloc");
                                    var blocCheck = $("input[type=checkbox][id=bloc_" + garCheckVoletBloc.split("_")[0] + "_" + garCheckVoletBloc.split("_")[1] + "]");
                                    var voletCheck = $("input[type=checkbox][id=volet_" + garCheckVoletBloc.split("_")[1] + "]");

                                    if (garAlternative != "0") {
                                        if (garCheck.is(":checked") && voletCheck.is(":checked") && blocCheck.is(":checked") && voletCheck.length > 0 && blocCheck.length > 0)
                                            errGarRelation += "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + "<br/><br/>sont alternative.<br/>Vous devez en cocher une des deux (mais pas les deux) pour pouvoir continuer.";
                                    }
                                    else {
                                        var garPeriode = $.grep(periodes, function (e) { return e.IdGarantie === parseInt(garId.split("_")[1]); })[0];
                                        var garPeriodeToCheck = $.grep(periodes, function (e) { return e.IdGarantie === parseInt(idGarCheck.split("_")[1]); })[0];

                                        var isEquals = (garPeriode != null) && (garPeriodeToCheck != null)
                                            && (isDateRangesEquals(ToJavaScriptDate(garPeriode.DateDebut),
                                                ToJavaScriptDate(garPeriode.DateFin),
                                                ToJavaScriptDate(garPeriodeToCheck.DateDebut),
                                                ToJavaScriptDate(garPeriodeToCheck.DateFin)) === true);
                                        var msg = "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + "<br/><br/>sont associées.<br/>Vous devez sélectionner l'autre pour pouvoir continuer.";
                                        if (!garCheck.is(":checked"))
                                            errGarRelation += msg
                                        else if (!voletCheck.is(":checked") || !blocCheck.is(":checked") && voletCheck.length > 0 && blocCheck.length > 0) {
                                            errGarRelation += msg;
                                        } else if (!isEquals) {
                                            errGarRelation += "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + ",<br/><br/>doivent avoir des plages de dates Identiques";
                                        }
                                    }

                                }
                            }
                        }
                    });
                }
            }
        }
    });

    if (errGarRelation == "") {
        $("input[type=checkbox][name^=garantieNiv]").each(function () {
            var path = $(this).attr("albNiv");
            var tPath = path.split("_");

            //Vérification que tous les pères sont cochés
            var checkGar = true;
            switch (tPath[0]) {
                case "niv1":
                    if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked")) {
                        checkGar = false;
                    }
                    break;
                case "niv2":
                    if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked")) {
                        checkGar = false;
                    }
                    break;
                case "niv3":
                    if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked")) {
                        checkGar = false;
                    }
                    break;
                case "niv4":
                    if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=niv3_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "_" + tPath[6] + "]").is(":checked")) {
                        checkGar = false;
                    }
                    break;
            }

            if (checkGar) {
                var garId = $(this).attr("id");
                var groupeAlt = $(this).attr("albgaralternative").split("_")[4];
                if (groupeAlt != "0") {
                    if (!$(this).is(":checked")) {
                        var garAlternative = $(this).attr("albgaralternative");
                        var garName = $(this).attr("name");
                        var garVoletBloc = $(this).attr("albVoletBloc");
                        $("input[type=checkbox][name!=" + garName + "][albvoletbloc=" + garVoletBloc + "][albgaralternative!=" + garAlternative + "][albgaralternative^=gar_" + garVoletBloc + "_][albgaralternative$=_" + groupeAlt + "]").each(function () {
                            var idGarCheck = $(this).attr("id");
                            if (errGarRelation == "" && !$(this).is(":checked")) {
                                errGarRelation += "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + "<br/><br/>sont alternative.<br/>Vous devez en cocher une des deux (mais pas les deux) pour pouvoir continuer.";
                            }
                        });
                    }
                    else {
                        var garAlternative = $(this).attr("albgaralternative");
                        var garName = $(this).attr("name");
                        var garVoletBloc = $(this).attr("albVoletBloc");
                        $("input[type=checkbox][name!=" + garName + "][albvoletbloc=" + garVoletBloc + "][albgaralternative!=" + garAlternative + "][albgaralternative^=gar_" + garVoletBloc + "_][albgaralternative$=_" + groupeAlt + "]").each(function () {
                            var idGarCheck = $(this).attr("id");
                            if (errGarRelation == "" && $(this).is(":checked")) {
                                errGarRelation += "Les garanties <br/><br/>" + $("td[albgardesc=" + garId + "]").text() + "<br/>et<br/>" + $("td[albgardesc=" + idGarCheck + "]").text() + "<br/><br/>sont alternative.<br/>Vous devez en cocher une des deux (mais pas les deux) pour pouvoir continuer.";
                            }
                        });
                    }

                }
            }
        });
    }


    return errGarRelation;
}

//----------Vérifie la cohérence des garanties sélectionnées--------
function checkGarCoherence() {
    var erreurCoherence = "";
    $("input[type=checkbox][name^=garantieNiv1]:checked").each(function () {
        var id = $(this).attr("id");
        var path = $(this).attr("albNiv");
        var tPath = path.split("_");
        var gar = $(this).attr("albGar");

        var checkGar = true;
        switch (tPath[0]) {
            case "niv1":
                if (!$("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") || !$("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked")) {
                    checkGar = false;
                }
                break;
        }

        if (checkGar) {
            var nbCheck = $("input[type=checkbox][albGar=" + gar + "]:checked").length;
            nbCheck = GetCheckedGar(gar);
            var presentGar = false;
            var checkCoherence = ", " + erreurCoherence + ", ";
            if (checkCoherence.replace(", " + gar.split("_")[1] + ", ", "") != checkCoherence)
                presentGar = true;
            if (nbCheck > 1 && !presentGar)
                erreurCoherence = erreurCoherence + ", " + gar.split("_")[1];
        }
    });
    if (erreurCoherence != "")
        erreurCoherence = erreurCoherence.substring(2);
    return erreurCoherence;
}
//--------Récupère tous les checkbox sélectionnées ayant le même nom---------
function GetCheckedGar(gar) {
    var nbCheck = 0;

    $("input[type=checkbox][albGar=" + gar + "]:checked").each(function () {
        var path = $(this).attr("albNiv");
        var tPath = path.split("_");

        switch (tPath[0]) {
            case "niv1":
                if ($("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") && $("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked")) {
                    nbCheck++;
                }
                break;
            case "niv2":
                if ($("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") && $("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked")) {
                    nbCheck++;
                }
                break;
            case "niv3":
                if ($("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") && $("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked")) {
                    nbCheck++;
                }
                break;
            case "niv4":
                if ($("input[type=checkbox][albNiv^=volet_" + tPath[1] + "]").is(":checked") && $("input[type=checkbox][albNiv^=bloc_" + tPath[1] + "_" + tPath[2] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv1_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv2_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "]").is(":checked") && $("input[type=checkbox][albNiv^=niv3_" + tPath[1] + "_" + tPath[2] + "_" + tPath[3] + "_" + tPath[4] + "_" + tPath[5] + "_" + tPath[6] + "]").is(":checked")) {
                    nbCheck++;
                }
                break;
        }
    });


    return nbCheck;
}

//---------Sauvegarde les informations de portée d'une garantie-------
function CheckPortee() {

    //Check dates
    if ($("#isFinEffet").is(':checked') && $("#DateDeb").val() != "") {
        var checkDH = checkDateHeure($("#DateDeb"), $("#DateFin"), $("#HeureDebHours"), $("#HeureFinHours"), $("#HeureDebMinutes"), $("#HeureFinMinutes"));
        if (!checkDH) {
            return false;
        }
    }

    var nbObj = $("input[type=checkbox][id=chkObjPortee]").length;
    var nbObjChecked = $("input[type=checkbox][id=chkObjPortee]:checked").length;

    if ($("#TypeAlimAssiette").val() != "B" && $("#TypeAlimAssiette").val() != "C") {
        if (nbObjChecked == 0 && $("#Action").val() != "") {
            ShowCommonFancy("Error", "", "Vous devez sélectionner au moins un objet.", 350, 80, true, true);
            return false;
        }
        else if (nbObjChecked == nbObj) {
            ShowCommonFancy("Error", "", "Vous ne pouvez pas sélectionner l'ensemble des objets.", 350, 80, true, true);
            return false;
        }
        else if (nbObjChecked != 0 && $("#Action").val().trim() == "") {
            ShowCommonFancy("Error", "", "Vous devez sélectionner au moins une action.", 350, 80, true, true);
            return false;
        }

    }

    var error = false;

    $("input[name=chkObjPortee]").each(function () {
        if ($(this).attr('checked') == "checked" && $("#TypeAlimAssiette").val() == "B" || $("#TypeAlimAssiette").val() == "C") {

            var numOb = $(this).attr('albCodeObjet');

            var mnt = $("#inValObj_" + numOb).val();
            var unite = $("#UnitePortee_" + numOb).val();
            var type = $("#TypePorteeCal_" + numOb).val();

            if (mnt != "0,00") {
                if (mnt == undefined || mnt == "" || unite == undefined || unite == "" || type == undefined || type == "") {
                    $("#inValObj_" + numOb).addClass('requiredField');
                    $("#UnitePortee_" + numOb).addClass('requiredField');
                    $("#TypePorteeCal_" + numOb).addClass('requiredField');
                    error = true;
                }
            }
            else {
                if ((unite == undefined || unite == "") && (type != undefined && type != "")) {
                    $("#inValObj_" + numOb).addClass('requiredField');
                    $("#UnitePortee_" + numOb).addClass('requiredField');
                    error = true;
                }
                else if ((unite != undefined && unite != "") && (type == undefined || type == "")) {
                    $("#inValObj_" + numOb).addClass('requiredField');
                    $("#TypePorteeCal_" + numOb).addClass('requiredField');
                    error = true;
                }
                else if ((unite != undefined && unite != "") && (type != undefined && type != "")) {
                    $("#inValObj_" + numOb).addClass('requiredField');
                    error = true;
                }
            }
            //if (mnt == undefined && unite == undefined && type == undefined) {
            //    error = false;
            //}
        }
    });

    if (error == true)
        return false;

    if ($("#Action").val() == "")
        ShowCommonFancy("Confirm", "Portee", "Etes-vous sûr de vouloir supprimer les informations d'accord/exclusion ?", 350, 80, true, true);
    else
        SavePortee();

}
//---------Sauvegarde les informations de portée d'une garantie-------
function SavePortee() {
    ShowLoading();
    var codesObj = "";
    if ($("#Action").val() != "") {
        $("input[type=checkbox][id=chkObjPortee]:checked").each(function () {
            codesObj += $(this).attr("albCodeObjet") + ";";
        });
    }

    var dataRow = "[";


    $("tr[name='lineObjPortee']").each(function () {
        var codeObj = $(this).attr("albCodeObj");
        var isChecked = $("input[type='checkbox'][name='chkObjPortee'][albcodeobjet='" + codeObj + "']").is(":checked");
        var unitDisabled = $("select[id='UnitePortee_" + codeObj + "']").is("disabled");

        if (isChecked) {
            if (unitDisabled) {
                $("select[id='UnitePortee_" + codeObj + "']").removeAttr("disabled");
            }

            dataRow += '{';

            dataRow += 'Code:"' + codeObj + '",';
            dataRow += 'ValPorteeObj:"' + ($("input[id='inValObj_" + codeObj + "']").val() != "" && $("input[id='inValObj_" + codeObj + "']").val() != undefined ? $("input[id='inValObj_" + codeObj + "']").val() : 0) + '",';
            dataRow += 'UnitPorteeObj:"' + $("select[id='UnitePortee_" + codeObj + "']").val() + '",';
            dataRow += 'TypePorteeCal:"' + $("select[id='TypePorteeCal_" + codeObj + "']").val() + '",';
            dataRow += 'PrimeMntCal:"' + $.trim($("span[name='primeCalObj_" + codeObj + "']").html()) + '"';

            dataRow += '},';

            if (unitDisabled) {
                $("select[id='UnitePortee_" + codeObj + "']").attr("disabled", "disabled");
            }
        }

    });

    if (dataRow != "[") {
        dataRow = dataRow.substring(0, dataRow.length - 1)
    }
    dataRow += "]";

    var reportCal = $("#chckReportCal") != undefined ? $("#chckReportCal").is(":checked") : false;
    var codeInven = $("img[name=icoAddInven][albniv=" + $("#currentGarantieNiv").val() + "]").attr("alborigincodeinven");
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    if (codeInven == undefined)
        codeInven = 0;

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/SavePorteeGarantie",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
            codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(),
            idGarantie: $("#IdGarantiePortee").val(), codeGarantie: $("#CodeGarantiePortee").val(),
            nature: $("#Action").val(), codeRsq: $("#CodeRsqPortee").val(), codesObj: codesObj, codeInven: codeInven, albNiv: $("#currentGarantieNiv").val(),
            modeNavig: $("#ModeNavig").val(), objPortee: dataRow, alimAssiette: $("#TypeAlimAssiette").val(), reportCal: reportCal,
            codeObjetRisque: codeObjetRisque
        },
        success: function (data) {
            ChangePorteeImg($("#IdGarantiePortee").val(), $("#Action").val());
            $("#divPorteGarantie").hide();
            $("#divDataPorteGarantie").html("");
            ReactivateShortCut();
            CloseLoading();
            $("#currentGarantieNiv").val("");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Vérifie le nombre d'objets sélectionnés avant d'afficher l'icône de portée---------
function CheckObjetRisque() {
    //var countObj = $("#ObjetRisqueCode").val().split(";")[1].split("_").length;
    var countObj = $("input[name='checkObj']:checked").parent().parent().not(".None").length;
    if (countObj > 1) {
        $("input[type=checkbox][name^=garantieNiv1_]").each(function () {
            var idGar = $(this).attr("albcodegarantie");
            if ($(this).is(":checked"))
                $("img[id=" + idGar + "]").show().parent().addClass('linkImage');
            else
                $("img[id=" + idGar + "]").hide().parent().removeClass('linkImage');
        });
    }
    else {
        $("img[name=icoPorteeGar][albalimassiette!='B'][albalimassiette!='C']").hide().parent().removeClass('linkImage');
    }
}
//---------Change l'image de la portée de garantie---------
function ChangePorteeImg(idGarantie, nature) {
    if (nature != "")
        $("img[name=icoPorteeGar][albgarinfo^=garInfo1_" + idGarantie + "_]").attr('src', '/Content/Images/portee_2.png');
    else
        $("img[name=icoPorteeGar][albgarinfo^=garInfo1_" + idGarantie + "_]").attr('src', '/Content/Images/portee.png');
}
//----Région div flottante Inventaire(sur Formule de garantie)
//-----Map les éléments  de la div flottante Inventaire de garantie
function MapElementDivFInventaireGarantie() {
    $("#btnValiderInventaire").die().live('click', function () {
        EnregistrerInventaire();
    });
    $("#btnAnnulerInventaire").die().live('click', function () {
        if ($("#NewInvenForm").val() == "True")
            SupprimerInventaire();
        else
            FermerInventaire();
    });
    $("#btnSupprimerInventaire").die().live('click', function () {
        ShowCommonFancy("Confirm", "Inventaire",
            "Vous allez définitivement supprimer cet inventaire. Voulez-vous continuer ?",
            350, 130, true, true);
    });
}
//---Affiche l'inventaire de la garantie en div flottante---
function AfficherInventaire(param, nivIds) {
    var tab = param.split('_');
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();

    var codeFormule = $("#CodeFormule").val();
    var codeGarantie = tab[0];
    var typeInventaire = tab[1];
    var codeInventaire = tab[2];
    var codeRisque = '';
    var codeObjet = '';
    var elem = $("#ObjetRisqueCode").val().split(';')
    if (elem.length > 1) {
        var codeRisque = elem[0];
        var tabObjet = elem[1].split('_');
        if (tabObjet.length > 0)
            codeObjet = tabObjet[0];
    }
    var codeOption = $("#CodeOption").val();
    var formGen = $("#FormGen").val();

    var tabGuid = $("#tabGuid").val();
    var isForceReadOnly = $("#IsForceReadOnly").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var addParamString = "addParam" + addParamType + "|||" + addParamValue + (isForceReadOnly == "True" ? "||FORCEREADONLY|1" : "") + "addParam";

    var id = codeOffre + "_" + version + "_" + type + "_" + "FormuleGarantie" + "_" + codeFormule + "_" + codeGarantie + "_" + typeInventaire + "_" + codeInventaire + "_" + codeRisque + "_" + codeObjet + "_" + codeOption + "_" + formGen + tabGuid + addParamString + GetFormatModeNavig($("#ModeNavig").val());
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RisqueInventaire/LoadInventaire",
        data: { id: id, idGaran: nivIds.split('_').last(), isReadonly: $("#OffreReadOnly").val() },
        success: function (data) {
            $("#divDataGarantieInventaire").html(data);
            $("#divGarantieInventaire").show();
            MapElementDivFInventaireGarantie();
            CheckNbRowInven();
            AffectDateFormat();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------------Enregistrement de l'inventaire depuis la div flottante---------------
function EnregistrerInventaire() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeRisque = $("#CodeRisque").val();
    var codeObjet = $("#CodeObjet").val();
    var codeInven = $("#Code").val();
    var descriptif = $("#Descriptif").val();
    var description = encodeURIComponent($("#Observations").val());
    var ecranProvenance = $("#EcranProvenance").val();
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var formGen = $("#FormGen").val();
    var albNiv = $("#currentGarantieNiv").val()

    var valReport = $("#Valeur").autoNumeric('get');
    var unitReport = $("#UniteLst").val();
    var typeReport = $("#TypeLst").val();
    var taxeReport = $("#TaxeLst").val();
    var activeReport = $("#ActiverReport").is(":checked");
    var typeAlim = $("#TypeAlim").val();

    $(".requiredField").removeClass('requiredField');

    if (descriptif == "") {
        $("#Descriptif").addClass('requiredField');
        CloseLoading();
        return false;
    }

    if (activeReport && (valReport == "" || unitReport == "" || typeReport == "")) {
        $("#Valeur").addClass('requiredField');
        $("#UniteLst").addClass('requiredField');
        $("#TypeLst").addClass('requiredField');
        CloseLoading();
        return false;
    }

    if (codeInven == undefined)
        codeInven = 0;

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/EnregistrerInventaire",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeRisque: codeRisque, codeObjet: codeObjet,
            codeInven: codeInven, descriptif: descriptif, description: description, tabGuid: $("#tabGuid").val(),
            codeFormule: codeFormule, codeOption: codeOption, albNiv: albNiv,
            ecranProvenance: ecranProvenance, typeAlim: typeAlim,
            valReport: $("#Valeur").autoNumeric('get').replace('.', ','), unitReport: unitReport, typeReport: typeReport, taxeReport: taxeReport, activeReport: activeReport, idGaran: $("#IdGaran").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            UpdateImgInven("on");
            SetListAddedInventaires();
            $("#divDataGarantieInventaire").html('');
            $("#divGarantieInventaire").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------Supprimer l'inventaire dans la div flottante-------
function SupprimerInventaire() {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeRisque = $("#CodeRisque").val();

    var codeFormule = $("#CodeFormule").val();
    var codeGarantie = $("#CodeGarantie").val();
    var codeInventaire = $("#Code").val();

    var codeOption = $("#CodeOption").val();
    var formGen = $("#FormGen").val();
    var albNiv = $("#currentGarantieNiv").val()

    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/SupprimerInventaire",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeGarantie: codeGarantie, codeInventaire: codeInventaire,
            codeOption: codeOption, albNiv: albNiv,
            formGen: formGen, tabGuid: $("#tabGuid").val(), saveCancel: $("#txtSaveCancel").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            UpdateImgInven();
            $("#divDataGarantieInventaire").html('');
            $("#divGarantieInventaire").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function FermerInventaire() {
    var tabId = $("#currentGarantieId").val().split('_');
    var codeInven = tabId[2];
    if (codeInven == "0") {
        SetListAddedInventaires();
        UpdateImgInven("on");
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvn = $("#NumAvenantPage").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        var albNiv = $("#currentGarantieNiv").val()

        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/FermerDivFInventaire",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn,
                tabGuid: $("#tabGuid").val(),
                codeFormule: codeFormule, codeOption: codeOption, albNiv: albNiv,
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    $("#divDataGarantieInventaire").html('');
    $("#divGarantieInventaire").hide();
}
function UpdateImgInven(param) {
    var id = $("#currentGarantieId").val();
    var tbId = id.split('_');
    if (param == "on") {
        $("img[id='" + $("#currentGarantieId").val() + "']")
            .attr('src', '/Content/Images/ajouterInventaire1616_on.png')
            .attr('id', tbId[0] + "_" + tbId[1] + "_" + $("#Code").val());
    }
    else {
        $("img[id='" + $("#currentGarantieId").val() + "']")
            .attr('src', '/Content/Images/ajouterInventaire1616.png')
            .attr('id', tbId[0] + "_" + tbId[1] + "_0");
    }

}

//----- la liste contenant les inventaires ajoutés depuis l'ouverture de l'interface création formule de garantie--
function SetListAddedInventaires() {
    var id = $("#currentGarantieId").val();
    var tbId = id.split('_');
    var code = $("#currentOriginCodeInventaire").val();
    if (code == "0" && tbId[2] == "0") {
        var codeGarantie = $("#CodeGarantie").val();
        var codeInventaire = $("#Code").val();
        if ($("#AddedInventaires").val() == "") {
            $("#AddedInventaires").val(codeGarantie + "_" + codeInventaire);
        }
        else {
            $("#AddedInventaires").val($("#AddedInventaires").val() + ";" + codeGarantie + "_" + codeInventaire);
        }
    }
}
//------Annuler et supprimer tous les inventaires ajoutés depuis l'ouverture de création de formule de garantie------
function Annuler() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addedInventaires = $("#AddedInventaires").val();

    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var isForceReadOnly = $("#IsForceReadOnly").val();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/Annuler/",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), addedInventaires: addedInventaires,
            tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, isModeConsultationEcran: $("#IsModeConsultationEcran").val(),
            isForceReadOnly: isForceReadOnly, isModeDuplication: ($("#modeDuplication").val() == "True" && $("#ObjetRisque").val() == "")
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Compte le nombre de ligne d'inventaire----------
function CheckNbRowInven() {
    if ($("tr[name=rowInventaire]").length <= 0 || $("td[id^=divMontant]").length <= 0) {
        $("#btnSum").attr("disabled", "disabled");
        $("#Valeur").attr("disabled", "disabled");
        $("#UniteLst").attr("disabled", "disabled");
        $("#TypeLst").attr("disabled", "disabled");
        $("#ActiverReport").attr("disabled", "disabled").removeAttr("checked");
    }
    else {
        $("#btnSum").removeAttr("disabled");
        $("#Valeur").removeAttr("disabled");
        $("#UniteLst").removeAttr("disabled");
        $("#TypeLst").removeAttr("disabled");
        $("#ActiverReport").removeAttr("disabled");
        if ($("#InvenSpec").val() == "O")
            $("#ActiverReport").attr("checked", "checked");
    }
}

//-----------Affiche le message d'erreur pour le calcul d'assiette---------
function DisplayErrorMsg(tMsg) {
    var customRow = $("#tblBodyErrorMsg").find("tr[id='rowError$$']");
    for (var i = 2; i < tMsg.length - 1; i++) {
        var garLib = $.trim(AlbJsSplitElem(tMsg[i], 0, '_'));
        var errLib = $.trim(AlbJsSplitElem(tMsg[i], 1, '_'));

        var newRow = customRow.clone();
        newRow.find("td[id='cellErrorGarantie$$']").attr('id', 'cellErrorGarantie' + (i - 1)).html(garLib);
        newRow.find("td[id='cellErrorMsg$$']").attr('title', errLib).attr('id', 'cellErrorMsg' + (i - 1)).html(errLib);
        newRow.attr('id', 'rowError' + (i - 1));
        $("#tblBodyErrorMsg").append(newRow);
    }
    $("tr[id='rowError$$']").remove();

    $("#divErrorAssietteMsg").show();
    CloseLoading();
    AlternanceLigne("BodyErrorMsg", "", false, null);

    $("#btnOKAssiette").click(function () {
        $("#tblBodyErrorMsg").html(customRow);
        $("#divErrorAssietteMsg").hide();
    });
}
//--------Récupère le libellé de la cible-----------
function GetLibCible() {
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/GetLibCible",
        data: { codeCible: $("#CodeCibleRsq").val() },
        success: function (data) {
            $("#CibleRisque").val(data);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function DeleteInventaireGarantieCache() {
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/DeleteInventaireGarantieCache",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(), modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            RedirectionFormuleGarantie("CreationFormuleGarantie", "Index", null, true);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function CalculMontantHT(codeObj) {
    var valObj = $("input[id='inValObj_" + codeObj + "']").autoNumeric("get");
    var unitObj = $("select[id='UnitePortee_" + codeObj + "']").val();
    var typeObj = $("select[id='TypePorteeCal_" + codeObj + "']").val();

    var mntCal = 0;

    if (valObj != 0 && unitObj != "" && typeObj != "") {
        if (typeObj == "M") {
            mntCal = valObj;
        }
        if (typeObj == "X") {
            switch (unitObj) {
                case "D":
                    mntCal = $("span[name='valObj_" + codeObj + "']").autoNumeric("get") * valObj;
                    break;
                case "%":
                    mntCal = $("span[name='valObj_" + codeObj + "']").autoNumeric("get") * valObj / 100;
                    break;
                case "%0":
                    mntCal = $("span[name='valObj_" + codeObj + "']").autoNumeric("get") * valObj / 1000;
                    break;
                default:
                    break;
            }
        }
        $("span[name='primeCalObj_" + codeObj + "']").autoNumeric("set", mntCal);
    }
    else {
        $("span[name='primeCalObj_" + codeObj + "']").html(0);
    }
    RecalculMntTotal();
}
function RecalculMntTotal() {
    var total = 0;
    var errCal = false;
    $("span[name^='primeCalObj_']").each(function () {
        var codeObj = $(this).attr("name").split("_")[1];
        //var mnt = $(this).autoNumeric("get");
        var mnt = $(this).html().replace(",", ".").replace(/ /g, '');
        var chkObj = $("input[type='checkbox'][name='chkObjPortee'][albcodeobjet='" + codeObj + "']").is(":checked");
        if (chkObj) {
            if (mnt == 0) {
                errCal = true;
            }
            else {
                total += mnt * 1;
            }
        }
    });
    if (!errCal) {
        $("span[name='mntTotal']").autoNumeric("set", total);
    }
    else {
        $("span[name='mntTotal']").html(0);
    }
    ReloadFormatDecimalValue();
}
//--------Reformate les input/span---------------
function ReloadFormatDecimalValue() {
    //common.autonumeric.applyAll('destroy', 'numeric');
    common.autonumeric.applyAll('update', 'numeric', null, null, null, '99999999999', null);
    //common.autonumeric.applyAll('destroy', 'pourmilledecimal');
    common.autonumeric.applyAll('update', 'pourmilledecimal', '');
    //common.autonumeric.applyAll('destroy', 'pourcentdecimal');
    common.autonumeric.applyAll('update', 'pourcentdecimal', '');
    //common.autonumeric.applyAll('destroy', 'decimal');
    common.autonumeric.applyAll('update', 'decimal', null, null, null, '9999999999999.99', null);
}


//--------Ouvre les enfants d'une garantie sortie-------
function OpenChildrenAvn(idGar) {
    var nameTdGar = $("input[albCodeGarantie='" + idGar + "']").attr("id");
    var nameTdGarReplace = nameTdGar.replace("bloc_", "").replace("garantie1_", "").replace("garantie2_", "").replace("garantie3_", "");
    $("tr[name$='" + nameTdGarReplace + "']").show();
}
//--------Ferme les enfants d'une garantie sortie-------
function CloseChildrenAvn(idGar) {
    var nameTdGar = $("input[albCodeGarantie='" + idGar + "']").attr("id");
    var nameTdGarReplace = nameTdGar.replace("bloc_", "").replace("garantie1_", "").replace("garantie2_", "").replace("garantie3_", "");
    $("tr[name$='" + nameTdGarReplace + "']").hide();
}


