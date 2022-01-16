$(document).ready(function () {
    AlternanceLigne("CourtiersBody", "", false, null);
    MapElementPageCoCourtier();
});
//------ fonction qui affiche la div flottante---------------
function ShowPopup(e) {
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var codeCourtier = 0;
    if (e != "") codeCourtier = e;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnCourtier/LoadCourtier",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, codeCourtier: codeCourtier, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataFullScreenCourtier").html(data);
            AlbScrollTop();
            $("#divFullScreenCourtier").show();
            FormatDecimalNumericValue();
            MapCommonAutoCompCourtier();
            BtnClick();
            AlternanceLigne("CourtiersBody", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------------Ajouter,modifier un courtier sur la div flottante----------------------
function BtnClick() {
    $('#btnAjouter').kclick(function () {
        UpdateBddCourtier('I');
    });
    $('#btnModifier').kclick(function () {
        UpdateBddCourtier('U');
    });
    $('#btnAnnulerDivFlottante').kclick(function () {
        ReactivateShortCut();
        $("#divDataFullScreenCourtier").clearHtml();
        $("#divFullScreenCourtier").hide();
    });
}
//-------------------------Action des Buttons
function MapElementPageCoCourtier() {
    //gestion de l'affichage de l'écran en mode readonly
    $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    if (window.isReadonly && $('#IsModifHorsAvn').val() === 'False') {
        $("img[name=supprCourtier]").hide();
        $("img[name=modifCourtier]").hide();
        $("img[name=ajoutCourtier]").hide();

        $("#chkTauxCAT").attr('disabled', 'disabled');
        $("#chkTauxHCAT").attr('disabled', 'disabled');

    }
    else {
        if ($("#commentairesCommissionCourtier").attr("id") != undefined)
            $("#commentairesCommissionCourtier").html($("#commentairesCommissionCourtier").html().replace(/&lt;br&gt;/ig, "\n"));
    }


    MapElementTableauCoCourtier();

    $("#btnAfficherActionCourtier").die().live('click', function () {
        ShowPopup("");
    });

    if ($("#ModeAffichage").val() == "Standard") {
        $("#btnAnnuler").die().live('click', function () {
            if (!CheckCommission())
                return false;
            ShowCommonFancy("Confirm", "Cancel",
            "Voulez-vous revenir à l'écran précédent ?<br/>",
            258, 130, true, true);
        });
        $("#btnConfirmOk").die().live('click', function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    Precedent();
                    break;
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancel").die().live('click', function () {
            CloseCommonFancy();
            $("#hiddenAction").val('');
        });

        $("#btnSuivant").kclick(function (evt, data) {
            SumbitPage(data && data.returnHome, $("#ModeAffichage").val());
        });
    }

    if ($('#IsModifHorsAvn').val() === 'True') {
        $("#chkTauxHCAT").removeAttr("disabled");
        $("#chkTauxCAT").removeAttr("disabled");
    }

    $("#chkTauxHCAT").unbind();
    $("#chkTauxHCAT").bind('click', function () {
        if ($("#chkTauxHCAT").is(':checked')) {
            $("#tauxContratHCAT").attr('disabled', 'disabled');
            $("#tauxContratHCAT").addClass("ReadOnly");
            $("#tauxContratHCAT").val($("#tauxStandardHCAT").val());
            if ($("#chkTauxCAT").is(':checked')) {
                $("#divCommentaires").hide();
            }
            else {
                $("#divCommentaires").show();
            }
        }
        else {
            $("#tauxContratHCAT").removeAttr("disabled");
            $("#tauxContratHCAT").removeClass("ReadOnly");
            $("#divCommentaires").show();
        }


        if (!$("#chkTauxCAT").is(':checked') || !$("#chkTauxHCAT").is(':checked')) {
            $("#commentairesCommissionCourtier").removeClass("ReadOnly");
            $("#commentairesCommissionCourtier").removeAttr("disabled");
        }
        else {
            $("#commentairesCommissionCourtier").addClass("ReadOnly");
            $("#commentairesCommissionCourtier").attr("disabled", "disabled");
        }
    });

    $("#chkTauxCAT").unbind();
    $("#chkTauxCAT").bind('click', function () {
        if ($("#chkTauxCAT").is(':checked')) {
            $("#tauxContratCAT").attr('disabled', 'disabled');
            $("#tauxContratCAT").addClass("ReadOnly");
            $("#tauxContratCAT").val($("#tauxStandardCAT").val());
            if ($("#chkTauxHCAT").is(':checked')) {
                $("#divCommentaires").hide();
            }
            else {
                $("#divCommentaires").show();
            }
        }
        else {
            $("#tauxContratCAT").removeAttr("disabled");
            $("#tauxContratCAT").removeClass("ReadOnly");
            $("#divCommentaires").show();
        }

        if (!$("#chkTauxCAT").is(':checked') || !$("#chkTauxHCAT").is(':checked')) {
            $("#commentairesCommissionCourtier").removeClass("ReadOnly");
            $("#commentairesCommissionCourtier").removeAttr("disabled");
        }
        else {
            $("#commentairesCommissionCourtier").addClass("ReadOnly");
            $("#commentairesCommissionCourtier").attr("disabled", "disabled");
        }
    });

    if (!$("#chkTauxCAT").is(':checked') || !$("#chkTauxHCAT").is(':checked')) {
        $("#divCommentaires").show();
    }
    else {
        $("#divCommentaires").hide();
    }

    FormatDecimalNumericValue();
    UpdateSumCommission();
    toggleDescription($("#commentairesCommissionCourtier"), true);
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
}

