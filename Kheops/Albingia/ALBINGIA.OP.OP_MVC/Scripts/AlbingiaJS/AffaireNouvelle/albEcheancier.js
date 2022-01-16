$(document).ready(function () {
    AlternanceLigne("EcheancesBody", "", false, null);
    MapElementPage();
    Suivant();
    Annuler();
});
//-------------------------Action des Buttons 
function MapElementPage() {
    MapElementListeEcheances();
    $("#PrimeComptant").live('change', function () {
        CalculMontantComptant();
    });
    $("#ComptantHT").live('change', function () {
        $("#PrimeComptant").val('');
        if (!CheckComptant(true)) {
            $("#ComptantHT").val('');
        }
        else {
            $("#MontantRestant").val((parseFloat($("#PrimeHT").autoNumeric('get')) - parseFloat($("#ComptantHT").autoNumeric('get'))).toFixed(2).replace(".", ","));
        }
    });
    FormatDecimalValue();
    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    if ($("#ActeGestion").val() == "AVNMD")
        $("#lblPrimeHT").html("Restant dû (€)");
}
//------Map les elements de la liste des echeances
function MapElementListeEcheances() {
    if (window.isReadonly) {
        $("img[name=btnAfficherActionEcheance]").hide();
    }
    $("#btnAfficherActionEcheance").die().live('click', function () {
        if (CheckComptant(false) == true)
            EditEcheance("");
    });
    $("img[name=modifEcheance]").each(function () {
        $(this).click(function () {
            if (CheckComptant(false) == true)
                EditEcheance($(this).attr("id"));
        });
    });
    $("img[name=supprEcheance]").each(function () {
        $(this).click(function () {
            SupprimerEcheance($(this).attr("id"));
        });
    });
    FormatDecimalValue();
}

