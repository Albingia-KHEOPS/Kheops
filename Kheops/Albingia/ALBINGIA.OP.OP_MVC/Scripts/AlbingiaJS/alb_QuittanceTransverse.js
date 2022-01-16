
$(document).ready(function () {
    globalInit.push(MapElementQuittanceTransversePage);
    MapElementQuittanceTransversePage();
   
    
});

function MapElementQuittanceTransversePage() {
    ChkGererEcheancierChange();

    AlternanceLigne("BodyCommission", "", false, null);
    AlternanceLigne("FormulesBody", "", false, null);
    AlternanceLigne("FormulesBodyPleinEcran", "", false, null);
    AlternanceLigne("BodyDetailsTotauxBase", "", false, null);

    $("#btnFraisAccess").kclick(function () {
        AfficherFraisAccessoires();
    });

    $('#btnDetailCot').kclick(function () {
        AfficherCoCourtiers();
    });

    $("button[type='button'][id='btnDetails'][albContext='quitTrans']").removeAttr('disabled');
    $("button[type='button'][id='btnDetails'][albContext='quitTrans']").removeClass('readonly');

    $("button[type='button'][id='btnDetails'][albContext='quitTrans']").kclick(function (evt) {
        AfficherDetailsCotisation();
    });

    $("td[id='lnkFraisAccQuittance'][albcontext='']").kclick(function () {
        OpenFraisAccessoiresAvn();
    });

    $("td[id='lnkFGAQuittance'][albcontext='']").kclick(function () {
        OpenFraisAccessoiresAvn();
    });

    $("#btnConfirmOkQuitt").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                if (quittance) {
                    quittance.cancelForm();
                }
                else {
                    CancelForm();
                }
                break;
            case "Warning":
                ShowLoading();
                quittance.redirectToQuit("ChoixClauses", "Index");
                CloseLoading();
                break;
            case "Suppr":
                let codeOffre = $("#Offre_CodeOffre").val();
                let version = $("#Offre_Version").val();
                let type = $("#Offre_Type").val();
                let tabGuid = $("#tabGuid").val();
                let codeAvn = $("#NumAvenantPage").val();
                $.ajax({
                    type: "POST",
                    url: "/Quittance/SupprimerEcheances",
                    data: {
                        codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid
                    },
                    success: function (data) {
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
                $("#hiddenInputId").val('');
                break;
            case "ChangeModeSaisieEcheancier":

                OpenEcheancier($("#hiddenInputId").val(), "");
                $("#hiddenInputId").val('');
                break;
            case "SuppressionEcheancier":
                SupprimerEcheancier();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancelQuitt").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#hiddenInputId").val('');
    });

}


//---------------------------------------#region Echeancier

