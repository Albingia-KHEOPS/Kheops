$(document).ready(function () {
    MapElementPage();
});

//-------------------------Action des Buttons
function MapElementPage(nbReload) {
    $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));
    toggleDescription($("#CommentForce"), true);
    AlternanceLigne("MontantsReferenceBody", "", false, null);
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "MntForce":
                ReloadMontantReference("INITFORCE");
                break;
            case "Cancel":
                Redirection("Engagements", "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "MntForce":
                ReloadMontantReference("FORCE");
                break;
        }
        $("#hiddenAction").val('');
    });


    if (window.isReadonly || window.isModifHorsAvenant) {
        $("img[name=modifMontantFormule]").hide();
        $("img[name=modifMontantTotal]").hide();
    }

    $("img[name=modifMontantFormule]").each(function () {
        $(this).click(function () {
            EditMontantForumle($(this).attr("id"));
        });
    });
    $("img[name=modifMontantTotal]").each(function () {
        $(this).click(function () {
            EditMontantTotal($(this).attr("id"));
        });
    });
    FormatDecimalValue();

    if (nbReload == null && $("#BoolMontantForce").val() == "1") {
        ShowCommonFancy("Confirm", "MntForce", "Voulez-vous effacer les montants forcés ?", 600, 150, true, true, true);
    }

    $("#btnRefresh").die().live('click', function () {
        if ($("#divRefresh").hasClass('CursorPointer')) {
            ReloadPage();
        }
        else {
            if ($("#txtSaveCancel").val() == "1")
                Redirection("RechercheSaisie", "Index");
        }
    });
    $("#btnReset").die().live('click', function () {
        if ($("#divReset").hasClass('CursorPointer'))
            Reset();
    });

    $("#btnSuivant").kclick(function (evt, data) {
        if (window.isHistory) {
            Redirection(data && data.returnHome ? "RechercheSaisie" : "ChoixClauses", "Index");
        }
        else {
            Redirection(data && data.returnHome ? "RechercheSaisie" : "Quittance", "Index");
        }
    });
    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    AffectTitleList($("#Periodicite"));
    AffectTitleList($("#TypeFraisAccessoires"));
}

function ChangeAcquis() {
    var isChecked = $("#chckMntAcquis").is(":checked");
    if (isChecked) {
        $("#MontantAcquis").attr("readonly", "readonly").addClass("readonly");
        var mntForce = parseInt($("#MontantForce").autoNumeric('get'));
        if (mntForce == 0 || $("#MontantForce").val() == "") {
            var mntCalc = parseInt($("#MontantCalcule").autoNumeric('get'));
            $("#MontantAcquis").val(mntCalc);
        }
        else {
            var mntAcqu = parseInt($("#MontantForce").autoNumeric('get'));
            $("#MontantAcquis").val(mntAcqu);
        }
    }
    else {
        $("#MontantAcquis").removeAttr("readonly").removeClass("readonly");
    }
}

