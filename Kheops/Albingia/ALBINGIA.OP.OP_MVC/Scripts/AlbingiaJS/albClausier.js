$(document).ready(function () {
    Clausier();
    SubmitForm();
    MapPageElement();
});
//---------------------Affecte les fonctions au boutons-------------
function MapPageElement() {
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
    });
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
            case "ListValid":
                Sauvegarde();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    $("#btnSuivant").css("display", "inline");
    $("#btnSuivant").attr("disabled", "disabled");
    LoadDDLSousRubriquesByRubrique();
    LoadDDLSequenceByRubriqueAndSousRubrique();

    //gestion title des dropdown list
    $("#MotCle1").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotCle2").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotCle3").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#Sequence").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#Mode").die().live('change', function () {
        AffectTitleList($(this));
    });

    AlternanceLigne("Objets", "", false, null);
    BtnMultiObj();
    $("#btnListCancel").die();
    $("#btnListCancel").click(function () {
        $("#divLstObj").hide();
    });
    $("#btnListValid").die();
    $("#btnListValid").click(function () {
        if ($("input[name=checkRsq]").is(':checked')) {
            $("#IsRsqSelected").val("True");
            validListRsqObj();
            $("td.LabelObjetRisque").html("S'applique au risque");
            $("#divLstObj").hide();
            ReactivateShortCut();
        }
        else {
            var objChecked = $("input[name=checkObj]:checked").attr('id');
            if (typeof (objChecked) != "undefined" && objChecked != "") {
                $("#IsRsqSelected").val("False");
                validListRsqObj();
                $("td.LabelObjetRisque").html("S'applique à l'objet");
                $("#divLstObj").hide();
                ReactivateShortCut();
            }
            else {
                common.dialog.bigError("Vous devez sélectionner le risque ou un objet.", true);
            }
        }

    });
    $("input[name=checkRsq]").die().live('click', function () {
        selectListObj($(this).is(':checked'));
    });
    $("input[name=checkObj]").die().live('click', function () {
        if ($(this).is(':checked'))
            $("input[name=checkRsq]").attr('checked', false);
    });

    $("#ObjetRisqueCode").val("");

    $("#FullScreen").die().live('click', function () {
        RechercherClause(true);
    });

}

