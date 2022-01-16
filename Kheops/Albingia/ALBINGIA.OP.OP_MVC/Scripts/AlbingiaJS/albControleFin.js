/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    Annuler();
    AlternanceLigne("Controles", "Code", true, null);
    Navigation();
    Suivant();
    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    $("span[name='lnkControl']").each(function () {
        $(this).click(function () {
            var redirectLnk = $(this).attr("albhref");
            RedirectLink(redirectLnk);
        });
    });
});
//--------------Redirection Controle-------------
function RedirectLink(param) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ControleFin/RedirectLink",
        data: { param: param },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Traitement Suivant----------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        if (data && data.returnHome) {
            Redirection("RechercheSaisie", "Index");
        }

        if ($("#txtParamRedirect").val() != "") {
            CommonRedirect($("#txtParamRedirect").val());
        }
        
        Redirection("DocumentGestion", "Index");
    });
}

//----------------------Traitement Annuler---------------------
function Annuler() {
    $("#btnAnnuler").click(function () {
        var type = $("#Offre_Type").val();
        if (type == "O")
            Redirection("Cotisations", "Index");
        else if (type == "P")
            if (($("#IsChekedEch").val() == "True") && ($("#ActeGestion").val() == "AVNRS"))
            {
                Redirection("AnnulationQuittances", "Index");
            }
            else {
                Redirection("Quittance", "Index");
            }
    });

    $("#btnRetourRecherche").die().live('click', function () {
        Redirection("RechercheSaisie", "Index");
    });
}

//----------------------Navigation à l'étape suivante sur aucune incohérence---------------------
function Navigation() {
    if ($("#tblControles tr").length == 1 || (window.isReadonly && $("#tblControles tr").length > 1)) {
        Redirection("DocumentGestion", "Index");
    }
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/ControleFin/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------Map les éléments de la div flottante---------
function MapDocDivElement() {
    AlternanceLigne("BodyDocument", "", null, false);
    $("#btnFancyAnnuler").die();
    //$("#btnFancyAnnuler").live('click', function () {
    //    $("#divRegenerateDocument").hide();
    //});
    //$("#btnFancyValider").die();
    //$("#btnFancyValider").live('click', function () {
    //    ValidSupprDoc();
    //});
    $("#checkAll").die().live('change', function () {
        if ($(this).is(":checked")) {
            $("input[type='checkbox'][name='checkDoc']").attr("checked", "checked");
        }
        else {
            $("input[type='checkbox'][name='checkDoc']").removeAttr("checked");
        }
    });
    $("input[type='checkbox'][name='checkDoc']").each(function () {
        $(this).change(function () {
            $("#checkAll").removeAttr("checked");
        });
    });

    $("td[name='tdNomDoc']").each(function () {
        $(this).click(function () {
            ShowLoading();
            var id = $(this).attr("albdocid");
            GetFileNameDocument($("#wDocDocType").val(), id, false, "P");
            CloseLoading();
        });
    });
}

function RefreshListeDocument()
{
    CloseLoading();
}