/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    if ($("#Contexte").val() != "") {
        Connexite();
    }
});

//--------------initialise la page tampon pour l'ouverture de la div flottante connexité
function Connexite() {
    var codeOffre = $("#CodeContrat").val();
    var version = $("#VersionContrat").val();
    var type = $("#TypeContrat").val();
    var tabGuid = $("#tabGuid").val();
    var id = codeOffre + "_" + version + "_" + type;
    OuvrirConnexites(tabGuid, id);
}

//--------------ouverture de la div flottante connexité--------------
function OuvrirConnexites(tabGuidParam, idParam) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    if (tabGuidParam != undefined)
        var tabGuid = tabGuidParam;
    else
        var tabGuid = $("#tabGuid").val();
    if (idParam != undefined)
        var id = idParam + tabGuid;
    else
        var id = codeOffre + "_" + version + "_" + type + tabGuid
    $.ajax({
        type: "POST",
        url: "/Connexite/OpenConnexites",
        data: { id: id },
        success: function (data) {
            $("#divDataConnexites").html(data);
            AlbScrollTop();
            $("#divConnexites").show();
            MapElementConnexites();
            MapElementPageConnexite();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----Map les éléments de la div flottante de la connexité-----
function MapElementConnexites() {
    //MapConnexites();
    ////gestion de l'affichage de l'écran en mode readonly
    //if (window.isReadonly) {
    //    $("img[name=btnAjouter]").hide();
    //    $("img[name=btnSupprimer]").hide();

    //}

    //AlternanceLigne("EngagementBody", "", false, null);
    //AlternanceLigne("RemplacementBody", "", false, null);
    //AlternanceLigne("InformationBody", "", false, null);
    //AlternanceLigne("ResiliationBody", "", false, null);


    $("#btnFermerConnexite").die().live('click', function () {
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        if ($("#Contexte").val() != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/Connexite/Redirection",
                data: { path: $("#Contexte").val(), parametres: $("#Parametres").val(), existEngCnx: $("#ExistEngCnx").val(), tabGuid: $("#tabGuid").val(), addParamType: addParamType, addParamValue: addParamValue },
                success: function (data) {
                    $("#divDataConnexites").html('');
                    $("#divConnexites").hide();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            $("#divDataConnexites").html('');
            //réactivation des raccourcis de l'écran appelant
            ReactivateShortCut();
            $("#divConnexites").hide();
        }

    });

    $("#btnFermerConnexiteEng").die().live('click', function () {
            $("#divDataConnexitesEng").html('');
            $("#divConnexitesEng").hide();
    });

    //$("img[name=btnSupprimer]").each(function () {
    //    $(this).click(function () {
    //        $("#hiddenInputId").val($(this).attr("id"));
    //        ShowCommonFancy("ConfirmTrans", "Suppr",
    //            "xxxVous allez supprimer la connexité de ce contrat avec les autres contrats. Voulez-vous continuer ?",
    //            350, 130, true, true);
    //    });
    //});
    $("#btnConfirmTransOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                window.SupprimerConnexite($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
        }
    });
    $("#btnConfirmTransCancel").die().live('click', function () {
        CloseCommonFancy();
    });
    MapCommonElement();

    $("#btnGestion").die().live('click', function () {

        OpenConnexitesEng();
    });

    synchronizeScrollbars();

}
