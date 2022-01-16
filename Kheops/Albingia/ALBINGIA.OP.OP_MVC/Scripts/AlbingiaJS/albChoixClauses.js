$(document).ready(function () {
    Provenance();
    MapPageElement();
    Navigation();

});
//---------------------Affecte les fonctions au boutons-------------
function MapPageElement() {
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly || $("#IsModifHorsAvn").val() == "True") {
        $("#Contexte").enable();
        $("#Filtre").enable();
        $("#divAjoutContexte").hide();
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    $("#btnSuivant").kclick(function (evt, data) {
        Suivant(data && data.returnHome);
    });
    $("#btnErrorOk").kclick(function () {
        CloseCommonFancy();
    });
    $("#btnAnnuler").kclick(function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 100, true, true, true);
    });
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
            case "ListValid":
                EnregistrerClauseLibre();
                break;
        }
        $("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").clear();
    });
    $("#btnFermerChoixClause").kclick(function () {
        $("#divFullScreenDetailsClause").hide();
    });
    $("#btnAnnulerClauseLibre").kclick(function () {
        ReactivateShortCut();
        $("#divEditClauseLibre").clearHtml();
        $("#divFullScreenClauseLibre").hide();
    });
    $("#btnFermerVisualisation").kclick(function () {
        $("#divVisualiserClause").clearHtml();
        $("#divFullScreenVisualiserClause").hide();
        ReactivateShortCut();
    });
    $("#btnModifierTexte").kclick(function () {
        $("#TitreClauseLibre").removeClass("requiredField");

        if (!$("#TitreClauseLibre").hasVal()) {
            $("#TitreClauseLibre").addClass("requiredField");
        }
        if (!$("#TitreClauseLibre").hasVal()) {
            return;
        }
        ModifierTexteClauseLibre();
    });
    $("#btnValiderClauseLibre").kclick(function () {
        $("#inLibelleClauseLibre").removeClass("requiredField");

        if (!$("#inLibelleClauseLibre").hasVal()) {
            $("#inLibelleClauseLibre").addClass("requiredField");
        }
        if (!$("#inLibelleClauseLibre").hasVal()) {
            return;
        }
        if ($("#IsRsqSelected").hasTrueVal())
            ShowCommonFancy("Confirm", "ListValid", "Êtes-vous sûr de vouloir associer cette clause à l'ensemble du risque ?", 450, 80, true, true);
        else
            EnregistrerClauseLibre();
    });
    $("#FullScreen").kclick(function () {
        OpenCloseFullScreen(true);
    });
    $("th[name=headerTri]").kclick(function () {

        var colTri = $(this).attr("albcontext");
        var transverse = $(this).hasClass('transverse');
        if (transverse) { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".transverseImageTri").attr("src").lastIndexOf('/') + 1); }
        else { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1); }

        img = img.substr(0, img.lastIndexOf('.'));
        FilterAndRefresh(colTri, img);
    });
    MapElementLstClause();

    contextMenu.createContextMenuClause();

    $("#btnCtxtMenuClause").kclick(function (e) {
        e.preventDefault();
        var pos = $("#btnCtxtMenuClause").offset();
        $("#btnCtxtMenuClause").contextMenu({ x: pos.left + $("#btnCtxtMenuClause").width() - 168, y: pos.top });
    });

}
//---------Ajout d'une clause à partir du clausier-----
function AddClause() {
    $(".requiredField").removeClass("requiredField");
    if (!$("#ContexteCible").hasVal()) {
        $("#ContexteCible").addClass('requiredField');
        return;
    }
    if (!$(this).attr('disabled')) {
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/VerifAjout",
            data: { etape: $("#Etape").val(), contexte: $("#ContexteCible").val(), typeAjt: "clausier" },
            success: function (data) {
                if (data != "") {
                    $("#ContexteCible").removeClass('requiredField');
                    var prov = $("#Provenance").val().replace(/\//g, "¤").replace(/_/g, "£");
                    ShowLoading();
                    Redirection("Clausier", "Index", prov, $("#ContexteCible").val());
                }
                else {
                    common.dialog.error("Impossible d'ajouter une clause à partir du clausier pour ce contexte.");
                }
            },
            error: function (request) {
                CloseLoading();
                common.error.showXhr(request);
            }
        });
    }
}
//--------Ajout d'une clause libre---------
function AddClauseLibre() {
    $(".requiredField").removeClass("requiredField");
    if (!$("#ContexteCible").hasVal()) {
        $("#ContexteCible").addClass('requiredField');
        return;
    }
    if (!$(this).attr('disabled')) {
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/VerifAjout",
            data: { etape: $("#Etape").val(), contexte: $("#ContexteCible").val(), typeAjt: "libre" },
            success: function (data) {
                if (data != "ERROR") {
                    //$("#ContexteCible").removeClass('requiredField');
                    //AfficherEcranClauseLibre($("#ContexteCible").val());
                    var splitHtmlChar = $("#SplitHtmlChar").val();
                    dialogClauseVisualisation(0, "True", "C" + splitHtmlChar + data);
                    $("#divEditClauseLibre").clearHtml();
                    $("#divFullScreenClauseLibre").hide();
                }
                else {
                    common.dialog.error("Impossible d'ajouter une clause libre pour ce contexte.");
                }
            },
            error: function (request) {
                CloseLoading();
                common.error.showXhr(request);
            }
        });
    }
}
//--------Ajout d'une pièce jointe dans les clauses--------
function AddPieceJointe() {
    $(".requiredField").removeClass("requiredField");
    if (!$("#ContexteCible").hasVal()) {
        $("#ContexteCible").addClass('requiredField');
        return;
    }
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/VerifAjout",
        data: { etape: $("#Etape").val(), contexte: $("#ContexteCible").val(), typeAjt: "piecesjointes" },
        success: function (data) {
            if (data != "ERROR") {
                LoadListPiecesJointes(data);
            }
            else {
                common.dialog.error("Impossible d'ajouter une pièce jointe pour ce contexte.");
            }
        },
        error: function (request) {
            CloseLoading();
            common.error.showXhr(request);
        }
    });
}
//---------Charge la div pieces jointes-----------
function LoadListPiecesJointes(param) {
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/LoadPiecesJointes",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeRisque: $("#CodeRisque").val(),
            etape: $("#Etape").val(), contexte: $("#ContexteCible").val(), param: param
        },
        success: function (data) {
            $("#divPieceJointesClauses").show();
            $("#divDataPieceJointesClauses").html(data);
            $("#divDataPieceJointesClauses").height(405);
            if ($("#Etape").val() == "RSQ") {
                $("#divAppliquePieceJointe").show();
            }
            else {
                $("#divDataPieceJointesClauses").height(377);
            }
            MapPiecesJointesElement();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------Map les éléments de la div des pièces jointes--------
function MapPiecesJointesElement() {
    $("#btnCancelPiecesJointes").kclick(function () {
        $("#divPieceJointesClauses").hide();
        $("#divDataPieceJointesClauses").clearHtml();
    });
    $("#btnValidPiecesJointes").kclick(function () {
        SavePiecesJointes();
    });
    AlternanceLigne("BodyListPiecesJointes", "", false, null);
    $("#btnPiecesJointesApplique").kclick(function () {
        OpenPiecesJointesApplique($(this));
    });
}

function Suivant(returnHome) {
    ShowLoading();
    var dataInfo = SerialiserInfoGlobales(returnHome);
    var dataIntermediaire = SerialiserIntermediaire();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/Suivant",
        data: {
            dataInfo: dataInfo, dataIntermediaire: dataIntermediaire,
            perimetre: $("#Etapes").val(), risque: $("#CodeRisque").val(), objet: 0,
            modeNavig: $("#ModeNavig").val(), isModeConsultationEcran: $("#IsModeConsultationEcran").val(), isForceReadOnly: $("#IsForceReadOnly").val()
        },
        error: function (request) {
            CloseLoading();
            common.error.showXhr(request);
        }
    });

}
//------------Map les éléments de la page---------
function MapElementLstClause() {

    AffectTitleList($("#Contexte"));
    AffectTitleList($("#Etapes"));
    AffectTitleList($("#Filtre"));
    AffectTitleList($("#ContexteCible"));

    AlternanceLigne("TableFichier", "", false, null);

    $("#dvLinkClose").kclick(function () {
        OpenCloseFullScreen(false);
    });

    $("#Contexte").offOn("change", function () {
        AffectTitleList($("#Contexte"));
        var filtre = $(this).val();
        $(".trFichier").contents().filter(function () {
            return true;
        }).closest('.trFichier').show();
        if (filtre != "Tous") {
            $(".trFichier").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtre;
            }).closest('.trFichier').hide();
        }
    });

    $("#Etapes").offOn("change", function () {
        ShowLoading();
        FilterAndRefresh().done(CloseLoading);

        $(".trFichier").contents().filter(function () {
            return true;
        }).closest('.trFichier').show();
        if (filtre != "Tous") {
            $(".trFichier").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtre;
            }).closest('.trFichier').hide();
        }
    });
    AffectTitleList($(this));
    $("#Filtre").offOn("change", function () {
        ShowLoading();
        FilterAndRefresh().done(CloseLoading);
    });

    $('img[name=suppression]')
        .css("cursor", "pointer")
        .kclick(function () {
            ShowLoading();
            const tri = getCurrentSort();

            const data = serializeContext();
            $.ajax({
                type: "POST",
                url: "/ChoixClauses/Supprime",
                context: $(this),
                data: $.extend(data, { id: $(this).attr("id").substring(4) }, tri)
            }).then(
                function (data) {
                    if ($("#modeLstClause").val() == "True")
                        $("#divDataFullScreen").html(data);
                    else
                        $("#divChoixClause").html(data);
                    MapElementLstClause();
                    CloseLoading();
                }).fail(common.error.showXhr);
        });

    $("#btnFSSuivant").kclick(function () {
        $("#btnSuivant").click();
    });

    $("input[type=checkbox][id^=IsCheck_]").kclick(function () {
        UpdateCheckClause($(this));
    });
    $("td[name=linkDetail]").kclick(function () {
        dialogClauseDetail($(this).attr("AlbLinkId"));//.split("_")[1]);
    });
    $("div[name=linkVisu][albContext=clause]").kclick(function () {
        ShowLoading();
        var origine = $(this).attr("AlbOrigine");
        dialogClauseVisualisation($(this).attr("AlbLinkId").split("_")[1], $(this).attr("AlbIsLibre"), "", origine);
        $("#divEditClauseLibre").clearHtml();
        $("#divFullScreenClauseLibre").hide();
    });

    //Ajout de l'ouverture des documents joints dans l'ancien plugin
    $("div[name=linkVisu][albcontext=docJoint]").kclick(function () {
        ShowLoading();
        var pathFile = $(this).attr("albdocfullpath");
        OpenDirectWordDocument(pathFile);
        CloseLoading();
    });
    if (window.isReadonly) {
        $("#divAjoutContexte").hide();
        $("#Contexte").removeAttr('disabled');
        $("#Filtre").removeAttr('disabled');
    }
    $("#ContexteCible").offOn("change", function () {
        AffectTitleList($(this));
    });

    $("label[albCFList]").each(function () {
        AffectTitleList($(this));
    });

    if ($("#IsModifHorsAvn").val() == "True") {
        $("#divAjoutContexte").hide();
}
}