//-------Supprime l'échéancier et met à jour la période du contrat
function SupprimerEcheancier() {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let tabGuid = $("#tabGuid").val();

    $.ajax({
        type: "POST",
        url: "/AnEcheancier/SupprimerEcheancier",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid
        },
        success: function (data) {
            ReloadCotisation(true, false);
            $("#divDataEcheancier").html("");
            $("#divEcheancier").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------CheckBox Etaler la prime avec un échéancier------
function ChkGererEcheancierChange() {
    $("button[type='button'][id='GererEcheancier'][albContext='quitTrans']").kclick(function () {
        OpenEcheancier("", $(this).attr("albcontext"));
    });

}
//------Ouvre l'échéancier en div flottante-
function OpenEcheancier(modeSaisie, context) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let tabGuid = $("#tabGuid").val();
    let modeNavig = $("#ModeNavig").val();
    let addParamType = $("#AddParamType").val();
    let addParamValue = $("#AddParamValue").val();
    let modeAffichage = $("input[id='ModeAffichage'][albcontext='" + context + "']").val();
    let id = codeOffre + "_" + version + "_" + type + tabGuid;
    if (modeSaisie == undefined)
        modeSaisie = "";
    if (modeAffichage == undefined)
        modeAffichage = "";
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnEcheancier/OpenEcheancier",
        data: {
            id: id, addParamType: addParamType, addParamValue: addParamValue, modeNavig: modeNavig, modeSaisie: modeSaisie, modeAffichage: modeAffichage
        },
        success: function (data) {
            $("#divDataEcheancier").html(data);
            MapElementPageEcheancier();
            $("#divEcheancier").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------Action des Buttons
function MapElementPageEcheancier() {
    AlternanceLigne("EcheancesBody", "", false, null);
    AlternanceLigne("ComptantBody", "", false, null);
    MapElementListeEcheances();
    $("#PrimeComptant").die().live('change', function () {
        CalculMontantComptant();
    });
    $("#ComptantHT").die().live('change', function () {
        $("#PrimeComptant").val('');
        if (!CheckComptant(true)) {
            $("#ComptantHT").val('');
        }
        else {
            $("#MontantRestant").val((parseFloat($("#PrimeHT").autoNumeric('get')) - parseFloat($("#ComptantHT").autoNumeric('get'))).toFixed(2).replace(".", ","));
        }
    });
    FormatDecimalValueEcheancier();

    let modeAffichage = $("#ModeAffichage").val();

    if ($("#ActeGestion").val() == "AVNMD")
        $("#lblPrimeHT").html("Restant dû (€)");
    $("#btnAnnulerEcheancier").die().live("click", function () {
        if ($("#OffreReadOnly").val() == "False" && modeAffichage != "Visu") {
            ReloadCotisation(true, false);
        }
        $("#divDataEcheancier").html("");
        $("#divEcheancier").hide();
    });


    $("#btnValiderEcheancier").die().live("click", function () {
        UpdateEcheancier();
    });

    $("input[type=radio][name=modeComptant]").each(function () {
        $(this).die();
        $(this).live("change", function () {
            $("#hiddenInputId").val($(this).val());
            ShowCommonFancy("ConfirmQuitt", "ChangeModeSaisieEcheancier",
                "Attention, en changeant de mode de saisie vous allez remettre à zero les échéances non émises.<br/><br/>Confirmez-vous le changement du mode de saisie ?<br/>",
                350, 130, true, true, true, false);

        });
    });

    $("#btnSupprimerEcheancier").die().live("click", function () {
        ShowCommonFancy("ConfirmQuitt", "SuppressionEcheancier",
            "Attention, retour à une émission unique.<br/><br/>Confirmez-vous la suppression de l'échéancier ?<br/>",
            350, 130, true, true, true, false);
    });

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
    FormatDecimalValueEcheancier();
}

//-----------Contrôles quand on clique sur suivant-----

function CheckEcheancier() {
    let toReturn = true;
    let msgErr = "";
    if ($("#NbrEcheances").val() == 0) {
        toReturn = false;
        msgErr += "Il faut saisir au moins une écheance </br> ";
    }
    else {
        $("tr[id^='tr']").each(function () {
            let num = $(this).children()[0].id;
            let dateEch = getDate($.trim($("#" + num).html()));
            let dNow = new Date();
            let iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
            let idateEch = dateEch.getFullYear() * 10000 + (dateEch.getMonth() + 1) * 100 + dateEch.getDate();
            if (idateEch < iNow) {
                toReturn = false;
                msgErr = "<u><b>Contrôle des dates</b></u> <br/>";
                msgErr += " Ligne N°" + num.replace("datEch", "") + ": " + "La date ne peut être antérieure à la date du jour<br/><br/>";
            }
        });

        let primeHT = parseFloat($("#PrimeHT").autoNumeric('get'));
        let comptantHT = parseFloat($("#ComptantHT").autoNumeric('get'));
        let totalMontantEcheance = parseFloat($("#TotalMontantEcheance").autoNumeric('get'));

        if (primeHT != (comptantHT + totalMontantEcheance).toFixed(2)) {
            toReturn = false;
            msgErr += "<u><b>Contrôle des montants de l'échéancier</b></u> <br/>";
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


//------ fonction qui calcul le montant -----
function CalculMontantComptant() {
    if (CheckComptant(true) && $("#PrimeComptant").val() != "") {
        $("#ComptantHT").val((parseFloat($("#PrimeHT").autoNumeric('get')) * parseFloat($("#PrimeComptant").autoNumeric('get')) / 100).toFixed(2).replace(".", ","));
        $("#MontantRestant").val(parseFloat(($("#PrimeHT").autoNumeric('get')) - parseFloat($("#ComptantHT").autoNumeric('get'))).toFixed(2).replace(".", ","));
    }
    else {
        $("#PrimeComptant").val('');
        $("#ComptantHT").val('');
    }
}

//-------Formate les input/span des valeurs-
function FormatDecimalValueEcheancier() {
    common.autonumeric.applyAll('init', 'decimal', ' ', ',',2, '99999999999.99', '-99999999999.99');
    common.autonumeric.applyAll('init', 'pourcentnumeric');
}
//--------Recharge les cotisations--------------
function ReloadCotisation(loadP400, modeCalculForce) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let acteGestion = $("#ActeGestion").val();
    let reguleId = $("#ReguleId").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/ReloadCotisation",
        data: {
            codeOffre: codeOffre, version: version, type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val(), acteGestion: acteGestion,
            acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId, loadPgm400: loadP400 != undefined ? loadP400 : false, modeCalculForce: modeCalculForce != undefined ? modeCalculForce : false, tabGuid: $("#tabGuid").val()
        },
        success: function (data) {
            let commentaires = $("#CommentairesFraisAcc").val();
            CloseLoading();
            $("#divCotisations").html(data);
            AlternanceLigne("BodyCommission", "", false, null);
            AlternanceLigne("FormulesBody", "", false, null);
            AlternanceLigne("FormulesBodyPleinEcran", "", false, null);
            AlternanceLigne("BodyDetailsTotauxBase", "", false, null);
            $("#divDataFraisAccessoires").html('');
            $("#divFraisAccessoires").hide();
            toggleDescription($("#CommentForce"), true);
            $("div[id=zoneTxtArea][albcontext=CommentForce]").html(commentaires);
            $("#CommentForce").html(commentaires);
            FormatDecimalValue();
            $("#divCalculForce").hide();
            $("#dvCalculForceData").html('');
            quittance.initPage();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function UpdateEcheancier() {
    if (!CheckEcheancier()) {
        return;
    }
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let tabGuid = $("#tabGuid").val();
    let primePourcent = $("#PrimeComptant").autoNumeric('get').replace(".", ",");
    let comptant = $("#ComptantHT").autoNumeric('get').replace(".", ",");
    let fraisAccessoires = $("#FraisAccessoiresComptantHT").autoNumeric('get').replace(".", ",");
    let taxeAttentat = $("#TaxeAttentatComptant").is(':checked');
    let primeHT = $("#PrimeHT").autoNumeric('get').replace(".", ",");

    let montantRestant = $("#MontantRestant").val().replace(".", ",");
    let TotalMontantEcheanceSansDerniere = $("#TotalMontantEcheanceSansDerniere").autoNumeric('get').replace(".", ",");

    let dateDerniereEcheance = $("#DateDerniereEcheance").val();
    let primeDerniereEcheance = parseFloat($("#PrimeDerniereEcheance").autoNumeric('get'));
    let fraisDerniereEcheance = parseFloat($("#FraisDerniereEcheance").autoNumeric('get'));
    let taxeAttentatDerniereEcheance = $("#TaxeAttentatDerniereEcheance").val();

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
            ReloadCotisation(true, false);
            $("#divDataEcheancier").html("");
            $("#divEcheancier").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------ fonction qui affiche la div flottante--
function EditEcheance(elem) {
    let guid = 0;
    let dateEcheance = 0;
    let montantEcheance = 0;
    let prime = 0;
    let fraisAccess = $("#FraisAccessoiresComptantHT").autoNumeric('get');
    let taxeAttentat = false;
    let totalMontantEcheance = $("#TotalMontantEcheance").autoNumeric('get');
    let montantRestant = $("#MontantRestant").val().replace(',', '.');
    let primeComptant = $("#PrimeComptant").autoNumeric('get');
    let primeHT = $("#PrimeHT").autoNumeric('get');
    if (elem != "") {
        let tab = elem.split("_");
        guid = tab[1];
        dateEcheance = tab[2];
        montantEcheance = tab[3];
        prime = tab[4];
        fraisAccess = tab[5];
        taxeAttentat = tab[6];
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnEcheancier/LoadEcheance",
        data: {
            guid: guid, dateEcheance: dateEcheance, montantEcheance: montantEcheance, prime: prime, fraisAccess: fraisAccess, taxeAttentat: taxeAttentat,
            primeHT: primeHT,
            totalMontantEcheance: totalMontantEcheance, montantRestant: montantRestant, primeComptant: primeComptant
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataUpdateEcheance").html(data);
            AlbScrollTop();
            $("#divUpdateEcheance").show();

            FormatDecimalValueEcheancier();

            MapElementDivFlottanteEcheancier();
            AlternanceLigne("EcheancesBody", "", false, null);
            CloseLoading();
            InitialiserDivEcheance();
            if (elem != "") {
                $("#currentPrimeUpd").val($("input[id='montantEch'][albcontext='" + guid + "']").val());
                $("#currentTauxUpd").val($("input[id='tauxEch'][albcontext='" + guid + "']").val());
            }
            AffectDateFormat();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//----------------------Ajouter,modifier un courtier sur la div flottante----------------------
function MapElementDivFlottanteEcheancier() {
    if (!window.isReadonly) {
        FormatDatePickerEcheancier();
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
        $("#currentPrimeUpd").val("0");
        $("#currentTauxUpd").val("0");
    });
    $("#PourcentagePrime").live('change', function () {
        $("#MontantEcheanceHT").val((parseFloat($("#PrimeHT").autoNumeric('get')) * parseFloat($("#PourcentagePrime").autoNumeric('get')) / 100).toFixed(2).replace(".", ","));
    });
    $("#EcheanceDate").live('change', function () {
        if (($("#EcheanceDate").val().split("/")[2] == $("#PeriodeDebut").val().split("/")[2] && $("#TaxeAttentatComptant").is(':checked')) || ($("#EcheanceDate").val() == "")) {
            $("#TaxeAttentat").attr('disabled', 'disabled');
        }
        else {
            $("#TaxeAttentat").removeAttr('disabled', 'disabled');
        }
    });
}

//---------------Création/Modification d'une échéance ---------
function UpdateEcheance(typeOperation) {

    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let tabGuid = $("#tabGuid").val();
    let dateEcheance = $("#EcheanceDate").val();
    let primePourcent = $("#PourcentagePrime").autoNumeric('get');
    let montantEcheance = $("#MontantEcheanceHT").autoNumeric('get');
    let fraisAccessoires = $("#FraisAccessoire").autoNumeric('get');
    let taxeAttentat = $("#TaxeAttentat").is(':checked');
    let primeHT = $("#PrimeHT").autoNumeric('get');
    let dateDeb = $("#PeriodeDebut").val();
    let dateFin = $("#PeriodeFin").val();
    let dateDerniereEcheance = $("#DateDerniereEcheance").val();
    let codeAvn = $("#NumAvenantPage").val();

    //#region Recalcule du montant de prime
    let primeComptant = $("#PrimeComptant").autoNumeric('get');
    let echPrimesPourcent = 0;
    $("span[name='EchPourcent']").each(function () {
        let echPrimePourcent = $(this).html();
        echPrimesPourcent += parseInt(echPrimePourcent);
    });
    echPrimesPourcent = echPrimesPourcent - parseInt($("#currentTauxUpd").val()) + parseInt(primePourcent);

    let is100Pourcent = $("#modePourcent").is(':checked') && parseInt(primeComptant) + echPrimesPourcent == 100;
    if (is100Pourcent) {
        let sumEcheance = 0;
        $("input[id='montantEch']").each(function () {
            sumEcheance += parseFloat($(this).val().replace(",", "."));
        });
        let currentEcheance = parseFloat($("#currentPrimeUpd").val().replace(",", "."));
        let newSumEcheance = (sumEcheance - currentEcheance);
        let comptantHT = $("#ComptantHT").autoNumeric('get');

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
            dateDerniereEcheance: dateDerniereEcheance, is100Pourcent: is100Pourcent
        },
        success: function (data) {
            $("#currentPrimeUpd").val("0");
            $("#currentTauxUpd").val("0");
            UpdateListeEcheances();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function FormatDatePickerEcheancier() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//---------------Contrôle de validité de l'écheance----
function CheckEcheance() {
    let toReturn = true;

    //%prime ou montant obligatoire selon le cas
    if ($("#MontantEcheanceHT").attr('readonly') && $("#PourcentagePrime").val() == "") {
        $("#PourcentagePrime").addClass('requiredField');
        toReturn = false;
    }
    else if ($("#PourcentagePrime").attr('readonly') && $("#MontantEcheanceHT").val() == "") {
        $("#MontantEcheanceHT").addClass('requiredField');
        toReturn = false;
    }
    //Date obligatoire
    if ($("#EcheanceDate").val() == "") {
        $("#EcheanceDate").addClass('requiredField');
        toReturn = false;
    }
    //Le %  doit être  < 100
    if ($("#PourcentagePrime").val() != "" && parseFloat($("#PourcentagePrime").autoNumeric('get')) == 100) {
        $("#PourcentagePrime").val('');
        ShowCommonFancy("Error", "", "Le % prime ne peut pas être égale à 100", 1212, 700, true, true, true, false);
        toReturn = false;
    }
    //Le Montant doit être < reste à émettre
    if ($("#MontantEcheanceHT").val() != "" && parseFloat($("#MontantEcheanceHT").autoNumeric('get')) > parseFloat($("#MontantRestant").val().replace(',', '.'))) {
        $("#MontantEcheanceHT").val('');
        ShowCommonFancy("Error", "", "Le montant doit être inférieur au reste à payer", 1212, 700, true, true, true, false);
        toReturn = false;
    }
    //Calculé ou saisi, Le montant doit être > 0
    if ($("#PourcentagePrime").val() != "" && parseFloat($("#PourcentagePrime").autoNumeric('get')) == 0) {
        $("#PourcentagePrime").val('');
        ShowCommonFancy("Error", "", "Le % prime ne peut pas être égale à 0", 1212, 700, true, true, true, false);
        toReturn = false;
    }

    return toReturn;
}

//-------Supprimer une écheance-------
function SupprimerEcheance(elem) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let tabGuid = $("#tabGuid").val();
    let dateEcheance = elem.split("_")[1];
    let codeAvn = $("#NumAvenantPage").val();
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
}
//------Initialiser les contrôles de la div flottante UpdateEcheance
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
    }
    if (($("#EcheanceDate").val().split("/")[2] == $("#PeriodeDebut").val().split("/")[2] && $("#TaxeAttentatComptant").is(':checked')) || ($("#EcheanceDate").val() == "")) {
        $("#TaxeAttentat").attr('disabled', 'disabled');
    }
    else {
        $("#TaxeAttentat").removeAttr('disabled', 'disabled');
    }

}

//------fonction qui met à jour la liste des echeances-
function UpdateListeEcheances() {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let tabGuid = $("#tabGuid").val();
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

//------ fonction qui contrôle la validité du comptant -----
function CheckComptant(calcul) {
    let toReturn = true;
    if (calcul == false) {
        if ($("#ActeGestion").val() == "AFFNOUV") {
            if ((parseFloat($("#ComptantHT").autoNumeric('get')) == 0 || $("#ComptantHT").val() == "") && (parseFloat($("#PrimeComptant").autoNumeric('get')) == 0 || $("#PrimeComptant").val() == "")) {
                ShowCommonFancy("Error", "", "Il faut saisir un Comptant  HT ou un % prime", 1212, 700, true, true, true, false);
                toReturn = false;
            }
        }
    }
    if (parseFloat($("#ComptantHT").autoNumeric('get')) > parseFloat($("#PrimeHT").autoNumeric('get'))) {
        ShowCommonFancy("Error", "", "Le comptant HT doit être inférieur à Prime HT", 1212, 700, true, true, true, false);
        toReturn = false;
    }
    if (parseFloat($("#PrimeComptant").autoNumeric('get')) == 100) {
        ShowCommonFancy("Error", "", "Le % prime ne peut pas être égale à 100", 1212, 700, true, true, true, false);
        toReturn = false;
    }
    return toReturn;
}


//--------------------------------------#EndRegion Echeancier
//-------------------------------------#region btn details cotisations

//-----Ouvre la div flottante de visu quittances
function OpenVisulisationQuittances(typeQuittances, isEntete) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let codeAvn = $("#NumAvenantPage").val();

    /* récupération des infos du bandeau recherche */
    if (codeOffre == "" || version == "") {
        codeOffre = $("#CodeAffaireRech").val();
        version = $("#VersionRech").val();
        codeAvn = $("#CodeAvnRech").val();
    }
    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/OpenVisualisationQuittances",
        data: { codeOffre: codeOffre, codeAvn: codeAvn, version: version, typeQuittances: typeQuittances, isEntete: isEntete, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataVisuListeQuittances").html(data);
            AlbScrollTop();
            common.dom.pushForward("divDataVisuListeQuittances");
            $("#divVisuListeQuittances").show();
            MapElementVisuQuittances();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
   
}

function MapElementVisuQuittances() {
    $("#btnFraisAccess").die().live("click", function () {
        AfficherFraisAccessoires();
    });

    $("#btnDetailCot").die().live("click", function () {
        AfficherCoCourtiers();
    });

    $("#btnFermerVisualisationQuittance").die().live("click", function () {
        if (!window.sessionStorage) return;
        var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
        if (!filter) return;
        $("#Offre_CodeOffre").val(filter.CodeOffre);
        $("#Offre_Version").val(filter.version);
        $("#Offre_Type").val(filter.TypeContrat);
        $("#divDataVisuListeQuittances").html('');
        $("#divVisuListeQuittances").hide();
    });

    $("#VisuQuittanceDateEmission, #VisuQuittanceTypeOperation, #VisuQuittanceSituation, #VisuQuittancePeriodeDebut, #VisuQuittancePeriodeFin").die().live("change", function () {
        FiltrerVisualisationQuittances();
    });

    let fromCotsiPrefixeSelctor = "#divDataVisuListeQuittances ";

    $(fromCotsiPrefixeSelctor + "#VisuQuittanceDateEmission").datepicker({ dateFormat: 'dd/mm/yy' });
    $(fromCotsiPrefixeSelctor + "#VisuQuittancePeriodeDebut").datepicker({ dateFormat: 'dd/mm/yy' });
    $(fromCotsiPrefixeSelctor + "#VisuQuittancePeriodeFin").datepicker({ dateFormat: 'dd/mm/yy' });

    MapElementVisuQuittancesTableau();

}

function FiltrerVisualisationQuittances(colTri, fromCotis) {
    let fromCotsiPrefixeSelctor = fromCotis === true ? "#divCotisations " : "#divDataVisuListeQuittances ";
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let numeroAvenant = $("#NumAvenantPage").val() || selection.avn;
    let dateEmission = $(fromCotsiPrefixeSelctor + "#VisuQuittanceDateEmission").val();
    let typeOperation = $(fromCotsiPrefixeSelctor + "#VisuQuittanceTypeOperation").val();
    let situation = $(fromCotsiPrefixeSelctor + "#VisuQuittanceSituation").val();
    let periodeDebut = $(fromCotsiPrefixeSelctor + "#VisuQuittancePeriodeDebut").val();
    let periodeFin = $(fromCotsiPrefixeSelctor + "#VisuQuittancePeriodeFin").val();
    let isEntete = $(fromCotsiPrefixeSelctor + "#IsOpenedFromHeader").val();

    if (isEntete == undefined || isEntete == "") {
        isEntete = false;
    }

    if (colTri == undefined) {
        colTri = "QuittNumDESC";
    }

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/FiltrerVisualisationQuittances",
        data: {
            codeOffre: codeOffre, codeAvn: numeroAvenant, version: version, isEntete: true, dateEmission: dateEmission, typeOperation: typeOperation,
            situation: situation, datePeriodeDebut: periodeDebut, datePeriodeFin: periodeFin, modeNavig: $("#quittanceNavig").val(), colTri: colTri,
            tabGuid: $("#tabGuid").val()
        },
        success: function (data) {
            $(fromCotsiPrefixeSelctor + "#divVisuQuittancesBody").html(data);

            if (colTri != "") {
                $(fromCotsiPrefixeSelctor + ".imageTri[albmodeaff='Visu']").attr("src", "/Content/Images/tri.png");
            }

            switch (colTri) {
                case "QuittNum":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNum'][albmodeaff='Visu']").attr("albcontext", "QuittNumDESC");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNum'][albmodeaff='Visu']").attr("albcontext", "QuittNumDESC").attr("src", "/Content/Images/tri_asc.png");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    break;
                case "QuittNumDESC":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum").attr("src", "/Content/Images/tri_desc.png");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    break;

                case "QuittNumInt":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumInt'][albmodeaff='Visu']").attr("albcontext", "QuittNumIntDESC");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumInt'][albmodeaff='Visu']").attr("albcontext", "QuittNumIntDESC").attr("src", "/Content/Images/tri_asc.png");
                    break;
                case "QuittNumIntDESC":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNum");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt").attr("src", "/Content/Images/tri_desc.png");
                    break;

                case "DateEch":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='DateEch'][albmodeaff='Visu']").attr("albcontext", "DateEchDESC");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='DateEch'][albmodeaff='Visu']").attr("albcontext", "DateEchDESC").attr("src", "/Content/Images/tri_asc.png");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    break;
                case "DateEchDESC":
                    $(fromCotsiPrefixeSelctor + "th[albcontext='DateEchDESC'][albmodeaff='Visu']").attr("albcontext", "DateEch");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='DateEchDESC'][albmodeaff='Visu']").attr("albcontext", "DateEch").attr("src", "/Content/Images/tri_desc.png");
                    $(fromCotsiPrefixeSelctor + "th[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    $(fromCotsiPrefixeSelctor + "img[albcontext='QuittNumIntDESC'][albmodeaff='Visu']").attr("albcontext", "QuittNumInt");
                    break;
                default:
                    break;
            }

            MapElementVisuQuittancesTableau(fromCotis);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementVisuQuittancesTableau(fromCotis) {
    AlternanceLigneSpecifique("#tblVisuQuittancesBody", "", false, null);
    $("td[name=linkVisu]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            let num = $(this).attr("id");
            if (num != undefined) {
                num = num.split("_")[1];
            }
            let numAvn = $(this).attr("albnumavn");

            //ach
            //VisualisationDetailsQuittance(num, numAvn, (num != undefined).toString());
            AfficherDetailsCotisation("Visu", num);
           
            MapElementsQuittanceTransverse();

        });
    });

    $("img[name=btnReediter]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            let num = $(this).attr("id");
            if (num != undefined) {
                num = num.split("_")[1];
            }
            if (num != undefined)
                ReediterBulletinAvis(num);
        });
    });

    $("th[name='headerTri']").die();
    $("th[name='headerTri']").live("click", function () {
        let colTri = $(this).attr("albcontext");
        let modeAffi = $(this).attr("albmodeaff");

        if (modeAffi != "Visu") {
            FiltrerAnnulationQuittances(colTri);
        }
        else {
            FiltrerVisualisationQuittances(colTri, fromCotis);
        }
    });

    if (fromCotis === true)
        $('#divCotisations .divHeightVisuquittances').css('height', '294px');
 
    FormatDecimalValueEcheancier();
}

function VisualisationDetailsQuittance(numeroQuittance, numAvn, sourceAnnQuitt, reguleId) {
    AfficherCotisation("Visu", numeroQuittance, numAvn, sourceAnnQuitt, reguleId);
    MapElementsQuittanceTransverse();
}

//--------------Affiche la div flottante de réédition des bulletin et avis
function ReediterBulletinAvis(numQuittanceVisuParam) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let numAvenant;
    let numQuittanceVisu = numQuittanceVisuParam;
    if (numQuittanceVisu == undefined)
        numQuittanceVisu = $("#numQuittanceVisu").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "" || type == undefined || type == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                numAvenant = infoRows.split("_")[3];
            }
        }
    }
    if (numAvenant == undefined)
        numAvenant = $("#NumAvenantPage").val();
    if (numAvenant == undefined)
        numAvenant = 0;

    ShowLoading();
    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: numAvenant, numQuittanceVisu: numQuittanceVisu },
        url: "/Quittance/GetReeditionBulletinAvis",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataBulletinAvis").html(data);
            AlbScrollTop();

            $("#btnFermerBulletinAvis").die().live("click", function () {
                $("#divFullScreenBulletinAvis").hide();
                $("#divDataBulletinAvis").html("");
            });

            $("#btnLancerBulletinAvis").die().live("click", function () {
                LancerBulletinAvis();
            });

            $("#divFullScreenBulletinAvis").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementsQuittanceTransverse() {
    $('#btnFermerQuittance').die();
    $('#btnFermerQuittance').live('click', function (evt) { 
        //ReactivateShortCut();
        $("#divDataDetailsCotisation").html('');
        $("#divFullScreenDetailsCotisation").hide();
    });

    $('#tabDetails').die();
    $('#tabDetails').live('click', function (evt) {
        AfficherDetailsCotisation($("input[id='ModeAffichage'][albcontext='quitTrans']").val());
    });

    $('#tabVentilationDetaillee').die();
    $('#tabVentilationDetaillee').live('click', function (evt) {
        AfficherVentilationDetaillee($("input[id='ModeAffichage'][albcontext='quitTrans']").val());
    });

    $('#tabVentilationCommission').die();
    $('#tabVentilationCommission').live('click', function (evt) {
        AfficherVentilationCommission($("input[id='ModeAffichage'][albcontext='quitTrans']").val());
    });

    $('#tabPartAlbingia').die();
    $('#tabPartAlbingia').live('click', function () {
        AfficherPartAlbingia();
    });

    $('#tabVentilationCoassureurs').die();
    $('#tabVentilationCoassureurs').live('click', function () {
        AfficherVentilationCoassureurs();
    });

}

