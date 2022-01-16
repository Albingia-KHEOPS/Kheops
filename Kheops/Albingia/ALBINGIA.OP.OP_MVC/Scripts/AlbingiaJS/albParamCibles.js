/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();


});

//----------Map les éléments de la page--------
function MapElementPage() {
    $("#btnRechercher").die().live('click', function () {
        rechercherCibles(1);
    });
    rechercherCibles();
    $("#btnRechercherActiv").die().live('click', function () {
        InitFamillesActivite();
    });

    $("#btnResetAct").die().live("click", function () {
        $("#CodeConcept").val("");
        $("#CodeFamille").val("");
    });
}
//-------Rechercher les familles------
function InitFamillesActivite() {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamCible/GetFamillesActiv",
        data: { codeConcept: "", codeFamille: "" },
        success: function (data) {
            $("#divDataEditValeur").html(data);
            $("#divEditValeur").show();
            AlternanceLigne("BodyFamilles", "Code", true, null);
            MapElementListeFamilles();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function RechercherFamillesActivite(codeConcept, codeFamille) {

    if ((codeConcept == null || codeConcept == undefined) || (codeFamille == null || codeFamille == undefined)) {
        codeConcept = "";
        codeFamille = "";
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamCible/GetFamillesActiv",
        data: { codeConcept: codeConcept, codeFamille: codeFamille },
        success: function (data) {
            $("#divRechercherResult").html(data);
            AlternanceLigne("BodyFamilles", "Code", true, null);
            SelectLigne();
            //MapElementListeFamilles();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function SelectLigne() {
    $("tr[albactivite]").die().live('click', function () {
        var code = $(this).attr("albactivite").split('_')[0];
        var nom = $(this).attr("albactivite").split('_')[1];
        $("#CodeConcept").val($.trim(code));
        $("#CodeFamille").val($.trim(nom));
        $("#btnFermerRecherche").trigger('click');
    });

}
function MapElementListeFamilles() {


    MapCommonAutoCompFamille();
    MapCommonAutoCompConcepts();
    AlternanceLigne("BodyFamilles", "", false, null);
    SelectLigne();
    $('#btnFermerRecherche').die();
    $('#btnFermerRecherche').click(function () {
        ReactivateShortCut();
        $("#divDataEditValeur").html('');
        $("#divEditValeur").hide();
    });

    $('#RechercherFamButton').die();
    $('#RechercherFamButton').click(function () {
        var codeConcept = $("#Concept").val().split("-")[0];
        var codeFamille = $("#CodeFamilleRecherche").val();
        RechercherFamillesActivite(codeConcept, codeFamille);
    });
    $('#btnInitializeRech').die();
    $('#btnInitializeRech').click(function () {
        $("#Concept").val('');
        $("#CodeFamilleRecherche").val('');
    });
}

//--------------Lance la recherche des cibles--------------
function rechercherCibles(param) {
    ShowLoading();
    $("#VoletDetail").hide();
    var codeCible = $("#RechercheCode").val();
    var descrCible = $("#RechercheDescription").val();
    $.ajax({
        type: "POST",
        url: "/ParamCible/Recherche",
        context: $("#divBodyCibles"),
        data: { code: codeCible, description: descrCible },
        success: function (data) {
            CloseLoading();
            $(this).html(data);
            $("#divLstCibles").show();
            affecterClick();
            AlternanceLigne("CibleBody", "Code", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une branche (recherche des sous-branches correspondantes)---------------
function ListBranchesSelectedItemChanged() {
    $("#drlBranches").change(function () {
        AffectTitleList($(this));
        var selectedBranche = $(this).val();
        if (selectedBranche != "") {
            $("#BtnAddBranche").show();
        }
        else {
            $("#BtnAddBranche").hide();
            $("#Categorie").html("<select></selct>");
        }
        //Chargement des sous-branches correspondantes
        $.ajax({
            type: "POST",
            url: "/ParamCible/GetSousBranches",
            data: { codeBranche: selectedBranche, drlSousBranches: "drlSousBranches" },
            success: function (data) {
                $("#SousBranche").html(data);
                ListSousBranchesSelectedItemChanged(selectedBranche);

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une sous-branche (recherche des catégories correspondantes)---------------
function ListSousBranchesSelectedItemChanged(selectedBranche) {
    $("#drlSousBranches").change(function () {
        AffectTitleList($(this));
        var selectedSousBranche = $(this).val();
        var selectedSousBrancheText = $('#drlSousBranches option:selected').text();
        if (selectedSousBrancheText != "") {
            $("#BtnAddBranche").show();
        }
        else {
            $("#BtnAddBranche").hide();
        }

        //Chargement des categories correspondantes à la combinaison branche/sous-branche

        $.ajax({
            type: "POST",
            url: "/ParamCible/GetCategories",
            data: { codeBranche: selectedBranche, codeSousBranche: selectedSousBranche, idCat: "drlCategorie" },
            success: function (data) {
                $("#Categorie").html(data);
                $("#Categorie").die().live('change', function () {
                    AffectTitleList($(this));
                });

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".linkCible").each(function () {
        $(this).click(function () {
            afficherInfoCible($(this).parent().attr("id"), 1);
            AlternanceLigne("CibleBody", "Code", true, null);

            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });
    $("img[name=modifCible]").each(function () {
        $(this).click(function () {
            afficherInfoCible($(this).attr("id"), 0);
            AlternanceLigne("CibleBody", "Code", true, null);
            $(this).parent().parent().css({ "background-color": "#FFDFDF" });
        });
    });
    $("img[name=supprCible]").each(function () {
        $(this).click(function () {
            VerificationAvantSuppressionCible($(this));
            AlternanceLigne("CibleBody", "Code", true, null);
        });
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SuppressionCible":
                var cible = $("#idCibleSuppr").val();
                SupprimerCible(cible);
                break;
            case "SupprBranche":
                var branche = $("#BrancheSuppr").val();
                supprimerBranche(branche);
                AlternanceLigne("BodyBSC", "Code", true, null);
                break;
            case "UpdateBranche":
                var branche = $("#BrancheSuppr").val();
                modifierBranche(branche);
                AlternanceLigne("BodyBSC", "", false);
                break;
            case "DelTemp":
                var idTemp = $("#TempSuppr").val();
                DeleteTemplates(idTemp);
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#DelAssu").val('');
    });

    $("#btnAjouter").die().live('click', function () {
        afficherInfoCible("_", 0);
    });

    $("#addBranche").die().live('click', function () {
        OpenAddBranche();
    });
}
//-------Affiche les informations d'ajout d'une branche-----------
function OpenAddBranche() {
    $("#divSectionAjoutBSC").show();
    $("#drlBranches").val("");
    $("#drlBranches").trigger("change");
}
//--------------Affecte la fonction du click sur les td Code--------------
function affecterClickBranche() {
    var splitCharHtml = $("#splitCharHtml").val();
    $("img[name=supprBranche]").each(function () {
        $(this).click(function () {
            $("#BrancheSuppr").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "SupprBranche", "Attention, si des volets et blocs sont associés à cette cible, alors ils ne seront plus associés. Confirmez-vous la suppression ?", 400, 80, true, true);
        });
    });
    $("td[name=selectableCol]").die().live('click', function () {
        var id = $(this).attr('id');
        EditBSC(id);
        AlternanceLigne("BodyBSC", "", false);
    });

    $("img[name=modifBranche]").each(function () {
        $(this).click(function () {
            $("#BrancheSuppr").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "UpdateBranche", "Attention, si des volets et blocs sont associés à cette cible, alors ils ne seront plus associés. Confirmez-vous la modification ?", 400, 80, true, true);
        });
    });

    $("td[id^=colTemplates_]").die().live('click', function () {
        $.ajax({
            type: "POST",
            url: "/ParamCible/EditTemplates",
            data: { idCible: $("#DetailGuidId").val(), cible: $("#CodeDetail").val() },
            success: function (data) {
                $("#divDataAddTemplate").html(data);
                $("#divAddTemplate").show();
                AlternanceLigne("BodyTemplate", "", false);
                MapEditTemplates();
                //SelTemplate(false, 'O', null);
                //SelTemplate(false, 'P', null);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

}
function EditBSC(e) {
    var codeGuidCible = $("#DetailGuidId").val();
    var tabSplit = e.split('_');
    var guid = tabSplit[1];
    var codeBranche = tabSplit[2];
    var codeSousBranche = tabSplit[3];
    var codeCategorie = tabSplit[4];

    //$("div[name=colReadOnly_" + guid + "]").hide();    
    $(".trEdition").hide();
    $(".trReadOnly").show();
    $.ajax({
        type: "POST",
        url: "/ParamCible/EditBSC",
        data: { codeGuidCible: codeGuidCible, guid: guid, codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie },
        success: function (data) {
            $("tr[name=trReadOnly_" + guid + "]").hide();
            $("tr[name=trEdition_" + guid + "]").show();
            $("tr[name=trEdition_" + guid + "]").html(data);
            BrancheSelectedChanged(guid);
            SousBrancheSelectedChanged(codeBranche, guid);
            affecterClickBranche();
            $("#drlCategories").live("change", function () {
                AffectTitleList($("#drlCategories"));
            });
            $("#divSectionAjoutBSC").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//--------------Affiche les infos de la cible----------------------------
function afficherInfoCible(codeId, readonly) {
    codeId = codeId.split("_")[1];

    $.ajax({
        type: "POST",
        url: "/ParamCible/ConsultCible",
        data: { codeId: codeId, readOnly: readonly },
        success: function (data) {
            afficheConsultCible(data);
            ListBranchesSelectedItemChanged();

            $("#VoletDetail").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Retour de la consultation cible---------------
function afficheConsultCible(data) {
    $("#cibleDetails").html(data);
    //  e.show();
    affecterClickBranche();
    AlternanceLigne("BodyBSC", "", false);
    $('#btnEnregistrer').click(function (evt) {
        enregistrerCible();
    });
}

function enregistrerCible() {
    $("#CodeDetail").val($("#CodeDetail").val().replace(/ /g, ""));
    var codeLib = $("#CodeDetail").val();
    var codeId = $("#DetailGuidId").val();
    var description = $("#Libelle").val();
    var grille = $("#Grille").val();
    var mode = $("#Mode").val();
    var famille = $("#CodeFamille").val();
    var concept = $("#CodeConcept").val().split("-")[0];;
    if (description == "" || codeLib == "") {
        AddClassRequired($("#CodeDetail"));
        AddClassRequired($("#Libelle"));
        return false;
    }

    var validCode = true;
    $("td[name=codeCible]").each(function () {
        if ($.trim($(this).text()).toUpperCase() == codeLib.toUpperCase() && codeId == "") {
            validCode = false;
        }
    });

    if (!validCode) {
        AddClassRequired($("#CodeDetail"));
        common.dialog.error("Code déjà existant.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/ParamCible/Enregistrer",
        data: { mode: mode, codeId: codeId, codeLib: codeLib, description: description, grille: grille, famille: famille, concept: concept },
        success: function (data) {
            rechercherCibles();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------Suppression d'une cible-----------------
function VerificationAvantSuppressionCible(e) {
    //Vérification de l'existance de la cible dans le portefeuille

    $.ajax({
        type: "POST",
        url: "/ParamCible/ExisteCiblePortefeuille",
        data: { guidIdCible: e.attr("id").split("_")[1] },
        success: function (data) {
            $("#idCibleSuppr").val(e.attr("id").split("_")[1]);
            ShowCommonFancy("Confirm", "SuppressionCible", "Etes-vous sûr de vouloir supprimer cette cible ?",
             320, 150, true, true);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function SupprimerCible(guidId) {
    $.ajax({
        type: "POST",
        url: "/ParamCible/SupprimerCible",
        data: { code: guidId },
        success: function (data) {
            if (data == "") {
                rechercherCibles();
                $("#VoletDetail").hide();
            }
            else {
                common.dialog.error(data); CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------Suppression d'une branche-----------------
function supprimerBranche(id) {
    var codeGuidCible = $("#DetailGuidId").val();

    $.ajax({
        type: "POST",
        url: "/ParamCible/SupprimerBranche",
        context: $("#divInfoCible"),
        data: { codeGuidCible: codeGuidCible, codeBSC: id.split("_")[1] },
        success: function (data) {
            $("#" + id).parent().parent().remove();
            $("#Branche").html(data);
            ListBranchesSelectedItemChanged();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Modification d'une branche---------------
function modifierBranche(id) {
    var codeGuidCible = $("#DetailGuidId").val();
    var codeBranche = $("#Branche").val() == undefined ? "" : $("#Branche").val();
    var codeSousBranche = $("#SousBranche").val() == undefined ? "" : $("#SousBranche").val();
    var codeCategorie = $("#drlCategories").val() == undefined ? "" : $("#drlCategories").val();
    $.ajax({
        type: "POST",
        url: "/ParamCible/ModifierBranche",
        context: $("#divInfoCible"),
        data: { codeGuidCible: codeGuidCible, codeBSC: id.split("_")[1], codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie },
        success: function (data) {
            $("#divListeBSC").html(data);
            AlternanceLigne("BodyBSC", "", false);
            affecterClickBranche();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Ajoute la class Required----------
function AddClassRequired(e) {
    e.addClass("requiredField");
}

//--------------Associe une branche à la cible (bouton btnBrancheAdd)---------------
function LinkCible() {
    var guidIdCible = $("#DetailGuidId").val();
    var codeCible = $("#CodeDetail").val();
    var codeBranche = $("#drlBranches").val() == undefined ? "" : $("#drlBranches").val();
    var codeSousBranche = $("#drlSousBranches").val() == undefined ? "" : $("#drlSousBranches").val();
    var codeCategorie = $("#drlCategories").val() == undefined ? "" : $("#drlCategories").val();

    $.ajax({
        type: "POST",
        url: "/ParamCible/AssocierBranche",
        data: { guidIdCible: guidIdCible, codeCible: codeCible, codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie },
        context: $("#divListBranche"),
        success: function (data) {

            $("#divListeBSC").html(data);
            AlternanceLigne("BodyBSC", "", false);
            affecterClickBranche();

            $("#divSectionAjoutBSC").hide();

            afficherInfoCible("tr_" + guidIdCible, 0);

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une branche en mode edition de BSC---------------
function BrancheSelectedChanged(guid) {
    $("#Branche").change(function () {
        var selectedBranche = $(this).val();
        if (selectedBranche == "") {
            $("#Categorie_" + guid).html("<select></selct>");
        }
        //Chargement des sous-branches correspondantes
        $.ajax({
            type: "POST",
            url: "/ParamCible/GetSousBranches",
            data: { codeBranche: selectedBranche, drlSousBranches: "drlSousBranches" },
            success: function (data) {
                $("#SousBranche_" + guid).html(data);
                SousBrancheSelectedChanged(selectedBranche, guid);

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une sous-branche en mode edition de BSC (recherche des catégories correspondantes)---------------
function SousBrancheSelectedChanged(selectedBranche, guid) {
    $("#SousBranche").change(function () {
        AffectTitleList($("#SousBranche"));
        var selectedSousBranche = $(this).val();
        //Chargement des categories correspondantes à la combinaison branche/sous-branche
        $.ajax({
            type: "POST",
            url: "/ParamCible/GetCategories",
            data: { codeBranche: selectedBranche, codeSousBranche: selectedSousBranche, idCat: "drlCategorie" },
            success: function (data) {
                $("#Categorie_" + guid).html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

//----------Map les éléments de la div Edit Templates----------
function MapEditTemplates() {
    var splitCharHtml = $("#splitCharHtml").val();
    MapCommonAutoCompTemplates();
    $("#btnFancyValider").bind('click', function () {
        $("#divDataAddTemplate").html('');
        $("#divAddTemplate").hide();
    });
    $("#btnFancyAnnuler").bind('click', function () {
        $("#divDataAddTemplate").html('');
        $("#divAddTemplate").hide();
    });
    $("input[name=checkTemp]").each(function () {
        $(this).change(function () {
            UpdateTemplate($(this));
        });
    });
    $("img[name=delTemp]").each(function () {
        $(this).click(function () {
            $("#TempSuppr").val($(this).attr("id").split(splitCharHtml)[1]);
            ShowCommonFancy("Confirm", "DelTemp", "Etes-vous sûr de vouloir supprimer l'association de ce template à la cible ?", 350, 80, true, true);
        });
    });
    $("img[name=insTemp]").die().live('click', function () {
        AssociateTemplate();
    });
    $("#tabRefresh").die().live('click', function () {
        RefreshTemplates($("#CibleTemplate").val());
    });
    $("img[name=updTemp]").each(function () {
        $(this).click(function () {
            OpenCreateTemplate($(this));
        });
    });
}
//---------Méthode de sélection des templates par défaut-----------
function SelTemplate(isChecked, typeTemp, e) {
    var splitCharHtml = $("#splitCharHtml").val();

    if (!isChecked)
        $("input[id^='checkTemp" + splitCharHtml + typeTemp + splitCharHtml + "']:first").attr("checked", "checked");
    else {
        $("input[id^='checkTemp" + splitCharHtml + typeTemp + splitCharHtml + "']").removeAttr("checked");
        if (e != null)
            e.attr("checked", "checked");
    }
}
//--------Rafraichissement de la liste des templates--------
function RefreshTemplates(cible) {
    $.ajax({
        type: "POST",
        url: "/ParamCible/RefreshListTemplate",
        data: { idCible: $("#DetailGuidId").val(), cible: cible },
        success: function (data) {
            $("#divLstTemplate").html(data);
            AlternanceLigne("BodyTemplate", "", false);
            MapEditTemplates();
            //SelTemplate(false, 'O', null);
            //SelTemplate(false, 'P', null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Met à jour un template-----------
function UpdateTemplate(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idTemp = elem.attr("id").split(splitCharHtml)[2];
    var typeTemp = elem.attr("id").split(splitCharHtml)[1];
    var isChecked = elem.is(":checked");
    $.ajax({
        type: "POST",
        url: "/ParamCible/UpdateTemplate",
        data: { idTemp: idTemp, isChecked: isChecked },
        success: function (data) {
            SelTemplate(isChecked, typeTemp, elem);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Suppression d'une liaison avec un template-----------
function DeleteTemplates(idTemp) {
    var typeTemp = $("img[id=delTem_" + idTemp + "]").attr("albTypeTemp");
    var delChecked = $("img[id=delTem_" + idTemp + "]").is(":checked");
    $.ajax({
        type: "POST",
        url: "/ParamCible/DeleteTemplate",
        data: { idCible: $("#DetailGuidId").val(), idTemp: idTemp },
        success: function (data) {
            $("#divLstTemplate").html(data);
            AlternanceLigne("BodyTemplate", "", false);
            MapEditTemplates();
            //SelTemplate(false, 'O', null);
            //SelTemplate(false, 'P', null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Associe un template à la cible-------------
function AssociateTemplate() {
    $(".requiredField").removeClass("requiredField");
    var splitCharHtml = $("#splitCharHtml").val();

    if ($("input[id='codeTemp" + splitCharHtml + "-9999']").val() == "" || $("input[id='descTemp" + splitCharHtml + "-9999']").val() == "") {
        common.dialog.error("Veuillez sélectionner un template.");
        $("input[id='codeTemp" + splitCharHtml + "-9999']").addClass("requiredField");
        $("input[id='descTemp" + splitCharHtml + "-9999']").addClass("requiredField");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/ParamCible/AssociateTemplate",
        data: { idCible: $("#DetailGuidId").val(), idTemp: $("input[id='idTemp" + splitCharHtml + "-9999']").val() },
        success: function (data) {
            $("#divLstTemplate").html(data);
            AlternanceLigne("BodyTemplate", "", false);
            MapEditTemplates();
            //SelTemplate(false, 'O', null);
            //SelTemplate(false, 'P', null);
            $("input[id='idTemp" + splitCharHtml + "-9999']").val("");
            $("input[id='codeTemp" + splitCharHtml + "-9999']").val("");
            $("input[id='descTemp" + splitCharHtml + "-9999']").val("");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Ouvre la création d'un(e) contrat/offre template--------
function OpenCreateTemplate(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    //var idCible = $("#DetailGuidId").val();
    //var codeCible = $("#CodeDetail").val();
    var idTemp = elem.attr("id").split(splitCharHtml)[1];
    var typeTemp = elem.attr("albtypetemp");
    var paramId;
    switch (typeTemp) {
        case "P":
            //$("#urlWinOpen").val("/AnCreationContrat/Index/");
            paramId = "_0_P";
            break;
        case "O":
            // $("#urlWinOpen").val("/CreationSaisie/Index/");
            paramId = "_0_O";
            break;
    }

    $.ajax({
        type: "POST",
        url: "/ParamCible/GetParamTemplate",
        data: { idTemp: idTemp },
        success: function (data) {
            if (data == "") {
                common.dialog.error("Erreur lors du chargement des informations du template");
                return false;
            }
            data = data;
            if (data.indexOf("albTemplate") >= 0)
                elem.attr("albparam", data + paramId);
            else
                elem.attr("albparam", data);
            WinOpen(elem);
            return false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

    //var param = elem.attr("albparam");
    //if (param == "")
    //    param = idTemp + "_" + idCible;

}