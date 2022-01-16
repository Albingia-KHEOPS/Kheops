$(document).ready(function () {
    MapElementPage();
   
});
//--------Map les éléments de la page---------
function MapElementPage() {
    toggleDescription();
    if ($("#AvenantError").hasVal()) {
        ShowCommonFancy("Error", "", $("#AvenantError").val(), 300, 80, true, true, true);
    }
    
    $("#btnAnnuler").kclick(function () {
        if (window.isReadonly) {
            Cancel();
        }
        else {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        }
    });
    $("#btnSuivant").kclick(function (evt, data) {
        CreateAvenant(data && data.returnHome);
    });

    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Cancel();
                break;
            case "ContinueSave":
                $("[name=TypeGestion]").attr('albConfirm', 'Y');
                CreateAvenant();
                break;
        }
        $("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").clear();
    });

    ChangeHour();

    AlternanceLigne("AlerteAvenant", "", false, null);

    $("#ResilAvenant").kclick(function () {
        ReloadAvenantResil();
    });

    $("#DateFinGarantie").offOn("change", function () {
        if ($(this).val() != "") {
            if ($("#HeureFinGaranties").val() == "") {
                $("#HeureFinGarantiesHours").val("23");
                $("#HeureFinGarantiesMinutes").val("59");
                $("#HeureFinGarantiesHours").change();
            }
        }
        else {
            $("#HeureFinGarantiesHours").clear();
            $("#HeureFinGarantiesMinutes").clear();
            $("#HeureFinGarantiesHours").change();
        }
    });

    $("#DateEffetAvt").offOn("change", function () {
        if ($(this).val() != "") {
            if (!$("#HeureEffetAvtHours").hasVal()) {
                $("#HeureEffetAvtHours").val("00");
                $("#HeureEffetAvtMinutes").val("00");
                $("#HeureEffetAvtHours").change();
            }
        }
        else {
            $("#HeureEffetAvtHours").clear();
            $("#HeureEffetAvtMinutes").clear();
            $("#HeureEffetAvtHours").change();
        }
    });

    $("td[id='linkAlerte']").click(function () {
        OpenAlerte($(this));
    });

    AffectDateFormat();
}
//----------Recharge les données de l'avenant de résiliation---------
function ReloadAvenantResil() {
    var isChecked = $("#ResilAvenant").isChecked();
    if (isChecked) {
        $("#dvDateFinHEch").hide();
        $("#dvDateFinEch").show();

        $("#HeureFinGarantiesHours").val("23").disable(true);
        $("#HeureFinGarantiesMinutes").val("59").disable(true);
        $("#HeureFinGarantiesHours").change();
    }
    else {
        $("#NumAvt").val($("#NumAvtHide").val());
        $("#dvDateFinHEch").show();
        $("#dvDateFinEch").hide();

        $("#HeureFinGarantiesHours").enable();
        $("#HeureFinGarantiesMinutes").enable();
    }


}
//----------Annule et retourne sur l'écran de recherche----------
function Cancel() {
    var tabGuid = $("#tabGuid").val();
    DeverouillerUserOffres(tabGuid);
    Redirection("RechercheSaisie", "Index");
}
//----------Affecte les fonctions sur les contrôles d'heure-------
function ChangeHour() {
    $("#HeureEffetAvtHours").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureEffetAvtMinutes").val() == "")
            $("#HeureEffetAvtMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureEffetAvtMinutes").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureEffetAvtHours").val() == "")
            $("#HeureEffetAvtHours").val("00");
        AffecteHour($(this));
    });
    $("#HeureFinGarantiesHours").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureFinGarantiesMinutes").val() == "")
            $("#HeureFinGarantiesMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureFinGarantiesMinutes").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureFinGarantiesHours").val() == "")
            $("#HeureFinGarantiesHours").val("00");
        AffecteHour($(this));
    });

    $("#HeureRemiseVigHours").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureRemiseVigMinutes").val() == "")
            $("#HeureRemiseVigMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureRemiseVigMinutes").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureRemiseVigHours").val() == "")
            $("#HeureRemiseVigHours").val("00");
        AffecteHour($(this));
    });

    $("#HeureResilHours").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureResilMinutes").val() == "")
            $("#HeureResilHours").val("00");
        AffecteHour($(this));
    });
    $("#HeureResilMinutes").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureResilHours").val() == "")
            $("#HeureResilMinutes").val("00");
        AffecteHour($(this));
    });

    $("#HeureFinEffetHours").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureFinEffetMinutes").val() == "")
            $("#HeureFinEffetHours").val("00");
        AffecteHour($(this));
    });

    $("#HeureFinEffetMinutes").offOn("change", function () {
        if ($(this).val() != "" && $("#HeureFinEffetHours").val() == "")
            $("#HeureFinEffetMinutes").val("00");
        AffecteHour($(this));
    });

}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "");

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").clear();
        $("#" + elemId + "Minutes").clear();
    }
}