//--------------Affiche la div flottante cotisation---------------
function AfficherCotisation(modeAffichageParam, numQuittanceVisuParam, numAvn, sourceAnnQuitt, reguleId) {
    debugger;
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();
    let modeAffichage = modeAffichageParam;
    let numQuittanceVisu = numQuittanceVisuParam;
    let numAvenant = numAvn;
    let acteGestion = $("#ActeGestion").val();
    if (modeAffichage == undefined) {
        modeAffichage = $("#ModeAffichage").val();
    }
    if (numQuittanceVisu == undefined) {
        numQuittanceVisu = $("#numQuittanceVisu").val();
    }
    let modeNavig = $("#quittanceNavig").val();
    if (modeNavig == undefined) {
        modeNavig = $("#ModeNavig").val();
    }

    if (numAvenant != $("#NumAvenantPage").val()) {
        modeNavig = "H";
    }
    else {
        numAvenant = $("#NumAvenantPage").val() || selection.avn;
    }

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "" || type == undefined || type == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                if (numAvenant != infoRows.split("_")[3]) {
                    modeNavig = "H";
                }
                else {
                    modeNavig = "S";
                }
            }
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: numAvenant, modeNavig: modeNavig, acteGestion: acteGestion,
            acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId, tabGuid: $("#tabGuid").val(),
            numQuittanceVisu: numQuittanceVisu
        },
        url: "/Quittance/GetCotisation",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataCotisation").html(data);
            common.dom.pushForward("divDataCotisation");
            AlbScrollTop();

            let fromCotsiPrefixeSelctor = "#divDataCotisation ";

            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblBodyCommissionL", "", false, null);
            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblBodyCommissionR", "", false, null);

            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblBodyCommission", "", false, null);
            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblFormulesBody", "", false, null);
            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblFormulesBodyPleinEcran", "", false, null);
            AlternanceLigneSpecifique(fromCotsiPrefixeSelctor + "#tblBodyDetailsTotauxBase", "", false, null);

            $("button[type='button'][id='btnDetails'][albContext='quitTrans']").die().live("click", function () {
                AfficherDetailsCotisation("Visu", numQuittanceVisuParam);
            });

            $("td[id='lnkFraisAccQuittance'][albcontext='quitTrans']").die().live("click", function () {
                OpenFraisAccessoiresAvn(numQuittanceVisuParam, sourceAnnQuitt, codeOffre, version, type, numAvenant);
            });

            $("td[id='lnkFGAQuittance'][albcontext='quitTrans']").die().live("click", function () {
                OpenFraisAccessoiresAvn(numQuittanceVisuParam, sourceAnnQuitt, codeOffre, version, type, numAvenant);
            });

            $("#btnFermerVisuDetails").die().live("click", function () {
                $("#divDataCotisation").html("");
                $("#divFullScreenCotisation").hide();
            });
            ChkGererEcheancierChange();

            $("#divFullScreenCotisation").show();
            FormatDecimalValue();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}



