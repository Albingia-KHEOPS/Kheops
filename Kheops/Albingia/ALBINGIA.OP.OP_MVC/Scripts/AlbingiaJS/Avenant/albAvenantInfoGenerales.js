/// <reference path="../Common/common.js" />
/// <reference path="../albCommon.js" />
$(document).ready(function () {
    MapElementsPage();

    if ($("#IsModifHorsAvn").val() == "True" && ($("#Periodicite").val() == "E" || $("#Periodicite").val() == "U")) {
        $("#FinEffet").attr("disabled", "disabled");
        $("#HeureFinEffetHours").attr("disabled", "disabled");
        $("#HeureFinEffetMinutes").attr("disabled", "disabled");
        $("#Duree").attr("disabled", "disabled");
        $("#DureeString").attr("disabled", "disabled");
    }
});
//-------------Map les éléments de la page-------------
function MapElementsPage() {
    toggleDescription($("#Observations"));
    $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));

    LoadCheckPeriod();

    //2017-03-14 : modification contrôles date V5
    //AMO 17/05/17 Pour traiter le bug 2400, il a été décidé de supprimer ce controle
    //DisableDureeFinEffet();
    if (!window.isReadonly) {
        ChangeNatureContrat(true);
    }
    else {
        FormatDecimalNumericValue(true);


        if ($('#IsModifHorsAvn').val() !== 'True') {
            $("#DateFinEffet").attr("disabled", "disabled");
            $("#HeureFinEffetHours").attr("disabled", "disabled");
            $("#HeureFinEffetMinutes").attr("disabled", "disabled");
            $("#Duree").attr("disabled", "disabled");
            $("#DureeString").attr("disabled", "disabled");
        }
        //$("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();
    MapCommonAutoCompAperiteur();

    $("#EffetCheck").die().live('change', function () {
        ChangeCheckBox($(this));
    });

    $("#DureeCheck").die().live('change', function () {
        ChangeCheckBox($(this));
    });

    $("#infoEcheance").die().live('click', function () {
        var position = $(this).position();
        var topPos = position.top;
        var leftPos = position.left;
        $("#userEcheance").show();
        $("#dataUserEcheance").css({ 'position': 'absolute', 'top': topPos + 20 + 'px', 'left': leftPos - 80 + 'px', 'z-index': 50 });
        $("#UserEcheance").val("");

        $("#btnEchCancel").die().live('click', function () {
            $("#userEcheance").hide();
        });

        $("#btnEchOK").die().live('click', function () {
            ControleEcheance();
        });
    });

    $("#Periodicite").die().live('change', function () {
        if ($("#AnciennePeriodicite").val() == "E" && $("#ExistEcheancier").val() == "True") {
            SupprimerEcheances();
        }
        else {
            ChangePreavisResil();
            ChangeAccesPeriodicite(true);
            AffectTitleList($(this));
        }
        $("#EffetCheck").removeAttr("disabled").removeClass("readonly");
        $("#DureeCheck").removeAttr("disabled").removeClass("readonly");
        $("#DateFinEffet").removeAttr("disabled").removeClass("readonly");
        $("#HeureFinEffetHours").removeAttr("disabled").removeClass("readonly");
        $("#HeureFinEffetMinutes").removeAttr("disabled").removeClass("readonly");
        $("#Duree").removeAttr("readonly").removeClass("readonly");
        $("#DureeString").removeAttr("disabled").removeClass("readonly");
    });

    $("#EcheancePrincipale").die().live('change', function () {
        if ($(this).val().length < 4) {
            $(this).val("").addClass("requiredField");
            return;
        }
        if ($(this).val().length === 4) {
            var formatDate = $(this).val().substr(0, 2) + "/" + $(this).val().substr(2, 2);
            $(this).val(formatDate);
        }
        $(this).removeClass("requiredField");
        var echDate = $(this).val() + "/2012";
        if (!isDate(echDate)) {
            $(this).val("").addClass("requiredField");
        }
        else {
            ChangePreavisResil();
        }
    });

    $("#Duree").die().live('change', function () {
        $("#DateFinEffet").val('');
        $("#HeureFinEffetHours").val('');
        $("#HeureFinEffetMinutes").val('');
        ChangePreavisResil();
    });

    $("#DureeString").die().live('change', function () {
        $("#DateFinEffet").val('');
        $("#HeureFinEffetHours").val('');
        $("#HeureFinEffetMinutes").val('');
        ChangePreavisResil();
    });

    $("#DateDebEffet").die().live("change", function () {
        RemoveCSS();
        if ($(this).val() != "") {
            if ($("#HeureEffetHours").val() == "") {
                $("#HeureEffetHours").val("00");
                $("#HeureEffetMinutes").val("00");
                $("#HeureEffetHours").trigger("change");
            }
        }
        else {
            $("#HeureEffetHours").val("");
            $("#HeureEffetMinutes").val("");
            $("#HeureEffetHours").trigger("change");
        }
        ChangePreavisResil();
    });

    $("#DateFinEffet").die().live("change", function () {
        $("#Duree").val('');
        $("#DureeString").val('');
        RemoveCSS();
        if ($(this).val() != "") {
            if ($("#HeureFinEffetHours").val() == "") {
                $("#HeureFinEffetHours").val("23");
                $("#HeureFinEffetMinutes").val("59");
                $("#HeureFinEffetHours").trigger("change");
            }
        }
        else {
            $("#HeureFinEffetHours").val("");
            $("#HeureFinEffetMinutes").val("");
            $("#HeureFinEffetHours").trigger("change");
        }
        ChangePreavisResil();
    });

    $("#HeureEffet").die().live("change", function () {
        RemoveCSS();
    });

    $("#HeureFinEffet").die().live("change", function () {
        RemoveCSS();
    });

    $("#HeureDebEffetAvtHours").die().live('change', function () {
        AffecteHour($(this));
        if ($(this).val() != "" && $("#HeureDebEffetAvtMinutes").val() == "")
            $("#HeureDebEffetAvtMinutes").val("00");
    });

    $("#HeureDebEffetAvtMinutes").die().live('change', function () {
        AffecteHour($(this));
        if ($(this).val() != "" && $("#HeureDebEffetAvtHours").val() == "")
            $("#HeureDebEffetAvtHours").val("00");
    });

    $("#HeureFinEffetHours").die().live('change', function () {
        $("#Duree").val('');
        $("#DureeString").val('');
        AffecteHour($(this));
        if ($(this).val() != "" && $("#HeureFinEffetMinutes").val() == "")
            $("#HeureFinEffetMinutes").val("00");
    });

    $("#HeureFinEffetMinutes").die().live('change', function () {
        $("#Duree").val('');
        $("#DureeString").val('');
        AffecteHour($(this));
        if ($(this).val() != "" && $("#HeureFinEffetHours").val() == "")
            $("#HeureFinEffetHours").val("00");
    });

    $("#IndiceReference").die().live('change', function () {
        CheckFieldIndice($(this).val(), $("#DateDebEffet").val());
        AffectTitleList($(this));
    });

    $("#NatureContrat").die().live('change', function () {
        ChangeNatureContrat();
        AffectTitleList($(this));
    });

    if (!window.isReadonly || $("#IsModifHorsAvn").val() === "True") {
        $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));
        ChangeAccesPeriodicite(false);
    }

    CheckFieldIndice($("#IndiceReference").val(), $("#DateDebEffet").val());

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        var valhiddenAction = $("#hiddenAction").val().split("_")[0];
        var txtSaveCanceltmp = $("#hiddenAction").val().split("_")[1];
        switch (valhiddenAction) {
            case "Suppr":
                var codeOffre = $("#Offre_CodeOffre").val();
                var version = $("#Offre_Version").val();
                var type = $("#Offre_Type").val();
                var tabGuid = $("#tabGuid").val();
                var codeAvn = $("#NumAvenantPage").val();
                $.ajax({
                    type: "POST",
                    url: "/AvenantInfoGenerales/SupprimerEcheances",
                    data: {
                        codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid
                    },
                    success: function (data) {
                        $("#AnciennePeriodicite").val($("#Periodicite").val());

                        ChangeAccesPeriodicite(true);
                        AffectTitleList($(this));
                        ChangePreavisResil();
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
                $("#hiddenInputId").val('');
                break;
            case "Echeance":
                SubmitForm(txtSaveCanceltmp == 1);
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                $("#Periodicite").val($("#AnciennePeriodicite").val());
                CloseCommonFancy();
                ReactivateShortCut();
                break;
        }
        $("#hiddenAction").val('');
    });

    //-----Affecte le title aux dropDownList
    //Initialisation
    AffectTitleList($("#Devise"));
    AffectTitleList($("#Periodicite"));
    AffectTitleList($("#IndiceReference"));
    AffectTitleList($("#NatureContrat"));
    AffectTitleList($("#MotClef1"));
    AffectTitleList($("#MotClef2"));
    AffectTitleList($("#MotClef3"));
    AffectTitleList($("#RegimeTaxe"));
    AffectTitleList($("#Antecedent"));
    AffectTitleList($("#Stop"));
    //Mise à jour sur changement de valeur
    $("#Devise").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#Antecedent").die().live('change', function () {
        AffectTitleList($(this));
        $("div[name='txtAreaLnk'][albcontext='ObservationAntecedents']").addClass("CursorPointer");
        if ($(this).val() != "A") {
            $("div[name='txtAreaLnk'][albcontext='ObservationAntecedents']").removeClass("CursorPointer");
            $("#txtArea[albContext='ObservationAntecedents']").hide();
            $("div[id='zoneTxtArea'][albcontext='ObservationAntecedents").html("");
            $("textarea[id='ObservationAntecedents']").html("");
        }
    });
    $("#MotClef1").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotClef2").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotClef3").die().live('change', function () {
        AffectTitleList($(this));
    });
    //-----Fin Affecte le title aux dropDownList

    $("#Descriptif").focus();
    $("#btnSuivant").kclick(function (evt, data) {
        Suivant(evt, data && data.returnHome);
    });

    $("img[albAutoComplete=autoCompImgAperiteur]").kclick(function () {
        var code = $(this).attr("albIdInfo");
        //if (code != undefined && code != "") {
        ShowLoading();
        var codeContrat = $("#Offre_CodeOffre").val();
        var versionContrat = $("#Offre_Version").val();
        var typeContrat = $("#Offre_Type").val();
        var aperiteurNom = $("#AperiteurNom").val();
        if (aperiteurNom != undefined && aperiteurNom != "")
            aperiteurNom = aperiteurNom.split(" - ")[1];
        if (aperiteurNom == undefined)
            aperiteurNom = "";
        var partAlbingia = $("#PartAlbingia").val();
        if (partAlbingia == undefined || partAlbingia == "")
            partAlbingia = 0;
        var interloAperiteur = $("#IdInterlocuteurAperiteur").val();
        var interloAperiteurLib = $("#libInterlocuteurAperiteur").val();
        var referenceAperiteur = $("#ReferenceAperiteur").val();
        var fraisAccAperiteur = $("#FraisAccAperiteur").val();

        if (fraisAccAperiteur == undefined || fraisAccAperiteur == "")
            fraisAccAperiteur = 0;
        var commissionAperiteur = $("#FraisApe").val();
        if (commissionAperiteur == undefined || commissionAperiteur == "")
            commissionAperiteur = 0;
        else
            commissionAperiteur = commissionAperiteur.replace(".", ",");
        $.ajax({
            type: "POST",
            url: "/AnInformationsGenerales/GetAperiteurDetail",
            data: {
                guidId: code, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat,
                modeNavig: $("#ModeNavig").val(),
                nomAperiteur: aperiteurNom, partAlbingia: partAlbingia,
                interloAperiteur: interloAperiteur, interloAperiteurLib: interloAperiteurLib, referenceAperiteur: referenceAperiteur,
                fraisAccAperiteur: fraisAccAperiteur, commissionAperiteur: commissionAperiteur,
                offreReadOnly: $("#OffreReadOnly").val(), isModifHorsAvn: $("#IsModifHorsAvn").val()
            },
            success: function (data) {
                DesactivateShortCut();
                $("#divDataEditPopIn").html(data);
                //FormatDecimalNumericValue();
                formatDatePicker();
                AffectDateFormat();
                AlbScrollTop();
                $("#divFullScreen").show();
                FillDataAperiteur();
                MapCommonAutoCompAperiteur();
                FormatDecimalNumericValue(true);
                $("#btnAnnulerEdition").die().live("click", function () {
                    $("#divDataEditPopIn").html('');
                    $("#divFullScreen").hide();
                });
                $("#btnEnregistrerEdition").die().live("click", function () {
                    EnregistrerDetailsAperiteur();
                });
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

    $("#Antecedent").trigger('change');

    if ($("#ProchaineEcheance").val() == "") {
        ChangePreavisResil();
    }

    $("#LTA").live('change', function () {
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            $("#lnkLTA").addClass("navig");
        }
        else {
            $("#lnkLTA").removeClass("navig");
        }
    })

    $("#lnkLTA").live("click", function () {
        if ($(this).hasClass("navig"))
            OpenWindowLTA();
    });
}
//--------------Grise les champs durée et fin d'effet selon paramètres
function DisableDureeFinEffet() {
    if ($("#EtatHisto").val() == "V" && $("#SituationHisto").val() == "X" && $("#Periodicite").val() != "U" && $("#Periodicite").val() != "E") {
        $("#EffetCheck").attr("disabled", "disabled").addClass("readonly");
        $("#DureeCheck").attr("disabled", "disabled").addClass("readonly");
        $("#DateFinEffet").attr("disabled", "disabled").addClass("readonly");
        $("#HeureFinEffetHours").attr("disabled", "disabled").addClass("readonly");
        $("#HeureFinEffetMinutes").attr("disabled", "disabled").addClass("readonly");
        $("#Duree").attr("readonly", "readonly").addClass("readonly");
        $("#DureeString").attr("disabled", "disabled").addClass("readonly");
    }
}
//----------------------Coche le type de période au load de la page---------------------
function LoadCheckPeriod() {
    if ($("#Duree").val() != "" || $("#DureeString").val() != "") {
        CheckPeriod("Duree");
    }
    else {
        if (($("#FinEffet").attr("id") != undefined && $("#FinEffet").val() != "") || $("#HeureFinEffet").val() != "") {
            CheckPeriod("Effet");
        }
    }
}
//----------------------Renseigne les champs cachés correspondant au checkbox de période---------------------
function CheckPeriod(e) {
    var checkbox = e + "Check";
    var input = "input" + e;
    $("#" + checkbox).attr("checked", true);
    $("#" + input).val("True");
    ChangeCheckBox($("#" + checkbox), true);
}
//----------------------Active les controles correspondant au checkbox de période---------------------
function ChangeCheckBox(e, init) {
    var currentId = e.attr('id');
    var currentCheck = e.is(':checked');
    var element = $("#" + currentId);

    var input1 = "";
    var input2 = "";
    var input3 = "";
    var inputReadOnly1 = "";
    var inputReadOnly2 = "";
    var inputReadOnly3 = "";
    var checkUnchecked = "";
    var div1 = "";
    var div2 = "";

    if (currentId == "EffetCheck") {
        input1 = $("#Duree");
        input2 = $("#DureeString");
        inputReadOnly1 = $("#DateFinEffet");
        inputReadOnly2 = $("#HeureFinEffetHours");
        inputReadOnly3 = $("#HeureFinEffetMinutes");
        checkUnchecked = $("#DureeCheck");
        div1 = $("#divDataFinEffet");
        div2 = $("#divDataDuree");
    }
    else {
        input1 = $("#DateFinEffet");
        input2 = $("#HeureFinEffetHours");
        input3 = $("#HeureFinEffetMinutes");
        inputReadOnly1 = $("#Duree");
        inputReadOnly2 = $("#DureeString");
        checkUnchecked = $("#EffetCheck");
        div1 = $("#divDataDuree");
        div2 = $("#divDataFinEffet");
    }

    if (currentCheck) {
        input1.attr('disabled', 'disabled');
        input1.addClass('readonly');
        //input1.val('');
        input2.attr('disabled', 'disabled');
        input2.addClass('readonly');
        //input2.val('');
        if (input3 != "") {
            input3.attr('disabled', 'disabled');
            input3.addClass('readonly');
            //input3.val('');
        }

        //inputReadOnly1.removeAttr('disabled');
        //inputReadOnly1.removeClass('readonly');
        //inputReadOnly2.removeAttr('disabled');
        //inputReadOnly2.removeClass('readonly');

        //if (inputReadOnly3 != "") {
        //    inputReadOnly3.removeAttr('disabled');
        //    inputReadOnly3.removeClass('readonly');
        //}

        if ($('#IsModifHorsAvn').val() == 'False' || element.data('can-check')) {
            inputReadOnly1.removeAttr('disabled');
            inputReadOnly1.removeClass('readonly');
            inputReadOnly2.removeAttr('disabled');
            inputReadOnly2.removeClass('readonly');
        }

        if (inputReadOnly3 != "") {
            if ($('#IsModifHorsAvn').val() == 'False' || element.data('can-check')) {
                inputReadOnly3.removeAttr('disabled');
                inputReadOnly3.removeClass('readonly');
            }
            else {
                if (inputReadOnly1.val() == '' && inputReadOnly2.val() == '') {
                    if (currentCheck === true) {
                        inputReadOnly1.removeAttr('disabled');
                        inputReadOnly1.removeClass('readonly');
                        inputReadOnly2.removeAttr('disabled');
                        inputReadOnly2.removeClass('readonly');
                        if (inputReadOnly3) {
                            inputReadOnly3.removeAttr('disabled');
                            inputReadOnly3.removeClass('readonly');
                        }
                    }
                }
            }
        }

        checkUnchecked.removeAttr('checked');
        div1.show();
        div2.hide();
    }
    else {
        inputReadOnly1.attr('disabled', 'disabled');
        inputReadOnly1.addClass('readonly');
        //inputReadOnly1.val('');
        inputReadOnly2.attr('disabled', 'disabled');
        inputReadOnly2.addClass('readonly');
        //inputReadOnly2.val('');
        if (inputReadOnly3 != "") {
            inputReadOnly3.attr('disabled', 'disabled');
            inputReadOnly3.addClass('readonly');
            //inputReadOnly3.val('');
        }

        div1.hide();
    }

    if (init != true || (init == true && $("#ProchaineEcheance").val() == ""))
        ChangePreavisResil();
};
//---------------------Supprime la classe RequiredField--------------------
function RemoveCSS() {
    $("#DateDebEffet").removeClass("requiredField");
    $("#DateFinEffet").removeClass("requiredField");
    $("#HeureEffetHours").removeClass("requiredField");
    $("#HeureEffetMinutes").removeClass("requiredField");
    $("#HeureFinEffetHours").removeClass("requiredField");
    $("#HeureFinEffetMinutes").removeClass("requiredField");
}
//-----------Charge le préavis de résiliation--------
function ChangePreavisResil() {
    var periodicite = $("#Periodicite").val();
    var echeancePrincipale = $("#EcheancePrincipale").val();
    var dateEffet = $("#DateDebEffet").val();
    var dateFinEffet = $("#DateFinEffet").val();
    var duree = $("#Duree").val();
    var dureeUnite = $("#DureeString").val();
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var acteGestion = $("#ActeGestion").val();
    var modeNavig = $("#ModeNavig").val();
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();

    //if (periodicite != "" && echeancePrincipale != "") {// && dateEffet != "" && (dateFinEffet != "" || (duree != "" && dureeUnite != ""))) {
    if (periodicite != "" && echeancePrincipale != "" && dateEffet != "") {// ECM 2015-02-27 remise de la date d'effet dans le test sinon le pgm 400 plante
        if (!$("#EffetCheck").is(":checked")) {
            dateFinEffet = "";
        }

        if (dateFinEffet == "" && $("#DureeCheck").is(":checked")) {
            switch (dureeUnite) {
                case "A":
                    dateFinEffet = incrementDate(dateEffet, 0, 0, duree, 0, true);
                    break;
                case "M":
                    dateFinEffet = incrementDate(dateEffet, 0, duree, 0, 0, true);
                    break;
                case "J":
                    dateFinEffet = incrementDate(dateEffet, duree, 0, 0, 0, true);
                    break;
            }
        }

        //if (dateFinEffet != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AvenantInfoGenerales/ChangePreavisResil",
            data: {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), dateEffet: dateEffet, dateFinEffet: dateFinEffet,
                dateAvenant: dateEffet, periodicite: periodicite, echeancePrincipale: echeancePrincipale, splitHtmlChar: splitHtmlChar, acteGestion: acteGestion,
                modeNavig: modeNavig, tabGuid: tabGuid, codeAvn: codeAvn
            },
            success: function (data) {
                if (data != "") {
                    var tData = AlbJsSplitArray(data, splitHtmlChar);
                    if (tData != "noData") {
                        $("#ProchaineEcheance").val(tData[2]);
                        $("#PeriodeDeb").val(tData[0]);
                        $("#PeriodeFin").val(tData[1]);

                        //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
                        if ($("#ProchaineEcheance").val() != $("#ProchEchHisto").val()) {
                            $("#infoEcheance").show();
                        }
                        else {
                            $("#infoEcheance").hide();
                        }
                    }
                    else {
                        $("#ProchaineEcheance").val("");
                        $("#PeriodeDeb").val("");
                        $("#PeriodeFin").val("");
                    }
                }

                if (($("#ProchaineEcheance").val() != $("#ProchEchHisto").val() || $("#Periodicite").val() != $("#PeriodiciteHisto").val() || ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D")) && ($("#Periodicite").val() != "U" && $("#Periodicite").val() != "E")) {
                    $("#infoEcheance").show();
                }
                else {
                    $("#infoEcheance").hide();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
        //}
    }
    else {
        $("#ProchaineEcheance").val("");
        $("#PeriodeDeb").val("");
        $("#PeriodeFin").val("");
    }
}
//------------Controle l'échéance renseignée par l'utilisateur----------
function ControleEcheance() {
    var dateValide = isDate($("#UserEcheance").val());
    var dateDeb = $("#DateDebEffet").val();
    var dateFin = "";

    var modeNavig = $("#ModeNavig").val();
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();

    //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable
    if (dateValide) {
        if (getDate($("#UserEcheance").val()) < getDate(dateDeb)) {
            common.dialog.error("La date de prochaine échéance doit être supérieure ou égale à la date d'effet de garanties");
            return false;
        }

        if ($("#EffetCheck").is(":checked"))
            dateFin = $("#DateFinEffet").val();


        if ($("#DureeCheck").is(":checked")) {
            switch ($("#DureeString").val()) {
                case "A":
                    dateFin = incrementDate(dateDeb, 0, 0, $("#Duree").val(), 0, true);
                    break;
                case "M":
                    dateFin = incrementDate(dateDeb, 0, $("#Duree").val(), 0, 0, true);
                    break;
                case "J":
                    dateFin = incrementDate(dateDeb, $("#Duree").val(), 0, 0, 0, true);
                    break;
            }
        }

        if (dateFin != "") {
            if (getDate(dateFin) < getDate($("#UserEcheance").val())) {
                if ($("#EffetCheck").is(":checked")) {
                    $("#DateFinEffet").addClass("requiredField");
                }
                if ($("#DureeCheck").is(":checked")) {
                    $("#Duree").addClass("requiredField");
                    $("#DureeString").addClass("requiredField");
                }
                common.dialog.error("La date de prochaine échéance doit être inférieure ou égale à la date de fin d'effet du contrat");
                return false;
            }
        }

        //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable
        if ($("#Periodicite").val() == "R") {
            $("#ProchaineEcheance").val($("#UserEcheance").val());
            var newFinPeriode = incrementDate($("#UserEcheance").val(), 0, 0, 0, 0, true);
            $("#PeriodeFin").val(newFinPeriode);
            $("#userEcheance").hide();
        }
        else {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AvenantInfoGenerales/ControleEcheance",
                data: {
                    codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                    modeNavig: modeNavig, tabGuid: tabGuid, codeAvn: codeAvn,
                    dateDeb: dateDeb, dateFin: dateFin,
                    prochaineEcheance: $("#UserEcheance").val(), periodicite: $("#Periodicite").val(), echeancePrincipale: $("#EcheancePrincipale").val()
                },
                success: function (data) {
                    if (data != "") {
                        common.dialog.error(data); CloseLoading();
                    }
                    else {
                        $("#ProchaineEcheance").val($("#UserEcheance").val());
                        var newFinPeriode = incrementDate($("#UserEcheance").val(), 0, 0, 0, 0, true);
                        $("#PeriodeFin").val(newFinPeriode);
                        $("#userEcheance").hide();
                    }
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    }
    else {
        common.dialog.error("La date de prochaine échéance n'est pas une date valide");
    }
}
//---------------Suppression de toutes les écheances du contrat--------
function SupprimerEcheances() {
    ShowCommonFancy("Confirm", "Suppr",
         "Attention, il existe un échéancier, cette action va le supprimer définitivement. Voulez-vous continuer ?",
         350, 130, true, true);
}
//----------------------Active ou désactive le controle EcheancePrincipale et Préavid de résil.---------------------
function ChangeAccesPeriodicite(isDefault) { //isDefault sert a recharger, ou non, la valeur par default de la périodicité AMO 28/02/17
    var periodicites = $("#Periodicite").val();
    var ctrlEcheancePrincipale = $("#EcheancePrincipale");
    var ctrlPreavis = $("#Preavis");
    $("#EcheancePrincipale").removeClass("requiredField");
    $("#Preavis").removeClass("requiredField");

    $("#infoEcheance").hide();
    if ($("#IsModifHorsAvn").val() !== "True") {
        if (periodicites == "U" || periodicites == "E") {// || periodicites == "R") {
            ctrlEcheancePrincipale.attr('disabled', 'disabled');
            ctrlEcheancePrincipale.addClass('readonly');
            ctrlEcheancePrincipale.val('');
            $("#ProchaineEcheance").val("");
            $("#infoEcheance").hide();
        }
        else {
            ctrlEcheancePrincipale.removeAttr('disabled');
            ctrlEcheancePrincipale.removeClass('readonly');

            //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
            if ($("#NatureContrat").val() == "C" && $("#NatureContrat").val() == "D") {
                $("#infoEcheance").show();
            }
            else {
                $("#infoEcheance").hide();
            }
        }
    }

    if (periodicites == "U" || periodicites == "E") { //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable || periodicites == "R") {
        ctrlPreavis.attr('disabled', 'disabled');
        ctrlPreavis.addClass('readonly');
        ctrlPreavis.val('');
    }
    else {
        ctrlPreavis.removeAttr('disabled');
        ctrlPreavis.removeClass('readonly');

        // START BUG 1452 : Task 2616
        if (($("#ProchaineEcheance").val() != $("#ProchEchHisto").val() || $("#Periodicite").val() != $("#PeriodiciteHisto").val() || ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D"))) {
            $("#infoEcheance").show();
        }
        else {
            $("#infoEcheance").hide();
        }
        // END BUG 1452 : Task 2616
    }

    // START BUG 1452 : Task 2616
    //if (($("#ProchaineEcheance").val() != $("#ProchEchHisto").val() || $("#Periodicite").val() != $("#PeriodiciteHisto").val() || ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D"))) {
    //    $("#infoEcheance").show();
    //}
    //else {
    //    $("#infoEcheance").hide();
    //}
    // END BUG 1452 : Task 2616

    if (isDefault && (periodicites != "U" || periodicites != "E") && ($("#Preavis").val() == "0" || $("#Preavis").val() == "")) {
        var str = "Periodicite" + $("#Periodicite").val();
        var valTPCN1 = $("#" + str).attr('albDescriptif');
        $("#Preavis").val(valTPCN1);
    }
}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "")

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }
}
//----------------------Met à jour le controle de l'indice---------------------
function CheckFieldIndice(currentIndice, currentDateEffet) {
    if (currentDateEffet != "" && currentIndice != "") {
        LoadIndiceValeur(currentIndice, currentDateEffet);
    }
    else {
        $("#Valeur").val("0");
    }
}
//----------------------Récupère la valeur de l'indice suivant la date d'effet en base---------------------
function LoadIndiceValeur(currentIndice, currentDateEffet) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AvenantInfoGenerales/GetValeurIndiceByCode",
        data: { indiceString: currentIndice, dateEffet: currentDateEffet },
        success: function (json) {
            UpdateValeurIndice(json);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
};
//----------------------Met à jour le controle de l'indice---------------------
function UpdateValeurIndice(item) {
    $("#Valeur").val(item.Indice);
};
//----------------------Active ou désactive les différents controles---------------------
function ChangeNatureContrat(initNumeric) {
    $("#dvAperiteur").show();
    $("#dvFraisAperiteur").show();
    $("#PartAlbingia").removeClass('requiredField');
    $("#AperiteurNom").removeClass('requiredField');
    if ($("#NatureContrat").val() != "") {
        $("#PartAlbingia").removeAttr('readonly');
        $("#PartAlbingia").removeClass('readonly');
        if ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D") {
            $("#AperiteurNom").removeAttr('readonly');
            $("#AperiteurNom").removeClass('readonly');
            $("#FraisApe").removeAttr('readonly');
            $("#FraisApe").removeClass('readonly');

            //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
            $("#infoEcheance").show();
        }
        else {
            $("#AperiteurNom").attr('readonly', 'readonly');
            $("#AperiteurNom").addClass('readonly');
            $("#AperiteurNom").val('');
            $("#FraisApe").attr('readonly', 'readonly');
            $("#FraisApe").addClass('readonly');
            $("#FraisApe").val('0');
            $("img[albAutoComplete=autoCompImgAperiteur]").attr("albIdInfo", "");
            $("#AperiteurCode").val("0");
            $("#PartAperiteur").val("0");
            $("#IdInterlocuteurAperiteur").val("0");
            $("#libInterlocuteurAperiteur").val("");
            $("#ReferenceAperiteur").val("");
            $("#FraisAccAperiteur").val("0");
            $("#CommissionAperiteur").val("0");

            //BUG 1452:
            //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
            //if (($("#ProchaineEcheance").val() != $("#ProchEchHisto").val() || $("#Periodicite").val() != $("#PeriodiciteHisto").val() || ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D"))) {
            //    $("#infoEcheance").hide();
            //}

            if ($("#ProchaineEcheance").val() === $("#ProchEchHisto").val() && $("#Periodicite").val() === $("#PeriodiciteHisto").val()) {
                $("#infoEcheance").hide();
            }

            //BUG 1452: End
        }

        if ($("#NatureContrat").val() == "A" || $("#NatureContrat").val() == "E") {
            $("#Couverture").removeAttr('readonly');
            $("#Couverture").removeClass('readonly');
            $("#dvAperiteur").hide();
            $("#dvFraisAperiteur").hide();
        }
        else {
            $("#Couverture").attr('readonly', 'readonly');
            $("#Couverture").addClass('readonly');
            $("#Couverture").val('100');
        }
    }
    else {
        $("#PartAlbingia").attr('readonly', 'readonly');
        $("#PartAlbingia").addClass('readonly');
        $("#PartAlbingia").val('100');
        $("#Aperiteur").attr('disabled', 'disabled');
        $("#AperiteurNom").addClass('readonly');
        $("#AperiteurNom").val('');
        $("#FraisApe").attr('readonly', 'readonly');
        $("#FraisApe").addClass('readonly');
        $("#FraisApe").val('0');
        $("#Couverture").attr('readonly', 'readonly');
        $("#Couverture").addClass('readonly');
        $("#Couverture").val('100');
        $("#dvAperiteur").hide();
        $("#dvFraisAperiteur").hide();

        //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
        if ($("#ProchaineEcheance").val() == $("#ProchEchHisto").val()) {
            $("#infoEcheance").hide();
        }
    }
    DestroyAutoNumeric();
    FormatDecimalNumericValue(initNumeric);
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue(initNumeric) {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    //FormatPourcentDecimal();
    //FormatPourcentNumerique();
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'pourcentnumeric', '', null, null, '100', '0');
    //FormatNumerique('numeric', ' ', '9999999999999', '0');
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'numeric', ' ', null, null, '9999999999999', '0');
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
    // FormatNumerique('numericDuree', ' ', '99', '0');
    common.autonumeric.apply($("#Duree"), initNumeric ? 'init' : 'update', 'numeric', ' ', null, null, '99', '0');
    //FormatNumerique('fraisAcc', ' ', '99999', '0');
    common.autonumeric.apply($("#inFraisAcc"), initNumeric ? 'init' : 'update', 'numeric', ' ', null, null, '99999', '0');
}
//-------Supprimer l'autoNumeric sur les input/span des valeurs----------
function DestroyAutoNumeric() {
    //$("input[albMask=pourcentdecimal]").autoNumeric('destroy');
    //$("input[albMask=pourcentnumeric]").autoNumeric('destroy');
}
//----------------------Validation du formulaire---------------------
function Suivant(evt, returnHome) {

    evt.preventDefault();
    //enlève les class de contour des input
    $(".requiredField").removeClass("requiredField");

    var erreurBool = false;

    if (!window.isReadonly || $("#IsModifHorsAvn").val() === "True") {

        $("#Descriptif").val($.trim($("#Descriptif").val()));
        $("#SouscripteurNom").val($.trim($("#SouscripteurNom").val()));
        $("#GestionnaireNom").val($.trim($("#GestionnaireNom").val()));

        if ($("#Identification").val() == "") {
            $("#Identification").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#Devise").val() == "") {
            $("#Devise").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#Periodicite").val() == "") {
            $("#Periodicite").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#SouscripteurNom").val() == "") {
            $("#SouscripteurNom").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#GestionnaireNom").val() == "") {
            $("#GestionnaireNom").addClass('requiredField');
            erreurBool = true;
        }
        if (($("#Periodicite").val() != "U" && $("#Periodicite").val() != "E" && $("#Periodicite").val() != "R") && $("#EcheancePrincipale").val() == "") {
            $("#EcheancePrincipale").addClass('requiredField');
            erreurBool = true;
        }

        if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && (!$("#EffetCheck").is(":checked") && !$("#DureeCheck").is(":checked"))) {
            //$("#EffetCheck").addClass('requiredField');
            //$("#DureeCheck").addClass('requiredField');

            $("#EffetCheckLabel").addClass('requiredField');
            $("#DureeCheckLabel").addClass('requiredField');

            erreurBool = true;
        }

        if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && $("#EffetGaranties").val() == "") {
            $("#Periodicite").addClass("requiredField");
            $("#EffetGaranties").addClass("requiredField");
            erreurBool = true;
        }

        //ECM 2015-09-10 : ajout du test de la prochaine échéance
        //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable
        if ($("#Periodicite").val() != "U" && $("#Periodicite").val() != "E" && $("#ProchaineEcheance").val() == "" && !$("#EffetCheck").is(":checked") && !$("#DureeCheck").is(":checked")) {
            $("#ProchaineEcheance").addClass("requiredField");
            erreurBool = true;
        }

        if ($("#EcheancePrincipale").val() != "") {
            //Ajout de 2012 à la date pour avoir une année bisextile
            if (!isDate($("#EcheancePrincipale").val() + "/2012")) {
                $("#EcheancePrincipale").addClass('requiredField');
                erreurBool = true;
            }
        }
        if ($("#EffetCheck").is(':checked') && $("#DateDebEffet").val() != "") {
            var checkDH = checkDateHeure($("#DateDebEffet"), $("#DateFinEffet"), $("#HeureEffetHours"), $("#HeureFinEffetHours"), $("#HeureEffetMinutes"), $("#HeureFinEffetMinutes"));
            if (!checkDH) {
                erreurBool = true;
            }
        }

        if ($("#EffetCheck").is(':checked')) {
            if ($("#DateDebEffet").val() == "" || $("#DateFinEffet").val() == "") {
                $("#DateDebEffet").addClass('requiredField');
                $("#DateFinEffet").addClass('requiredField');
                $("#HeureEffetHours").addClass('requiredField');
                $("#HeureFinEffetHours").addClass('requiredField');
                $("#HeureEffetMinutes").addClass('requiredField');
                $("#HeureFinEffetMinutes").addClass('requiredField');
                erreurBool = true;
            }
        }
        else {
            $("#HeureFinEffet").val('');
            $("#HeureFinEffetHours").val('');
            $("#HeureFinEffetMinutes").val('');
        }

        if ($("#DureeCheck").is(':checked') && ($("#Duree").val() == "" || $("#Duree").val() == "0" || $("#DureeString").val() == "")) {
            $("#Duree").addClass('requiredField');
            $("#DureeString").addClass('requiredField');
            if ($("#DateDebEffet").val() == "") {
                $("#DateDebEffet").addClass('requiredField');
                $("#HeureEffetHours").addClass('requiredField');
                $("#HeureEffetMinutes").addClass('requiredField');
            }
            erreurBool = true;
        }
        if ($("#Preavis").is(':disabled') == false && (($("#Preavis").val() == "") || $("#Preavis").val() == "0")) {
            $("#Preavis").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#DateAccord").val() == "") {
            $("#DateAccord").addClass('requiredField');
            erreurBool = true;
        }
        if ($("#PartAlbingia").val() == "") {
            $("#PartAlbingia").addClass('requiredField');
            erreurBool = true;
        }
        if ($("#NatureContrat").val() != "" && $("#PartAlbingia").val() == "100,00") {
            common.dialog.bigError("La part Albingia doit être inférieure à 100", true);
            $("#PartAlbingia").addClass('requiredField');
            erreurBool = true;
        }
        if (($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D") && $("#AperiteurNom").val() == "" && $("#AperiteurNom").is(':disabled') == false) {
            $("#AperiteurNom").addClass('requiredField');
            erreurBool = true;
        }

        if (($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D") && $("#PartAperiteur").val() == "0" && $("#AperiteurNom").is(':disabled') == false) {
            common.dialog.bigError("La part de l'apériteur doit être complétée", true);
            $("#imgInfoAperiteur").addClass('requiredField');
            erreurBool = true;
        }
        //SLA (25.03.2015) : desactivation voir bug 1340
        //if ($("#DateDebEffet").val() != "" && $("#DateAccord").val() != "") {
        //    if (!checkDate($("#DateAccord"), $("#DateDebEffet"))) {
        //        $("#DateAccord").addClass('requiredField');
        //        $("#DateDebEffet").addClass('requiredField');
        //        erreurBool = true;
        //    }
        //}
        if ($("#Antecedent").val() == "A" && $.trim($("#ObservationAntecedents").val()) == "") {
            $("div[id=zoneTxtArea][albcontext=ObservationAntecedents]").addClass('requiredField');
            erreurBool = true;
        }

        if ($("#ActeGestion").val() == "AVNMD") {
            if ($("#ProchaineEcheance").val() != "") {
                if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#ProchaineEcheance").val())) {
                    ShowCommonFancy("Error", "", "Incohérence entre la prochaine échéance et la date d'avenant, " + $("#Echeance").val(), 300, 80, true, true, true);
                    $("#DateDebEffetAvt").addClass("requiredField");
                    $("#HeureDebEffetAvtHours").addClass("requiredField");
                    $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                    $("#ProchaineEcheance").addClass("requiredField");
                    erreurBool = true;
                }

                //20160215 : Ajout suivant le doc Dates avenant_tech_v_1.docx
                //if (getDate($("#ProchaineEcheance").val()) < getDate($("#DebPeriodHisto").val())) {
                //    common.dialog.error("La date de prochaine échéance doit être supérieure au début de période précédente en cours.");
                //    $("#ProchaineEcheance").addClass("requiredField");
                //    erreurBool = true;
                //}
            }
            //var checkDH = checkDateHeure($("#DateDebEffet"), $("#DateFinEffet"), $("#HeureEffetHours"), $("#HeureFinEffetHours"), $("#HeureEffetMinutes"), $("#HeureFinEffetMinutes"));
            if (!checkDateHeure($("#DateDebEffet"), $("#DateDebEffetAvt"), $("#HeureEffetHours"), $("#HeureDebEffetAvtHours"), $("#HeureEffetMinutes"), $("#HeureDebEffetAvtMinutes"))) {
                common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                erreurBool = true;
            }

            //2017-03-14 : Ajout du contrôles Spec Contrôles Dates V5
            if ($("#Periodicite").val() != "U" && $("#Periodicite").val() != "E") {
                //#region Contrôles si Périodicité != "U" & "E"
                if ($("#EffetCheck").is(":checked") && $("#DateFinEffet").val() != "") {
                    if ($("#HeureFinEffet").val() == "23:59:00" || $("#HeureFinEffet").val() == "") {
                        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate($("#DateFinEffet").val(), 1, 0, 0, 0, false))) {
                            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                            $("#DateDebEffetAvt").addClass("requiredField");
                            $("#HeureDebEffetAvtHours").addClass("requiredField");
                            $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                            $("#DateFinEffet").addClass("requiredField");
                            $("#HeureFinEffetHours").addClass("requiredField");
                            $("#HeureFinEffetMinutes").addClass("requiredField");
                            erreurBool = true;
                        }
                    }
                    else {
                        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#DateFinEffet").val(), $("#HeureFinEffet").val())) {
                            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                            $("#DateDebEffetAvt").addClass("requiredField");
                            $("#HeureDebEffetAvtHours").addClass("requiredField");
                            $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                            $("#DateFinEffet").addClass("requiredField");
                            $("#HeureFinEffetHours").addClass("requiredField");
                            $("#HeureFinEffetMinutes").addClass("requiredField");
                            erreurBool = true;
                        }
                    }
                }
                if ($("#DureeCheck").is(":checked") && $("#Duree").val() != "") {
                    var dateFinEffet = "";
                    switch ($("#DureeString").val()) {
                        case "A":
                            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, 0, $("#Duree").val(), 0, true);
                            break;
                        case "M":
                            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, $("#Duree").val(), 0, 0, true);
                            break;
                        case "J":
                            dateFinEffet = incrementDate($("#DateDebEffet").val(), $("#Duree").val(), 0, 0, 0, true);
                            break;
                    }
                    if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate(dateFinEffet, 1, 0, 0, 0, false))) {
                        common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                        $("#DateDebEffetAvt").addClass("requiredField");
                        $("#HeureDebEffetAvtHours").addClass("requiredField");
                        $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                        $("#Duree").addClass("requiredField");
                        $("#DureeString").addClass("requiredField");
                        erreurBool = true;
                    }
                }
                //#endregion
            }
            else {
                //ECM 2017-06-13 : mise en commentaire du contrôle si Périodicité = "U" ou "E"
                //#region Contrôles si Périodicité = "U" ou "E"
                //if ($("#EffetCheck").is(":checked") && $("#DateFinEffet").val() != "") {
                //    if ($("#HeureFinEffet").val() == "") {
                //        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#DateFinEffet").val(), "00:00:00")) {
                //            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                //            $("#DateDebEffetAvt").addClass("requiredField");
                //            $("#HeureDebEffetAvtHours").addClass("requiredField");
                //            $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                //            $("#DateFinEffet").addClass("requiredField");
                //            $("#HeureFinEffetHours").addClass("requiredField");
                //            $("#HeureFinEffetMinutes").addClass("requiredField");
                //            erreurBool = true;
                //        }
                //    }
                //    else {
                //        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#DateFinEffet").val(), $("#HeureFinEffet").val())) {
                //            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                //            $("#DateDebEffetAvt").addClass("requiredField");
                //            $("#HeureDebEffetAvtHours").addClass("requiredField");
                //            $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                //            $("#DateFinEffet").addClass("requiredField");
                //            $("#HeureFinEffetHours").addClass("requiredField");
                //            $("#HeureFinEffetMinutes").addClass("requiredField");
                //            erreurBool = true;
                //        }
                //    }
                //}
                //if ($("#DureeCheck").is(":checked")) {
                //    var dateFinEffet = "";
                //    switch ($("#DureeString").val()) {
                //        case "A":
                //            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, 0, $("#Duree").val(), 0, true);
                //            break;
                //        case "M":
                //            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, $("#Duree").val(), 0, 0, true);
                //            break;
                //        case "J":
                //            dateFinEffet = incrementDate($("#DateDebEffet").val(), $("#Duree").val(), 0, 0, 0, true);
                //            break;
                //    }
                //    if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate(dateFinEffet, 1, 0, 0, 0, false))) {
                //        common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                //        $("#DateDebEffetAvt").addClass("requiredField");
                //        $("#HeureDebEffetAvtHours").addClass("requiredField");
                //        $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                //        $("#Duree").addClass("requiredField");
                //        $("#DureeString").addClass("requiredField");
                //        erreurBool = true;
                //    }
                //}
                //#endregion
            }


            //#region 2017-03-17 Mise en commentaires du contrôles de fin d'effet
            //if ($("#DateFinEffet").val() != "") {
            //    if ($("#HeureFinEffet").val() == "23:59:00" || $("#HeureFinEffet").val() == "") {
            //        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate($("#DateFinEffet").val(), 1, 0, 0, 0, false))) {
            //            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
            //            $("#DateDebEffetAvt").addClass("requiredField");
            //            $("#HeureDebEffetAvt").addClass("requiredField");
            //            $("#DateFinEffet").addClass("requiredField");
            //            $("#HeureFinEffet").addClass("requiredField");
            //            erreurBool = true;
            //        }
            //    }
            //    else {
            //        if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#DateFinEffet").val(), $("#HeureFinEffet").val())) {
            //            common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
            //            $("#DateDebEffetAvt").addClass("requiredField");
            //            $("#HeureDebEffetAvt").addClass("requiredField");
            //            $("#DateFinEffet").addClass("requiredField");
            //            $("#HeureFinEffet").addClass("requiredField");
            //            erreurBool = true;
            //        }
            //    }
            //}

            //if ($("#Duree").val() != "") {
            //    var dateFinEffet = "";
            //    switch ($("#DureeString").val()) {
            //        case "A":
            //            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, 0, $("#Duree").val(), 0, true);
            //            break;
            //        case "M":
            //            dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, $("#Duree").val(), 0, 0, true);
            //            break;
            //        case "J":
            //            dateFinEffet = incrementDate($("#DateDebEffet").val(), $("#Duree").val(), 0, 0, 0, true);
            //            break;
            //    }
            //    if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate(dateFinEffet, 1, 0, 0, 0, false))) {
            //        common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
            //        $("#DateDebEffetAvt").addClass("requiredField");
            //        $("#HeureDebEffetAvt").addClass("requiredField");
            //        $("#Duree").addClass("requiredField");
            //        $("#DureeString").addClass("requiredField");
            //        erreurBool = true;
            //    }

            //}
            //#endregion
        }
        if ($("#ActeGestion").val() == "AVNRM") {
            var DateResil = $("#DateResil").val();
            // ARA - 1154

            if ($("#EffetCheck").is(":checked") && $("#DateFinEffet").val() != "") {
                if ($("#HeureFinEffet").val() == "23:59:00" || $("#HeureFinEffet").val() == "") {
                    if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate($("#DateFinEffet").val(), 1, 0, 0, 0, false))) {
                        common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                        $("#DateDebEffetAvt").addClass("requiredField");
                        $("#HeureDebEffetAvtHours").addClass("requiredField");
                        $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                        $("#DateFinEffet").addClass("requiredField");
                        $("#HeureFinEffetHours").addClass("requiredField");
                        $("#HeureFinEffetMinutes").addClass("requiredField");
                        erreurBool = true;
                    }
                }
                else {
                    if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate($("#DateFinEffet").val(), $("#HeureFinEffet").val())) {
                        common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                        $("#DateDebEffetAvt").addClass("requiredField");
                        $("#HeureDebEffetAvtHours").addClass("requiredField");
                        $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                        $("#DateFinEffet").addClass("requiredField");
                        $("#HeureFinEffetHours").addClass("requiredField");
                        $("#HeureFinEffetMinutes").addClass("requiredField");
                        erreurBool = true;
                    }
                }
            }
            if ($("#DureeCheck").is(":checked") && $("#Duree").val() != "") {
                var dateFinEffet = "";
                switch ($("#DureeString").val()) {
                    case "A":
                        dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, 0, $("#Duree").val(), 0, true);
                        break;
                    case "M":
                        dateFinEffet = incrementDate($("#DateDebEffet").val(), 0, $("#Duree").val(), 0, 0, true);
                        break;
                    case "J":
                        dateFinEffet = incrementDate($("#DateDebEffet").val(), $("#Duree").val(), 0, 0, 0, true);
                        break;
                }
                if (getDate($("#DateDebEffetAvt").val(), $("#HeureDebEffetAvt").val()) > getDate(incrementDate(dateFinEffet, 1, 0, 0, 0, false))) {
                    common.dialog.error("Date d'avenant incohérente avec la période du contrat.");
                    $("#DateDebEffetAvt").addClass("requiredField");
                    $("#HeureDebEffetAvtHours").addClass("requiredField");
                    $("#HeureDebEffetAvtMinutes").addClass("requiredField");
                    $("#Duree").addClass("requiredField");
                    $("#DureeString").addClass("requiredField");
                    erreurBool = true;
                }
            }

        }
    }
    if (erreurBool) {
        return false;
    }
    
    if ($("#ProchEchHisto").val() != "" && $("#ProchEchHisto").val() != $("#ProchaineEcheance").val() && (!window.isReadonly)) {
        var strMsg = "<u><b>Ancienne échéance</b></u> : " + $("#ProchEchHisto").val();
        strMsg += "<br/><u><b>Nouvelle échéance</b></u> : " + $("#ProchaineEcheance").val();
        ShowCommonFancy("Confirm", "Echeance" + "_" + (returnHome ? 1 : 0), strMsg + "<br/><br/>Voulez-vous continuer ?", 1000, 80, true, true, true);
    }
    else {
        SubmitForm(returnHome);
    }
}
//----------------------Envoi de la form pour l'enregistrement---------------------
function SubmitForm(returnHome) {
    $("#DateDebEffetAvt").removeAttr("disabled");
    $("#HeureDebEffetAvtHours").removeAttr("disabled");
    $("#HeureDebEffetAvtMinutes").removeAttr("disabled");
    $("#tabGuid").removeAttr("disabled");
    $("#Offre_CodeOffre").removeAttr("disabled");
    $("#Offre_Version").removeAttr("disabled");
    $("#Offre_Type").removeAttr("disabled");
    $("#txtSaveCancel").removeAttr("disabled");
    $("#txtParamRedirect").removeAttr("disabled");
    $("#ModeNavig").removeAttr("disabled");
    $("#RegimeTaxe").removeAttr("disabled");
    $("#SoumisCatNat").removeAttr("disabled");

    $("#AddParamType").removeAttr("disabled");
    $("#AddParamValue").removeAttr("disabled");

    $("#PartAperiteur").removeAttr("disabled");
    $("#IdInterlocuteurAperiteur").removeAttr("disabled");
    $("#ReferenceAperiteur").removeAttr("disabled");
    $("#FraisAccAperiteur").removeAttr("disabled");
    $("#CommissionAperiteur").removeAttr("disabled");

    var isModifHorsAvn = $('#IsModifHorsAvn').val() === 'True' ? true : false;

    if (isModifHorsAvn) {
        $('input:not([albhorsavn])').removeAttr("disabled");
        $('select:not([albhorsavn])').removeAttr("disabled");
    }

    let obj = $("body :input").not("#div_popups :input").serializeObject();
    obj.txtSaveCancel = returnHome ? 1 : 0;
    var formDataInitial = JSON.stringify(obj);
    var formData = formDataInitial.replace("Contrat.CodeContrat", "CodeContrat").replace("Contrat.VersionContrat", "VersionContrat").replace("Contrat.Type", "Type").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue");
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AvenantInfoGenerales/Enregistrer",
        data: formData,
        contentType: "application/json",
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

    if (isModifHorsAvn) {
        $('input:not([albhorsavn])').attr("disabled", "disabled");
        $('select:not([albhorsavn])').attr("disabled", "disabled");
    }

    $("#RegimeTaxe").attr("disabled", "disabled");
    $("#SoumisCatNat").attr("disabled", "disabled");
    $("#HeureEffetHours").attr("disabled", "disabled");
    $("#HeureEffetMinutes").attr("disabled", "disabled");
    $("#HeureEffet").attr("disabled", "disabled");
    $("#HeureFinEffetHours").attr("disabled", "disabled");
    $("#HeureFinEffetMinutes").attr("disabled", "disabled");
    $("#HeureFinEffet").attr("disabled", "disabled");
};

