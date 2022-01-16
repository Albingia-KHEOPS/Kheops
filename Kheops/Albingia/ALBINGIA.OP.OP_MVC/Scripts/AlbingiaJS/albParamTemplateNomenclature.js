/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
    //RechercherTemplatesNomenclature();
});

//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $("#btnRechercher").unbind();
    $("#btnRechercher").bind("click", function () {
        RechercherTemplatesNomenclature();
    });

    $("#btnAjouterNomenclature").unbind();
    $("#btnAjouterNomenclature").bind("click", function () {
        AfficherDetailsTemplate("");
    });

    $('#btnAnnuler').unbind();
    $('#btnAnnuler').bind('click', function (evt) {
        Annuler();
    });

}

//------------Map les lignes du tableau de templates--------
function MapElementsTableau() {
    $("tr[name=ligneTemplate]").unbind();
    $("tr[name=ligneTemplate]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        AfficherDetailsTemplate(id);
    });

    AlternanceLigne("TemplatesBody", "noInput", true, null);
}

//------------Map les éléments de la partie détails du template nomenclature----------
function MapElementsDetails() {
    $("#btnEnregistrer").unbind();
    $("#btnEnregistrer").bind("click", function () {
        EnregistrerTemplateNomenclature();
    });

    LoadListCibleByBranche();
}

//------------enregistre le template nomenclature (insert ou update)
function EnregistrerTemplateNomenclature() {
    var isCheck = true;
    var branche = $("#drlBrancheEdit").val();
    var cible = $("#drlCibleEdit").val();
    var libelle = $("#descriptionTemplateNomenclatureEdit").val();
    var guidId = $("#GuidIdDetails").val();
    var modeSaisie = $("#ModeSaisie").val();

    if (branche == "" || branche == undefined) {
        $("#drlBrancheEdit").addclass("requiredField");
        isCheck = false;
    }
    if (cible == "" || cible == undefined) {
        $("#drlCibleEdit").addclass("requiredField");
        isCheck = false;
    }
    if (libelle == "" || libelle == undefined) {
        $("#descriptionTemplateNomenclatureEdit").addclass("requiredField");
        isCheck = false;
    }

    if (isCheck) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTemplateNomenclature/EnregistrerTemplateNomenclature",
            data: { modeSaisie: modeSaisie, guidId: guidId, branche: branche, cible: cible, libelle: libelle },
            success: function (data) {
                $("#divNomenclaturesBody").html(data);
                MapElementsTableau();
                $("#VoletDetails").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//-----------Lance la recherche des templates nomenclature------------
function RechercherTemplates() {
    var descrRecherche = $.trim($("#rechercheDescription").val());
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamTemplateNomenclature/RechercheTemplatesNomenclature",
        data: { descriptionTemplate: descrRecherche },
        success: function (data) {
            $("#divNomenclaturesBody").html(data);
            MapElementsTableau();
            $("#VoletDetails").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------fonction qui récupère les détails du template nomenclature (nouveau ou sélectionné)--------
function AfficherDetailsTemplate(idLigne) {
    if (idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTemplateNomenclature/GetDetailsTemplateNomenclature",
            data: { idTemplate: idLigne },
            success: function (data) {
                $("#divNomenclatureDetails").html(data);
                MapElementsDetails();
                $("#VoletDetails").show();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------------Application de la recherche ajax des cibles par branche----------------------
function LoadListCibleByBranche() {
    $("#drlBrancheEdit").die().live('change', function () {
        AffectTitleList($(this));
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTemplateNomenclature/GetCibles",
            data: { codeBranche: $(this).val() },
            success: function (data) {
                $("#divCibles").html(data);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}


function Annuler() {
    window.location.href = "/BackOffice/Index";
}