function MapElementTableauCoCourtier() {
    $("img[name=supprCourtier]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr("id"));
            DesactivateShortCut();
            SupprimerCourtier($("#hiddenInputId").val());
        });
    });
    $("img[name=modifCourtier]").each(function () {
        $(this).click(function () {
            EditCourtier($(this));
        });
    });
}
//-----------Mise à jour de la commission--------------
function UpdateSumCommission() {
    var sumCommission = $("#tblCourtiersBody tr:last").attr("albSumCommission");
    $("#TotalCom").val(sumCommission);
    common.autonumeric.apply($("#TotalCom"), 'update', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
}
//------------------fonction qui met à jour un courtier (ajout ou modification)---------
function UpdateBddCourtier(typeOperation) {
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var typeContrat = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var codeCourtier = $("#CodeCourtier").val();
    var commission = $("#Commission").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();


    if (typeOperation == 'I') {
        if (codeCourtier == "") {
            $("#CodeCourtier").addClass('requiredField');
            return false;
        }
        if (!$('#CourtierInvalideDiv').is(':empty')) {
            return false;
        }
    }
    if ($("#Commission").val() == "") {
        $("#Commission").addClass('requiredField');
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AnCourtier/UpdateCourtier",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat, tabGuid: tabGuid, codeCourtier: codeCourtier, commission: commission, typeOperation: typeOperation, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) {
            UpdateListCourtiers();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------------fonction qui supprime un courtier selectionné-----------------
function SupprimerCourtier(e) {
    ShowCommonFancy("Confirm", "Suppr",
              "Etes-vous sûr de vouloir supprimer ce co-courtier ?",
              350, 130, true, true);
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        ReactivateShortCut();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                var codeContrat = $("#Offre_CodeOffre").val();
                var versionContrat = $("#Offre_Version").val();
                var typeContrat = $("#Offre_Type").val();
                var tabGuid = $("#tabGuid").val();
                var addParamType = $("#AddParamType").val();
                var addParamValue = $("#AddParamValue").val();

                $.ajax({
                    type: "POST",
                    url: "/AnCourtier/SupprimerCourtier",
                    data: { codeCourtier: e, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
                    success: function (data) {
                        UpdateListCourtiers();
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
                $("#hiddenInputId").val('');
                break;
        }
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        ReactivateShortCut();
    });

}
function EditCourtier(e) {
    ShowPopup(e.attr("id"));
}
//----------------------fonction qui met à jour la liste de courtiers----------------------
function UpdateListCourtiers() {
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    $.ajax({
        type: "POST",
        url: "/AnCourtier/UpdateListCourtiers",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            ReactivateShortCut();
            $("#tblCourtiersBody").html(data);
            AlternanceLigne("CourtiersBody", "", false, null);
            $("#divDataFullScreenCourtier").html('');
            $("#divFullScreenCourtier").hide();
            MapElementTableauCoCourtier();
            UpdateSumCommission();
            FormatDecimalNumericValue();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Redirection------------------
function Redirection(cible, job, readonlyDisplay) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnCourtier/Redirection/",
        data: {
            cible: cible,
            job: job,
            codeContrat: $("#Offre_CodeOffre").val(),
            versionContrat: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val(),
            addParamType: $("#AddParamType").val(),
            addParamValue: $("#AddParamValue").val(),
            readonlyDisplay: readonlyDisplay,
            isModeConsultationEcran: $("#IsModeConsultationEcran").val()
        },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Precedent() {
    ShowLoading();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/AnCourtier/Precedent/",
        data: { codeContrat: $("#Offre_CodeOffre").val(), versionContrat: $("#Offre_Version").val(), type: $("#Offre_Type").val(), tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, isModeConsultationEcran: $("#IsModeConsultationEcran").val() },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Submit la page--------
function SumbitPage(returnHome, modeAffichage) {
    if (!CheckCommission())
        return false;
    suivant(returnHome,modeAffichage);
}
function suivant(returnHome, modeAffichage) {
    let valCourtier = $("#commentairesCommissionCourtier").val();
    if (valCourtier == undefined) {
        valCourtier = "";
    }
    let codeContrat = $("#Offre_CodeOffre").val();
    let versionContrat = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let tabGuid = $("#tabGuid").val();
    let risqueObjet = $("#RisqueObj").val();
    let txtSaveCancel = $("#txtSaveCancel").val();
    let tauxStdHCAT = $("#tauxStandardHCAT").val();
    let tauxStdCAT = $("#tauxStandardCAT").val();
    let tauxContratHCAT = $("#tauxContratHCAT").val();
    let tauxContratCAT = $("#tauxContratCAT").val();
    let commentaire = valCourtier;
    let codeEncaissementContrat = $("#CodeEncaissementContrat").val();
    let modeNavig = $("#ModeNavig").val();
    let addParamType = $("#AddParamType").val();
    let addParamValue = $("#AddParamValue").val();
    let acteGestion = $("#ActeGestion").val();

    if (codeEncaissementContrat == "D") {
        tauxStdHCAT = 0;
        tauxStdCAT = 0;
        tauxContratHCAT = 0;
        tauxContratCAT = 0;
        commentaire = "";
    }

    if (!window.isReadonly) {
        if ((!$("#chkTauxHCAT").is(":checked") || !$("#chkTauxCAT").is(":checked")) && valCourtier == "" && $("#CodeEncaissementContrat").val() != "D"
            && $("#divCommentaires").is(":visible")) {
            $("div[id='zoneTxtArea'][albcontext='commentairesCommissionCourtier']").addClass("requiredField");
            return;
        }

        if ($("#divCommentaires").is(":visible") && valCourtier == "") {
            $("div[id='zoneTxtArea'][albcontext='commentairesCommissionCourtier']").addClass("requiredField");
            return;
        }
    }

    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/AnCourtier/Suivant",
        data: {
            codeContrat: codeContrat, versionContrat: versionContrat, type: type, codeAvn: codeAvn, tabGuid: tabGuid, risqueObjet: risqueObjet, txtSaveCancel: returnHome ? 1 : 0,
            modeAffichage: modeAffichage, tauxStdHCAT: tauxStdHCAT, tauxStdCAT: tauxStdCAT, tauxContratHCAT: tauxContratHCAT, tauxContratCAT: tauxContratCAT,
            commentaire: commentaire, codeEncaissementContrat: codeEncaissementContrat, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig,
            hasRisques: $("#HasRisques").val(), addParamType: addParamType, addParamValue: addParamValue, isModeConsultationEcran: $("#IsModeConsultationEcran").val(),
            acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: $("#ReguleId").val()
        },
        success: function (data) {
            if (modeAffichage == "Popup") {
                if ($("#divCommissionsQuittance").attr("id") != undefined) {
                    $("#divCommissionsQuittance").html(data);
                    $("div[id=zoneTxtArea][albContext=CommentForce").html(commentaire);
                    $("#CommentForce").html(commentaire);

                    AlternanceLigne("BodyCommission", "", false, null);
                    FormatDecimalValue();
                    ReloadCotisation(true, false);
                }
                else if ($("#divCommentForce").attr("id") != undefined) {
                    $("div[id=zoneTxtArea][albContext=CommentForce").html(commentaire);
                    $("#CommentForce").html(commentaire);
                    $("#txtAreaLnk").trigger("click").trigger("click");
                }
                common.page.isLoading = false;
            }
            $("#divDataCoCourtiers").clearHtml();
            $("#divCoCourtiers").hide();
            ReactivateShortCut();
            toggleDescription($("#CommentForce"), true);
        },
        error: function (request) {
            let result = null;
            try {
                result = JSON.parse(request.responseText);
            } catch (e) { result = null; }

            if (!kheops.alerts.blacklist.displayAll(result)) {
                common.error.showXhr(request);
            }
            common.page.isLoading = false;
        }
    });
}

function CheckCoCourtiers() {
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();
    var risqueObjet = $("#RisqueObj").val();
    var txtSaveCancel = $("#txtSaveCancel").val();
    var modeAffichage = $("#ModeAffichage").val();
    var tauxStdHCAT = $("#tauxStandardHCAT").val();
    var tauxStdCAT = $("#tauxStandardCAT").val();
    var tauxContratHCAT = $("#tauxContratHCAT").val();
    var tauxContratCAT = $("#tauxContratCAT").val();
    var commentaire = $("#commentairesCommissionCourtier").val();
    var codeEncaissementContrat = $("#CodeEncaissementContrat").val();

    var modeAffichage = $("#ModeAffichage").val();

    if (codeEncaissementContrat == "D") {
        tauxStdHCAT = 0;
        tauxStdCAT = 0;
        tauxContratHCAT = 0;
        tauxContratCAT = 0;
        commentaire = "";
    }

    if (($("#CodeEncaissementContrat").val() != "D") && (modeAffichage == "Popup")) {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var acteGestion = $("#ActeGestion").val();
        $.ajax({
            type: "POST",
            url: "/Quittance/CheckCoCourtier/",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, isReadonly: $("#OffreReadOnly").val(), modeNavig: $("#ModeNavig").val(),
                acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
                var message = data;
                if (message != "" && message != undefined) {
                    if (message.split("WARNING").length > 1) {
                        ShowCommonFancy("Confirm", "Warning",
                          message.split("WARNING")[1] + "<br/><br/>Etes-vous sûr de vouloir continuer ?<br/>",
                          350, 130, true, true);
                        return false;
                    }
                    if (message.split("ERREUR").length > 1) {
                        ShowCommonFancy("Error", "", message.split("ERREUR")[1], 1212, 700, true, true);
                        $("div[id=zoneTxtArea][albcontext=commentairesCommissionCourtier]").addClass('requiredButton');
                        return false;
                    }
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "/AnCourtier/Suivant/",
                        data: {
                            codeContrat: codeContrat, versionContrat: versionContrat, type: type, tabGuid: tabGuid, risqueObjet: risqueObjet, txtSaveCancel: txtSaveCancel,
                            modeAffichage: modeAffichage, tauxStdHCAT: tauxStdHCAT, tauxStdCAT: tauxStdCAT, tauxContratHCAT: tauxContratHCAT, tauxContratCAT: tauxContratCAT,
                            commentaire: commentaire, codeEncaissementContrat: codeEncaissementContrat, paramRedirect: $("#txtParamRedirect").val(), acteGestion: acteGestion,
                            acteGestionRegule: $("#ActeGestionRegule").val()
                        },
                        success: function (data) {
                            $("#divDataCoCourtiers").html('');
                            $("#divCoCourtiers").hide();
                            toggleDescription($("#CommentForce"), true);
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
            },
            error: function (request) {
                common.error.showXhr(request);
                return false;
            }
        });
    }
    else {
        var acteGestion = $("#ActeGestion").val();
        $.ajax({
            type: "POST",
            url: "/AnCourtier/Suivant/",
            data: {
                codeContrat: codeContrat, versionContrat: versionContrat, type: type, tabGuid: tabGuid, risqueObjet: risqueObjet, txtSaveCancel: txtSaveCancel,
                modeAffichage: modeAffichage, tauxStdHCAT: tauxStdHCAT, tauxStdCAT: tauxStdCAT, tauxContratHCAT: tauxContratHCAT, tauxContratCAT: tauxContratCAT,
                commentaire: commentaire, codeEncaissementContrat: codeEncaissementContrat, paramRedirect: $("#txtParamRedirect").val(), acteGestion: acteGestion,
                acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
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
}

//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    common.autonumeric.applyAll('init', 'pourcentnumeric', '', null, null, '100', '0');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
}

//--------Contrôle du total du commissionnement---------
function CheckCommission() {
    $(".requiredField").removeClass("requiredField");
    var erreurBool = false;
    if ($("#TotalCom").val() != "100,00")
        erreurBool = true;
    if (erreurBool && !window.isReadonly && ($("#CodeEncaissementContrat").val() != "D" && $("#CodeEncaissementContrat").val() != "")) {
        common.dialog.bigError("La somme des % de commissions doit être égale à 100", true);
        return false;
    }
    return true;
}