function FilterAndRefresh(coltri, imgtri) {
    const data = serializeContext();
    let tri = { colTri: coltri, imgTri: imgtri };
    if (!coltri) {
        tri = getCurrentSort();
    }
    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/Filtrer",
        context: $("#Filtre"),
        data: $.extend(data, tri)
    }).then(function (data) {
        if ($("#modeLstClause").val() == "True")
            $("#divDataFullScreen").html(data);
        else
            $("#divChoixClause").html(data);
        MiseAJourImagesTri(tri.colTri, tri.imgTri);
        MapElementLstClause();

        $(".trFichier").contents().closest('.trFichier').show();

        AffectTitleList($("#Filtre"));
    }).fail(function (request) {
        common.error.showXhr(request);
    });
}




//----------Ouvre le full screen-----------
function OpenCloseFullScreen(open) {
    let filtre = $("#Filtre").val();
    let contexte = $("#Contexte").val();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/LoadLstClause",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            etape: $("#Etapes").val(),
            provenance: $("#Provenance").val(),
            tabGuid: $("#tabGuid").val(),
            codeRisque: $("#CodeRisque").val(),
            codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(),
            filtre: filtre,
            contexte: contexte,
            modeNavig: $("#ModeNavig").val(),
            fullScreen: open
        },
        success: function (data) {
            if (open) {
                DesactivateShortCut();
                $("#divChoixClause").clearHtml();
                $("#divFullScreen").show();
                $("#divDataFullScreen").html(data);
            }
            else {
                ReactivateShortCut();
                $("#divChoixClause").html(data);
                $("#divFullScreen").hide();
                $("#divDataFullScreen").clearHtml();
            }
            MapElementLstClause();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Redirection------------------
function Redirection(cible, job, provenance, contexte) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var etape = $("#Etapes").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/ChoixClauses/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, etape: etape, provenance: provenance, contexte: contexte, modeNavig: $("#ModeNavig").val(), addParamType: addParamType, addParamValue: addParamValue, isForceReadOnly: $("#IsForceReadOnly").val() },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------------- Reset de la page.
function CancelForm() {
    Redirection("", "", $("#Provenance").val(), "");
}

