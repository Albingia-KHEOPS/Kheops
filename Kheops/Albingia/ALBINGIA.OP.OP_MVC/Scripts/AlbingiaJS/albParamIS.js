/// <reference path="../Jquery/jquery-1.4.4-vsdoc.js" />
$(document).ready(function () {
    InitPage();
    MapElementPage();
});
//----------------Initialise les contrôles de la page---------
function InitPage() {
    $("td[class~='edit']").hide();
    affecterClick();

    AlternanceLigne("LigneInfo", "SqlOrder", true, null);
}


//----------------Map les éléments de la page------------------
function MapElementPage() {

    $('#drlBranche').die();
    $('#drlBranche').live('change', function (evt) {
        rechercherLignesInfo();
    });

    $('#drlSection').die();
    $('#drlSection').live('change', function (evt) {
        rechercherLignesInfo();
    });

    $('#btnAjouter').die();
    $('#btnAjouter').live('click', function (evt) {
        afficherLigneInfoVide();
    });

    $('#btnAnnuler').die();
    $('#btnAnnuler').live('click', function (evt) {
        window.location.href = "/BackOffice/Index";
    });

    $('#btnVisualiser').die();
    $('#btnVisualiser').live('click', function (evt) {
        Visualiser();
    });      

    $('#btnEnregistrer').die();
    $('#btnEnregistrer').live('click', function (evt) {
        EnregistrerLignesInfos();
    });

    $('#btnEnregistrerNouvelleLigne').die();
    $('#btnEnregistrerNouvelleLigne').live('click', function (evt) {
        EnregistrerNouvelleLigneInfo();
    });

    $('#btnAnnulerEdit').die();
    $('#btnAnnulerEdit').live('click', function (evt) {
        $.fancybox.close();
    });

    $('#btnOKEdit').die();
    $('#btnOKEdit').live('click', function (evt) {
        EnregistrerModificationLigneInfo($('#hiddenIdEdit').val());
        $.fancybox.close();
    });

    $('#btnOKVisualiser').die();
    $('#btnOKVisualiser').live('click', function (evt) {
        $.fancybox.close();
    });

    $('#btnEditerSelect').die();
    $('#btnEditerSelect').live('click', function (evt) {
        EditerSqlRequest("Select", $("#lblSelect").val());
    });

    $('#btnEditerInsert').die();
    $('#btnEditerInsert').live('click', function (evt) {
        EditerSqlRequest("Insert", $("#lblInsert").val());
    });

    $('#btnEditerUpdate').die();
    $('#btnEditerUpdate').live('click', function (evt) {
        EditerSqlRequest("Update", $("#lblUpdate").val());
    });

    $('#btnEditerSelectExist').die();
    $('#btnEditerSelectExist').live('click', function (evt) {
        EditerSqlRequest("SelectExist", $("#lblSelectExist").val());
    });

    $('#btnOKEditionSql').die();
    $('#btnOKEditionSql').live('click', function (evt) {
        UpdateSqlRequest();
    });

    $('#btnPopulateDB').die();
    $('#btnPopulateDB').live('click', function (evt) {
        PopulerBDDReferentiel();
    });

    $("td[name=btnSupprimerLigneInfo]").each(function () {
        $(this).click(function () {
            SupprimerLigneInfo($(this).attr("id"));
        });
    });

    $("td[name=btnEnregistrerModificationLigneInfo]").each(function () {
        $(this).click(function () {
            EnregistrerModificationLigneInfo($(this).attr("id"));
        });
    });
}

//--------------Lance la recherche des lignes info--------------
function rechercherLignesInfo() {
    var branche = $("#drlBranche").val();
    var section = $("#drlSection").val();
    ShowLoading();
    $("#divBodyLignesInfo").hide();
    $("#divSqlRequest").hide();

    $.ajax({
        type: "POST",
        url: "/ParamIS/Recherche",
        data: { branche: branche, section: section },

        success: function (data) {
            $("#divDonnee").html(data);
            $("td[class~='edit']").hide();

            $("#btnVisualiser").removeAttr('disabled');
            $("#btnEnregistrer").attr('disabled', 'disabled');//deactivation du bouton d'enregistrement       
            $("#divLstLignesInfo").show();
            $("#divBodyLignesInfo").show();
            $("#divSqlRequest").show();
            CloseLoading();
            affecterClick();
            AlternanceLigne("LigneInfo", "SqlOrder", true, null);
            MapElementPage();
        },
        error: function (request) {
            $("#btnVisualiser").removeAttr('disabled');
            $("#btnEnregistrer").attr('disabled', 'disabled');//deactivation du bouton d'enregistrement       
            $("#divLstLignesInfo").show();
            $("#divBodyLignesInfo").show();
            $("#divSqlRequest").show();
            common.error.showXhr(request);
        }
    });



}