//------------Fonction qui execute le bulletin/avis
function LancerBulletinAvis() {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let numAvenant = $("#NumAvenantPage").val();

    let numQuittanceVisu = $("#numQuittanceReeditionBulletin").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "" || type == undefined || type == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                numAvenant = infoRows.split("_")[3];
            }
        }
    }
    let nbExemplaire = $("#nbExemplaires").val();
    let typeCopie = $("#CopieDuplicata").val();
    let isAvisEcheance = $("#avisEcheance").is(":checked");

    ShowLoading();
    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: numAvenant, numQuittanceVisu: numQuittanceVisu, nbExemplaire: nbExemplaire, typeCopie: typeCopie, isAvisEcheance: isAvisEcheance },
        url: "/Quittance/LancerBulletinAvis",
        success: function (data) {
            if (data != "") {
                ShowCommonFancy("Info", "", data, 1212, 700, true, true, true, false);
            }
            $("#divFullScreenBulletinAvis").hide();
            $("#divDataBulletinAvis").html("");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function AfficherDetailsCotisation(modeAffichageParam, numQuittanceVisuParam) {
    ShowLoading();
    //alert("ok");
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();
    let codeAvn = selection.avn || $("#NumAvenantPage").val();
    let modeAffichage = modeAffichageParam;
    let numQuittanceVisu = numQuittanceVisuParam;
    let acteGestion = $("#ActeGestion").val();
    if (modeAffichage == undefined || modeAffichage == "") {
        modeAffichage = $("#ModeAffichage").val();
    }
    if (numQuittanceVisu == undefined) {
        let numQuittanceDivCotisation = $("#divDataCotisation input[id='numQuittanceVisu']").val();

        numQuittanceVisu = numQuittanceDivCotisation != undefined &&
            numQuittanceDivCotisation != ""
            ? numQuittanceDivCotisation
            : $("#numQuittanceVisu").val();
    }

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                codeAvn = infoRows.split("_")[3];
                acteGestion = selectRadio.attr("albtypeavt");
            }
        }
    }
    let reguleId = $("#ReguleId").val();

 
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: $("#ModeNavig").val(), modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu,
            acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId, tabGuid: $("#tabGuid").val()
        },
        url: "/Quittance/GetDetailsCotisation",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsCotisation").html(data);
            common.dom.pushForward("divDataDetailsCotisation");
            $("#natureContratQuittance").val($("#codeNatureContrat").val());
            AlternanceLigne("DetailsBodyTTC", "", false, null);
            AlternanceLigne("BodyDetailsCommission", "", false, null);
            AlternanceLigne("BodyDetailsTotaux", "", false, null);


            AlbScrollTop();
            $("#divFullScreenDetailsCotisation").show();
            $('#tabDetails').removeClass("onglet");
            $('#tabDetails').addClass("onglet-actif");

            $("#tabVentilationDetaillee").removeClass("onglet-actif");
            $("#tabVentilationDetaillee").addClass("onglet");

            $("#tabVentilationCommission").removeClass("onglet-actif");
            $("#tabVentilationCommission").addClass("onglet");
            FormatDecimalValueWithNegative();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Affiche la div flottante Ventilation détaillée------------