//SAB24042016: Pagination clause

function dialogClauseDetail(id) {
    var typeOffreContrat = $("#Offre_Type").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var codeAvn = $("#NumAvenantPage").val();
    var etape = $("#Etapes").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/ObtenirClauseDetails",
        data: {
            idClause: id,
            codeOffreContrat: codeOffre,
            versionOffreContrat: version,
            typeOffreContrat: typeOffreContrat,
            codeAvn: codeAvn,
            etape: etape,
            tabGuid: $("#tabGuid").val(),
            codeRisque: $("#CodeRisque").val(),
            codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDetailsClause").html(data);
            AlbScrollTop();
            $("#divFullScreenDetailsClause").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementAppliqueA() {
    BtnMultiObj();
    $("#btnListCancel").kclick(function () {
        $("#divLstObj").hide();
    });
    $("#btnListValid").kclick(function () {
        if ($("input[name=checkRsq]").isChecked()) {
            $("#IsRsqSelected").val("True");
            validListRsqObj();
            $("#LabelObjetRisque").html("S'applique au risque");
            $("#divLstObj").hide();
            ReactivateShortCut();
        }
        else {
            var objChecked = $("input[name=checkObj]:checked").attr('id');
            if (typeof (objChecked) != "undefined" && objChecked != "") {
                $("#IsRsqSelected").val("False");
                validListRsqObj();
                $("#LabelObjetRisque").html("S'applique à l'objet");
                $("#divLstObj").hide();
                ReactivateShortCut();
            }
            else {
                common.dialog.bigError("Vous devez sélectionner le risque ou un objet.", true);
            }
        }
    });
    $("input[name=checkRsq]").kclick(function () {
        selectListObj($(this).isChecked());
    });
    $("input[name=checkObj]").kclick(function () {
        if ($(this).isChecked()) {
            $("input[name=checkRsq]").uncheck();
        }
    });
}
//-------------------- Gere l'affichage des infos suivant la provenance.
function Provenance() {
    if (!$("#Provenance").hasVal()) { return false; }
    var prov = $("#Provenance").val().substring(1, $("#Provenance").val().substring(1).indexOf('/') + 1);
    switch (prov) {
        case "InformationsSpecifiquesRisques":
            $("#Risque").show();
            $("#Objet").hide();
            $("#Garantie").hide();
            $("#Condition").hide();
            break;
        case "InformationsSpecifiquesGarantie":
            $("#Risque").hide();
            $("#Objet").hide();
            $("#Garantie").show();
            $("#Condition").hide();
            break;
        case "ConditionsGarantie":
            $("#Risque").hide();
            $("#Objet").hide();
            $("#Garantie").show();
            $("#Condition").show();
            break;
        default:
            $("#Risque").hide();
            $("#Objet").hide();
            $("#Garantie").hide();
            $("#Condition").hide();
            break;
    }
}

//-------------------- Page suivante si PB --------------------
function Navigation() {
    if (common.albParam.getValue("REGULMOD") == "PB" || common.albParam.getValue("REGULMOD") == "BNS") {
        $("#btnSuivant").trigger("click");
    }
}

//----------------------Formate le controle cleditor de la clause libre---------------------
function FormatCLEditor() {
    $("#ClauseLibreEdit").cleditor({ width: 1000, height: 265, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}
function FormatCLEditorVisualisation() {
    $("#TexteClauseLibreVisualisation").cleditor({ width: 1009, height: 325, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}

//SAB24042016: Pagination clause
//-------------Fonction qui enregistre une clause libre en base
function EnregistrerClauseLibre() {
    var contexte = $("#ContexteCible").val();
    var libelle = $("#inLibelleClauseLibre").val();
    if (libelle == "") {
        $("#inLibelleClauseLibre").addClass("requiredField");
    }
    else {
        $("#inLibelleClauseLibre").removeClass("requiredField");
    }

    if ((contexte == "") || (libelle == "")) {
        return false;
    }

    var ClauseLibreText = $.trim($("#ClauseLibreEdit").val());
    ClauseLibreText = encodeURIComponent(ClauseLibreText);

    $.ajax({
        type: "POST",
        url: "/ChoixClauses/EnregistrerClauseLibre",
        data: $.extend(serializeContext(), {
            contexte: contexte,
            libelle: libelle,
            texte: ClauseLibreText,
        }),
        success: function (data) {
            if ($("#modeLstClause").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divChoixClause").html(data);

            MapElementLstClause();
            $("#divEditClauseLibre").clearHtml();
            $("#divFullScreenClauseLibre").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------------Affiche la pop-up d'édition de choix clause------------------
function AfficherEcranClauseLibre(contexte) {
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/AfficherEcranClauseLibre",
        data: {
            codeOffreContrat: $("#Offre_CodeOffre").val(),
            versionOffreContrat: $("#Offre_Version").val(),
            typeOffreContrat: $("#Offre_Type").val(),
            codeRisque: $("#CodeRisque").val(),
            provenance: $("#Provenance").val(),
            etape: $("#Etapes").val(),
            contexte: contexte
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divEditClauseLibre").html(data);
            FormatCLEditor();
            AlbScrollTop();
            $("#divFullScreenClauseLibre").show();

            $("#divVisualiserClause").clear();
            $("#divFullScreenVisualiserClause").hide();

            MapElementAppliqueA();
            $("#ObjetRisqueCode").clear();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------- Valide la sélection des objets dans la liste ---------
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
        strListObjLibelle = $("input[name=checkRsq]").val().split('_')[0] + " - " + $("input[name=checkRsq]").val().split('_')[1];
        $("#ObjetRisqueCode").clear();
    }
    $("#ObjetRisque").val(strListObjLibelle);
}
//--------------- Selectionne un risque ou objet dans la liste---------------
function selectListObj(checkbox) {
    if (checkbox) {
        $("input[name=checkObj]").uncheck();
    }
}
//--------------- Coche les objets lors de l'ouverture ----------------
function checkListRsqObj() {
    var retour = false;
    $("input[name=checkObj]").removeAttr('checked');
    $("input[name=checkRsq]").removeAttr('checked');
    var tabObjet = $("#ObjetRisqueCode").val().split(';');
    if ($("#IsRsqSelected").val() != "True") {
        for (var i = 0; i < tabObjet.length; i++) {
            $(".objRisque").hide();
            $("input[id=check_" + tabObjet[i] + "]").check();
            $("div[name=obj_" + tabObjet[i] + "]").show();
        }
        if (tabObjet == "")
            $("input[name=checkRsq]").check();

    }
    else {
        $("input[name=checkRsq]").check();
        $(".objRisque").show();
        retour = true;
    }

    if (tabObjet != "") {
        retour = true;
    }
    return retour;
}
//----------------Evenement liés au buton btnMultiObj-----
function BtnMultiObj() {
    $("#divBtnMultiObj").kclick(function () {
        if (!window.isReadonly && $("#NbrObjets").val() > 1) {
            $("#divLstObj").show();
            checkListRsqObj();
            AlternanceLigne("Objets", "", false, null);
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
//---------------------
function UpdateCheckClause(chkbox) {
    var isCheck = chkbox.isChecked();
    var chkId = chkbox.attr('id').split("_")[1];
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/UpdateCheckBox",
        data: {
            clauseId: chkId, isChecked: isCheck
        },
        success: function (data) {
            if (data == "O") {
                $("#imgVoyant_" + chkId).attr("src", "/Content/Images/voyant_rouge_petit.png");
            }
            else {
                $("#imgVoyant_" + chkId).attr("src", "/Content/Images/voyant_blank_petit.png");
            }
            if ($("#NumAvenantPage").val() != "0") {
                var ischekorigine = $("#IsCheckOrigine_" + chkId).val();
                var numInterne = $("#NumAvnHisto_" + chkId).val();
                if (numInterne == "") {
                    numInterne = "0";
                }
                if (isCheck) {
                    if (ischekorigine == "True") {
                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textRouge");
                        $("#lblNumAvn_" + chkId).html(numInterne).addClass("textnoir");
                    }
                    else {
                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textInvisible");
                        $("#lblNumAvn_" + chkId).html($("#NumAvenantPage").val()).addClass("textRouge");
                    }
                }
                else {
                    if (ischekorigine == "True") {
                        // Ajout la classe numInterneInvisible
                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textRouge");
                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textnoir");
                        $("#lblNumAvn_" + chkId).html($("#NumAvenantPage").val()).addClass("textInvisible");
                    }
                    else {

                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textRouge");
                        $("#lblNumAvn_" + chkId).clearHtml().removeClass("textnoir");
                        $("#lblNumAvn_" + chkId).html(numInterne).addClass("textInvisible");
                    }


                }
            }

            CloseLoading();
        },
        error: function (request) {
            CloseLoading();
            common.error.showXhr(request);
        }
    });
}

//---------------------Construit la chaine sérialisée des clauses choisies--------------
function SerialiserClauses() {
    var values = Array.from($("#tblTableFichier tr")).map(function (x) {
        let idLigne = x.id;
        return {
            "Id": idLigne + '",',
            "EtatTitre": $("#EtatTitre_" + idLigne).val(),
            "IsCheck": $("#IsCheck_" + idLigne).val(),
            "IsCheckOrigine": $("#IsCheckOrigine_" + idLigne).val()
        };
    });
    return JSON.stringify(values);
}

function SerialiserInfoGlobales(returnHome) {
    var cibleCode = $("#ContratCible").val();
    if ($.trim(cibleCode.split(" - ")[0]) != "")
        cibleCode = $.trim(cibleCode.split(" - ")[0]);
    let val = {
        "txtSaveCancel": returnHome ? 1 : 0,
        "txtParamRedirect": $("#txtParamRedirect").val(),
        "CodeOffre": $("#Offre_CodeOffre").val(),
        "Version": $("#Offre_Version").val(),
        "TabGuid": $("#tabGuid").val(),
        "Type": $("#Offre_Type").val(),
        "AddParamType": $("#AddParamType").val(),
        "AddParamValue": $("#AddParamValue").val(),
        "RisqueObj": $("#RisqueObj").val(),
        "Provenance": $("#Provenance").val(),
        "CodeFormule": $("#CodeFormule").val(),
        "CodeOption": $("#CodeOption").val(),
        "ContratIdentification": $("#ContratIdentification").val(),
        "ContratCible": cibleCode,
        "HasRisques": $("#HasRisques").val(),
        "ModeAvt": $("#ModeAvt").val(),
        "TypeAvt": $("#TypeAvt").val()
    };
    return JSON.stringify(val);
}

function SerialiserIntermediaire() {
    return JSON.stringify({ "Etape": $("#Etape").val() });

}
//SAB24042016: Pagination clause
//------- Modifier le texte de la clause libre en visualisation-----
function ModifierTexteClauseLibre() {
    var clauseLibreText = $("#TexteClauseLibreVisualisation").val();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/UpdateTextClauseLibre",
        data: $.extend(serializeContext(), {
            clauseId: $("#ClauseId").val(),
            titre: $("#TitreClauseLibre").val(),
            texte: clauseLibreText,
        }),
        success: function (data) {
            if ($("#modeLstClause").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divChoixClause").html(data);

            MapElementLstClause();
            $("#divVisualiserClause").clearHtml();
            $("#divFullScreenVisualiserClause").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function getCurrentSort() {
    const col = $("img.imgTri, img.imageTri").filter("[src*=_]").attr('albcontext');
    const img = $("img.imgTri, img.imageTri").filter("[src*=_]").attr('src');
    const tri = (img || "").replace(/^.*(asc|desc).*$/, '$1');
    const oppositeTri = "tri_" + (tri == "desc" ? "asc" : "desc");
    return { colTri: col, imgTri: oppositeTri };
}

function MiseAJourImagesTri(colTri, img) {
    const base = "/Content/Images/tri";
    const ext = ".png";
    const asc = "asc";
    const desc = "desc";
    let tri = (img == "tri_asc") ? desc : asc;
    if (colTri == "Contexte") { tri = "asc"; }
    $("img.imgTri, img.imageTri").attr("src", base + ext);
    $("img[albcontext=" + colTri + "]").attr("src", base + "_" + tri + ext);
}

//-----------Sauvegarde la clause ouverte-----------
function SaveCurrentClause() {
    let boolError = false;
    const typeClause = $("#wvClauseType").val();
    $(".requiredField").removeClass("requiredField");

    if (!$("#wvTitreClause").hasClass("readonly") && $("#wvTitreClause").val() == "") {
        $("#wvTitreClause").addClass("requiredField");
        boolError = true;
    }
    if (!$("#wvContexte").hasClass("readonly") && $("#wvContexte").val() == "") {
        $("#wvContexte").addClass("requiredField");
        boolError = true;
    }
    if (!$("#wvEmplacement").hasClass("readonly") && $("#wvEmplacement").val() == "") {
        $("#wvEmplacement").addClass("requiredField");
        boolError = true;
    }
    if (!$("#wvOrdre").hasClass("readonly") && $("#wvOrdre").val() == "") {
        $("#wvOrdre").addClass("requiredField");
        boolError = true;
    }

    if (boolError) {
        $("#btnValidWordCla").removeAttr("disabled");
        return $.when(false);
    }
    else {
        if (typeClause == "Libre") {
            if ($("#wvCreateClause").val() == "C") {
                return SaveClauseLibre();
            }
            else {
                return SaveClauseMagnetic();
            }
        }
        else if (typeClause == "Ajoutée") {
            return SaveClauseEntete();
        }
        else {
            $("#divWordContainer").hide();
        }
    }
    $.when(true);
}
//-----------Sauvegarde l'entete de la clause seulement si la modif est activée et autorisée
function SaveClauseEntete() {
    if (!$("#wvEmplacement").is(":disabled") && !$("#wvSousEmplacement").is(":disabled") && !$("#wvOrdre").is(":disabled")) {
        const emplacement = $("#wvEmplacement").val();
        const sousemplacement = $("#wvSousEmplacement").val();
        const ordre = $("#wvOrdre").val().replace(",", ".");
        const idClause = $("#wvClauseId").val();

        if (idClause != undefined && idClause != "" && idClause > 0) {
            return $.ajax({
                type: "POST",
                url: "/ChoixClauses/SaveClauseEntete",
                data: { idClause: idClause, emplacement: emplacement, sousemplacement: sousemplacement, ordre: ordre },
            }).then(FilterAndRefresh)
                .fail(common.error.showXhr)
                .always(function () {
                    $("#divWordContainer").hide();
                });
        }
    }
    else {
        $("#divWordContainer").hide();
    }
    $.when(false);
}
//----------Sauvegarde document Magnetic--------
function SaveDocumentMagnetic() {
    var codeAffaire = $("#Offre_CodeOffre").val();
    var version = $("Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRsq = $("#CodeRisque").val() != "" && $("#CodeRisque").val() != undefined ? $("#CodeRisque").val() : "0";
    var codeObj = "0";
    var codeFor = $("#CodeFormule").val() != "" && $("#CodeFormule").val() != undefined ? $("#CodeFormule").val() : "0";
    var codeOpt = $("#CodeOption").val() != "" && $("#CodeOption").val() != undefined ? $("#CodeOption").val() : "0";
    var typeDocument = $("#wvTypeDocument").val();
    var emplacement = "";
    if ($("#wvEmplacement").is(":disabled")) {
        $("#wvEmplacement").removeAttr("disabled");
        emplacement = $("#wvEmplacement").val();
        $("#wvEmplacement").attr("disabled", "disabled");
    }
    else {
        emplacement = $("#wvEmplacement").val();
    }
    var sousemplacement = $("#wvSousEmplacement").val();
    var ordre = $("#wvOrdre").val().replace(",", ".");

    var idClause = $("#wvClauseId").val();

    var clauseName = $("#wvTitreClause").val();
    var pathName = $("#wvFileName").val();

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/GetPathOfferClauseLibreMagnetic",
        data: {
            codeAffaire: codeAffaire, version: version, type: type, idClause: idClause, emplacement: emplacement, sousemplacement: sousemplacement, ordre: ordre
        },
        async: false,
        success: function (data) {
            SaveClauseMagnetic();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------Sauvegarde Magnetic------
function SaveClauseMagnetic() {
    $("#Etape").removeAttr("disabled");
    var etape = $("#Etape").val();
    $("#Etape").attr("disabled", "disabled");
    var idClause = $("#wvClauseId").val();

    if ($("#wvEmplacement").is(":disabled")) {
        $("#wvEmplacement").removeAttr("disabled");
        emplacement = $("#wvEmplacement").val();
        $("#wvEmplacement").attr("disabled", "disabled");
    }
    else {
        emplacement = $("#wvEmplacement").val();
    }
    var sousemplacement = $("#wvSousEmplacement").val();
    var ordre = $("#wvOrdre").val();

    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/SaveClauseMagnetic",
        data: {
            codeAffaire: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
            provenance: $("#Provenance").val(), tabGuid: $("#tabGuid").val(), codeRisque: $("#CodeRisque").val(), codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(), modeNavig: $("#ModeNavig").val(), fullScreen: $("#modeLstClause").val(),
            emplacement: emplacement, sousemplacement: sousemplacement, ordre: ordre, contexte: $("#wvContexte").val(),
            idDoc: $("#wvClauseIdDoc").val(), acteGest: $("#ActeGestion").val(), etape: etape, nameClause: $("#wvTitreClause").val(), fileName: $("#wvFileName").val(), idClause: idClause
        }
    }).then(function (data) {
        $("#MagneticIframe").innerHTML = "";
        $("#divWordContainer").hide();

        //Reload de la liste des clauses
        if ($("#modeLstClause").val() == "True")
            $("#divDataFullScreen").html(data);
        else
            $("#divChoixClause").html(data);

        MapElementLstClause();
        $("#divEditClauseLibre").clearHtml();
        $("#divFullScreenClauseLibre").hide();
        $("#ChangeClause").val($("#ChangeClause").val() + ";" + idClause);
        //CloseLoading();
    }).fail(function (request) {
        common.error.showXhr(request);
    });
}

//-----------iFrame sauvegarde----------
function SaveDocumentIframe(isSaveAs) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRsq = $("#CodeRisque").val() != "" && $("#CodeRisque").val() != undefined ? $("#CodeRisque").val() : "0";
    var codeObj = $("#ObjetRisqueCode").val() != "" && $("#ObjetRisqueCode").val() != undefined ? $("#ObjetRisqueCode").val() : "0";
    var codeFor = $("#CodeFormule").val() != "" && $("#CodeFormule").val() != undefined ? $("#CodeFormule").val() : "0";
    var codeOpt = $("#CodeOption").val() != "" && $("#CodeOption").val() != undefined ? $("#CodeOption").val() : "0";
    var typeDocument = $("#wvTypeDocument").val();
    var emplacement = "";
    if ($("#wvEmplacement").is(":disabled")) {
        $("#wvEmplacement").removeAttr("disabled");
        emplacement = $("#wvEmplacement").val();
        $("#wvEmplacement").attr("disabled", "disabled");
    }
    else {
        emplacement = $("#wvEmplacement").val();
    }
    var sousemplacement = $("#wvSousEmplacement").val();
    var ordre = $("#wvOrdre").val().replace(",", ".");

    var idClause = $("#wvClauseId").val();
    var nameDoc = $("iframe[id='WordContainerIFrame']").contents().find("#physicNameDoc").val();

    var docFile = nameDoc.replace(/\\/g, "**");
    var indice = docFile.lastIndexOf("**");
    var docFileTitle = docFile.substr(indice + 2);
    var newNameFile = codeOffre + "_" + version + "_" + type + "_" + codeRsq + "_" + codeObj + "_" + codeFor + "_" + codeOpt + "_" + emplacement + "_" + sousemplacement + "_" + ordre + "_" + idClause + ".docx";

    if (typeDocument == "Clause") {
        var saveC = $("#WordContainerIFrame")[0].contentWindow.SaveDocument(isSaveAs, "");

        var oldNameFile = $("iframe[id='WordContainerIFrame']").contents().find("#physicNameDoc").val();
        $.ajax({
            type: "POST",
            url: "/CommonNavigation/GetPathOfferClauseLibre",
            data: {
                codeOffre: codeOffre, version: version, type: type,
                idClause: idClause, emplacement: emplacement, sousemplacement: sousemplacement, ordre: ordre,
                docFile: docFileTitle, newNameFile: newNameFile, oldNameFile: oldNameFile,
                shortNewName: codeOffre + "_" + version + "_" + type + "_" + codeRsq + "_" + codeObj + "_" + codeFor + "_" + codeOpt + "_" + emplacement + "_" + sousemplacement + "_" + ordre + "_" + idClause + ".docx"
            },
            async: false,
            success: function (data) {
                if (data != "") {
                    if (saveC) {
                        CreateDocumentLibre(nameDoc, idClause);
                        common.dialog.info("Document sauvegardé");
                        if ($("#DefaultDisplayDocModule").val() == "true" && !$("#WordContainerIFrame")[0].contentWindow.CloseWordDoc()) {
                            common.dialog.error("Erreur lors de la fermeture du document.");
                        }
                        else {
                            SaveClauseEntete();
                        }
                    }
                    else {
                        common.dialog.error("Erreur lors de la sauvegarde du document.");
                        CloseLoading();
                    }

                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//--------Sauvegarde la clause libre-------
function SaveClauseLibre() {
    var emplacement = "";
    if ($("#wvEmplacement").is(":disabled")) {
        $("#wvEmplacement").removeAttr("disabled");
        emplacement = $("#wvEmplacement").val();
        $("#wvEmplacement").attr("disabled", "disabled");
    }
    else {
        emplacement = $("#wvEmplacement").val();
    }

    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/SaveClauseLibre",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvt: $("#NumAvenantPage").val(), contexte: $("#ContexteCible").val(), etape: $("#Etapes").val(),
            codeRsq: $("#CodeRisque").val(), codeObj: $("#codeObjViewer").val(), codeFor: $("#CodeFormule").val(), codeOpt: $("#CodeOption").val(),
            emplacement: emplacement, sousemplacement: $("#wvSousEmplacement").val(), ordre: $("#wvOrdre").val(), nameDoc: $("#wvTitreClause").val()
        }
    }).then(function (data) {
        $("#wvClauseId").val(data);
        return SaveClauseMagnetic();
    }).fail(common.error.showXhr);

}
//----------Mise à jour de la clause libre-------
function UpdateDocumentLibre(idClause, idDoc) {
    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/UpdateDocumentLibre",
        data: {
            idClause: idClause, idDoc: idDoc, codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
            etape: $("#Etapes").val(), provenance: $("#Provenance").val(), tabGuid: $("#tabGuid").val(), codeRisque: $("#CodeRisque").val(), codeObjet: $("#codeObjViewer").val(), codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(), modeNavig: $("#ModeNavig").val(), fullScreen: $("#modeLstClause").val()
        }
    }).then(function (data) {
        $("#divWordContainer").hide();

        //Reload de la liste des clauses
        if ($("#modeLstClause").hasTrueVal()) {
            $("#divDataFullScreen").html(data);
        }
        else {
            $("#divChoixClause").html(data);
        }
        MapElementLstClause();
        $("#divEditClauseLibre").clearHtml();
        $("#divFullScreenClauseLibre").hide();
        $("#ChangeClause").val($("#ChangeClause").val() + ";" + idClause);
    }).fail(function (request) {
        common.error.showXhr(request);
    });
}
//--------Creation document clause libre--------------
function CreateDocumentLibre(nameDoc, idClause) {
    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/CreateDocumentLibre",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), etape: $("#Etapes").val(),
            idClause: idClause, pathDoc: nameDoc, nameDoc: $("#wvTitreClause").val(), createDoc: $("#wvCreateClause").val()
        }
    }).then(function (data) {
        if (data != "") {
            var idClause = $("#wvClauseId").val();
            return UpdateDocumentLibre(idClause, data);
        }
        else {
            common.dialog.error("Erreur lors de la sauvegarde du document");
        }
    }).fail(function (request) {
        $("#divWordContainer").hide();
        common.error.showXhr(request);
    });
}

//--------Sauvegarde la sélection des pièces jointes--------
function SavePiecesJointes() {
    var piecesJointesId = "";
    $("input[type='checkbox'][name='chkPieceJointe']").each(function () {
        if ($(this).isChecked())
            piecesJointesId += $(this).attr('albPJId') + ";";
    });
    if (piecesJointesId == "") {
        common.dialog.error("Veuillez sélectionner au moins une pièce jointe.");
        return false;
    }

    var boolError = false;
    $(".requiredField").removeClass("requiredField");
    if ($("#PiecesJointesContexte").val() == "") {
        $("#PiecesJointesContexte").addClass("requiredField");
        boolError = true;
    }

    if ($("#PiecesJointesEmplacement").val() == "") {
        $("#PiecesJointesEmplacement").addClass("requiredField");
        boolError = true;
    }

    if ($("#PiecesJointesSousEmplacement").val() == "") {
        $("#PiecesJointesSousEmplacement").addClass("requiredField");
        boolError = true;
    }

    if ($("#PiecesJointesOrdre").val() == "") {
        $("#PiecesJointesOrdre").addClass("requiredField");
        boolError = true;
    }

    if (boolError) {
        return false;
    }
    else {
        var codeRsqObj = $("#codeObjPiecesJointes").val();

        var codeRsq = 0;
        var codeObj = 0;

        if (codeRsqObj != "" && codeRsqObj != undefined) {
            codeRsq = AlbJsSplitElem(codeRsqObj, 0, "_");
            codeObj = AlbJsSplitElem(codeRsqObj, 1, "_");
        }
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/SavePiecesJointes",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), contexte: $("#PiecesJointesContexte").val(),
                etape: $("#Etapes").val(), codeRsq: codeRsq, codeObj: codeObj, codeFor: $("#CodeFormule").val(), codeOpt: $("#CodeOption").val(),
                emplacement: $("#PiecesJointesEmplacement").val(), sousemplacement: $("#PiecesJointesSousEmplacement").val(), ordre: $("#PiecesJointesOrdre").val(),
                piecesjointesId: piecesJointesId
            },
            success: function (data) {
                OpenCloseFullScreen(false);
                $("#divDataPieceJointesClauses").hide();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//--------Ouvre le s'applique à pour la pièce jointe-------
function OpenPiecesJointesApplique(elem) {
    $("#divLstObj").show();
    var position = elem.offset();
    $("#divDataLstObj").css({ 'position': 'absolute', 'top': position.top - 10 + 'px', 'left': position.left + 18 + 'px', 'z-index': '81', 'border': '1px solid black' });
    $("input[id='check_" + $("#codeObj").val() + "']").attr('checked', 'checked');
    $("#btnListCancel").kclick(function () {
        $("#divLstObj").hide();
    });
    $("#btnListValid").kclick(function () {
        ValidRsqObjChecked();
        //$("#WordContainerIFrame").show();
    });
}
//-------Valide la sélection des risques/objets pour la pièce jointe------
function ValidRsqObjChecked() {
    var nCheckBoxSel = $("input[type='radio'][name='checkObj']:checked").length;
    var rsqSel = $("input[type='radio'][name='checkRsq']:checked").val();
    var codeRsq = AlbJsSplitElem(rsqSel, 0, "_");
    var libRsq = AlbJsSplitElem(rsqSel, 1, "_");

    if (nCheckBoxSel > 0) {
        var nCheckBox = $("input[type='radio'][name='checkObj']").length;
        var objSel = $("input[type='radio'][name='checkObj']:checked").val();
        var codeObj = AlbJsSplitElem(objSel, 0, "_");
        var libObj = AlbJsSplitElem(objSel, 1, "_");

        if (nCheckBox > 1) {
            $("#AppliqueLib").text("S'applique à l'objet");
            $("input[name='wvCodeObj']").val(codeRsq + "_" + codeObj);
            $("#codeObjViewer").val(codeObj);
            $("#codeObjPiecesJointes").val(codeRsq + "_" + codeObj);
            $("input[name='wvApplique']").val(libObj);
        }
        else {
            $("#AppliqueLib").text("S'applique au risque");
            $("input[name='wvCodeObj']").val(codeRsq + "_0");
            $("#codeObjViewer").val("0");
            $("#codeObjPiecesJointes").val(codeRsq + "_0");
            $("input[name='wvApplique']").val(libRsq);
        }
    }
    else {
        $("#AppliqueLib").text("S'applique au risque");
        $("input[name='wvCodeObj']").val(codeRsq + "_0");
        $("#codeObjViewer").val("0");
        $("#codeObjPiecesJointes").val(codeRsq + "_0");
        $("input[name='wvApplique']").val(libRsq);
    }
    $("#divLstObj").hide();
}

function serializeContext() {
    return {
        codeOffreContrat: $("#Offre_CodeOffre").val(),
        versionOffreContrat: $("#Offre_Version").val(),
        typeOffreContrat: $("#Offre_Type").val(),
        codeAvn: $("#NumAvenantPage").val(),
        etape: $("#Etapes").val(),
        provenance: $("#Provenance").val(),
        fullScreen: $("#modeLstClause").val(),
        tabGuid: $("#tabGuid").val(),
        codeObj: $("#ObjetRisqueCode").val(),
        codeRisque: $("#CodeRisque").val(),
        codeFormule: $("#CodeFormule").val(),
        codeOption: $("#CodeOption").val(),
        filtre: $("#Filtre").val(),
        modeNavig: $("#ModeNavig").val(),
        acteGestion: $("#ActeGestion").val(),
        acteGestionRegule: $("#ActeGestionRegule").val(),
        isReadOnly: $("#OffreReadOnly").val(),
        isModifHorsAvn: $("#IsModifHorsAvn").val()
    };
}

function SetCursor() {
    $("#wvTitreClause").focus();
}
