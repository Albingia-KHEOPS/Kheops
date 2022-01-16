var ListePbs = function () {
    //--------Map les éléments de la page---------
    this.initPage = function () {
        pbs.liste.init();

        $("#btnFermer").kclick(function () {
            var tabGuid = $("#tabGuid").val();
            DeverouillerUserOffres(tabGuid);
            listePbs.redirect("RechercheSaisie", "Index");
        });

    };

    //----------------Redirection------------------
    this.redirect = function (cible, job, elem) {
        ShowLoading();
        var modeAvt = $("#ModeAvt").val();
        var typAvt = $("#TypeAvt").val();
        var codeContrat = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvenant = $("#NumInterne").val();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var codeAvenantExterne = $("#NumAvt").val();
        var lotID = $("#lotId").val();
        var reguleId = $("#ReguleId").val();
        var codeRsq = $("#CodeRsq").val();
        var dateDebReg = $("#inDateDebRsqRegule").val();
        var dateFinReg = $("#inDateFinRsqRegule").val();
        var codeFor = "";
        var codeOpt = "";
        var idGar = "";
        var codeGAr = "";
        var libgar = "";
        var isReadonly = window.isReadonly ? true : false;
        if (elem != undefined && elem != null) {
            codeFor = elem.attr("albcodefor");
            codeOpt = elem.attr("albcodeopt");
            idGar = elem.attr("albidgar");
            codeGAr = elem.attr("albcodgar");
            libgar = elem.attr("alblibgar");
        }
        var isSaveAndQuit = $("#txtSaveCancel").val() == "1";

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/Redirection/",
            data: {
                cible: cible, job: job, codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
                codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotID: lotID, reguleId: reguleId, codeGAr: codeGAr, libgar: libgar,
                addParamType: addParamType, addParamValue: addParamValue, codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(), typAvt: typAvt,
                acteGestionRegule: $("#ActeGestionRegule").val(), dateDebReg: dateDebReg, dateFinReg: dateFinReg, isReadonly: isReadonly, isSaveAndQuit: isSaveAndQuit
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
};

var listePbs = new ListePbs();

$(function () {
    listePbs.initPage();
});