//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvenant = $("#NumInterne").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var codeAvenantExterne = $("#NumAvt").val();

    $.ajax({
        type: "POST",
        url: "/CreationAvenant/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
            addParamType: addParamType, addParamValue: addParamValue, codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(), reguleId: $("#ReguleId").val()
        },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Créer l'avenant--------
function CreateAvenant(returnHome) {
    ShowLoading();
    $(".requiredField").removeClass("requiredField");
    let typeAvt = common.$stringVal("TypeAvt").split("-").first().trim();

    if (window.isReadonly || window.isModifHorsAvenant) {
        switch (typeAvt) {
            case "AVNMD":
            case "AVNRG":
            case "REGUL":
            case "AVNRS":
                if (typeAvt !== "AVNRS" || !window.isModifHorsAvenant) {
                  
                    Redirection(returnHome ? "RechercheSaisie" : "AvenantInfoGenerales", "Index");
                    return;
                }
                break;
            case "AVNRM":
                Redirection(returnHome ? "RechercheSaisie" : "AnnulationQuittances", "Index");
                return;
            default:
                return;
        }
    }

    if ($("input[name='inputAlerte']:not([value=''])").length > 0) {
        common.dialog.error("Veuillez visualiser toutes les alertes bloquantes");
        CloseLoading();
        return false;
    }

    var errorCheck = false;
    switch (typeAvt) {
        case "AVNMD":
        case "AVNRG":
            errorCheck = CheckDataAvtModif();
            break;
        case "AVNRS":
            errorCheck = CheckDataAvtResil();
            break;
        case "AVNRM":
            errorCheck = CheckDataAvtRemisVig();
            break;
        default:
            break;
    }

    if (errorCheck) {
        CloseLoading();
        return false;
    }
    else {
        var modeleAvtModif = {
            TypeAvt: typeAvt,
            NumInterneAvt: $("#NumInterne").val(),
            DateEffetAvt: $("#DateEffetAvt").val(),
            HeureEffetAvt: $("#HeureEffetAvt").val(),
            NumAvt: $("#NumAvt").val(),
            MotifAvt: $("#MotifAvt").val(),
            DescriptionAvt: $("#Description").val(),
            //ObservationsAvt: $.trim($("#Observation").html().replace(/<br>/ig, "\n")),
            IsModifHorsAvenant: window.isModifHorsAvenant
        };
        var argModeleAvtModif = typeAvt == "AVNMD" || typeAvt == "AVNRG" ? JSON.stringify(modeleAvtModif) : "";

        var modeleAvtResil = {
            TypeAvt: typeAvt,
            NumInterneAvt: $("#NumInterne").val(),
            AvenantEch: $("#ResilAvenant").isChecked(),
            DateFinGarantie: $("#ResilAvenant").isChecked() ? $("#DateFin").val() : $("#DateFinGarantie").val(),
            HeureFinGarantie: $("#HeureFinGaranties").val(),
            NumAvt: $("#NumAvt").val(),
            MotifAvt: $("#MotifAvt").val(),
            DescriptionAvt: $("#Description").val(),
            //ObservationsAvt: $.trim($("#Observation").html().replace(/<br>/ig, "\n")),
            IsModifHorsAvenant: window.isModifHorsAvenant
        };

        var modeleRemiseVig = {};

        if (typeAvt == "AVNRM") {
            modeleRemiseVig = {
                TypeAvt: typeAvt,
                NumInterneAvt: $("#NumInterne").val(),
                NumAvt: $("#NumAvt").val(),
                DateRemiseVig: $("#DateRemiseVig").val(),
                HeureRemiseVig: $("#HeureRemiseVig").val(),
                DescriptionAvt: $("#Description").val(),
                ObservationsAvt: $.trim($("#ObservationsAvt").html().replace(/<br>/ig, "\n")),
                DateResil: $("#DateResil").val(),
                HeureResil: $("#HeureResil").val(),
                TypeGestion: $("#TypeGestion").val(),
                PrimeReglee: $("#PrimeReglee").val(),
                PrimeReglementDate: $("#PrimeReglementDate").val(),
                DateSuspDeb: $("#DateSuspDeb").val(),
                DateSuspFin: $("#DateSuspFin").val(),
                DateFinEffet: $("#DateFinEffet").val(),
                HeureFinEffet: $("#HeureFinEffet").val(),
                IsModifHorsAvenant: window.isModifHorsAvenant
            };
        }

        var argModeleAvtResil = typeAvt == "AVNRS" ? JSON.stringify(modeleAvtResil) : "";
        var argModeleRemiseVig = typeAvt == "AVNRM" ? JSON.stringify(modeleRemiseVig) : "";

        $.ajax({
            type: "POST",
            url: "/CreationAvenant/CreateAvenant",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                typeAvt: typeAvt, modeAvt: $("#ModeAvt").val(),
                souscripteur: $("#SouscripteurCode").val(), gestionnaire: $("#GestionnaireCode").val(),
                argModeleAvtModif: argModeleAvtModif, argModeleAvtResil: argModeleAvtResil, argModeleRemiseVig: argModeleRemiseVig
            },
            success: function (data) {
                if (data && data.length) {
                    console.error(data);
                    data.filter(function (x) { return x.indexOf(" ") < 0; }).forEach(function (x) {
                        if (x === "HeureEffetAvt") {
                            $("#" + x + "Hours").setInvalid();
                            $("#" + x + "Minutes").setInvalid();
                        }
                        else {
                            $("#" + x).setInvalid();
                        }
                    });
                    common.dialog.error(data.filter(function (x) { return x.indexOf(" ") > 0; }).join("<br />"));
                    CloseLoading();
                }
                else {
                    if (returnHome) {
                        DeverouillerUserOffres($("#tabGuid").val());
                        Redirection("RechercheSaisie", "Index");
                        return;
                    }
                    switch ($.trim(typeAvt)) {
                        case "AVNMD":
                        case "AVNRG":
                            Redirection("AvenantInfoGenerales", "Index");
                            break;
                        case "AVNRM":
                            Redirection($("#TypeGestion").val() == "V" ? "Quittance" : "AvenantInfoGenerales", "Index");
                            break;
                        case "AVNRS":
                            Redirection(window.isModifHorsAvenant ? "AvenantInfoGenerales" : "AnnulationQuittances", "Index");
                            break;
                        default:
                            break;
                    }
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//-------Vérifie les données de l'avenant de modification-------
function CheckDataAvtModif() {
    if ($("#HeureEffetAvt").val() == "") {
        $("#HeureEffetAvtHours").addClass("requiredField");
        $("#HeureEffetAvtMinutes").addClass("requiredField");
    }

    if ($("#NumAvt").val() == "" || parseInt($("#NumAvt").val()) < parseInt($("#NumInterne").val())) {
        $("#NumAvt").addClass("requiredField");
    }

    if ($("#MotifAvt").val() == "") {
        $("#MotifAvt").addClass("requiredField");
    }
    
    if ($(".requiredField").exists()) {
        return true;
    }

    if ($("#DateEffetAvt").val() == "") {
        $("#DateEffetAvt").addClass("requiredField");
        return true;
    }
    
    let error = false;
    //Contrôle avec la date de début d'effet de garantie (PBEFA/M/J)
    let heureAvn = $("#HeureEffetAvtHours").val() + $("#HeureEffetAvtMinutes").val();
    if (getDate($("#EffetGaranties").val(), $("#HeureEffetGaranties").val()) > getDate($("#DateEffetAvt").val(), heureAvn)) {
        common.dialog.error("La date d'effet de l'avenant ne doit pas être antérieure à la date d'effet du contrat");
        error = true;
    }
    // B1738
    // Description : Pour les contrats en tempo, date d’effet de l’avenant de modif = ouvert à toutes les dates, dates comprise ds la période de garanties
    var endEffectDate = $("#FinEffet").val();
    var endEffectHour = $("#FinEffetHeure").val();
    var effectAvnDate = $("#DateEffetAvt").val();
    var effectAvnHour = $("#HeureEffetAvt").val();
    if (endEffectDate && effectAvnDate) {
        if (getDate(effectAvnDate, effectAvnHour) > getDate(endEffectDate, endEffectHour)) {
            common.dialog.error("La date d'effet de l'avenant ne doit pas être postérieure à la date de fin d'effet du contrat");
            $("#DateEffetAvt").addClass("requiredField");
            $("#HeureEffetAvtHours").addClass("requiredField");
            $("#HeureEffetAvtMinutes").addClass("requiredField");
            error = true;
        }
    }

    //Contrôle si la périodicité est différente de "U" & "E" (Tacite)
    if ($("#CodePeriodicite").val() != "U" && $("#CodePeriodicite").val() != "E") {
        //Récupération des numéros d'avenant (en cours & histo)
        var numAvt = $("#NumAvenantPage").val() - 0;
        var numAvtPrec = $("#NumInterne").val() - 1;

        //Contrôle de la prochaine échéance lors de la création
        if (numAvt == numAvtPrec && $("#Echeance").val() != "") {
            if (getDate($("#DateEffetAvt").val(), $("#HeureEffetAvt").val()) > getDate($("#Echeance").val())) {
                ShowCommonFancy("Error", "", "Incohérence entre la prochaine échéance et la date d'avenant, " + $("#Echeance").val(), 300, 80, true, true, true);
                $("#DateEffetAvt").addClass("requiredField");
                $("#HeureEffetAvtHours").addClass("requiredField");
                $("#HeureEffetAvtMinutes").addClass("requiredField");
                $("#Echeance").addClass("requiredField");
                error = true;
            }
        }

        //Contrôle de la prochaine échéance lors de la modification
        if (numAvt != numAvtPrec && $("#EcheanceHisto").val() != "") {
            if (getDate($("#DateEffetAvt").val(), $("#HeureEffetAvt").val()) > getDate($("#EcheanceHisto").val())) {
                common.dialog.error("Date d'avenant incohérente avec la prochaine échéance en historique");
                $("#DateEffetAvt").addClass("requiredField");
                $("#HeureEffetAvtHours").addClass("requiredField");
                $("#HeureEffetAvtMinutes").addClass("requiredField");
                $("#EcheanceHisto").addClass("requiredField");
                error = true;
            }
        }
    }
    
    return error;
}

//-------Vérifie les données de l'avenant de résiliation--------
function CheckDataAvtResil() {
    var error = false;
    if ($("#ResilAvenant").isChecked()) {
        if ($("#DateFin").val() == "") {
            $("#DateFin").addClass("requiredField");
            error = true;
        }
    }
    else {
        if ($("#DateFinGarantie").val() == "") {
            $("#DateFinGarantie").addClass("requiredField");
            error = true;
        }
        else {
            if (!checkDate($("#EffetGaranties"), $("#DateFinGarantie"))) {
                common.dialog.error("La date de fin d'effet de l'avenant ne doit pas être antérieure à la date d'effet du contrat");
                error = true;
            }
            //if ($("#FinEffetPrev").val() != "" && checkDate($("#DateFinGarantie"), $("#FinEffetPrev"))) {
            if ($("#FinEffetPrev").val() != "" && getDate($("#DateFinGarantie").val(), $("#HeureFinGaranties").val()) > getDate($("#FinEffetPrev").val())) {
                common.dialog.error("La date de fin d'effet de l'avenant ne doit pas être postérieure à la date de fin d'effet du contrat");
                error = true;
            }

            if ($("#CodePeriodicite").val() != "U" && $("#CodePeriodicite").val() != "E" && $("#CodePeriodicite").val() != "R") {
                var numAvt = $("#NumAvenantPage").val() - 0;
                var numAvtPrec = $("#NumInterne").val() - 1;
                if (numAvt == numAvtPrec && $("#Echeance").val() != "") {
                    if (getDate($("#DateFinGarantie").val(), $("#HeureFinGaranties").val()) >= getDate($("#Echeance").val())) {
                        common.dialog.error("La date de fin de garantie doit être antérieure à la prochaine échéance");
                        $("#DateFinGarantie").addClass("requiredField");
                        $("#HeureFinGarantiesHours").addClass("requiredField");
                        $("#HeureFinGarantiesMinutes").addClass("requiredField");
                        $("#Echeance").addClass("requiredField");
                        error = true;
                    }
                }
                if (numAvt != numAvtPrec && $("#EcheanceHisto").val() != "") {
                    if (getDate($("#DateFinGarantie").val(), $("#HeureFinGaranties").val()) >= getDate($("#EcheanceHisto").val())) {
                        ShowCommonFancy("Error", "", "La date de fin de garantie doit être antérieure à la prochaine échéance historique (" + $("#EcheanceHisto").val().split(" ")[0] + ") ", 300, 80, true, true, true);
                        $("#DateFinGarantie").addClass("requiredField");
                        $("#HeureFinGarantiesHours").addClass("requiredField");
                        $("#HeureFinGarantiesMinutes").addClass("requiredField");
                        $("#EcheanceHisto").addClass("requiredField");
                        error = true;
                    }
                }
            }
        }
    }
    if ($("#HeureFinGaranties").val() == "") {
        $("#HeureFinGarantiesHours").addClass("requiredField");
        $("#HeureFinGarantiesMinutes").addClass("requiredField");
        error = true;
    }
    if ($("#NumAvt").val() == "" || parseInt($("#NumAvt").val()) < parseInt($("#NumInterne").val())) {
        $("#NumAvt").addClass("requiredField");
        error = true;
    }
    if ($("#MotifAvt").val() == "") {
        $("#MotifAvt").addClass("requiredField");
        error = true;
    }
    return error;
}

//-------Vérifie les données de l'avenant de remise en vigueur
function CheckDataAvtRemisVig() {
    let error = false;


    // date de remise en vigueur obligatoire
    if ($("#DateRemiseVig").val() == "" || $("#HeureRemiseVigHours").val() == "" || $("#HeureRemiseVigMinutes").val() == "") {
        $("#DateRemiseVig").addClass("requiredField");
        $("#HeureRemiseVigHours").addClass("requiredField");
        $("#HeureRemiseVigMinutes").addClass("requiredField");
        error = true;
    }
    const typeGestion = $("[name=TypeGestion]");
    if (typeGestion.val() == "M" && typeGestion.attr("disabled") == null && typeGestion.attr('albConfirm') == null) {
        const mText = typeGestion.find("option[value='M']").text();
        const text = "Si vous validez le choix de \"" + mText + "\"<p>Le type de gestion ne pourra plus être modifié.</p><p>Souhaitez-vous poursuivre la sauvegarde ?</p>";
        ShowCommonFancy("Confirm", "ContinueSave", text, 400, 150, true, true, true);
        error = true;
    }

    var dateRemiseEnvigueur = getDate($("#DateRemiseVig").val(), $("#HeureRemiseVig").val());

    if ($.trim($("#EffetGaranties").val()) !== "") {
        var dateEffet = getDate($.trim($("#EffetGaranties").val()));
        if (dateRemiseEnvigueur < dateEffet) {
            common.dialog.error("La date de remise en vigueur ne doit pas être antérieure à la date d'effet du contrat");

            $("#DateRemiseVig").addClass("requiredField");
            $("#HeureRemiseVigHours").addClass("requiredField");
            $("#HeureRemiseVigMinutes").addClass("requiredField");

            error = true;

        }
        var dateFinEffet = getDate($("#DateFinEffet").val(), $("#HeureFinEffet").val());
        if ($("#DateFinEffet").val() != "" && dateRemiseEnvigueur > dateFinEffet) {
            common.dialog.error("La date de remise en vigueur ne doit pas être postérieure à la date de fin d'effet");

            $("#DateRemiseVig").addClass("requiredField");
            $("#HeureRemiseVigHours").addClass("requiredField");
            $("#HeureRemiseVigMinutes").addClass("requiredField");

            error = true;
        }
    }

    if ($("#TypeGestion").val() == "") {
        $("#TypeGestion").addClass("requiredField");
        error = true;
    }

    return error;

}