function AfficherVentilationDetaillee(modeAffichageParam) {
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();
    let natureContrat = $("#codeNatureContrat").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }
    if (natureContrat == undefined || natureContrat == "") {
        natureContrat = $("#natureContratQuittance").val();
    }
    else {
        natureContrat = natureContrat.split('-')[0];
    }

    let modeAffichage = modeAffichageParam;
    if (modeAffichage == undefined || modeAffichage == "")
        modeAffichage = $("#ModeAffichage").val();
    let numQuittanceVisu = $("#numQuittanceVisu").val();
    if (numQuittanceVisu == undefined || numQuittanceVisu == "") {
        let numQuittanceDivCotisation = $("#divDataCotisation input[id='numQuittanceVisu']").val();

        numQuittanceVisu = numQuittanceDivCotisation != undefined &&
            numQuittanceDivCotisation != ""
            ? numQuittanceDivCotisation
            : $("#numQuittanceVisu").val();
    }


    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/GetVentilationDetaillee",
        data: {
            codeOffre: codeOffre, version: version, type: type, natureContrat: natureContrat, modeNavig: $("#ModeNavig").val(),
            acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(),
            modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsCotisation").html(data);
            common.dom.pushForward("divDataDetailsCotisation");
            AlternanceLigne("VentilationDetailleeGarantieBody", "", false, null);
            AlternanceLigne("VentilationDetailleeTaxeBody", "", false, null);
            AlternanceLigne("VentilationDetailleeRegimeBody", "", false, null);
            $('#tabDetails').addClass("onglet");
            $('#tabDetails').removeClass("onglet-actif");

            $("#tabVentilationDetaillee").addClass("onglet-actif");
            $("#tabVentilationDetaillee").removeClass("onglet");

            $("#tabVentilationCommission").addClass("onglet");
            $("#tabVentilationCommission").removeClass("onglet-actif");
            CloseLoading();

            FormatDecimalValueWithNegative();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Affiche la div flottante Ventilation commission------------
function AfficherVentilationCommission(modeAffichageParam) {
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    let natureContrat = $("#codeNatureContrat").val();
    if (natureContrat == undefined || natureContrat == "") {
        natureContrat = $("#natureContratQuittance").val();
    }
    else {
        natureContrat = natureContrat.split('-')[0];
    }
    let modeAffichage = modeAffichageParam;
    if (modeAffichage == undefined || modeAffichage == "") {
        modeAffichage = $("#ModeAffichage").val();
    }
    let numQuittanceVisu = $("#numQuittanceVisu").val();
    if (numQuittanceVisu == undefined || numQuittanceVisu == "") {
        let numQuittanceDivCotisation = $("#divDataCotisation input[id='numQuittanceVisu']").val();

        numQuittanceVisu = numQuittanceDivCotisation != undefined &&
            numQuittanceDivCotisation != ""
            ? numQuittanceDivCotisation
            : $("#numQuittanceVisu").val();
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/GetVentilationCommission",
        data: {
            codeOffre: codeOffre, version: version, type: type, natureContrat: natureContrat, modeNavig: $("#ModeNavig").val(),
            acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(),
            modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsCotisation").html(data);
            common.dom.pushForward("divDataDetailsCotisation");
            AlternanceLigne("VentilationCommissionCourtierBody", "", false, null);
            AlternanceLigne("VentilationCommissionGarantieBody", "", false, null);
            $('#tabDetails').addClass("onglet");
            $('#tabDetails').removeClass("onglet-actif");

            $("#tabVentilationDetaillee").addClass("onglet");
            $("#tabVentilationDetaillee").removeClass("onglet-actif");

            $("#tabVentilationCommission").addClass("onglet-actif");
            $("#tabVentilationCommission").removeClass("onglet");
            CloseLoading();
            FormatDecimalValueWithNegative();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Affiche l'onglet part albingia
function AfficherPartAlbingia() {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    let modeAffichage = $("#ModeAffichage").val();
    let numQuittanceVisu = $("#numQuittanceVisu").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/GetTabPartAlbingia",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), natureContrat: $("#codeNatureContrat").val(),
            modeNavig: $("#ModeNavig").val(), acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(), modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsCotisation").html(data);

            AlternanceLigne("PartAlbingiaGarantieBody", "", false, null);

            CloseLoading();
            FormatDecimalValueWithNegative();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Affiche l'onglet ventilation coassureurs
function AfficherVentilationCoassureurs() {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    let modeAffichage = $("#ModeAffichage").val();
    let numQuittanceVisu = $("#numQuittanceVisu").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/GetTabVentilationCoassureurs",
        data: {
            codeOffre: codeOffre, version: version, type: type, natureContrat: $("#codeNatureContrat").val(), modeNavig: $("#ModeNavig").val(),
            acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(), modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsCotisation").html(data);

            AlternanceLigne("VentilationCoassBody", "", false, null);

            MapElementTabVentilationCoassureurs();
            CloseLoading();
            FormatDecimalValueWithNegative();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function AfficherVentilationCoassureursParGarantie(idCoass) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let libelleCoass = $("input[id='libelle_" + idCoass + "']").val();
    let partCoass = $("input[id='part_" + idCoass + "']").val();
    let modeAffichage = $("#ModeAffichage").val();
    let numQuittanceVisu = $("#numQuittanceVisu").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/GetVentilationCoassureurGaranties",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeCoAssureur: idCoass, libelleCoAssureur: libelleCoass, partCoAssureur: partCoass,
            acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(),
            modeAffichage: modeAffichage, numQuittanceVisu: numQuittanceVisu
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataDetailsVentilationCoassGarantie").html(data);
            AlternanceLigne("VentilationCoassGarantieBody", "", false, null);
            common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');
            $("#btnFermerDetailsCoassGarantie").die().live("click", function () {
                $("#divFullScreenDetails").hide();
            });
            CloseLoading();

            $("#divFullScreenDetails").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Map elements de l'onglet ventilation coassureur
function MapElementTabVentilationCoassureurs() {
    $("img[name=detailsCoassureur]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            let idCoass = $(this).attr("id");
            if (idCoass != undefined) {
                idCoass = idCoass.split("_")[1];
                AfficherVentilationCoassureursParGarantie(idCoass);
            }
        });
    });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
}
//-------Formate les input/span des valeurs positives ou negatives----------
function FormatDecimalValueWithNegative() {
    try {
        common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '99999999999', '-99999999999');
        common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');
        common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '-100.00');
    }
    catch (e) {
        console.error(e);
    }
}

//-------------------------------------#EndRegion btn details cotisations
//---------------------------------------#region FGA/Frais
//Ouvre la div des frais accessoires
function OpenFraisAccessoiresAvn(numQuittanceVisu, sourceAnnQuitt, codOffre, Version, Type, numAvenant) {
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let effetAnnee = $("#EffetAnnee").val();
    let tabGuid = $("#tabGuid").val();
    let acteGestion = $("#ActeGestion").val();
    let isEntete = $("#isOpenedFromHeader").val();
    let reguleId = $("#ReguleId").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "") {
        codeOffre = codOffre;
        version = Version;
        type = Type;
        codeAvn = numAvenant;
    }
    if (isEntete == undefined)
        isEntete = $("#isAnnulQuittance").val();

    if (isEntete == undefined)
        isEntete = false;

    if (sourceAnnQuitt == undefined) {
        sourceAnnQuitt = "";
    }
    let commentaire = "";

    let modeNavig = $("#ModeNavig").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, effetAnnee: effetAnnee, commentaire: commentaire,
            tabGuid: tabGuid, isReadonly: $("#OffreReadOnly").val(), modeNavig: modeNavig, acteGestion: acteGestion,
            acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId,
            isEntete: isEntete, numQuittanceVisu: numQuittanceVisu, sourceAnnQuitt: sourceAnnQuitt, isModifHorsAvn: $("#IsModifHorsAvn").val()
        },
        url: "/Quittance/GetFraisAccessoiresAvn",
        success: function (data) {

            $("#divDataFraisAccessoires").html(data);

            $("#divFraisAccessoires").show();

            MapElementPageFraisAccessoiresAvn();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementPageFraisAccessoiresAvn() {


    common.autonumeric.apply($("#FraisRetenusAvn"), 'init', 'numeric', '', null, null, '9999999', '0');

    $('#btnFermerFraisAccessoiresAvn').die();
    $('#btnFermerFraisAccessoiresAvn').click(function () {
        ReactivateShortCut();
        $("#divDataFraisAccessoires").html('');
        $("#divFraisAccessoires").hide();

    });
    $('#btnValiderFraisAccessoiresAvn').die();
    $('#btnValiderFraisAccessoiresAvn').click(function () {
        if ($("#FraisRetenusAvn").val() == "") {
            $("#FraisRetenusAvn").addClass('requiredField');
        }
        else {
            UpdateFraisAccessoiresAvn();
        }
    });

    $("#FraisRetenusAvn").live('change', function () {
        if ($("#FraisRetenusAvn").val() != "0") {
            $("#TaxeAttentatAvn").removeAttr('disabled');
        }
        else {
            $("#TaxeAttentatAvn").attr('disabled', 'disabled');
        }
    });
}
function UpdateFraisAccessoiresAvn() {

    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let acteGestion = $("#ActeGestion").val();
    let tabGuid = $("#tabGuid").val();
    let fraisRetenus = $("#FraisRetenusAvn").val();
    let taxeAttentat = $("#TaxeAttentatAvn").is(':checked');
    if (fraisRetenus == undefined || fraisRetenus == "") {
        fraisRetenus = 0;
    }
    let reguleId = $("#ReguleId").val();
    let isModifHorsAvn = $("#IsModifHorsAvn").val() === "True";
    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, acteGestion: acteGestion,
            acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId, tabGuid: tabGuid,
            fraisRetenus: fraisRetenus, taxeAttentat: taxeAttentat, modeNavig: $("#ModeNavig").val()
        },
        url: "/Quittance/UpdateFraisAccessoiresAvn",
        success: function (data) {
            if (!isModifHorsAvn) {
                $("#divCotisations").html(data);
                MapElementQuittanceTransversePage();
                quittance.initPage();
            } else {
                toggleDescription($("#CommentForce"), true);
                $("div[id=zoneTxtArea][albcontext=CommentForce]").html(commentaires);
                $("#CommentForce").html(commentaires);
            }

            $("#divDataFraisAccessoires").html('');
            $("#divFraisAccessoires").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------------------------------#EndRegion region FGA/Frais


//--------------Affiche la div flottante de frais accessoires--------------
function AfficherFraisAccessoires() {
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val() || selection.avn;
    let effetAnnee = $("#EffetAnnee").val();
    let tabGuid = $("#tabGuid").val();
    let isModifHorsAvn = $("#IsModifHorsAvn").val() === "True";
    let isReadOnly = window.isReadonly || !$("#Offre_CodeOffre").val();
    let commentaire = isReadOnly && !isModifHorsAvn ? $("#CommentForce").html() : $("#CommentForce").val();
    let acteGestion = $("#ActeGestion").val();
    let reguleId = $("#ReguleId").val();
    let modeNavig = $("#ModeNavig").val();
    let visuQuitt = !$("#Offre_CodeOffre").val();

    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "" || type == undefined || type == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                numAvenant = infoRows.split("_")[3];
            }
            isReadOnly = true;
            effetAnnee = "0";
            visuQuitt = true;
        }
    }

    if (effetAnnee == undefined)
        effetAnnee = "0";

    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, effetAnnee: effetAnnee, commentaire: commentaire, tabGuid: tabGuid,
            isReadonly: isReadOnly, modeNavig: modeNavig, acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: reguleId, isModifHorsAvn: isModifHorsAvn
        },
        url: "/Quittance/GetFraisAccessoires",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataFraisAccessoires").html(data);
            AlbScrollTop();
            $("#divFraisAccessoires").show();
            FormatNumericValue();
            MapElementPageFraisAccessoires(visuQuitt);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Map elements de la div flottante frais accessoires---------
