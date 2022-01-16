/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();
});

//----------Map les éléments principaux de la vue-------
function MapElementPage() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("input[type='radio'][name='radioType']").each(function () {
        $(this).change(function () {
            ChangeDisplayFiltre();
        });
    });
    $("#dvExpandCollapse").die().live('click', function () {
        ExpandCollapseRechAvancee();
    });
    $("#btnFiltrer").die().live('click', function () {
        SearchDoc();
    });
    $("#btnRAZ").die().live('click', function () {
        ResetFiltre();
    });
    MapListDocuments();

    FormatNumericField();

    $("#btnAnnulerSuiviDoc").die().live('click', function () {
        CloseSuiviDoc();
    });

    $("#btnReeditSuiviDoc").die().live('click', function () {
        ReeditSuiviDoc();
    });

    $("#btnConfirmCancelSuiviDoc").die().live('click', function () {
        $("#divMsgSuiviDoc").hide();
    });
    $("#btnConfirmGenSuiviDoc").die().live('click', function () {
        GenerateSuiviDoc();
    });
    $("#btnConfirmOkSuiviDoc").die().live('click', function () {
        if ($(this).val() == "generate")
            PrintSuiviDoc();
        if ($(this).val() == "reedit")
            ReeditionSuiviDoc();
    });

    $("img[name='pjdoc']").each(function () {
        $(this).click(function () {
            OpenDivPJ($(this).attr("albdocid"));
        });
    });
}
//----------Map les éléments de la liste des documents-----
function MapListDocuments() {
    AlternanceLigne("BodyLstDoc", "", false, null);
    $("#dvBodyLstDoc").bind('scroll', function () {
        $("#dvHeaderLstDoc").scrollLeft($(this).scrollLeft());
    });

    $("td[name='tdLot']").each(function () {
        var lotId = $(this).attr('alblotid');
        var countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='N']").length;
        if (countN > 0)
            $(this).addClass("situationRed");
        else {
            countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='Z']").length;
            if (countN > 0)
                $(this).addClass("situationRed");
            else {
                countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='A']").length;
                if (countN > 0)
                    $(this).addClass("situationBlue");
            }
        }
    });

    $("td[name='tdNomDoc']").each(function () {
        $(this).click(function () {
            OpenDoc($(this));
        });
    });

    $("td[albIdDest]").each(function () {
        $(this).mouseover(function () {
            ShowInfoDestinataire($(this));
        });
        $(this).mouseout(function () {
            $("#divDestinataire").html("").hide();
        });
    });

    $("#PaginationDocPremierePage").die().live('click', function () {
        alert('first');
    });

    $("#PaginationDocPrecedent").die().live('click', function () {
        alert('previous');
    });

    $("#PaginationDocSuivant").die().live('click', function () {
        alert('next');
    });

    $("#PaginationDocDernierePage").die().live('click', function () {
        alert('last');
    });

}
//---------Formate les champs numériques----------
function FormatNumericField() {
    common.autonumeric.apply($("#inFiltreVersion"), "destroy");
    common.autonumeric.apply($("#inFiltreVersion"), "init", "numeric", "", null, null, "9999", "0");
    common.autonumeric.apply($("#inFiltreAvenant"), "destroy");
    common.autonumeric.apply($("#inFiltreAvenant"), "init", "numeric", "", null, null, "999", "0");
    common.autonumeric.apply($("#inFiltreLot"), "destroy");
    common.autonumeric.apply($("#inFiltreLot"), "init", "numeric", "", null, null, "999999999", "0");
}
//---------Change l'affichage des filtres----------
function ChangeDisplayFiltre() {
    if ($("#radioGen").is(":checked")) {
        $("#dvGenerale").show();
        $("#dvFiltreUser").show();
        $("#dvFiltreTypeAff").show();
        $("#dvFiltreNumOffre").show();
    }
    if ($("#radioAff").is(":checked")) {
        if ($("#imgExpandCollapse").attr("albaction") == "expand") {
            $("#dvGenerale").hide();
            $("#dvFiltreUser").hide();
        }
        $("#dvFiltreTypeAff").hide();
        $("#dvFiltreNumOffre").hide();
    }
}
//---------Dépile/Rempile la div de recherche avancée------
function ExpandCollapseRechAvancee() {
    var nextAction = $("#imgExpandCollapse").attr("albaction");
    switch (nextAction) {
        case "expand":
            $("#imgExpandCollapse").attr("src", "/Content/Images/collapse.png");
            $("#imgExpandCollapse").attr("albaction", "collapse");
            $("#dvRechAvancee").show();
            if ($("#radioAff").is(":checked")) {
                $("#dvGenerale").show();
                $("#dvFiltreUser").show();
            }
            break;
        case "collapse":
            $("#imgExpandCollapse").attr("src", "/Content/Images/expand.png");
            $("#imgExpandCollapse").attr("albaction", "expand");
            $("#dvRechAvancee").hide();
            if ($("#radioAff").is(":checked")) {
                $("#dvGenerale").hide();
                $("#dvFiltreUser").hide();
            }
            break;
    }
}
//---------Lance la recherche en prenant en compte les filtres-------
function SearchDoc(pageNumber) {
    if (pageNumber = undefined || pageNumber == 0) {
        pageNumber = 1;
    }

    $(".requiredField").removeClass("requiredField");
    var errorParam = false;

    var dateDebSituation = 0;
    var dateFinSituation = 0;
    if ($("#inFiltreSituationDeb").val() != "" && isDate($("#inFiltreSituationDeb").val())) {
        var dateDebS = getDate($("#inFiltreSituationDeb").val());
        dateDebSituation = dateDebS.getFullYear() * 10000 + (dateDebS.getMonth() + 1) * 100 + dateDebS.getDate();
    }
    if ($("#inFiltreSituationFin").val() != "" && isDate($("#inFiltreSituationFin").val())) {
        var dateFinS = getDate($("#inFiltreSituationFin").val());
        dateFinSituation = dateFinS.getFullYear() * 10000 + (dateFinS.getMonth() + 1) * 100 + dateFinS.getDate();
    }
    if (dateDebSituation > 0 && dateFinSituation > 0 && getDate($("#inFiltreSituationDeb").val()) > getDate($("#inFiltreSituationFin").val())) {
        $("#inFiltreSituationDeb").addClass("requiredField");
        $("#inFiltreSituationFin").addClass("requiredField");
        errorParam = true;
    }

    var dateDebEdition = 0;
    var dateFinEdition = 0;
    if ($("#inFiltreEditionDeb").val() != "" && isDate($("#inFiltreEditionDeb").val())) {
        var dateDebE = getDate($("#inFiltreEditionDeb").val());
        dateDebEdition = dateDebE.getFullYear() * 10000 + (dateDebE.getMonth() + 1) * 100 + dateDebE.getDate();
    }
    if ($("#inFiltreEditionFin").val() != "" && isDate($("#inFiltreEditionFin").val())) {
        var dateFinE = getDate($("#inFiltreEditionFin").val());
        dateFinEdition = dateFinE.getFullYear() * 10000 + (dateFinE.getMonth() + 1) * 100 + dateFinE.getDate();
    }
    if (dateDebEdition > 0 && dateDebEdition > 0 && getDate($("#inFiltreEditionDeb").val()) > getDate($("#inFiltreEditionFin").val())) {
        $("#inFiltreEditionDeb").addClass("requiredField");
        $("#inFiltreEditionFin").addClass("requiredField");
        errorParam = true;
    }

    if (errorParam)
        return false;

    var numInterneAvn = $("#inFiltreAvenant").val();
    if (numInterneAvn == undefined || numInterneAvn == "") {
        numInterneAvn = -1;
    }

    var filtre = {
        CodeOffre: $("#radioGen").is(":checked") ? $("#inFiltreNumOffreContrat").val() : $("#inCodeOffre").val(),
        Version: $("#radioGen").is(":checked") ? $("#inFiltreVersion").val() : $("#inVersion").val(),
        Type: $("#radioGen").is(":checked") ? $("input[type='radio'][name='inFiltreType']:checked").val() : $("#inType").val(),
        User: $("#radioGen").is(":checked") ? $("#inFiltreUser").val() : "",
        Avenant: numInterneAvn,
        NumLot: $("#inFiltreLot").val(),
        Situation: $("#inFiltreSituation").val(),
        DateDebSituation: dateDebSituation,
        DateFinSituation: dateFinSituation,
        UniteService: $("#dvRechAvancee").is(":visible") ? $("#inFiltreUnite").val() : "",
        DateDebEdition: $("#dvRechAvancee").is(":visible") ? dateDebEdition : 0,
        DateFinEdition: $("#dvRechAvancee").is(":visible") ? dateFinEdition : 0,
        TypeDestinataire: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeDestinataire").val() : "",
        CodeDestinataire: $("#dvRechAvancee").is(":visible") ? $("#inFiltreCodeDestinataire").val() : 0,
        CodeInterlocuteur: $("#dvRechAvancee").is(":visible") ? $("#inFiltreCodeInterlocuteur").val() : "",
        TypeDoc: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeDoc").val() : "",
        CourrierType: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeCour").val() : "",
        PageNumber: pageNumber
    };
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/SearchDoc",
        data: { displayType: $("input[type='radio'][name='radioType']:checked").val(), filtre: JSON.stringify(filtre), modeNavig: $("#ModeNavig").val(), readOnly: $("#OffreReadOnly").val() },
        success: function (data) {
            $("#dvListDoc").html(data);
            MapListDocuments();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    })
}
//---------Réinitialise les filtres------------
function ResetFiltre() {
    $("#inFiltreUser").val("");
    $("#inFiltreTypeOffre").attr('checked', false);
    $("#inFiltreTypeContrat").attr('checked', false);
    $("#inFiltreNumOffreContrat").val("");
    $("#inFiltreAvenant").val("");
    $("#inFiltreLot").val("");
    $("#inFiltreSituation").val("");
    $("#inFiltreSituationDeb").val("");
    $("#inFiltreSituationFin").val("");
    $("#inFiltreUnite").val("");
    $("#inFiltreEditionDeb").val("");
    $("#inFiltreEditionFin").val("");
    $("#inFiltreTypeDestinataire").val("");
    $("#inFiltreCodeDestinataire").val("0");
    $("#inFiltreDestinataire").val("");
    $("#inFiltreCodeInterlocuteur").val("0");
    $("#inFiltreInterlocuteur").val("");
    $("#inFiltreTypeDoc").val("");
    $("#inFiltreTypeCour").val("");
}
//---------Ouvre le document-----------------
function OpenDoc(elem) {
    ShowLoading();
    var nomDoc = elem.attr('albdocnom');
    var cheminDoc = elem.attr('albdocpath');

    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenDoc",
        data: { docPath: cheminDoc, docName: nomDoc },
        success: function (data) {
            common.dialog.info("En cours de développement");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Affiche les informations du destinataire---------
function ShowInfoDestinataire(elem) {
    var idDest = elem.attr("albIdDest");
    var typeDest = elem.attr("albTypeDest");
    var typeEnvoi = elem.attr("albTypeEnvoi");

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/ShowInfoDest",
        data: { idDest: idDest, typeDest: typeDest, typeEnvoi: typeEnvoi },
        success: function (data) {
            $("#divDestinataire").html(data);
            var pos = elem.position();
            $("#divDestinataire").css({ "position": "absolute", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
            $("#divDestinataire").show();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Réédition du suivi des documents----------
function ReeditSuiviDoc() {
    $("#msgSuiviDocInfoReedit").hide();
    $("#msgSuiviDocInfoGenerateLot").hide();
    $("#msgSuiviDocInfoPrintLot").hide();
    $("#btnConfirmOkSuiviDoc").hide().val("");
    $("#btnConfirmGenSuiviDoc").hide().val("");

    var idDoc = "";
    var codeLot = "";
    var libLot = "";
    var situationDoc = "";
    var nomDoc = "";
    var typeDoc = "";
    var typeCodeDoc = "";
    var typeDest = "";
    var codeDest = "";
    var nomDest = ""

    var countLotCheck = $("input[type='checkbox'][name='checkLot']:checked").length;
    if (countLotCheck == 0) {
        common.dialog.error("Veuillez sélectionner au moins un document à rééditer");
        return false;
    }

    var countLotN = $("input[type='checkbox'][name='checkLot'][albsituation='N']:checked").length;
    var countLotZ = $("input[type='checkbox'][name='checkLot'][albsituation='Z']:checked").length;
    var countLotA = $("input[type='checkbox'][name='checkLot'][albsituation='A']:checked").length;
    if (countLotN > 0 || countLotZ > 0) {
        var msg = $("#msgSuiviDocInfoGenerateLot").html();
        idDoc = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albdocid");

        codeLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotid");
        libLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotlib");

        msg = msg.replace("xxLotxx", codeLot);
        msg = msg.replace("xxLotLibxx", libLot);

        $("#btnConfirmGenSuiviDoc").show();
        $("#msgSuiviDocInfoGenerateLot").html(msg).show();
    }
    else if (countLotA > 0) {
        var msg = $("#msgSuiviDocInfoPrintLot").html();
        idDoc = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albdocid");

        codeLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotid");
        libLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotlib");

        msg = msg.replace("xxLotxx", codeLot);
        msg = msg.replace("xxLotLibxx", libLot);

        $("#btnConfirmOkSuiviDoc").val("generate").show();
        $("#msgSuiviDocInfoPrintLot").html(msg).show();
    }
    else {
        if (countLotCheck > 1) {
            common.dialog.error("Veuillez ne sélectionner qu'un document pour la réédition");
            return false;
        }

        var msg = $("#msgSuiviDocInfoReedit").html();

        situationDoc = $("input[type='checkbox'][name='checkLot']:checked").attr("albsituation");
        idDoc = $("input[type='checkbox'][name='checkLot']:checked").attr("albdocid");
        nomDoc = $("td[name='tdNomDoc'][albdocid='" + idDoc + "']").attr("albdocnom");
        typeCodeDoc = $("td[name='tdTypeDoc'][albdocid='" + idDoc + "']").attr("albcodetypedoc");
        typeDoc = $("td[name='tdTypeDoc'][albdocid='" + idDoc + "']").attr("albtypedoc");
        typeDest = $("td[name='tdDestDoc'][albdocid='" + idDoc + "']").attr("albtypedest");
        codeDest = $("td[name='tdDestDoc'][albdocid='" + idDoc + "']").attr("albtypedest");
        nomDest = $("td[name='tdDestDoc'][albdocid='" + idDoc + "']").attr("albnomdest");

        msg = msg.replace("xxNomDocxx", nomDoc);
        msg = msg.replace("xxCodeTypeDocxx", typeCodeDoc);
        msg = msg.replace("xxTypeDocxx", typeDoc);
        msg = msg.replace("xxTypeDestxx", typeDest);
        msg = msg.replace("xxCodeDestxx", codeDest);
        msg = msg.replace("xxNomDestxx", nomDest);

        $("#btnConfirmOkSuiviDoc").val("reedit").show();
        $("#msgSuiviDocInfoReedit").html(msg).show();
    }


    $("#divMsgSuiviDoc").show();
}
//---------Génération des documents----------
function GenerateSuiviDoc() {
    var idDoc = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albdocid");
    var codeAff = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albnumaff");
    var version = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albvers");
    var type = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albtypaff");
    var avenant = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albavtaff");

    var codeLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotid");

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/GenerateDocuments",
        data: { numAffaire: codeAff, version: version, type: type, avenant: avenant, lotid: codeLot },
        success: function (data) {
            CloseLoading();
            if (data == "True") {
                $("#divMsgSuiviDoc").hide();
                $("#btnFiltrer").trigger('click');
            }
            else
                common.dialog.error("Erreur lors de la génération des documents");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Impression des documents-----------
function PrintSuiviDoc() {
    
}
//----------Réédition des documents-------
function ReeditionSuiviDoc() {
 

    ShowLoading();
   
}
//----------Ouverture de la div des pièces jointes d'un document----------
function OpenDivPJ(docId) {
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenPJ",
        data: { docId: docId },
        success: function (data) {
            $("#divPJDoc").show();
            $("#divDataPJDoc").html(data);
            MapDivPJ();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Map les éléments de la div des PJ------
function MapDivPJ() {
    $("#btnFancyAnnuler").die().live('click', function () {
        $("#divPJDoc").hide();
        $("#divDataPJDoc").html("");
    });
}
//-------Ferme l'écran de suivi de documents-------
function CloseSuiviDoc() {
    $.ajax({
        type: "POST",
        url: "/SuiviDOcuments/Redirection",
        data: { cible: "RechercheSaisie", job: "Index" },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