function FillDataAperiteur() {
    if ($("#inCode").val() != "") {
        $("#inCode").val($("#AperiteurCode").val());
        $("#inLibelle").val($("#AperiteurNom").val().split(" - ")[1]);
        $("#inPourcentPart").val($("#PartAperiteur").val());
        $("#inInterlocuteurCode").val($("#IdInterlocuteurAperiteur").val());
        $("#inInterlocuteur").val($("#libInterlocuteurAperiteur").val());
        $("#inReference").val($("#ReferenceAperiteur").val());
        $("#inFraisAcc").val($("#FraisAccAperiteur").val());
        $("#inCommissionApe").val($("#FraisApe").val());
    }
}

function EnregistrerDetailsAperiteur() {
    var codeAperiteur = $("#inCode").val();
    var nomAperiteur = $("#inLibelle").val();
    //var partAperiteur = parseFloat($("#inPourcentPart").val());
    var partAperiteur = $("#inPourcentPart").autoNumeric('get');
    var interloAperiteur = $("#inInterlocuteurCode").val();
    var interloLabel = $("#inInterlocuteur").val();
    var referenceAperiteur = $("#inReference").val();
    var fraisAccAperiteur = $("#inFraisAcc").autoNumeric('get');
    var commissionAperiteur = $("#inCommissionApe").autoNumeric('get');



    var partAlbingia = $("#PartAlbingia").autoNumeric('get');

    if ((partAperiteur == "") || (parseFloat(partAperiteur.replace('.', ',')) <= 0)) {
        $("#inPourcentPart").addClass("requiredField");
        return false;
    }

    if (parseFloat(partAperiteur.replace('.', ',')) + parseFloat(partAlbingia.replace('.', ',')) > 100) {
        $("#inPourcentPart").addClass("requiredField");
        common.dialog.bigError("La somme de la part de l'apériteur et de la part Albingia ne peut dépasser 100 %", true);
        return false;
    }

    $("#AperiteurNom").val(codeAperiteur + " - " + nomAperiteur);
    $("#AperiteurCode").val(codeAperiteur);
    $("#AperiteurSelect").val("1");
    $("img[albAutoComplete=autoCompImgAperiteur]").attr("albIdInfo", codeAperiteur);
    //$("#PartAlbingia").val(100 - partAperiteur);

    $("#PartAperiteur").val(partAperiteur.replace('.', ','));
    $("#IdInterlocuteurAperiteur").val(interloAperiteur);
    $("#libInterlocuteurAperiteur").val(interloLabel);
    $("#ReferenceAperiteur").val(referenceAperiteur);
    $("#FraisAccAperiteur").val(fraisAccAperiteur.replace('.', ','));
    $("#CommissionAperiteur").val(commissionAperiteur.replace('.', ','));

    $("#FraisApe").val(commissionAperiteur.replace('.', ','));
    $("#divDataEditPopIn").html('');
    $("#divFullScreen").hide();

}