//--------------- Selectionne un risque ou objet dans la liste---------------
function selectListObj(checkbox) {
    if (checkbox) {
        $("input[name=checkObj]").attr('checked', false);
        //$("input[name=checkObj]").attr("disabled", "disabled");
    }
    //else {
    //    $("input[name=checkObj]").removeAttr("disabled");
    //}
}
//--------------- Coche les objets lors de l'ouverture ----------------
function checkListRsqObj() {
    var retour = false;
    $("input[name=checkObj]").removeAttr('checked');
    $("input[name=checkRsq]").removeAttr('checked');
    var tabObjet = $("#ObjetRisqueCode").val().split(';');
    if ($("#IsRsqSelected").val() != "True") {
        for (i = 0; i < tabObjet.length; i++) {
            $(".objRisque").hide();
            $("input[id=check_" + tabObjet[i] + "]").attr('checked', true);
            $("div[name=obj_" + tabObjet[i] + "]").show();
        }
        if (tabObjet == "")
            $("input[name=checkRsq]").attr('checked', true);

    }
    else {
        $("input[name=checkRsq]").attr('checked', true);
        $(".objRisque").show();
        retour = true;
    }

    if (tabObjet != "") {
        retour = true;
    }
    return retour;
}
//--------------- Valide la sélection des objets dans la liste ---------
function validListRsqObj() {
    var strListObjCodes = ";";
    var strListObjLibelle = ";";
    if ($("#IsRsqSelected").val() != "True") {
        $("input[name=checkObj]").each(function () {
            if ($(this).is(":checked")) {
                if (strListObjCodes.indexOf(";" + $(this).val().split('_')[0] + ";") < 0) {
                    strListObjCodes += $(this).val().split('_')[0] + ";";
                    strListObjLibelle += $(this).val().split('_')[0] + "-" + $(this).val().split('_')[1] + ";";
                }
            }
            else {
                strListObjCodes = strListObjCodes.replace(";" + $(this).val().split('_')[0] + ";", ";");
                strListObjLibelle = strListObjLibelle.replace(";" + $(this).val().split('_')[0] + "-" + $(this).val().split('_')[1] + ";", ";");
            }
        });
        strListObjCodes = strListObjCodes.substring(1).substring(0, strListObjCodes.length - 2);
        $("#ObjetRisqueCode").val(strListObjCodes);
        strListObjLibelle = strListObjLibelle.substring(1).substring(0, strListObjLibelle.length - 2);
    }
    else {
        strListObjLibelle = $("input[name=checkRsq]").val().split('_')[1];
        $("#ObjetRisqueCode").val("");
    }
    $("td[name=ObjetRisque]").html(strListObjLibelle);
}
//----------------Evenement liés au buton btnMultiObj-----
function BtnMultiObj() {
    $("#divBtnMultiObj").die();
    $("#divBtnMultiObj").click(function () {
        if (!window.isReadonly && $("#NbrObjets").val() > 1) {
            $("#divLstObj").show();
            checkListRsqObj();
        }
    });
    $("div[name=btnMultiObj]").bind('mouseover', function () {
        var position = $(this).offset();
        var top = position.top;
        if (checkListRsqObj() == true) {
            $("#divSelectedObjets").css({ 'position': 'absolute', 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
        }
    });
    $("div[name=btnMultiObj]").bind('mouseout', function () {
        $("#divSelectedObjets").hide();
    });

    $("div[name=divSelectedObjets]").bind('mouseover', function () {
        var position = $("#divBtnMultiObj").offset();
        var top = position.top;
        if (checkListRsqObj() == true) {
            $("#divSelectedObjets").css({ 'position': 'absolute', 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
        }
    });
    $("div[name=divSelectedObjets]").bind('mouseout', function () {
        $("#divSelectedObjets").hide();
    });
}
//----------------------Application de la recherche ajax des SousRubriques par Rubrique----------------------
function LoadDDLSousRubriquesByRubrique() {
    $("#Rubrique").live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSousRubriques",
            data: { codeRubrique: $(this).val() },
            success: function (data) {
                $("#SousRubriqueDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//----------------------Application de la recherche ajax des Sequences par SousRubrique----------------------
function LoadDDLSequenceByRubriqueAndSousRubrique() {
    $("#SousRubrique").live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSequences",
            data: { codeSousRubrique: $(this).val(), codeRubrique: $("#Rubrique").val() },
            success: function (data) {
                $("#SequenceDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var provenance = $("#Provenance").val().replace(/\//g, "¤").replace(/_/g, "£");
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/Clausier/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, provenance: provenance, modeNavig: $("#ModeNavig").val(), addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Clausier() {
    $("#btnRechercherClausier").kclick(function () {
        RechercherClause(false);
    });

}

function RechercherClause(pleinEcran)
{
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Clausier/Recherche",
        data: {
            libelle: $("#Libelle").val(),
            motCle1: $("#MotCle1").val(),
            motCle2: $("#MotCle2").val(),
            motCle3: $("#MotCle3").val(),
            sequence: $("#Sequence").val(),
            rubrique: $("#Rubrique").val(),
            sousrubrique: $("#SousRubrique").val(),
            selectionPossible: $("#SelectionPossible").val(),
            modaliteAffichage: $("#ModaliteAffichage").val(),
            date: $("#Date").val()
        },
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (dataReturn) {
            CloseLoading();
            if (pleinEcran) {
                $("#divDataFullScreen").html(dataReturn);
                $("#dvRechercheGeneral").html('');
                MapElementPleinEcran();
                $("#divFullScreen").show();
                $("#bandeauPleinEcran").show();
            }
            else {
                $("#divFullScreen").hide();
                $("#divDataFullScreen").html('');
                $("#bandeauPleinEcran").hide();
                $("#divBodyClausier").removeClass("fullscreenHeight");
                $("#dvRechercheGeneral").show();
                $("#dvRechercheGeneral").html(dataReturn);
                $("#btnSuivant").css("display", "inline");
                $("#btnSuivant").attr("disabled", "disabled");
            }

            AlternanceLigne("BodyClausier", "", false, null);
            InitTable();
            ViualisationClause();
            ChoixClause();
            MapSorts();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementPleinEcran()
{
    $("#divBodyClausier").addClass("fullscreenHeight");

    $("#btnSuivantPleinEcran").click(function () {
        if ($("#Contexte").val() == "") {
            $("#Contexte").addClass('requiredField');
            return false;
        }
        if ($("#IsRsqSelected").val() == "True")
            ShowCommonFancy("Confirm", "ListValid", "Êtes-vous sûr de vouloir associer cette clause à l'ensemble du risque ?", 450, 80, true, true);
        else
            Sauvegarde();
    });

    $("#dvLinkClose").die().live("click", function ()
    {
        RechercherClause(false);
    });
}

//------ affiche le bouton valider lors du choix d'une clause et filtre la liste des contextes
function ChoixClause() {
    $(".ChkClause").die().live('click', function () {
        var clause = $(this).val().split('_');
        $("#CodeClause").val(clause[0]);
        $("#RubriqueClause").val(clause[1]);
        $("#SousRubriqueClause").val(clause[2]);
        $("#SequenceClause").val(clause[3]);
        $("#VersionClause").val(clause[4]);
        var date = $("#Date").val();
        var dateDeb = clause[5];
        var dateFin = clause[6];
        var rubrique = clause[7];
        var sousrubrique = clause[8];
        var sequence = clause[9];
        var code = clause[0];
        var libelle = clause[11];
        var versionClause = clause[4];

        if (dateDeb != 0 && (dateDeb > date || (dateFin < date && dateFin != 0))) {
            if (versionClause == 1) {
                ShowCommonFancy("Error", "", "Cette clause n’est pas valide au " + $("#DateFormate").val(), 1212, 700, true, true);
            }
            else Historique("Histo_" + rubrique + "_" + sousrubrique + "_" + sequence + "_" + code + "_" + libelle, "ClauseInvalide");
            $("#btnSuivant").attr("disabled", "disabled");
            $("#btnSuivantPleinEcran").attr("disabled", "disabled");
        }
        else {
            $("#btnSuivant").removeAttr("disabled");
            $("#btnSuivantPleinEcran").removeAttr("disabled");
        }
    });
}
//-------------------- Permet de dynamiser la derniere entete du tableau suivant la scroll verticale.
function InitTable() {

}
//---------------------- Reset de la page.
function CancelForm() {
    Redirection("ChoixClauses", "Index");
}


function StartHisto() {
    AlternanceLigne("Histo", "", false, null);
    $("#btnFermer").live("click", function () {
        $('#InformationSaisie_InformationCible_Cible').val(filter.Cible);
        $("#fancybox-close").trigger("click");
    });
    if ($("#tblHisto").css("height") < $("#divHisto").css("height")) {
        $("#DinamyqueCol").css("width", 60).children().css("width", 60);
        $(".colHisto1").css("width", 73).children().css("width", 73);
    }
}
//----------------------- Submit la form
function SubmitForm() {
    $("#btnSuivant").kclick(function () {
        if ($("#Contexte").val() == "") {
            $("#Contexte").addClass('requiredField');
            return false;
        }
        if ($("#IsRsqSelected").val() == "True")
            ShowCommonFancy("Confirm", "ListValid", "Êtes-vous sûr de vouloir associer cette clause à l'ensemble du risque ?", 450, 80, true, true);
        else
            Sauvegarde();
    });
}

function Sauvegarde() {
    ShowLoading();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/Clausier/Valider",
        data: {
            type: $("#Offre_Type").val(), numeroOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), tabGuid: $("#tabGuid").val(), natureClause: "P",
            codeClause: $("#CodeClause").val(), rubrique: $("#RubriqueClause").val(), sousRubrique: $("#SousRubriqueClause").val(), sequence: $("#SequenceClause").val(), versionClause: $("#VersionClause").val(), actionEnchaine: "", contexte: $("#ContexteCible").val(),
            provenance: $("#Provenance").val(), saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), etape: $("#Etape").val(),
            codeRsq: $("#CodeRsq").val(), codeFor: $("#CodeFor").val(), codeOption: $("#CodeOption").val(),
            codeObj: $("#ObjetRisqueCode").val(), modeNavig: $("#ModeNavig").val(), addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) {
        },
        error: function (request) {
            $("input[name=rbt]").attr('checked', false);
            common.error.showXhr(request);

        }
    });
}
function SauvegardeInvalide() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Clausier/Valider",
        data: {
            type: $("#Offre_Type").val(), numeroOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), tabGuid: $("#tabGuid").val(), natureClause: "P",
            codeClause: $("#CodeClauseSelected").val(), rubrique: $("#RubriqueClause").val(), sousRubrique: $("#SousRubriqueClause").val(), sequence: $("#SequenceClause").val(), versionClause: $("#VersionClauseSelected").val(), actionEnchaine: "", contexte: $('#ContexteInvalide :selected').val(),
            provenance: $("#Provenance").val(), saveCancel: $("#txtSaveCancel").val(), etape: $("#Etape").val(),
            codeRsq: $("#CodeRsq").val(), codeFor: $("#CodeFor").val(), codeOption: $("#CodeOption").val()
        },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