function UpdateMntAcquis() {
    var isChecked = $("#chckMntAcquis").is(":checked");
    if (isChecked) {
        var mntForce = parseInt($("#MontantForce").autoNumeric('get'));
        if (mntForce != 0 || $("#MontantForce").val() == "") {
            $("#MontantAcquis").val($("#MontantForce").val());
        }
        else {
            var mntCalc = parseInt($("#MontantCalcule").autoNumeric('get'));
            $("#MontantAcquis").val(mntCalc);
        }
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
    var codeCommentaire = $("#codeCommentaireForce").val();
    var argModelCommentForce = $("#CommentForce").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/AnMontantReference/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, argModelCommentForce: argModelCommentForce, codeCommentForce: codeCommentaire, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------------Affichage de la div flottant de modification du montant de formule
function EditMontantForumle(elem) {
    var tab = elem.split("_");
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRsq = tab[1];
    var codeForm = tab[2];

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnMontantReference/LoadMontantFormule",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), codeRsq: codeRsq, codeForm: codeForm, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataModifMontantFormule").html(data);
            AlbScrollTop();
            $("#divModifMontantFormule").show();
            FormatDecimalValueInit();
            MapElementDivFlottanteMntFormule();
            AlternanceLigne("MontantsReferenceBody", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------------Affichage de la div flottant de modification du montant total
function EditMontantTotal(elem) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnMontantReference/LoadMontantTotal",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataModifMontantTotal").html(data);
            AlbScrollTop();
            $("#divModifMontantTotal").show();
            FormatDecimalValueInitMnt();
            MapElementDivFlottanteMntTotal();
            AlternanceLigne("MontantsReferenceBody", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Ajouter,modifier un courtier sur la div flottante Montant formule----------------------
function MapElementDivFlottanteMntFormule() {
    $("#btnValiderMntFormule").die().live('click', function () {
        ValidMontantFormule();
    });
    $('#btnAnnulerMntFormule').die();
    $('#btnAnnulerMntFormule').live('click', function () {
        ReactivateShortCut();
        $("#divDataModifMontantFormule").html('');
        $("#divModifMontantFormule").hide();
    });
    $("#MontantZero").die().live('change', function () {
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            $("#MontantForce").val(0).attr('readonly', 'readonly').addClass('readonly');
            $("#MontantProvisionnel").attr('disabled', 'disabled').removeAttr('checked');
            $("#MontantProvisionnel").attr('readonly', 'readonly').removeAttr('checked');
        }
        else {
            $("#MontantForce").removeAttr('readonly').removeClass('readonly');
            $("#MontantProvisionnel").removeAttr('disabled');
        }
        common.autonumeric.apply($("#MontantForce"), 'destroy');
        common.autonumeric.apply($("#MontantForce"), 'init', 'decimal', null, null, null, '999999999.99', null);
    });
    $("#MontantForce").die().live('change', function () {
        $("#topMntForce").val(1);
        $("#chckMntAcquis").trigger("change");
    });
    $("#MontantAcquis").die().live('change', function () {
        $("#topMntAcquis").val(1);
    });

    $("#chckMntAcquis").die().live("change", function () {
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            $("#MontantAcquis").attr("readonly", "readonly").addClass("readonly");
            var mntForce = parseFloat($("#MontantForce").autoNumeric('get'));
            if (mntForce == 0 || $("#MontantForce").val() == "") {
                $("#MontantAcquis").val($("#MontantCalcule").val());
            }
            else {
                $("#MontantAcquis").val($("#MontantForce").val());
            }
        }
        else {
            $("#MontantAcquis").removeAttr("readonly").removeClass("readonly");
        }
        common.autonumeric.apply($("#MontantAcquis"), "update", "decimal", null, null, null, "299999999999.97", null);
    });
}
//----------------------Ajouter,modifier un courtier sur la div flottante Montant total----------------------
function MapElementDivFlottanteMntTotal() {
    $("#btnValiderMntTotal").die().live('click', function () {
        ValidMontantTotal();
    });
    $('#btnAnnulerMntTotal').die();
    $('#btnAnnulerMntTotal').live('click', function () {
        ReactivateShortCut();
        $("#divDataModifMontantTotal").html('');
        $("#divModifMontantTotal").hide();
    });
    $("#chckMntAcquis").die().live('click', function () {
        ChangeAcquis();
    });

    $("#MontantForce").die().live('change', function () {
        $("#topMntForceTotal").val(1);
        UpdateMntAcquis();
    });

}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    common.autonumeric.applyAll('init', 'decimal');
    common.autonumeric.applyAll('init', 'numeric', null, null, null, '99999999999', null);
    common.autonumeric.apply($("#spanMntCalcule"), 'init', 'decimal', null, null, null, '299999999999.97', null);
    common.autonumeric.apply($("#spanMntForce"), 'init', 'decimal', null, null, null, '2999999999.97', null);
    common.autonumeric.apply($("#spanTotalAcquis"), 'init', 'decimal', null, null, null, '299999999999', null);
}

function FormatDecimalValueInit() {
    common.autonumeric.apply($("#MontantCalcule"), 'init', 'decimal', null, null, null, '299999999999.97', null);
    common.autonumeric.apply($("#MontantForce"), 'init', 'decimal', null, null, null, '2999999999.97', null);
    common.autonumeric.apply($("#MontantAcquis"), 'init', 'decimal', null, null, null, '2999999999.97', null);
}