function MapElementPageFraisAccessoires(visuQuitt) {
    $('#btnFermerFraisAccessoires').kclick(function () {
        ReactivateShortCut();
        $("#divDataFraisAccessoires").html('');
        $("#divFraisAccessoires").hide();
        toggleDescription($("#CommentForce"), true);
    });
    $('#btnValiderFraisAccessoires').kclick(function () {
        if (!quittance) return;
        if ($("#TypeFrais").val() == "S" && $("#FraisStandards").attr("albModifStd") != "") {
            let modifFraisStd = $("#FraisStandards").attr("albModifStd");
            let fraisStd = $("#FraisStandards").val();

            modifFraisStd = "_" + modifFraisStd + "_";
            if (modifFraisStd.replace("_" + fraisStd + "_", "_") != modifFraisStd) {
                quittance.updateFraisAccessoires();
            }
            else {
                ShowCommonFancy("Error", "", "Erreur de frais standards<br/>Frais standards valides : " + $("#FraisStandards").attr("albModifStd").replace(/_/g, "€ ou ") + "€", 300, 80, true, true, true);
                return false;
            }
        }
        else {
            quittance.updateFraisAccessoires();
        }
    });

    if (!window.isReadonly || $("#IsModifHorsAvn").val() === "True") {
        if ($("#TypeFrais").val() == "P") {
            $("#FraisRetenus").removeAttr('readonly', 'readonly').removeClass('readonly');
        }
        else {
            if ($("#TypeFrais").val() == "S")
                $("#FraisRetenus").val($("#FraisStandards").val());
            if ($("#TypeFrais").val() == "N")
                $("#FraisRetenus").val(0);
            $("#FraisRetenus").attr('readonly', 'readonly').addClass('readonly');
        }
    }
    TypeFraisChange();

    if (!window.isReadonly) {
        if ($("#AppliqueFraisSpecifiques").is(':checked')) {
            $("#FraisSpecifiques").removeAttr('readonly', 'readonly').removeClass('readonly');
        }
        else {
            $("#FraisSpecifiques").attr('readonly', 'readonly').addClass('readonly');
            $("#FraisSpecifiques").val(0);
        }
    }
    ChkFraisSpecifiquesChange();
    let acteGestion = $("#ActeGestion").val();
    if (!window.isReadonly && !visuQuitt) {
        $("#Commentaires").html($("#CommentairesFraisAcc").html().replace(/&lt;br&gt;/ig, "\n"));
    }
    toggleDescription($("#CommentairesFraisAcc"), true);
    if (window.isReadonly && $("#IsModifHorsAvn").val() === "False") {
        $("#btnFermerFraisAccessoires").html("<u>F</u>ermer").attr("data-accesskey", "f");
        $("#btnValiderFraisAccessoires").hide();
    }

    AffectTitleList($("#TypeFrais"));

}
function TypeFraisChange() {
    $("#TypeFrais").die();
    $("#TypeFrais").change(function () {
        AffectTitleList($(this));
        if ($("#TypeFrais").val() == "P") {
            $("#FraisRetenus").removeAttr('readonly', 'readonly').removeClass('readonly');
            $("#FraisStandards").attr('readonly', 'readonly').addClass('readonly');
        }
        else {
            if ($("#TypeFrais").val() == "S") {
                $("#FraisRetenus").val($("#FraisStandards").val());
                if ($("#FraisStandards").attr("albmodifstd").toLowerCase() != "") {
                    $("#FraisStandards").removeAttr('readonly', 'readonly').removeClass('readonly');
                }
            }
            if ($("#TypeFrais").val() == "N") {
                $("#FraisRetenus").val(0);
                $("#FraisStandards").attr('readonly', 'readonly').addClass('readonly');
            }
            $("#FraisRetenus").attr('readonly', 'readonly').addClass('readonly');
        }
    });
}