//----------------Gestion du click sur le bouton d'ajout d'une ligne-----------
function afficherLigneInfoVide() {
    RAZLigneVide();
    $("#divLstLignesInfo").show();
    $("#divLigneInfoVide").show();
}

//---------------Gestion de l'enregistrement d'une nouvelle ligne (click sur ok)-------
function EnregistrerNouvelleLigneInfo() {
    ConstruireNouvelleLigneTable();
    $("#btnEnregistrer").removeAttr('disabled');
}

//---------------Gestion de l'enregistrement des modifications sur une ligne en édition (click sur ok)-------
function EnregistrerModificationLigneInfo(ligneInfoId) {
    ligneInfoId = ligneInfoId.split("_")[1];
    var modeSaisie = $('input[type=radio][name=rdModeSaisie]:checked').attr('value');

    //récupération des valeurs saisies par l'utilisateur
    if (modeSaisie == "inline") {
        var vSqlOrder = $("#inSqlOrder_" + ligneInfoId).val();
        var vLib = $("#inLib_" + ligneInfoId).val();;
        var vCells = $("#inCells_" + ligneInfoId).val();;
        var vDbMapCol = $("#inDbMapCol_" + ligneInfoId).val();;
        var vLink = $("#drlLink_" + ligneInfoId).val();
        var vType = $("#drlType_" + ligneInfoId).val();
        var vSqlRequest = $("#inSqlRequest_" + ligneInfoId).val();
        var vConvertTo = $("#drlConvertTo_" + ligneInfoId)[0].options[$("#drlConvertTo_" + ligneInfoId)[0].options.selectedIndex].text;
        var vTextLabel = $("#inTextLabel_" + ligneInfoId).val();
        var vHierarchyOrder = $("#drlHierarchyOrder_" + ligneInfoId)[0].options[$("#drlHierarchyOrder_" + ligneInfoId)[0].options.selectedIndex].text;
        var vLineBreak = $("#drlLineBreak_" + ligneInfoId)[0].options[$("#drlLineBreak_" + ligneInfoId)[0].options.selectedIndex].text;
        var vRequired = $("#drlRequired_" + ligneInfoId)[0].options[$("#drlRequired_" + ligneInfoId)[0].options.selectedIndex].text;
        var vTypeUIControl = $("#drlUIControl_" + ligneInfoId).val();
        var vLinkBehaviour = $("#inLinkBehaviour_" + ligneInfoId).val();
        var vBehaviour = $("#inBehaviour_" + ligneInfoId).val();
        var vEventBehaviour = $("#inEventBehaviour_" + ligneInfoId).val();
        var vDisabled = $("#drlDisabled_" + ligneInfoId)[0].options[$("#drlDisabled_" + ligneInfoId)[0].options.selectedIndex].text;
    }
    else if (modeSaisie == "form") {
        var vSqlOrder = $("#inEditSqlOrder").val();
        var vLib = $("#inEditLib").val();
        var vCells = $("#inEditCells").val();
        var vDbMapCol = $("#inEditDbMapCol").val();
        var vLink = $("#drlEditLink").val();
        var vType = $("#drlEditType").val();
        var vSqlRequest = $("#inEditSqlRequest").val();
        var vConvertTo = $("#drlEditConvertTo").val();
        var vTextLabel = $("#inEditTextLabel").val();
        var vHierarchyOrder = $("#drlEditHierarchyOrder").val();
        var vLineBreak = $("#drlEditLineBreak").val();
        var vRequired = $("#drlEditRequired").val();
        var vTypeUIControl = $("#drlEditUIControl").val();
        var vLinkBehaviour = $("#inEditLinkBehaviour").val();
        var vBehaviour = $("#inEditBehaviour").val();
        var vEventBehaviour = $("#inEditEventBehaviour").val();
        var vDisabled = $("#drlEditDisabled").val();
        $.fancybox.close();

        $("#inSqlOrder_" + ligneInfoId).val(vSqlOrder);
        $("#inLib_" + ligneInfoId).val(vLib);
        $("#inCells_" + ligneInfoId).val(vCells);
        $("#inDbMapCol_" + ligneInfoId).val(vDbMapCol);
        $("#drlLink_" + ligneInfoId).val(vLink);
        $("#drlType_" + ligneInfoId).val(vType);
        $("#inSqlRequest_" + ligneInfoId).val(vSqlRequest);
        $("#drlConvertTo_" + ligneInfoId).val(vConvertTo);
        $("#inTextLabel_" + ligneInfoId).val(vTextLabel);
        $("#drlHierarchyOrder_" + ligneInfoId).val(vHierarchyOrder);
        $("#drlLineBreak_" + ligneInfoId).val(vLineBreak);
        $("#drlRequired_" + ligneInfoId).val(vRequired);
        $("#drlUIControl_" + ligneInfoId).val(vTypeUIControl);
        $("#inLinkBehaviour_" + ligneInfoId).val(vLinkBehaviour);
        $("#inBehaviour_" + ligneInfoId).val(vBehaviour);
        $("#inEventBehaviour_" + ligneInfoId).val(vEventBehaviour);
        $("#drlDisabled_" + ligneInfoId).val(vDisabled);

        vLink = $("#drlEditLink")[0].options[$("#drlEditLink")[0].options.selectedIndex].text;
        vType = $("#drlEditType")[0].options[$("#drlEditType")[0].options.selectedIndex].text;
        vConvertTo = $("#drlEditConvertTo")[0].options[$("#drlEditConvertTo")[0].options.selectedIndex].text;
        vHierarchyOrder = $("#drlEditHierarchyOrder")[0].options[$("#drlEditHierarchyOrder")[0].options.selectedIndex].text;
        vLineBreak = $("#drlEditLineBreak")[0].options[$("#drlEditLineBreak")[0].options.selectedIndex].text;
        vRequired = $("#drlEditRequired")[0].options[$("#drlEditRequired")[0].options.selectedIndex].text;
        vTypeUIControl = $("#drlEditUIControl")[0].options[$("#drlEditUIControl")[0].options.selectedIndex].text;
        vDisabled = $("#drlEditDisabled")[0].options[$("#drlEditDisabled")[0].options.selectedIndex].text;
    }

    $("#lblSqlOrder_" + ligneInfoId).html(vSqlOrder);
    $("#lblLib_" + ligneInfoId).html(vLib);
    $("#lblCells_" + ligneInfoId).html(vCells);
    $("#lblDbMapCol_" + ligneInfoId).html(vDbMapCol);
    $("#lblLink_" + ligneInfoId).html(vLink);
    $("#lblType_" + ligneInfoId).html(vType);
    $("#lblSqlRequest_" + ligneInfoId).html(vSqlRequest);
    $("#lblConvertTo_" + ligneInfoId).html(vConvertTo);
    $("#lblTextLabel_" + ligneInfoId).html(vTextLabel);
    $("#lblHierarchyOrder_" + ligneInfoId).html(vHierarchyOrder);
    $("#lblLineBreak_" + ligneInfoId).html(vLineBreak);
    $("#lblRequired_" + ligneInfoId).html(vRequired);
    $("#lblTypeUIControl_" + ligneInfoId).html(vTypeUIControl);
    $("#lblLinkBehaviour_" + ligneInfoId).html(vLinkBehaviour);
    $("#lblBehaviour_" + ligneInfoId).html(vBehaviour);
    $("#lblEventBehaviour_" + ligneInfoId).html(vEventBehaviour);
    $("#lblDisabled_" + ligneInfoId).html(vDisabled);

    $(".edit").hide();
    $(".lock").show();
    $(".suppr").show();
    $("#btnEnregistrer").removeAttr('disabled');
}

