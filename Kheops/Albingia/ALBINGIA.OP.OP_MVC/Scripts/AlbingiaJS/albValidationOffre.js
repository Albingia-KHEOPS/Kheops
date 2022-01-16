
$(document).ready(function () {
    $("#Observation").html($("#Observation").html().replace(/&lt;br&gt;/ig, "\n"));
    MapElementPage();
    //Numerique();
    Suivant();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    toggleDescription($("#Observation"), true);

    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("img[name=imgDetail]").die().live('click', function () {
        OpenDetailNiv($(this));
    });
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        var isReadOnly = window.isReadonly;
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                if (isReadOnly || $("#ActeGestion").val() == "ATTES") {
                    Redirection("DocumentGestion", "Index");
                }
                else {
                    Redirection("ControleFin", "Index");
                }
                break;
            case 'Valider':
                ValiderOffreContrat('valider');
                break;
            case 'Editer':
                ValiderOffreContrat('editer');
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("#OffreComplete").die().live('change', function () {
        AffectTitleList($(this));
        if ($(this).val() == "Non") {
            $("#Motif").removeClass("readonly").removeAttr("disabled");
            $("#Validable").val($(this).val());
            $("#btnValider").attr("disabled", "disabled");
        }
        else {
            $("#Motif").addClass("readonly").attr("disabled", "disabled");
            $("#Motif").val("");
            if ($("#etatDossier").val() == "A") {
                $("#Validable").val($(this).val());
                $("#btnValider").removeAttr("disabled");

            }
        }

    });

    $("#Motif").die().live('change', function () {
        AffectTitleList($(this));
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    if (window.isReadonly) {

    }

    $("#btnValider").die().live("click", function () {
        VerifPartenaires();
    });

    AffectTitleList($("#Motif"));
    AffectTitleList($("#MotifLabel"));

    //$("#divCriteres").hide();
    AlternanceLigne("DetailsBodyCotisationVisu", "", null, false);

    $("#btnTerminer").die().live('click', function () {

        RetourAccueil();

    });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let tabGuid = $("#tabGuid").val();
    let modeNavig = $("#ModeNavig").val();
    let addParamType = $("#AddParamType").val();
    let addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/ValidationOffre/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Suivant--------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        let context = $(this).attr("albcontext") || "";
        if (context.toLocaleLowerCase() == "terminer" || data && data.returnHome) {
            RetourAccueil();
        }
        else if (context.toLocaleLowerCase() == "editer") {
            OpenEdition("editer");
        }
    });
}
//-------------Ouvre les détails du niveau------------
function OpenDetailNiv(elem) {
    ShowLoading();
    var niveau = elem.attr('id').split('_')[1];
    $.ajax({
        type: "POST",
        url: "/ValidationOffre/OpenDetail/",
        data: { niveau: niveau, modeNavig: $("#ModeNavig").val() },
        context: $("#divDataDetailNiv"),
        success: function (data) {
            DesactivateShortCut();
            $(this).html(data);
            AlbScrollTop();
            $("#detailNiveau").show();
            MapElementDetail();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Map les éléments de la fancy------------
function MapElementDetail() {
    AlternanceLigne("BodyDetail", "", null, false);
    $("#btnFancyAnnuler").click(function () {
        $("#divDataDetailNiv").html("");
        $("#detailNiveau").hide();
        ReactivateShortCut();
    });
}

//---------valide l'offre/le contrat
function ValiderOffreContrat(mode) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();

    var codeObservation = $("#INCodeObservation").val();
    var observation = $("#Observation").val();
    var validable = $("#OffreComplete").val();

    var isComplet = $("#OffreComplete").val();
    var motif = $("#Motif").val();

    var lotsId = "";
    $("input[type='checkbox']:checked").each(function () {
        if ((";" + lotsId + ";").replace(";" + $(this).attr('alblotid') + ";", ";") == (";" + lotsId + ";"))
            lotsId += ";" + $(this).attr('alblotid');
    });
    if (lotsId != "")
        lotsId = lotsId.substring(1);

    var docsId = "";
    $("input[type='checkbox'][name='checkDoc']:checked").each(function () {
        if ((";" + docsId + ";").replace(";" + $(this).attr('albdocid') + ";", ";") == (";" + docsId + ";"))
            docsId += ";" + $(this).attr('albdocid');
    });
    if (docsId != "")
        docsId = docsId.substring(1);

    var avenant = $("#NumAvenantPage").val();
    $.ajax({
        type: "POST",
        url: "/ValidationOffre/ValiderOffreContrat/",
        data: {
            codeOffre: codeOffre, version: version, type: type, avenant: avenant, acteGestion: $("#ActeGestion").val(), tabGuid: tabGuid, codeObservation: codeObservation,
            observation: observation, mode: mode, validable: validable, complet: isComplet, motif: motif, lotsId: lotsId, modeNavig: $("#ModeNavig").val(), addParamValue: $("#AddParamValue").val()
        },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Retourne à l'accueil avec l'offre courante en paramètre----
function RetourAccueil() {
    $(".requiredField").removeClass("requiredField");
    var isCheck = true;

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeObservation = $("#INCodeObservation").val();
    var observation = $("#Observation").val();

    var isComplet = $("#ActeGestion").val() == "REGUL" ? "Oui" : $("#OffreComplete").val();
    var isValidable = $("#Validable").val();
    var motif = $("#Motif").val();

    if (isComplet == "Non" && (motif == undefined || motif == "")) {
        $("#Motif").addClass("requiredField");
        isCheck = false;
    }

    if (isCheck) {
        $.ajax({
            type: "POST",
            url: "/ValidationOffre/RetourAccueil/",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid, codeObservation: codeObservation, observation: observation,
                isComplet: isComplet, motif: motif, isValidable: isValidable, saveCancel: "1", paramRedirect: $("#txtParamRedirect").val(),
                acteGestion: $("#ActeGestion").val(), addParamValue: $("#AddParamValue").val()
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//-----------Ouvre synthèse des documents éditables sans validation--------------
function OpenEdition(modeEcran) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeNavig = $("#ModeNavig").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();

    $.ajax({
        type: "POST",
        url: "/ValidationOffre/LoadDocumentsEditables/",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeEcran: modeEcran, modeNavig: modeNavig,
            acteGestion: $("#ActeGestion").val(), saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), tabGuid: tabGuid, addParam: $("#AddParamValue").val()
        },
        success: function (data) {
            if (!(data.indexOf("window.location") >= 0)) {
                $("#divDataEditionDocument").html(data);
                MapElementEditDoc();
                $("#divEditionDocument").show();
                CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementEditDoc() {
    $("#btnAnnulerEditDoc").die().live("click", function () {
        $("#divDataEditionDocument").html("");
        $("#divEditionDocument").hide();
    });

    $("#btnValiderEditDoc").die().live("click", function () {
        var context = $(this).attr("albcontext");
        var codeOffre = $("#Offre_CodeOffre").val();
        if (context == "editer") {
            ShowCommonFancy("Confirm", "Editer",
                "Vous allez éditer les documents du dossier " + codeOffre + ".<br/>Les documents ci avant vont être édités et sauvegardés<br/>Confirmez-vous l'édition ?<br/>",
                350, 130, true, true);
        }
        else {
            ValiderOffreContrat('valider');
            //ShowCommonFancy("Confirm", "Valider",
            //"Vous allez valider le dossier " + codeOffre + ". Il ne sera plus modifiable.<br/><br/>Confirmez-vous la validation ?<br/>",
            //350, 130, true, true);
        }
    });

    AlternanceLigne("Traite", "", null, false);
    AlternanceLigne("DetailsBodyCotisation", "", null, false);

    $("td[name='tdNomDoc']").each(function () {
        $(this).click(function () {
            ShowLoading();
            var id = $(this).attr("albdocid");
            GetFileNameDocument($("#wDocDocType").val(), id, false, "V", "", "", true);
            CloseLoading();
        });
    });

    common.autonumeric.applyAll("init", "decimal", null, null, null, "999999999.99", "-999999999.99");
}

function VerifPartenaires() {
    var codeAvn = "";
    var modeNavig = "";
    if ($("#NumAvenantPage")) {
        codeAvn = $("#NumAvenantPage").val();
    }
    if ($("#ModeNavig")) {
        modeNavig = $("#ModeNavig").val();
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ValidationOffre/VerifPartenaires",
        data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: codeAvn, modeNavig: modeNavig },
        success: function (data) {
            if (data.success == true || $("#NumAvenantPage").val() != "0") {
                OpenEdition("valider");
            }
            CloseLoading();
        },
        error: function (request) {
            let result = null;
            try {
                result = JSON.parse(request.responseText);
            } catch (e) { result = null; }

            if (!kheops.alerts.blacklist.displayAll(result)) {
                common.error.showXhr(request);
            }
        }
    });
}
//--------Actualise la liste des documents--------
function RefreshListDocEdit() {
    ////ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeNavig = $("#ModeNavig").val();
    var codeAvn = $("#NumAvenantPage").val();
    var modeEcran = $("#ModeEcranDocEdit").val();

    return $.ajax({
        type: "POST",
        url: "/ValidationOffre/RefreshListDocEdit",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: modeNavig, acteGestion: $("#ActeGestion").val(), modeEcran: modeEcran }
    }).then(function (data) {
        $("#divBodyDocument").html(data);
        MapElementEditDoc();
    }).fail(function (request) {
        common.error.showXhr(request, true);
    }
    );
}