function FormatDecimalValueInitMnt() {
    common.autonumeric.apply($("#MontantCalcule"), 'init', 'decimal', null, null, null, '299999999999.97', null);
    common.autonumeric.apply($("#MontantForce"), 'init', 'decimal', null, null, null, '2999999999.97', null);
    common.autonumeric.apply($("#MontantAcquis"), 'init', 'numeric', null, null, null, '2999999999', null);
}
//-------Enlève le format des input/span des valeurs----------
function DestroyFormatDecimalValue() {
    common.autonumeric.applyAll('destroy', 'decimal');
    common.autonumeric.applyAll('destroy', 'numeric');
    common.autonumeric.apply($("#spanMntCalcule"), 'destroy');
    common.autonumeric.apply($("#spanMntForce"), 'destroy');
    common.autonumeric.apply($("#spanTotalAcquis"), 'destroy');
}
//-------Recharge les données des montants--------
function ReloadMontantReference(mode) {
    $.ajax({
        type: "POST",
        url: "/AnMontantReference/ReloadMontant",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(), mode: mode, isReadonly: $("#OffreReadOnly").val(), modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#divMontantRef").html(data);
            MapElementPage(1);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Valide et enregistre les montants de la formule--------
function ValidMontantFormule() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRsq = $("#codeRsq").val();
    var codeForm = $("#codeForm").val();
    var montantCalc = $("#MontantCalcule").val();

    common.autonumeric.apply($("#MontantForce"), 'update', 'decimal', '');
    var montantAcquis = $("#MontantAcquis").val();
    if (montantAcquis == undefined || montantAcquis == "") {
        montantAcquis = 0;
        $("#MontantAcquis").val(0);
    }
    var mntForce = $("#MontantForce").val();
    if (mntForce == undefined || mntForce == "") {
        mntForce = 0;
        $("#MontantForce").val(0);
    }

    if (montantCalc == "0,00" && mntForce != "0,00" && mntForce != undefined) {
        common.dialog.error("Un montant forcé ne peut être indiqué sans avoir renseigné un montant sur les garanties.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AnMontantReference/ValidMontantFormule",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeRsq: codeRsq, codeForm: codeForm,
            mntForce: mntForce, mntProvi: $("#MontantProvisionnel").is(':checked'),
            mntAcquis: montantAcquis, chkMntAcquis: $("#chckMntAcquis").is(":checked")
        },
        success: function (data) {
            var elemForce = $("#mntForce_" + codeRsq + "_" + codeForm);
            var elemAcquis = $("#mntAcquis_" + codeRsq + "_" + codeForm);

            elemForce.autoNumeric('set', $("#MontantForce").autoNumeric('get'));
            elemAcquis.autoNumeric('set', $("#MontantAcquis").autoNumeric('get'));

            if ($("#topMntForce").val() == "1" || $("#topMntAcquis").val() == "1") {
                if ($("#topMntForce").val() == "1")
                    elemForce.addClass("greenValue");
                ToggleButton("Formule");
            }
            var mntProvi = $("#MontantProvisionnel").is(':checked');
            if (mntProvi == false)
            {
                $("img[id='mntProv_" + codeRsq + "_" + codeForm).hide();
            }
            else
            {
                $("img[id='mntProv_" + codeRsq + "_" + codeForm).show();
            }

            ReactivateShortCut();
            $("#divDataModifMontantFormule").html('');
            $("#divModifMontantFormule").hide();
        },
        error: function (request) {
            common.autonumeric.apply($("#MontantForce"), 'update', 'decimal');
            common.autonumeric.apply($("#MontantAcquis"), 'update', 'numeric', null, null, null, '99999999999', null);

            common.error.showXhr(request);
        }
    });
}
//-------Valide et enregistre les montants totaux---------
function ValidMontantTotal() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var montantCalc = $("#MontantCalcule").val();

    common.autonumeric.apply($("#MontantForce"), 'update', 'decimal', '');
    common.autonumeric.apply($("#MontantAcquis"), 'update', 'numeric', '');

    var montantAcquis = $("#MontantAcquis").val();
    if (montantAcquis == undefined || montantAcquis == "") {
        montantAcquis = 0;
        $("#MontantAcquis").val(0);
    }

    var mntForce = $("#MontantForce").val();
    if (mntForce == "" || mntForce == undefined) {
        mntForce = 0;
        $("#MontantForce").val(0);
    }


    if (montantCalc == "0,00" && mntForce != "0,00" && mntForce != undefined) {
        common.dialog.error("Un montant forcé ne peut être indiqué sans avoir renseigné un montant sur les garanties.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AnMontantReference/ValidMontantTotal",
        data: {
            codeOffre: codeOffre, version: version, type: type,
            mntForce: mntForce, mntAcquis: montantAcquis,
            checkedA: $("#chckMntAcquis").is(':checked'), checkedP: $("#MontantProvisionnel").is(':checked')
        },
        success: function (data) {
            $("#spanMntForce").autoNumeric('set', $("#MontantForce").autoNumeric('get'));
            $("#spanTotalAcquis").autoNumeric('set', $("#MontantAcquis").autoNumeric('get'));

            if ($("#topMntForceTotal").val() == "1") {
                $("#spanMntForce").addClass("greenValue");
                ToggleButton("Total");
                $("#btnSuivant").attr('disabled', 'disabled');
            }
            var checkedP = $("#MontantProvisionnel").is(':checked');
            if (checkedP == false) {
                $("img[name=mntProv]").hide();
            }
            else {
                $("img[name=mntProv]").show();
            }
            ReactivateShortCut();
            $("#divDataModifMontantTotal").html('');
            $("#divModifMontantTotal").hide();
        },
        error: function (request) {
            common.autonumeric.apply($("#MontantForce"), 'update', 'decimal', null, null, null, '2999999999.97', null);
            common.autonumeric.apply($("#MontantAcquis"), 'update', 'decimal', null, null, null, '2999999999.97', null);

            common.error.showXhr(request);
        }
    });
}
//--------------Change l'état des boutons------------------
function ToggleButton(etat) {
    if (etat == "Formule") {
        $("#divRefresh").addClass("CursorPointer");
        $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
        $("#btnSuivant").attr('disabled', 'disabled');
        $("#modif").removeClass("CursorPointer");
        $("#modif").attr('src', '/Content/Images/editer1616_gris.png');
        $("#modif").attr('disabled', 'disabled');
    }
    else {
        $("#divRefresh").addClass("CursorPointer");
        $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
        $("#btnSuivant").removeAttr('disabled');
        $("img[name=modifMontantFormule]").each(function () {
            $(this).removeClass("CursorPointer");
            $(this).attr('src', '/Content/Images/editer1616_gris.png');
            $(this).attr('disabled', 'disabled');
        });
    }
}
//-------------Rafraichit la page avec les nouvelles données----------
function ReloadPage() {
    ShowLoading();

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var acteGestion = $("#ActeGestion").val();
    var topForce = $("#topMntForce").val() == "1";
    var topAcquis = $("#topMntAcquis").val() == "1";
    var topForceTotal = $("#topMntForceTotal").val() == "1";

    var argModelCommentForce = $("#CommentForce").val();
    var codeCommentaire = $("#codeCommentaireForce").val();

    if ($("#txtParamRedirect").val() != "") {
        $.ajax({
            type: "POST",
            url: "/AnMontantReference/UpdateRedirect",
            data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, topForce: topForce, topAcquis: topAcquis, topForceTotal: topForceTotal, argModelCommentForce: argModelCommentForce, codeCommentForce: codeCommentaire, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, acteGestion: acteGestion },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    } else {
        $.ajax({
            type: "POST",
            url: "/AnMontantReference/UpdateInPage",
            data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, topForce: topForce, topAcquis: topAcquis, topForceTotal: topForceTotal, argModelCommentForce: argModelCommentForce, codeCommentForce: codeCommentaire, tabGuid: tabGuid, saveCancel: $("#txtSaveCancel").val(), modeNavig: modeNavig, acteGestion: acteGestion },
            success: function (data) {
                $("#divMontantRef").html(data);
                MapElementPage(1);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//--------Réinitialise les montants----------
function Reset() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var modeNavig = $("#ModeNavig").val();
    var acteGestion = $("#ActeGestion").val();

    $.ajax({
        type: "POST",
        url: "/AnMontantReference/ResetPage",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: modeNavig, acteGestion: acteGestion },
        success: function (data) {
            $("#divMontantRef").html(data);
            MapElementPage(1);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