//------ fonction qui calcul le montant -----
function CalculMontantComptant() {
    if (CheckComptant(true) && $("#PrimeComptant").val() != "") {
        $("#ComptantHT").val((parseFloat($("#PrimeHT").autoNumeric('get')) * parseFloat($("#PrimeComptant").autoNumeric('get')) / 100).toFixed(2).replace(".", ","));
        $("#MontantRestant").val((parseFloat($("#PrimeHT").autoNumeric('get')) - parseFloat($("#ComptantHT").autoNumeric('get'))).toFixed(2).replace(".", ","));
    }
    else {
        $("#PrimeComptant").val('');
        $("#ComptantHT").val('');
    }
}
//----------------------Ajouter,modifier un courtier sur la div flottante----------------------
function MapElementDivFlottante() {
    if (!window.isReadonly) {
        formatDatePicker();
    }

    $('#btnAjouter').die();
    $('#btnAjouter').live('click', function () {
        UpdateEcheance('I');
    });
    $('#btnModifier').die();
    $('#btnModifier').live('click', function () {
        UpdateEcheance('U');
    });
    $('#btnAnnulerDivFlottante').die();
    $('#btnAnnulerDivFlottante').live('click', function () {
        ReactivateShortCut();
        $("#divDataUpdateEcheance").html('');
        $("#divUpdateEcheance").hide();
    });
    $("#PourcentagePrime").live('change', function () {
        $("#MontantEcheanceHT").val((parseFloat($("#PrimeHT").autoNumeric('get')) * parseFloat($("#PourcentagePrime").autoNumeric('get')) / 100).toFixed(2).replace(".", ","));
    });
    $("#DateEcheance").live('change', function () {
        if (($("#DateEcheance").val().split("/")[2] == $("#PeriodeDebut").val().split("/")[2] && $("#TaxeAttentatComptant").is(':checked')) || ($("#EcheanceDate").val() == "")) {
            $("#TaxeAttentat").attr('disabled', 'disabled');
        }
        else {
            $("#TaxeAttentat").removeAttr('disabled', 'disabled');
        }
    });
}
//----------------------Initialiser les contrôles de la div flottante UpdateEcheance
function InitialiserDivEcheance() {

    if ($("#ComptantHT").val() != "" && $("#PrimeComptant").val() == "") {
        $("#PourcentagePrime").attr('readonly', 'readonly').addClass('readonly');
        $("#PourcentagePrime").val('');
        $("#MontantEcheanceHT").removeAttr('readonly', 'readonly').removeClass('readonly');
    }
    else if ($("#PrimeComptant").val() != "") {
        $("#MontantEcheanceHT").attr('readonly', 'readonly').addClass('readonly');
        $("#MontantEcheanceHT").val('');
        $("#PourcentagePrime").removeAttr('readonly', 'readonly').removeClass('readonly');
    };
    if (($("#DateEcheance").val().split("/")[2] == $("#PeriodeDebut").val().split("/")[2] && $("#TaxeAttentatComptant").is(':checked')) || ($("#EcheanceDate").val() == "")) {
        $("#TaxeAttentat").attr('disabled', 'disabled');
    }
    else {
        $("#TaxeAttentat").removeAttr('disabled', 'disabled');
    }

}
//---------------Création/Modification d'une échéance ---------
function UpdateEcheance(typeOperation) {
    if (!CheckEcheance()) {
        return;
    }
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var dateEcheance = $("#DateEcheance").val();
    var primePourcent = $("#PourcentagePrime").autoNumeric('get');
    var montantEcheance = $("#MontantEcheanceHT").autoNumeric('get');
    var fraisAccessoires = $("#FraisAccessoire").autoNumeric('get');
    var taxeAttentat = $("#TaxeAttentat").is(':checked');
    var typeOperation = typeOperation;
    var primeHT = $("#PrimeHT").autoNumeric('get');
    var dateDeb = $("#PeriodeDebut").val();
    var dateFin = $("#PeriodeFin").val();
    var dateDerniereEcheance = $("#DateDerniereEcheance").val();
    var codeAvn = $("#NumAvenantPage").val();

    //#region Recalcule du montant de prime
    var primeComptant = $("#PrimeComptant").autoNumeric('get');
    var echPrimesPourcent = 0;
    $("span[name='EchPourcent']").each(function () {
        var echPrimePourcent = $(this).html();
        echPrimesPourcent += parseInt(echPrimePourcent);
    });
    echPrimesPourcent = echPrimesPourcent - parseInt($("#currentTauxUpd").val()) + parseInt(primePourcent);

    var is100Pourcent = $("#modePourcent").is(':checked') && parseInt(primeComptant) + echPrimesPourcent == 100;
    if (is100Pourcent) {
        var sumEcheance = 0;
        $("input[id='montantEch']").each(function () {
            sumEcheance += parseFloat($(this).val().replace(",", "."));
        });
        var currentEcheance = parseFloat($("#currentPrimeUpd").val().replace(",", "."));
        var newSumEcheance = (sumEcheance - currentEcheance);
        var comptantHT = $("#ComptantHT").autoNumeric('get');

        montantEcheance = (primeHT - newSumEcheance - comptantHT).toFixed(2);
    }

    //#endregion

    $.ajax({
        type: "POST",
        url: "/AnEcheancier/UpdateEcheance",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid, dateEcheance: dateEcheance, primePourcent: primePourcent,
            montantEcheance: montantEcheance, fraisAccessoires: fraisAccessoires, taxeAttentat: taxeAttentat, typeOperation: typeOperation,
            primeHT: primeHT, dateDeb: dateDeb, dateFin: dateFin,
            dateDerniereEcheance: dateDerniereEcheance
        },
        success: function (data) {
            UpdateListeEcheances();
            $("#currentPrimeUpd").val("0");
            $("#currentTauxUpd").val("0");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//--------------Supprimer une écheance------------
function SupprimerEcheance(elem) {
    ShowCommonFancy("Confirm", "Suppr",
             "Etes-vous sûr de vouloir supprimer cette échéance ?",
             350, 130, true, true);
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        ReactivateShortCut();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                var codeOffre = $("#Offre_CodeOffre").val();
                var version = $("#Offre_Version").val();
                var type = $("#Offre_Type").val();
                var tabGuid = $("#tabGuid").val();
                var dateEcheance = elem.split("_")[1];
                var codeAvn = $("#NumAvenantPage").val();
                $.ajax({
                    type: "POST",
                    url: "/AnEcheancier/SupprimerEcheance",
                    data: {
                        codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid, dateEcheance: dateEcheance
                    },
                    success: function (data) {
                        UpdateListeEcheances();
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
//----------------------fonction qui met à jour la liste des echeances----------------------
function UpdateListeEcheances() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();
    $.ajax({
        type: "POST",
        url: "/AnEcheancier/UpdateListeEcheances",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: $("#ModeNavig").val(), tabGuid: tabGuid },
        success: function (data) {
            ReactivateShortCut();
            $("#tblEcheancesBody").html(data);
            AlternanceLigne("EcheancesBody", "", false, null);
            $("#divDataUpdateEcheance").html('');
            $("#divUpdateEcheance").hide();
            MapElementListeEcheances();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------Contrôle de validité de l'écheance----
function CheckEcheance() {
    let errorsFound = false;

    // %prime ou montant obligatoire selon le cas
    if ($("#MontantEcheanceHT").attr('readonly') && $("#PourcentagePrime").val() == "") {
        $("#PourcentagePrime").addClass('requiredField');
        errorsFound = true;
    }
    else if ($("#PourcentagePrime").attr('readonly') && $("#MontantEcheanceHT").val() == "") {
        $("#MontantEcheanceHT").addClass('requiredField');
        errorsFound = true;
    }
    // Date obligatoire
    if ($("#DateEcheance").val() == "") {
        $("#DateEcheance").addClass('requiredField');
        errorsFound = true;
    }
    let errors = [];
    // Le %  doit être  < 100
    if ($("#PourcentagePrime").val() != "" && parseFloat($("#PourcentagePrime").autoNumeric('get')) == 100) {
        $("#PourcentagePrime").val('');
        errors.push("Le % prime ne peut pas être égale à 100");
    }
    // Le Montant doit être < reste à émettre
    if ($("#MontantEcheanceHT").val() != "" && parseFloat($("#MontantEcheanceHT").autoNumeric('get')) > parseFloat($("#MontantRestant").val().replace(',', '.'))) {
        $("#MontantEcheanceHT").val('');
        errors.push("Le montant doit être inférieur au reste à payer");
    }
    // Calculé ou saisi, Le montant doit être > 0 
    if ($("#PourcentagePrime").val() != "" && parseFloat($("#PourcentagePrime").autoNumeric('get')) == 0) {
        $("#PourcentagePrime").val('');
        errors.push("Le % prime ne peut pas être égale à 0");
    }
    
    if (errors.length > 0) {
        common.dialog.bigError(errors.join("\n"));
    }
    return errorsFound || errors.length > 0;
}

//------------Suivant--------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt) {
        if (!$(this).attr('disabled')) {
            ShowLoading();
            if (!window.isReadonly) {
                UpdateEcheancier();
            }
            else {
                Redirection("ChoixClauses", "Index");
            }
            CloseLoading();
        }
    });
}
//-----Annuler------
function Annuler() {
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Redirection("Quittance", "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
}
//---------- Validation de l'echeancier-----
function UpdateEcheancier() {
    if (!CheckEcheancier()) {
        return;
    }
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();
    var primePourcent = $("#PrimeComptant").autoNumeric('get').replace(".", ",");
    var comptant = $("#ComptantHT").autoNumeric('get').replace(".", ",");
    var fraisAccessoires = $("#FraisAccessoiresComptantHT").autoNumeric('get').replace(".", ",");
    var taxeAttentat = $("#TaxeAttentatComptant").is(':checked');
    var primeHT = $("#PrimeHT").autoNumeric('get').replace(".", ",");

    var montantRestant = $("#MontantRestant").val().replace(".", ",");;
    var TotalMontantEcheanceSansDerniere = $("#TotalMontantEcheanceSansDerniere").autoNumeric('get').replace(".", ",");

    var dateDerniereEcheance = $("#DateDerniereEcheance").val();
    var primeDerniereEcheance = parseFloat($("#PrimeDerniereEcheance").autoNumeric('get'));
    var fraisDerniereEcheance = parseFloat($("#FraisDerniereEcheance").autoNumeric('get'));
    var taxeAttentatDerniereEcheance = $("#TaxeAttentatDerniereEcheance").val();

    if (primePourcent == "")
        primePourcent = "0";
    if (primeHT == "")
        primeHT = "0";
    if (fraisAccessoires == "")
        fraisAccessoires = "0";
    if (comptant == "")
        comptant = "0";

    $.ajax({
        type: "POST",
        url: "/AnEcheancier/UpdateEcheancier",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid, primePourcent: primePourcent,
            comptant: comptant, fraisAccessoires: fraisAccessoires, taxeAttentat: taxeAttentat, primeHT: primeHT,
            montantRestant: montantRestant, TotalMontantEcheanceSansDerniere: TotalMontantEcheanceSansDerniere, dateDerniereEcheance: dateDerniereEcheance, primeDerniereEcheance: primeDerniereEcheance,
            fraisDerniereEcheance: fraisDerniereEcheance, taxeAttentatDerniereEcheance: taxeAttentatDerniereEcheance,
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            Redirection("ChoixClauses", "Index");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Contrôles quand on clique sur suivant-----

function CheckEcheancier() {
    var toReturn = true;
    var msgErr = "";
    if ($("#NbrEcheances").val() == 0) {
        toReturn = false;
        msgErr += "Il faut saisir au moins une écheance </br> ";
    }
    else {
        $("input[id='DateDerniereEcheance']").each(function () {
            var dateEch = getDate($(this).val());
            var dNow = new Date();
            var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 ;
            var idateEch = dateEch.getFullYear() * 10000 + (dateEch.getMonth() + 1) ;
            if (idateEch < iNow) {
                toReturn = false;
                msgErr="<u><b>Contrôle des dates</b></u> </br>"
                msgErr += " La date ne peut être antérieure à la date du jour </br>";
            }
        });
        if (parseFloat($("#PrimeHT").autoNumeric('get')) != (parseFloat($("#ComptantHT").autoNumeric('get')) + parseFloat($("#TotalMontantEcheance").autoNumeric('get')))) {
            toReturn = false;
            msgErr = "<u><b>Contrôle des montants de l'échéancier</b></u> </br>"
            msgErr += "La somme des montants n'est pas égale à la prime totale";
        }
    }
    if (toReturn == false) {
        ShowCommonFancy("Error", "", msgErr, 1212, 700, true, true, true, false);
        return toReturn;
    }
    else {
        return toReturn;
    }

}

//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0');
    common.autonumeric.applyAll('init', 'pourcentnumeric');
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/AnEcheancier/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type,
            tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: $("#ModeNavig").val(),
            addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
