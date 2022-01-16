
//----------------Map les éléments de la page------------------
function MapElementPage() {
    $("#btnAnnuler").kclick(function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").clear();
    });

    toggleDescription();
    FormatNumericValue();

    $("#Antecedent").offOn("change", function () {
        AffectTitleList($(this));
        $("div[name='txtAreaLnk'][albcontext='Description']").addClass("CursorPointer");
        if ($(this).val() != "A") {
            $("div[name='txtAreaLnk'][albcontext='Description']").removeClass("CursorPointer");
            $("#txtArea[albContext='Description']").hide();
            $("div[id='zoneTxtArea'][albcontext='Description").html("");
            $("textarea[id='Description']").html("");
        }
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    if ($("#Offre_Type").val() == "P") {
        $("label[for='ValiditeOffre']").text("Validité du contrat (j)*");
    }
    $("#Antecedent").change();
}

//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}

//----------------------Traitement checkbox relance---------------------
function CheckBoxRelance() {
    $("#Relance").kclick(function () {
        if ($(this).isChecked()) {
            $("#RelanceValeur").removeAttr('disabled');
        }
        else {
            $("#RelanceValeur").attr("disabled", "disabled");
            $("#RelanceValeur").val('0');
        }
    });
}

// Mise en place d'un contrôle bloquant en offre 
// si incohérence entre la date d'effet de l'AN et la durée de validité du projet
function projectDateControl() {
    var startProjectDate = $("#DateProjet");
    var duration = $("#ValiditeOffre");
    var isReadonly = $("#OffreReadOnly");
    var EffectDate = $("#dateDebOffre");

    if ($("#dateDebOffre").val() == "" || isReadonly == "True") {
        return true;
    }

    if (startProjectDate && duration) {
        var endProjectDate = getDate(startProjectDate.val());
        endProjectDate.setDate(endProjectDate.getDate() + (parseInt(duration.val()) || 0));
        if (endProjectDate > getDate(EffectDate.val())) {
            common.dialog.error("La date de fin de validité de l'offre ne peut être supérieure ou égale à la date de début d'effet du contrat");
            return false;
        }
    }
    return true;
}

//----------------------Traitement suivant---------------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        $(":input").removeClass("requiredField");

        $(".requiredField").removeClass("requiredField");
        var errDate = false;
        if ($("#DateProjet").val() != "" && !isDate($("#DateProjet").val())) {
            $("#DateProjet").addClass("requiredField");
            errDate = true;
        }

        var periodicite = $("#PeridiciteOffre").val();
        if ((periodicite == "A" || periodicite == "S" || periodicite == "T" || periodicite == "R") && ($("#Preavis").length == 0 || $("#Preavis").val() == 0)) {
            $("#Preavis").addClass("requiredField");
            errDate = true;
        }

        if (errDate) {
            return false;
        }

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var Antecedent = $("#Antecedent").val();
        var Description = $("#Description").val();
        var ValiditeOffre = $("#ValiditeOffre").val();
        if (ValiditeOffre.length == 0) {
            ValiditeOffre = '0';
        }
        var DateProjet = $("#DateProjet").val();
        var Relance = $("#Relance").is(':checked');
        var RelanceValeur = $("#RelanceValeur").val();
        if (RelanceValeur.length == 0) {
            RelanceValeur = '0';
        }

        var Preavis = $("#Preavis").val();
        if (Preavis.length == 0) {
            Preavis = '0';
        }

        var ModeleFinOffreInfos = {
            Antecedent: Antecedent,
            Description: Description,
            ValiditeOffre: ValiditeOffre,
            DateProjet: DateProjet,
            Relance: Relance,
            RelanceValeur: RelanceValeur,
            Preavis: Preavis
        };

        if (!window.isReadonly) {
            if (ModeleFinOffreInfos.Antecedent == 'A' && ModeleFinOffreInfos.Description.length == 0) {
                $("#zoneTxtArea").addClass('requiredField');
                $("#zoneTxtArea").attr("title", "La description ne peut être vide si l'antécédent est égal à 'A'");
                return;
            }
            if (ModeleFinOffreInfos.ValiditeOffre.length == 0 || ModeleFinOffreInfos.ValiditeOffre == '0') {
                $("#ValiditeOffre").addClass("requiredField");
                $("#ValiditeOffre").attr("title", "La durée de validité de l'offre doit être supérieure à 0");
                return;
            }
            if (ModeleFinOffreInfos.DateProjet.length == 0) {
                $("#DateProjet").addClass("requiredField");
                $("#DateProjet").attr("title", "Date du projet obligatoire");
                return;
            }
            if (ModeleFinOffreInfos.Relance && (ModeleFinOffreInfos.RelanceValeur.length == 0 || ModeleFinOffreInfos.RelanceValeur == '0')) {
                $("#RelanceValeur").addClass("requiredField");
                $("#RelanceValeur").attr("title", "La durée de relance doit être supérieure à 0");
                return;
            }
        }

        var argModeleFinOffreInfos = JSON.stringify(ModeleFinOffreInfos);
        var argModeleFinOffreAnnotation = $("#AnnotationFin").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        common.page.isLoading = true;
        let saveCancel = data && data.returnHome ? 1 : 0;
        $.ajax({
            type: "POST",
            url: "/FinOffre/Update",
            data: { codeOffre: codeOffre, version: version, type: type, argModeleFinOffreInfos: argModeleFinOffreInfos, argModeleFinOffreAnnotation: argModeleFinOffreAnnotation, tabGuid: tabGuid, saveCancel: saveCancel, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function () { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

//----------------------Traitement Annuler---------------------
function CancelForm() {
    Redirection("Cotisations", "Index");
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
        url: "/FinOffre/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------Formate les input/span des valeurs----------
function FormatNumericValue() {
    common.autonumeric.applyAll('init', 'numeric', '', null, null, '99', '0');
    common.autonumeric.apply($("#ValiditeOffre"), 'init', 'numeric', '', null, null, '999', '0');
}


$(document).ready(function () {
    formatDatePicker();
    if (!window.isReadonly) {
        $("#AnnotationFin").html($("#AnnotationFin").html().replace(/&lt;br&gt;/ig, "\n"));
        $("#Description").html($("#Description").html().replace(/&lt;br&gt;/ig, "\n"));
    }
    CheckBoxRelance();
    Suivant();
    MapElementPage();
});

