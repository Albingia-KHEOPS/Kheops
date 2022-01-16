$(document).ready(function () {
    AlternanceLigne("Detail", "", false, null);
    AjusteEnteteTable();
    EvenementChange();
    AffectBoutons();
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {
    toggleDescription($("#CommentForce"), true);
    //FormatCLEditor($("#CommentForce"));
    if (!window.isReadonly) {
        $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));
    }
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
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
    FormatDecimalNumericValue();
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    // FormatNumerique('numeric', ' ', '9999999999999', '0');
    common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '9999999999999', '0');
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
}

//-- Ajuste la derniere colonne pour l'ascenseur
function AjusteEnteteTable() {
    if ($("#tblDetail").css("height") > $("#divTblDetail").css("height")) {
        $("#col6").css("width", "120px");
    }
    else {
        $("#col6").css("width", "100px");
    }
}

function EvenementChange() {
    $("#tblDetail").find("input").each(function () {
        var id = $(this).attr("id");
        $("#" + id).change(function () {
            VerifieValeur(id);
        });
    });
    $("#tblDetail").find("select").each(function () {
        var id = $(this).attr("id");
        $("#" + id).change(function () {
            VerifieValeur(id);
        });
    });
}

function VerifieValeur(argId) {
    var typeControl = argId.split('¤')[0];
    var id = argId.split('¤')[1];
    var valeur = (typeControl == "Check" ? $("#" + argId).is(':checked') : $("#" + argId).val());
    //-- Flag la ligne
    $("#flag¤" + id).val("true");
    //-- Dynamise les couleurs
    switch (typeControl) {
        case "Check":
            if (valeur) {
                if ($("#Type¤" + id).val() == "%") {
                    $("#SMPretenu¤" + id).addClass("greenValue");
                } else {
                    if ($("#Type¤" + id).val() == "") {
                        $("#SMPcalcule¤" + id).addClass("greenValue");
                    }
                    else {
                        $("#Valeur¤" + id).addClass("greenValue");
                    }
                }
            }
            else {
                $("#SMPcalcule¤" + id).removeClass("greenValue");
                $("#Valeur¤" + id).removeClass("greenValue");
                $("#SMPretenu¤" + id).removeClass("greenValue");
            }
            break;
        case "Type":
            if (valeur == "" && $("#Valeur¤" + id).val() != "") {
                alert("Le type est obligatoire.");
            }
            switch (valeur) {
                case "%":
                    if ($("#Valeur¤" + id).val() > 100) {
                        alert("Votre pourcentage doit être inferieur à 100.");
                    }
                    $("#SMPcalcule¤" + id).removeClass("greenValue");
                    $("#Valeur¤" + id).removeClass("greenValue");
                    $("#SMPretenu¤" + id).addClass("greenValue");
                    break;
                case "":
                    $("#SMPcalcule¤" + id).addClass("greenValue");
                    $("#Valeur¤" + id).removeClass("greenValue");
                    $("#SMPretenu¤" + id).removeClass("greenValue");
                    break;
                default:
                    $("#SMPcalcule¤" + id).removeClass("greenValue");
                    $("#Valeur¤" + id).addClass("greenValue");
                    $("#SMPretenu¤" + id).removeClass("greenValue");
                    break;
            }
            break;
        case "Valeur":
            if ((valeur == "" || valeur == "0") && $("#Type¤" + id).val() != "") {
                alert("La valeur est obligatoire.");
            }
            if (valeur > 100 && $("#Type¤" + id).val() == "%") {
                alert("Votre pourcentage doit être inferieur à 100.");
            }
            break;
    }
    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
    $("#divRefresh").addClass("CursorPointer");
}

function AffectBoutons() {
    $("#btnRefresh").click(function () {
        if ($("#btnRefresh").attr('src') == "/Content/Images/boutonRefresh_3232.png") {
            var Str = "[";
            $("input[name=flag]").each(function () {
                var idFlag = $(this).attr("id");
                var id = idFlag.split('¤')[1];
                if ($.parseJSON($("#" + idFlag).val())) {
                    Str += '{IdGarantie: "' + id + '", CheckBox: "' + $("#Check¤" + id).is(':checked') + '", Type: "' + $("#Type¤" + id).val() + '", Valeur: "' + $("#Valeur¤" + id).val() + '" },';
                }
            });
            Str = Str.substr(0, Str.length - 1);
            Str += "]";
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/DetailCalculSMP/Recalcule",
                data: {
                    argCodeOffre: $("#Offre_CodeOffre").val(), argVersion: $("#Offre_Version").val(), argType: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), argLignes: Str, argIdRisque: $("#IdRisque").val(),
                    argIdVolet: $("#IdVolet").val(), saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: $("#ModeNavig").val()
                },
                success: function (data) {
                    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_gris3232.png');
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            if ($("#txtSaveCancel").val() == "1")
                Redirection("RechercheSaisie", "Index");
        }
    });
    $("#btnReset").click(function () {
        if ($("#divReset").hasClass("CursorPointer"))
            ReloadPage();
    });
    $("#btnSuivant").click(function (evt, data) {
        CancelForm(data && data.returnHome);
    });

}
//----------------Redirection------------------
function Redirection(cible, job, paramSMP) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val()
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/DetailCalculSMP/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, paramSMP: paramSMP, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------- Reload la form ------------------------
function ReloadPage() {
    var paramSMP = $("#IdRisque").val() + "¤" + $("#IdVolet").val();
    Redirection("DetailCalculSMP", "Index", paramSMP);
}
function CancelForm(returnHome) {
    Redirection(returnHome ? "RechercheSaisie" : "EngagementTraite", "Index");
}