function ChkFraisSpecifiquesChange() {
    $("#AppliqueFraisSpecifiques").die();
    $("#AppliqueFraisSpecifiques").click(function () {
        if ($("#AppliqueFraisSpecifiques").is(':checked')) {
            $("#FraisSpecifiques").removeAttr('readonly', 'readonly').removeClass('readonly');
        }
        else {
            $("#FraisSpecifiques").attr('readonly', 'readonly').addClass('readonly');
            $("#FraisSpecifiques").val(0);
        }
    });

}

//--------------Affiche la div flottante des co-courtiers------------------
function AfficherCoCourtiers() {
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val() || selection.avn;
    let tabGuid = $("#tabGuid").val();
    let commentaire = window.isReadonly ? $("#CommentForce").html() : $("#CommentForce").val();
    let acteGestion = $("#ActeGestion").val();
    let modeAffiche = "Popup";
    let forceReadOnly = $("#isOpenedFromHeader").hasTrueVal() || !$("#Offre_CodeOffre").val();
    if (codeOffre == undefined || version == undefined || codeOffre == "" || version == "" || type == undefined || type == "") {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        if (selectRadio != undefined) {
            let infoRows = $(selectRadio).val();
            if (infoRows != undefined) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
                type = infoRows.split("_")[2];
                codeAvn = infoRows.split("_")[3];
            }
            forceReadOnly = true;
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid,
            modeNavig: $("#ModeNavig").val(), acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(), modeAffiche: modeAffiche,
            forceReadOnly: forceReadOnly
        },
        url: "/Quittance/LoadCoCourtiers",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataCoCourtiers").html(data);
            if ($('#divButtonCourtier').hasClass('requiredButton')) {
                $("#divButtonCourtier").removeClass('requiredButton');
                $("div[id=zoneTxtArea][albcontext=commentairesCommissionCourtier]").addClass('requiredButton');
            }
            $("div[id=zoneTxtArea][albcontext=commentairesCommissionCourtier]").html(commentaire);
            $("#commentairesCommissionCourtier").html(commentaire);

            AlbScrollTop();
            $("#divCoCourtiers").show();
            MapElementAdditionnelCoCourtier(forceReadOnly, modeAffiche);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Map les éléments propre au mode popup des CoCourtiers
function MapElementAdditionnelCoCourtier(forceReadOnly, modeAffichage) {
   let isModifHorsAvn = $("#IsModifHorsAvn").val() === "True";
   if (forceReadOnly && !isModifHorsAvn) {
        $("img[name=supprCourtier]").hide();
        $("img[name=modifCourtier]").hide();
        $("img[name=ajoutCourtier]").hide();

        $("#chkTauxCAT").attr('disabled', 'disabled');
        $("#chkTauxHCAT").attr('disabled', 'disabled');
    }

    $("#btnAnnulerCoCourtierPopup").unbind().bind('click', function () {
        $("#divDataCoCourtiers").html('');
        $("#divCoCourtiers").hide();
        ReactivateShortCut();
        toggleDescription($("#CommentForce"), true);
    });

    $("#btnSuivantCoCourtierPopup").unbind().bind('click', function () {
        SumbitPage(false,modeAffichage);
    });


}