//---------------Gestion de la suppression d'une ligne info (click sur la corbeille)
function SupprimerLigneInfo(ligneInfoId) {
    ligneInfoId = ligneInfoId.split("_")[1];
    $('#tblLigneInfo tr[id=tr_' + ligneInfoId + ']').remove();
    $("#btnEnregistrer").removeAttr('disabled');
}

//-----------function appelée lors du clic sur le bouton "enregistrer les modifications", sauvegarde toutes les lignes info affichées dans le fichier xml----------
function EnregistrerLignesInfos() {
    var branche = $("#drlBranche").val();
    var section = $("#drlSection").val();
    ShowLoading();
    //Vérification des champs obligatoires
    var sqlSelect = $.trim($("#lblSelect").val());
    var sqlInsert = $.trim($("#lblInsert").val());
    var sqlUpdate = $.trim($("#lblUpdate").val());
    var sqlSelectExist = $.trim($("#lblSelectExist").val());

    if (sqlSelect.length == 0 || sqlInsert.length == 0 || sqlUpdate.length == 0 || sqlSelectExist.length == 0) {
        common.dialog.bigError("Les champs Select, Insert, Update, SelectExist sont obligatoires", true);
        CloseLoading();
        return;
    }

    //Fin vérification des champs obligatoires

    var lignesInfoData = "[";
    //Pour chaque ligne de tblLigneInfo   
    $("#tblLigneInfo tr").each(
        function () {
            var idLigne = this.id;
            lignesInfoData = appendLigne(lignesInfoData, idLigne);
        });
    lignesInfoData = lignesInfoData.substring(0, lignesInfoData.length - 1);
    lignesInfoData += "]";

    var sqlRequest = "[";
    sqlRequest += '{';
    sqlRequest += 'Select:"' + sqlSelect + '",';
    sqlRequest += 'Insert:"' + sqlInsert + '",';
    sqlRequest += 'Update:"' + sqlUpdate + '",';
    sqlRequest += 'SelectExist:"' + sqlSelectExist + '"';
    sqlRequest += '}';
    sqlRequest += ']';
    //appel AJAX
    $.ajax({
        type: "POST",
        url: "/ParamIS/SaveLignesInfos",
        data: {
            data: lignesInfoData, sqlRequestData: sqlRequest, branche: branche, section: section
        },
        success: function (data) {
            $("#btnEnregistrer").attr('disabled', 'disabled');
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------fonction qui ouvre la div flottante d'édtion des sql request-------------------
function EditerSqlRequest(typeRequete, requete) {
    var branche = $("#drlBranche").val();
    var section = $("#drlSection").val();

    $.ajax({
        type: "POST",
        url: "/ParamIS/ObtenirSqlRequestDetail",
        data: { branche: branche, section: section, typeRequete: typeRequete, requete: requete },
        success: function (data) {
            $("#divSqlRequest").html(data);
            AlbScrollTop();
            $("#divFullScreen").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//-------------fonction qui ferme la div flottante d'édition des sql request et qui met à jour les contrôles de la page--------------
function UpdateSqlRequest() {
    var typeRequete = $("#TypeEditSql").val();

    var requete = $("#SqlEdit").val().toUpperCase();
    switch (typeRequete) {
        case "Select": $("#lblSelect").val(requete); break;
        case "Insert": $("#lblInsert").val(requete); break;
        case "Update": $("#lblUpdate").val(requete); break;
        case "SelectExist": $("#lblSelectExist").val(requete); break;
    }
    $("#divFullScreen").hide();
    $('#btnEnregistrer').removeAttr('disabled');
}

//-----------fonction appelée lors du clic sur le bouton visualiser, affiche un aperçu des paramètres dans une fancybox---------
function Visualiser() {
    var branche = $("#drlBranche").val();
    var section = $("#drlSection").val();

    $.ajax({
        type: "POST",
        url: "/ParamIS/VisualiserLigneInfo",
        data: { branche: branche, section: section },
        success: function (data) {
            $("#divFancyVisualiserData").html(data);
            $.fancybox(
                 {
                     'content': $("#divFancyVisualiser").html(),
                     'autodimensions': false,
                     'width': 500,
                     'height': 500,
                     'transitionIn': 'elastic',
                     'transitionOut': 'elastic',
                     'speedin': 400,
                     'speedOut': 400,
                     'easingOut': 'easeInBack',
                     'modal': true
                 }
              );
            $("#fancybox-wrap").addClass('fancybox-wrap_Top');
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//
//fonctions privées (uniquement appelées par le code)

//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".lock").each(function () {
        $(this).click(function () {
            //récupération du mode de saisie (Inline ou form)
            var modeSaisie = $('input[type=radio][name=rdModeSaisie]:checked').attr('value');

            if (modeSaisie == "inline") {
                $("td[class~='edit']").hide();
                $("td[class~='lock']").show();
                $("td[class~='suppr']").show();
                $(this).parent().children(".edit").show();
                $(this).parent().children(".lock").hide();
                $(this).parent().children(".suppr").hide();
            }
            else if (modeSaisie == "form") {

                //Gestion de la fancyBox d'édition
                var lineId = $(this).parent().attr("id").split("_")[1];

                //             
                //récupération des valeurs de la ligne en édition
                var vSqlOrder = $("#inSqlOrder_" + lineId).clone().attr("id", "inEditSqlOrder").removeClass("col_SqlOrder").addClass("col_Formulaire");
                var vLib = $("#inLib_" + lineId).clone().attr("id", "inEditLib").removeClass("col_Lib").addClass("col_Formulaire");
                var vCells = $("#inCells_" + lineId).clone().attr("id", "inEditCells").removeClass("col_Cells").addClass("col_Formulaire");
                var vDbMapCol = $("#inDbMapCol_" + lineId).clone().attr("id", "inEditDbMapCol").removeClass("col_DbMapCol").addClass("col_Formulaire");
                var vLink = $("#drlLink_" + lineId).clone().attr("id", "drlEditLink").removeClass("col_Link").addClass("col_Formulaire");
                var vType = $("#drlType_" + lineId).clone().attr("id", "drlEditType").removeClass("col_Type").addClass("col_Formulaire");
                var vSqlRequest = $("#inSqlRequest_" + lineId).clone().attr("id", "inEditSqlRequest").removeClass("col_SqlRequest").addClass("col_Formulaire");
                var vConvertTo = $("#drlConvertTo_" + lineId).clone().attr("id", "drlEditConvertTo").removeClass("col_ConvertTo").addClass("col_Formulaire");
                var vTextLabel = $("#inTextLabel_" + lineId).clone().attr("id", "inEditTextLabel").removeClass("col_TextLabel").addClass("col_Formulaire");
                var vHierarchyOrder = $("#drlHierarchyOrder_" + lineId).clone().attr("id", "drlEditHierarchyOrder").removeClass("col_HierarchyOrder").addClass("col_Formulaire");
                var vLineBreak = $("#drlLineBreak_" + lineId).clone().attr("id", "drlEditLineBreak").removeClass("col_LineBreak").addClass("col_Formulaire");
                var vRequired = $("#drlRequired_" + lineId).clone().attr("id", "drlEditRequired").removeClass("col_Required").addClass("col_Formulaire");
                var vTypeUIControl = $("#drlUIControl_" + lineId).clone().attr("id", "drlEditUIControl").removeClass("col_TypeUIControl").addClass("col_Formulaire");
                var vLinkBehaviour = $("#inLinkBehaviour_" + lineId).clone().attr("id", "inEditLinkBehaviour").removeClass("col_LinkBehaviour").addClass("col_Formulaire");
                var vBehaviour = $("#inBehaviour_" + lineId).clone().attr("id", "inEditBehaviour").removeClass("col_Behaviour").addClass("col_Formulaire");
                var vEventBehaviour = $("#inEventBehaviour_" + lineId).clone().attr("id", "inEditEventBehaviour").removeClass("col_EventBehaviour").addClass("col_Formulaire");
                var vDisabled = $("#drlDisabled_" + lineId).clone().attr("id", "drlEditDisabled").removeClass("col_Disabled").addClass("col_Formulaire");
                //
                //Correction d'un problème lors d'un clone d'un select, les valeurs selectionnées ne sont pas conservées
                vLink.val($("#drlLink_" + lineId).val());
                vType.val($("#drlType_" + lineId).val());
                vConvertTo.val($("#drlConvertTo_" + lineId).val());
                vHierarchyOrder.val($("#drlHierarchyOrder_" + lineId).val());
                vLineBreak.val($("#drlLineBreak_" + lineId).val());
                vRequired.val($("#drlRequired_" + lineId).val());
                vTypeUIControl.val($("#drlUIControl_" + lineId).val());
                vDisabled.val($("#drlDisabled_" + lineId).val());
                //
                //assignation de ces valeurs aux champs de la Fancy
                $("#hiddenIdEdit").val(lineId);
                $("#divInEditSqlOrder").html(vSqlOrder);
                $("#divInEditLib").html(vLib);
                $("#divInEditCells").html(vCells);
                $("#divInEditDbMapCol").html(vDbMapCol);
                $("#divInEditSqlRequest").html(vSqlRequest);
                $("#divInEditTextLabel").html(vTextLabel);
                $("#divInEditLinkBehaviour").html(vLinkBehaviour);
                $("#divInEditBehaviour").html(vBehaviour);
                $("#divInEditEventBehaviour").html(vEventBehaviour);

                $("#divDrlEditHierarchyOrder").html(vHierarchyOrder);
                $("#divDrlEditLink").html(vLink);
                $("#divDrlEditType").html(vType);
                $("#divDrlEditConvertTo").html(vConvertTo);
                $("#divDrlEditLineBreak").html(vLineBreak);
                $("#divDrlEditRequired").html(vRequired);
                $("#divDrlEditTypeUIControl").html(vTypeUIControl);
                $("#divDrlEditDisabled").html(vDisabled);

                ShowFancyEdit(lineId);
                $("#hrefLine_" + lineId).trigger('click');

            }

            AlternanceLigne("LigneInfo", "SqlOrder", true, null);
            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");

        });
    });
}

//-----------fonction privée permettant de serialiser la ligne info en paramètre dans la chaine en parametre----------
function appendLigne(lignesInfoData, idLigne) {
    lignesInfoData += '{';
    lignesInfoData += 'SqlOrder:"' + idLigne + '",';
    lignesInfoData += 'Lib:"' + $("#inLib_" + idLigne).val() + '",';
    lignesInfoData += 'Cells:"' + $("#inCells_" + idLigne).val() + '",';
    lignesInfoData += 'DbMapCol:"' + $("#inDbMapCol_" + idLigne).val() + '",';
    lignesInfoData += 'Link:"' + $("#drlLink_" + idLigne).val() + '",';
    lignesInfoData += 'Type:"' + $("#drlType_" + idLigne).val() + '",';
    lignesInfoData += 'SqlRequest:"' + $("#inSqlRequest_" + idLigne).val() + '",';
    lignesInfoData += 'ConvertTo:"' + $("#drlConvertTo_" + idLigne).val() + '",';
    lignesInfoData += 'TextLabel:"' + $("#inTextLabel_" + idLigne).val() + '",';
    lignesInfoData += 'HierarchyOrder:"' + $("#drlHierarchyOrder_" + idLigne).val() + '",';
    lignesInfoData += 'LineBreak:"' + $("#drlLineBreak_" + idLigne).val() + '",';
    lignesInfoData += 'Required:"' + $("#drlRequired_" + idLigne).val() + '",';
    lignesInfoData += 'TypeUIControl:"' + $("#drlUIControl_" + idLigne).val() + '",';
    lignesInfoData += 'LinkBehaviour:"' + $("#inLinkBehaviour_" + idLigne).val() + '",';
    lignesInfoData += 'Behaviour:"' + $("#inBehaviour_" + idLigne).val() + '",';
    lignesInfoData += 'EventBehaviour:"' + $("#inEventBehaviour_" + idLigne).val() + '",';
    lignesInfoData += 'Disabled:"' + $("#drlDisabled_" + idLigne).val() + '"';
    lignesInfoData += '},';

    return lignesInfoData;
}

//------------Mise à vide des champs de la ligne vide--------------
function RAZLigneVide() {
    var vSqlOrder = $("#inSqlOrder").val('');
    var vLib = $("#inLib").val('');
    var vCells = $("#inCells").val('');
    var vDbMapCol = $("#inDbMapCol").val('');
    var vLink = $("#drlLink").val('');
    var vType = $("#drlType").val('');
    var vSqlRequest = $("#inSqlRequest").val('');
    var vConvertTo = $("#drlConvertTo").val('');
    var vTextLabel = $("#inTextLabel").val('');
    var vHierarchyOrder = $("#drlHierarchyOrder").val('');
    var vLineBreak = $("#drlLineBreak").val('');
    var vRequired = $("#drlRequired").val('');
    var vTypeUIControl = $("#drlUIControl").val('');
    var vLinkBehaviour = $("#inLinkBehaviour").val('');
    var vBehaviour = $("#inBehaviour").val('');
    var vEventBehaviour = $("#inEventBehaviour").val('');
    var vDisabled = $("#drlDisabled").val('');
}

//-----------Affiche le formulaire d'édition d'une ligne dans une fancybox (mode de saisie = form)
function ShowFancyEdit(lineId) {
    AlbScrollTop();
    $("a#hrefLine_" + lineId).fancybox(
              {
                  'autodimensions': false,
                  'width': 800,
                  'height': 500,
                  'transitionIn': 'elastic',
                  'transitionOut': 'elastic',
                  'speedin': 400,
                  'speedOut': 400,
                  'easingOut': 'easeInBack',
                  'modal': true
              }
           );
    $("#fancybox-wrap").addClass('fancybox-wrap_Top');
}

//-----------Construit une nouvelle ligne dans la table grâce aux champs de la ligne vide--------------------
function ConstruireNouvelleLigneTable() {
    //détermination du GuidId de la nouvelle ligne
    var lineId = $('#tblLigneInfo tr').length + 1;

    //Copie des champs de la ligne vide
    var vSqlOrder = $("#inSqlOrder").clone().attr("id", "inSqlOrder_" + lineId);
    var vLib = $("#inLib").clone().attr("id", "inLib_" + lineId);
    var vCells = $("#inCells").clone().attr("id", "inCells_" + lineId);
    var vDbMapCol = $("#inDbMapCol").clone().attr("id", "inDbMapCol_" + lineId);
    var vLink = $("#drlLink").clone().attr("id", "drlLink_" + lineId);
    var vType = $("#drlType").clone().attr("id", "drlType_" + lineId);
    var vSqlRequest = $("#inSqlRequest").clone().attr("id", "inSqlRequest_" + lineId);
    var vConvertTo = $("#drlConvertTo").clone().attr("id", "drlConvertTo_" + lineId);
    var vTextLabel = $("#inTextLabel").clone().attr("id", "inTextLabel_" + lineId);
    var vHierarchyOrder = $("#drlHierarchyOrder").clone().attr("id", "drlHierarchyOrder_" + lineId);
    var vLineBreak = $("#drlLineBreak").clone().attr("id", "drlLineBreak_" + lineId);
    var vRequired = $("#drlRequired").clone().attr("id", "drlRequired_" + lineId);
    var vTypeUIControl = $("#drlUIControl").clone().attr("id", "drlUIControl_" + lineId);
    var vLinkBehaviour = $("#inLinkBehaviour").clone().attr("id", "inLinkBehaviour_" + lineId);
    var vBehaviour = $("#inBehaviour").clone().attr("id", "inBehaviour_" + lineId);
    var vEventBehaviour = $("#inEventBehaviour").clone().attr("id", "inEventBehaviour_" + lineId);
    var vDisabled = $("#drlDisabled").clone().attr("id", "drlDisabled_" + lineId);

    //Correction d'un problème lors d'un clone d'un select, les valeurs selectionnées ne sont pas conservées
    vLink.val($("#drlLink").val());
    vType.val($("#drlType").val());
    vConvertTo.val($("#drlConvertTo").val());
    vHierarchyOrder.val($("#drlHierarchyOrder").val());
    vLineBreak.val($("#drlLineBreak").val());
    vRequired.val($("#drlRequired").val());
    vTypeUIControl.val($("#drlUIControl").val());
    vDisabled.val($("#drlDisabled").val());

    //Construction de la chaine html
    var html = "";
    html += "<tr id=\"" + lineId + "\">";
    html += "     <td class=\"col_SqlOrder lock\" title=\"" + vSqlOrder.val() + "\">";
    html += "          <label id=\"lblSqlOrder_" + lineId + "\">" + vSqlOrder.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_SqlOrder edit\">";
    html += "          <div id=\"divInSqlOrder_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Lib lock\" title=\"" + vLib.val() + "\">";
    html += "          <label id=\"lblLib_" + lineId + "\">" + vLib.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Lib edit\">";
    html += "          <div id=\"divInLib_" + lineId + "\" ></div>";
    html += "      </td>";
    html += "      <td class=\"col_Cells lock\" title=\"" + vCells.val() + "\">";
    html += "          <label id=\"lblCells_" + lineId + "\">" + vCells.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Cells edit\">";
    html += "          <div id=\"divInCells_" + lineId + "\" ></div>";
    html += "      </td>";
    html += "      <td class=\"col_DbMapCol lock\" title=\"" + vDbMapCol.val() + "\">";
    html += "          <label id=\"lblDbMapCol_" + lineId + "\">" + vDbMapCol.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_DbMapCol edit\">";
    html += "          <div id=\"divInDbMapCol_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Link lock\" title=\"" + vLink.val() + "\">";
    html += "          <label id=\"lblLink_" + lineId + "\">" + vLink.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Link edit\">";
    html += "          <div id=\"divDrlLink_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Type lock\" title=\"" + vType.val() + "\">";
    html += "          <label id=\"lblType_" + lineId + "\">" + vType.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Type edit\">";
    html += "           <div id=\"divDrlType_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_SqlRequest lock\" title=\"" + vSqlRequest.val() + "\">";
    html += "          <label id=\"lblSqlRequest_" + lineId + "\">" + vSqlRequest.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_SqlRequest edit\">";
    html += "          <div id=\"divInSqlRequest_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_ConvertTo lock\" title=\"" + vConvertTo.val() + "\">";
    html += "          <label id=\"lblConvertTo_" + lineId + "\">" + vConvertTo.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_ConvertTo edit\">";
    html += "         <div id=\"divDrlConvertTo_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_TextLabel lock\" title=\"" + vTextLabel.val() + "\">";
    html += "          <label id=\"lblTextLabel_" + lineId + "\">" + vTextLabel.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_TextLabel edit\">";
    html += "          <div id=\"divInTextLabel_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_HierarchyOrder lock\" title=\"" + vHierarchyOrder.val() + "\">";
    html += "          <label id=\"lblHierarchyOrder_" + lineId + "\">" + vHierarchyOrder.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_HierarchyOrder edit\">";
    html += "         <div id=\"divDrlHierarchyOrder_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_LineBreak lock\" title=\"" + vLineBreak.val() + "\">";
    html += "          <label id=\"lblLineBreak_" + lineId + "\">" + vLineBreak.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_LineBreak edit\">";
    html += "         <div id=\"divDrlLineBreak_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Required lock\" title=\"" + vRequired.val() + "\">";
    html += "          <label id=\"lblRequired_" + lineId + "\">" + vRequired.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Required edit\">";
    html += "         <div id=\"divDrlRequired_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_TypeUIControl lock\" title=\"" + vTypeUIControl.val() + "\">";
    html += "          <label id=\"lblTypeUIControl_" + lineId + "\">" + vTypeUIControl.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_TypeUIControl edit\">";
    html += "         <div id=\"divDrlUIControl_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_LinkBehaviour lock\" title=\"" + vLinkBehaviour.val() + "\">";
    html += "          <label id=\"lblLinkBehaviour_" + lineId + "\">" + vLinkBehaviour.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_LinkBehaviour edit\">";
    html += "         <div id=\"divInLinkBehaviour_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Behaviour lock\" title=\"" + vBehaviour.val() + "\">";
    html += "          <label id=\"lblBehaviour_" + lineId + "\">" + vBehaviour.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Behaviour edit\">";
    html += "         <div id=\"divInBehaviour_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_EventBehaviour lock\" title=\"" + vEventBehaviour.val() + "\">";
    html += "          <label id=\"lblEventBehaviour_" + lineId + "\">" + vEventBehaviour.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_EventBehaviour edit\">";
    html += "         <div id=\"divInEventBehaviour_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Disabled lock\" title=\"" + vDisabled.val() + "\">";
    html += "          <label id=\"lblDisabled_" + lineId + "\">" + vDisabled.val() + "</label>";
    html += "      </td>";
    html += "      <td class=\"col_Disabled edit\">";
    html += "         <div id=\"divDrlDisabled_" + lineId + "\"></div>";
    html += "      </td>";
    html += "      <td class=\"col_Action lock\">&nbsp;";
    html += "      </td>";
    html += "      <td class=\"col_Action edit\">";
    html += "          <input type=\"button\" value=\"OK\" onclick=\"javascript:EnregistrerModificationLigneInfo(\"edt_\"" + lineId + ");\" />";
    html += "      </td>";
    html += "      <td style=\"display:none\">";
    html += "          <a id=\"hrefLine_" + lineId + "\" href=\"#divDataEditPopIn\"></a>";
    html += "      </td>";
    html += "</tr>";
    //Ajout de la ligne au tableau
    $('#tblLigneInfo').append(html);

    //Ajout des contrôles à la ligne nouvellement créée
    $("#divInSqlOrder_" + lineId).html(vSqlOrder);
    $("#divInLib_" + lineId).html(vLib);
    $("#divInCells_" + lineId).html(vCells);
    $("#divInDbMapCol_" + lineId).html(vDbMapCol);
    $("#divDrlLink_" + lineId).html(vLink);
    $("#divDrlType_" + lineId).html(vType);
    $("#divInSqlRequest_" + lineId).html(vSqlRequest);
    $("#divDrlConvertTo_" + lineId).html(vConvertTo);
    $("#divInTextLabel_" + lineId).html(vTextLabel);
    $("#divDrlHierarchyOrder_" + lineId).html(vHierarchyOrder);
    $("#divDrlLineBreak_" + lineId).html(vLineBreak);
    $("#divDrlRequired_" + lineId).html(vRequired);
    $("#divDrlUIControl_" + lineId).html(vTypeUIControl);
    $("#divInLinkBehaviour_" + lineId).html(vLinkBehaviour);
    $("#divInBehaviour_" + lineId).html(vBehaviour);
    $("#divInEventBehaviour_" + lineId).html(vEventBehaviour);
    $("#divDrlDisabled_" + lineId).html(vDisabled);

    $("#divLigneInfoVide").hide();
    RAZLigneVide();
    $(".edit").hide();
    $(".lock").show();
    affecterClick();
    AlternanceLigne("LigneInfo", "SqlOrder", true, null);

}

//----------fonction qui charge le fichier xml en base de données
function PopulerBDDReferentiel() {
    var branche = $("#drlBranche").val();
    var section = $("#drlSection").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamIS/PopulerBddISReferentiels",
        data: {
            branche: branche, section: section
        },
        success: function (data) {
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//
//fin fonctions privées