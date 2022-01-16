
$(document).ready(function () {
    MapElementPage();
    Suivant();
});
//------------- Annule la form ------------------------
function CancelForm() {
    ////Vérifier si le contrat possède une connexité de type engagement
    //if ($("#Offre_Type").val() == "O")
    //    Redirection("Engagements", "Index");
    //else if ($("#Offre_Type").val() == "P") {
    //    if ($("#ExistEngCnx").val() == "True")
    //        Redirection("EngagementsConnexite", "Index");
    //    else Redirection("Engagements", "Index");
    //}
    RedirectionAnnuler();
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
    var commentForce = $("#CommentForce").val();
    $.ajax({
        type: "POST",
        url: "/AttentatGareat/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, commentForce: commentForce },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Redirection Cancel
function RedirectionAnnuler() {
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
        url: "/AttentatGareat/RedirectionAnnuler/",
        data: { codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Map les éléments de la page------------------
function MapElementPage() {

    toggleDescription($("#CommentForce"), true);
    //FormatCLEditor($("#CommentForce"));

    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    $("#CapitauxForces").live("change", function () {
        $("#fieldInput").val("REC");
        $("#valueInput").val($(this).val());
        DisableControl($(this));
        ToggleButton();
    });
    $("#CommissionRet").live("change", function () {
        $("#fieldInput").val("COM");
        $("#valueInput").val($(this).val());
        DisableControl($(this));
        ToggleButton();
    });
    $("#SoumisRet").live("change", function () {
        InitialiazeRetenu();
        ToggleButton();
    });
    $("#btnRefresh").live('click', function () {
        if ($("#divRefresh").hasClass('CursorPointer')) {
            ReloadPage();
        }
        else {
            if ($("#txtSaveCancel").val() == "1")
                Redirection("RechercheSaisie", "Index");
        }
    });
    $("#btnReset").live('click', function () {
        if ($("#divReset").hasClass('CursorPointer'))
            ReloadPage();
    });
    FormatDecimalNumricValue();
    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    else {
        $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));
    }
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumricValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
}
//--------------Grise les contrôles de la page--------------
function DisableControl(elem) {
    $("input").attr('readonly', 'readonly').addClass('readonly');
    $("input[type=checkbox]").attr('disabled', 'disabled').addClass('readonly');
    elem.removeAttr('readonly').removeClass('readonly');
}
//--------------Change l'état des boutons------------------
function ToggleButton() {
    $("#divRefresh").addClass("CursorPointer");
    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
    $("#btnSuivant").attr('disabled', 'disabled');
}
//-------------Rafraichit la page avec les nouvelles données----------
function ReloadPage() {
    if ($("#fieldInput").val() != "") {
        ShowLoading();

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();

        var field = $("#fieldInput").val();
        var value = $("#valueInput").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var acteGestion = $("#ActeGestion").val();
        var codeAvn = $("#NumAvenantPage").val();
        //var modelCommentForce = {
        //    CommentForce: encodeURIComponent($("#CommentForce").val())
        //}
        //var argModelCommentForce = JSON.stringify(modelCommentForce);
        var argModelCommentForce = $("#CommentForce").val();

        if ($("#txtParamRedirect").val() != "") {
            $.ajax({
                type: "POST",
                url: "/AttentatGareat/UpdateRedirect",
                data: { codeOffre: codeOffre, version: version, type: type, codeAvn:codeAvn, field: field, value: value, argModelCommentForce: argModelCommentForce, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, acteGestion: acteGestion },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: "/AttentatGareat/UpdateInPage",
                context: $("#divAttentat"),
                data: { codeOffre: codeOffre, version: version, type: type,codeAvn:codeAvn, field: field, value: value, argModelCommentForce: argModelCommentForce, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), modeNavig: modeNavig, acteGestion: acteGestion },
                success: function (data) {
                    $(this).html(data);
                    if (!window.isReadonly)
                        $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));

                    toggleDescription($("#CommentForce"), true);
                    //FormatCLEditor($("#CommentForce"));
                    FormatDecimalNumricValue();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }

    }
}
//-------------Réinitialise les contrôles retenu--------------
function InitialiazeRetenu() {
    $("#TrancheRet").val("");
    $("#TauxCessionRet").val("0");
    $("#FraisGestionRet").val("0");
    $("#FactureRet").val("0");
    $("#CommissionRet").val("0");
}
//-------------Redirection vers les cotisations-----------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        var type = $("#Offre_Type").val();
        //Obligé de faire un test pour savoir si le bouton est disabled ou non (à cause d'IE)
        if (!$(this).attr('disabled')) {
            if (type == "O")
                Redirection(data && data.returnHome ? "RechercheSaisie" : "Cotisations", "Index");
            else if (type == "P")
                Redirection(data && data.returnHome ? "RechercheSaisie" : "AnMontantReference", "Index");
        }
    });
}
