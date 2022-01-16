$(document).ready(function () {
    FormatTextAreaDatePeriodicite();
    DescriptifFocus();
    ChangeClass();
    CheckPeriodEffet();
    CheckPeriodDuree();
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();
    MapCommonAutoCompAperiteur();
    ChangeHour();
    Suivant();
    LoadCheckPeriod();
    MapElementPage();
    if (!$("#ProchaineEcheance").hasVal()) {
        ChangePreavisResil();
    }
    checkFieldIndice($("#IndiceReference").val(), $("#EffetGaranties").val());

    if ($("#IsModifHorsAvn").hasTrueVal() && ($("#Periodicite").val() == "E" || $("#Periodicite").val() == "U")) {
        $("#FinEffet, #HeureFinEffetHours, #HeureFinEffetMinutes, #Duree, #DureeString").disable();
    }
});
var dureeStr = "";
var preavisStr = "";

//-------------------------Mapper les elements de la page
function MapElementPage() {
    toggleDescription($("#Observations"));
    $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));

    toggleDescription($("#Description"));
    $("#Description").html($("#Description").html().replace(/&lt;br&gt;/ig, "\n"));

    if (!window.isReadonly || $("#IsModifHorsAvn").hasTrueVal()) {
        ChangeNatureContrat(true);
    }
    else {
        $("#FinEffet, #HeureFinEffetHours, #HeureFinEffetMinutes, #Duree, #DureeString").disable();
        $("#btnAnnuler").html("<u>P</u>récédent").assignAccessKey("p");
    }
    $("#btnAnnuler").kclick(function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Cancel();
                break;
        }
        $("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").clear();
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
    $("#Devise").offOn("change", function () {
        AffectTitleList($(this));
    });
    $("#Antecedent").offOn("change", function () {
        AffectTitleList($(this));
        $("div[name='txtAreaLnk'][albcontext='Description']").addClass("CursorPointer");
        //if ($(this).val() != "A") {
        //    $("div[name='txtAreaLnk'][albcontext='Description']").removeClass("CursorPointer");
        //    $("#txtArea[albContext='Description']").hide();
        //    $("div[id='zoneTxtArea'][albcontext='Description").clearHtml();
        //    $("textarea[id='Description']").clearHtml();
        //}
    });
    $("#MotClef1").offOn("change", function () {
        AffectTitleList($(this));
    });
    $("#MotClef2").offOn("change", function () {
        AffectTitleList($(this));
    });
    $("#MotClef3").offOn("change", function () {
        AffectTitleList($(this));
    });
    $("#FraisApe").offOn("change", function () {
        $("#CommissionAperiteur").val($("#FraisApe").val().replace('.', ','));
    });
    //-----Fin Affecte le title aux dropDownList
    FormatDecimalNumericValue(window.isReadonly);

    $("#EcheancePrincipale").offOn("change", function () {
        if ($(this).val().length < 4) {
            $(this).clear().addClass("requiredField");
            return;
        }
        if ($(this).val().length === 4) {
            var formatDate = $(this).val().substr(0, 2) + "/" + $(this).val().substr(2, 2);
            $(this).val(formatDate);
        }
        $(this).removeClass("requiredField");
        var echDate = $(this).val() + "/2012";
        if (!isDate(echDate)) {
            $(this).clear().addClass("requiredField");
        }
        else {
            ChangePreavisResil();
        }
    });
    $("#Duree").offOn("change", function () {
        $("#FinEffet").clear();
        $("#HeureFinEffetHours").clear();
        $("#HeureFinEffetMinutes").clear();
        ChangePreavisResil();
    });
    $("#DureeString").offOn("change", function () {
        $("#FinEffet").clear();
        $("#HeureFinEffetHours").clear();
        $("#HeureFinEffetMinutes").clear();
        ChangePreavisResil();
    });

    $("#infoEcheance").kclick(function () {
        var position = $(this).position();
        var topPos = position.top;
        var leftPos = position.left;
        $("#userEcheance").show();
        $("#dataUserEcheance").css({ 'position': 'absolute', 'top': topPos + 20 + 'px', 'left': leftPos - 80 + 'px', 'z-index': 50 });
        $("#UserEcheance").clear();

        $("#btnEchCancel").kclick(function () {
            $("#userEcheance").hide();
        });

        $("#btnEchOK").kclick(function () {
            ControleEcheance();
        });
    });

    $("img[albAutoComplete=autoCompImgAperiteur]").kclick(function () {
        var code = $(this).attr("albIdInfo");
        ShowLoading();
        var codeContrat = $("#Offre_CodeOffre").val();
        var versionContrat = $("#Offre_Version").val();
        var typeContrat = $("#Offre_Type").val();
        var aperiteurNom = $("#AperiteurNom").val();
        if (aperiteurNom != undefined && aperiteurNom != "") {
            aperiteurNom = aperiteurNom.split(" - ")[1];
        }
        if (aperiteurNom == undefined) {
            aperiteurNom = "";
        }
        var partAlbingia = $("#PartAlbingia").val();
        if (partAlbingia == undefined || partAlbingia == "") {
            partAlbingia = 0;
        }
        var interloAperiteur = $("#IdInterlocuteurAperiteur").val();
        var interloAperiteurLib = $("#libInterlocuteurAperiteur").val();
        var referenceAperiteur = $("#ReferenceAperiteur").val();
        var fraisAccAperiteur = $("#FraisAccAperiteur").val();
        if (fraisAccAperiteur == undefined || fraisAccAperiteur == "") {
            fraisAccAperiteur = 0;
        }
        var commissionAperiteur = $("#FraisApe").val();
        if (commissionAperiteur == undefined || commissionAperiteur == "") {
            commissionAperiteur = 0;
        }
        else {
            commissionAperiteur = commissionAperiteur.replace(".", ",");
        }
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
                formatDatePicker();
                AffectDateFormat();
                AlbScrollTop();
                $("#divFullScreen").show();
                FillDataAperiteur();
                MapCommonAutoCompAperiteur();
                FormatDecimalNumericValue(true);
                $("#btnAnnulerEdition").kclick(function () {
                    $("#divDataEditPopIn").clearHtml();
                    $("#divFullScreen").hide();
                });
                $("#btnEnregistrerEdition").kclick(function () {
                    EnregistrerDetailsAperiteur();
                });
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
    $("#Antecedent").change();

    $("#LTA").offOn("change", function () {
        var isChecked = $(this).isChecked();
        if (isChecked) {
            $("#lnkLTA").addClass("navig");
        }
        else {
            $("#lnkLTA").removeClass("navig");
        }
    })

    $("#lnkLTA").kclick(function () {
        if ($(this).hasClass("navig"))
            OpenWindowLTA();
    });
}
//------------- Annule la form ------------------------
function Cancel() {
    Redirection("RechercheSaisie", "Index");
}

//----------------------Met le focus sur le premier controle au démarrage---------------------
function DescriptifFocus() {
    $("#Descriptif").focus();
}
//---------------------Appel de la méthode RemoveCSS------------------------
function ChangeClass() {
    $("#EffetGaranties").offOn("change", function () {
        RemoveCSS();
        if ($(this).hasVal()) {
            if ($("#HeureEffetHours").val() == "") {
                $("#HeureEffetHours").val("00");
                $("#HeureEffetMinutes").val("00");
                $("#HeureEffetHours").change();
            }
        }
        else {
            $("#HeureEffetHours").clear();
            $("#HeureEffetMinutes").clear();
            $("#HeureEffetHours").change();
        }
        ChangePreavisResil();
    });
    $("#FinEffet").offOn("change", function () {
        $("#Duree").clear();
        $("#DureeString").clear();
        RemoveCSS();
        if ($(this).hasVal()) {
            if ($("#HeureFinEffetHours").val() == "") {
                $("#HeureFinEffetHours").val("23");
                $("#HeureFinEffetMinutes").val("59");
                $("#HeureFinEffetHours").change();
            }
        }
        else {
            $("#HeureFinEffetHours").clear();
            $("#HeureFinEffetMinutes").clear();
            $("#HeureFinEffetHours").change();
        }
        ChangePreavisResil();
    });
    $("#HeureEffet").offOn("change", function () {
        RemoveCSS();
    });
    $("#HeureFinEffet").offOn("change", function () {
        RemoveCSS();
    });
}
//---------------------Supprime la classe RequiredField--------------------
function RemoveCSS() {
    $("#EffetGaranties").removeClass("requiredField");
    $("#FinEffet").removeClass("requiredField");
    $("#HeureEffetHours").removeClass("requiredField");
    $("#HeureEffetMinutes").removeClass("requiredField");
    $("#HeureFinEffetHours").removeClass("requiredField");
    $("#HeureFinEffetMinutes").removeClass("requiredField");
}
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Date fin---------------------
function CheckPeriodEffet() {
    $("#EffetCheck").offOn("change", function () {
        changeCheckBox($(this));
    });
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Durée---------------------
function CheckPeriodDuree() {
    $("#DureeCheck").offOn("change", function () {
        changeCheckBox($(this));
    });
}
//----------------------Active les controles correspondant au checkbox de période---------------------
function changeCheckBox(e) {
    var currentId = e.attr('id');
    var currentCheck = e.isChecked();

    var input1 = "";
    var input2 = "";
    var input3 = "";
    var inputReadOnly1 = "";
    var inputReadOnly2 = "";
    var inputReadOnly3 = "";
    var checkUnchecked = "";

    if (currentId == "EffetCheck") {
        input1 = $("#Duree");
        input2 = $("#DureeString");
        inputReadOnly1 = $("#FinEffet");
        inputReadOnly2 = $("#HeureFinEffetHours");
        inputReadOnly3 = $("#HeureFinEffetMinutes");
        checkUnchecked = $("#DureeCheck");
    }
    else {
        input1 = $("#FinEffet");
        input2 = $("#HeureFinEffetHours");
        input3 = $("#HeureFinEffetMinutes");
        inputReadOnly1 = $("#Duree");
        inputReadOnly2 = $("#DureeString");
        checkUnchecked = $("#EffetCheck");
    }

    if (currentCheck) {
        input1.disable(true);
        input2.disable(true);
        if (input3 != "") {
            input3.disable(true);
        }

        inputReadOnly1.removeAttr('disabled');
        inputReadOnly1.removeClass('readonly');
        inputReadOnly2.removeAttr('disabled');
        inputReadOnly2.removeClass('readonly');

        if (inputReadOnly3 != "") {
            inputReadOnly3.removeAttr('disabled');
            inputReadOnly3.removeClass('readonly');
        }

        checkUnchecked.uncheck();
    }
    else {
        inputReadOnly1.disable(true);
        inputReadOnly2.disable(true);
        if (inputReadOnly3 != "") {
            inputReadOnly3.disable(true);
        }
    }
};
//----------------------Appel de la fonction ChangeAccesPeriodicite---------------------
$("#Periodicite").offOn("change", function () {
    if ($("#AnciennePeriodicite").val() == "E" && $("#ExistEcheancier").val() == "True") {
        SupprimerEcheances();
    }
    else {
        ChangeAccesPeriodicite(true);
        AffectTitleList($(this));
        ChangePreavisResil();
    }
});
//---------------Suppression de toutes les écheances du contrat--------
function SupprimerEcheances() {
    ShowCommonFancy("Confirm", "Suppr",
         "Attention, il existe un échéancier, cette action va le supprimer définitivement. Voulez-vous continuer ?",
         350, 130, true, true);
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        ReactivateShortCut();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                var codeOffre = $("#Offre_CodeOffre").val();
                var version = $("#Offre_Version").val();
                var type = $("#Offre_Type").val();
                var tabGuid = $("#tabGuid").val();
                var codeAvn = $("#NumAvenantPage").val();
                $.ajax({
                    type: "POST",
                    url: "/AnInformationsGenerales/SupprimerEcheances",
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
                $("#hiddenInputId").clear();
                break;
        }
    });
    $("#btnConfirmCancel").kclick(function () {
        $("#Periodicite").val($("#AnciennePeriodicite").val());
        CloseCommonFancy();
        ReactivateShortCut();
    });
}
//----------------------Active ou désactive le controle EcheancePrincipale et Préavid de résil.---------------------
function ChangeAccesPeriodicite(isDefault) { //isDefault sert a recharger, ou non, la valeur par default de la périodicité AMO 28/02/17
    var periodicites = $("#Periodicite").val();
    var ctrlEcheancePrincipale = $("#EcheancePrincipale");
    var ctrlPreavis = $("#Preavis");
    $("#EcheancePrincipale").removeClass("requiredField");
    $("#Preavis").removeClass("requiredField");

    if ($("#IsModifHorsAvn").val() !== "True") {
        if (periodicites == "U" || periodicites == "E") {// || periodicites == "R") {
            ctrlEcheancePrincipale.attr('disabled', 'disabled');
            ctrlEcheancePrincipale.addClass('readonly');
            ctrlEcheancePrincipale.clear();
            $("#ProchaineEcheance").clear();
            $("#PeriodeDebut").clear();
            $("#PeriodeFin").clear();
            $("#infoEcheance").hide();
        }
        else {
            ctrlEcheancePrincipale.removeAttr('disabled');
            ctrlEcheancePrincipale.removeClass('readonly');
            $("#infoEcheance").show();
        }
    }
    if (periodicites == "U" || periodicites == "E") { //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable || periodicites == "R") {
        ctrlPreavis.attr('disabled', 'disabled');
        ctrlPreavis.addClass('readonly');
        ctrlPreavis.clear();
    }
    else {
        ctrlPreavis.removeAttr('disabled');
        ctrlPreavis.removeClass('readonly');
    }

    if (isDefault && (periodicites != "U" || periodicites != "E") && ($("#Preavis").val() == "0" || $("#Preavis").val() == "")) {
        var str = "Periodicite" + $("#Periodicite").val();
        var valTPCN1 = $("#" + str).attr('albDescriptif');
        $("#Preavis").val(valTPCN1);
    }
}

//----------------------Appel de la fonction ChangeNatureContrat---------------------
$("#NatureContrat").offOn("change", function () {
    ChangeNatureContrat();
    AffectTitleList($(this));
});
//----------------------Active ou désactive les différents controles---------------------
function ChangeNatureContrat(initNumeric) {
    $("#dvAperiteur").show();
    $("#dvFraisAperiteur").show();
    $("#PartAlbingia").removeClass('requiredField');
    $("#AperiteurNom").removeClass('requiredField');
    if ($("#NatureContrat").hasVal()) {
        $("#PartAlbingia").removeAttr('readonly');
        $("#PartAlbingia").removeClass('readonly');
        if ($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D") {
            $("#AperiteurNom").removeAttr('readonly');
            $("#AperiteurNom").removeClass('readonly');
            $("#FraisApe").removeAttr('readonly');
            $("#FraisApe").removeClass('readonly');
        }
        else {
            $("#AperiteurNom").attr('readonly', 'readonly');
            $("#AperiteurNom").addClass('readonly');
            $("#AperiteurNom").clear();
            $("#FraisApe").attr('readonly', 'readonly');
            $("#FraisApe").addClass('readonly');
            $("#FraisApe").val('0');
            $("img[albAutoComplete=autoCompImgAperiteur]").attr("albIdInfo", "");
            $("#AperiteurCode").val("0");
            $("#PartAperiteur").val("0");
            $("#IdInterlocuteurAperiteur").val("0");
            $("#libInterlocuteurAperiteur").clear();
            $("#ReferenceAperiteur").clear();
            $("#FraisAccAperiteur").val("0");
            $("#CommissionAperiteur").val("0");
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
        $("#AperiteurNom").clear();
        $("#FraisApe").attr('readonly', 'readonly');
        $("#FraisApe").addClass('readonly');
        $("#FraisApe").val('0');
        $("#Couverture").attr('readonly', 'readonly');
        $("#Couverture").addClass('readonly');
        $("#Couverture").val('100');
        $("#dvAperiteur").hide();
        $("#dvFraisAperiteur").hide();
    }
    DestroyAutoNumeric();
    FormatDecimalNumericValue(initNumeric);
}
//----------------------Validation du formulaire---------------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        evt.preventDefault();

        // enlève les class de contour des input
        $(".requiredField").removeClass("requiredField");

        var erreurBool = false;

        if (!window.isReadonly) {

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

            if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && (!$("#EffetCheck").isChecked() && !$("#DureeCheck").isChecked())) {
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
            if ($("#Periodicite").val() != "U" && $("#Periodicite").val() != "E" && $("#ProchaineEcheance").val() == "" && !$("#EffetCheck").isChecked() && !$("#DureeCheck").isChecked()) {
                $("#ProchaineEcheance").addClass("requiredField");
                erreurBool = true;
            }

            if ($("#EcheancePrincipale").hasVal()) {
                //Ajout de 2012 à la date pour avoir une année bisextile
                //if (Date.parse($("#EcheancePrincipale").val() + "/2012") == null) {
                if (!isDate($("#EcheancePrincipale").val() + "/2012")) {
                    $("#EcheancePrincipale").addClass('requiredField');
                    erreurBool = true;
                }
            }
            if ($("#EffetCheck").isChecked() && $("#EffetGaranties").hasVal()) {
                var checkDH = checkDateHeure($("#EffetGaranties"), $("#FinEffet"), $("#HeureEffetHours"), $("#HeureFinEffetHours"), $("#HeureEffetMinutes"), $("#HeureFinEffetMinutes"));
                if (!checkDH) {
                    erreurBool = true;
                }
            }

            if ($("#EffetCheck").isChecked() && ($("#EffetGaranties").val() == "" || $("#FinEffet").val() == "")) {
                $("#EffetGaranties").addClass('requiredField');
                $("#FinEffet").addClass('requiredField');
                $("#HeureEffetHours").addClass('requiredField');
                $("#HeureFinEffetHours").addClass('requiredField');
                $("#HeureEffetMinutes").addClass('requiredField');
                $("#HeureFinEffetMinutes").addClass('requiredField');
                erreurBool = true;
            }

            if ($("#DureeCheck").isChecked() && ($("#Duree").val() == "" || $("#Duree").val() == "0" || $("#DureeString").val() == "")) {
                $("#Duree").addClass('requiredField');
                $("#DureeString").addClass('requiredField');
                if ($("#EffetGaranties").val() == "") {
                    $("#EffetGaranties").addClass('requiredField');
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
            else {
                var dateValide = isDate($("#DateAccord").val());
                if (!dateValide) {
                    $("#DateAccord").addClass('requiredField');
                    ShowCommonFancy("Error", "", "Erreur de formatage de date.<br/>Formats acceptés : DD/MM/YYYY ; DDMMYYYY ; DD/MM/YY ; DDMMYY", 300, 150, true, true, true);
                    erreurBool = true;
                }

            }
            if ($("#PartAlbingia").val() == "") {
                $("#PartAlbingia").addClass('requiredField');
                erreurBool = true;
            }
            if ($("#NatureContrat").hasVal() && $("#PartAlbingia").val() == "100,00") {
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
            //SLA 25.03.2015 : desactivation de ce controle, voir bug 1340
            //if ($("#EffetGaranties").hasVal() && $("#DateAccord").hasVal()) {
            //    if (!checkDate($("#DateAccord"), $("#EffetGaranties"))) {
            //        $("#DateAccord").addClass('requiredField');
            //        $("#EffetGaranties").addClass('requiredField');
            //        erreurBool = true;
            //    }
            //}
            if ($("#Antecedent").val() == "A" && $.trim($("#Description").val()) == "") {
                $("div[id=zoneTxtArea][albcontext=Description]").addClass('requiredField');
                erreurBool = true;
            }
        }
        if (erreurBool) {
            return false;
        }
        SubmitForm(data && data.returnHome);
    });
}
//----------------------Envoi de la form pour l'enregistrement---------------------
function SubmitForm(returnHome) {
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
    let obj = $('body :input').not("#div_popups :input").serializeObject();
    obj.txtSaveCancel = returnHome ? "1" : "0";
    let formDataInitial = JSON.stringify(obj);
    let formData = formDataInitial.replace("Contrat.CodeContrat", "CodeContrat").replace("Contrat.VersionContrat", "VersionContrat").replace("Contrat.Type", "Type").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue");
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnInformationsGenerales/AnInformationsGeneralesEnregistrer",
        data: formData,
        contentType: "application/json",
        success: function () {
            $("#btnSuivant").removeAttr("disabled");
        },
        error: function (request) {
            common.error.showXhr(request);
            $("#btnSuivant").removeAttr("disabled");
        }
    });

    if (isModifHorsAvn) {
        $('input:not([albhorsavn])').disable();
        $('select:not([albhorsavn])').disable();
    }

    $("#RegimeTaxe").disable();
    $("#SoumisCatNat").disable();
};
//----------------------Coche le type de période au load de la page---------------------
function LoadCheckPeriod() {
    if ($("#Duree").hasVal() || $("#DureeString").hasVal()) {
        checkPeriod("Duree");
    }
    else {
        if ($("#FinEffet").hasVal() || $("#HeureFinEffet").hasVal()) {
            checkPeriod("Effet");
        }
    }
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Date fin---------------------
function CheckPeriodEffet() {
    $("#EffetCheck").offOn("change", function () {
        //$("#EffetCheck").removeClass("requiredField");
        //$("#DureeCheck").removeClass("requiredField");
        $("#EffetCheckLabel").removeClass('requiredField');
        $("#DureeCheckLabel").removeClass('requiredField');
        changeCheckBox($(this));
    });
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Durée---------------------
function CheckPeriodDuree() {
    $("#DureeCheck").offOn("change", function () {
        //$("#EffetCheck").removeClass("requiredField");
        //$("#DureeCheck").removeClass("requiredField");
        $("#EffetCheckLabel").removeClass('requiredField');
        $("#DureeCheckLabel").removeClass('requiredField');
        changeCheckBox($(this));
    });
}
//----------------------Renseigne les champs cachés correspondant au checkbox de période---------------------
function checkPeriod(e) {
    var checkbox = e + "Check";
    var input = "input" + e;
    $("#" + checkbox).attr("checked", true);
    $("#" + input).val("True");
    changeCheckBox($("#" + checkbox), true);
}
//----------------------Active les controles correspondant au checkbox de période---------------------
function changeCheckBox(e, init) {
    var currentId = e.attr('id');
    var currentCheck = e.isChecked();
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
        inputReadOnly1 = $("#FinEffet");
        inputReadOnly2 = $("#HeureFinEffetHours");
        inputReadOnly3 = $("#HeureFinEffetMinutes");
        checkUnchecked = $("#DureeCheck");
        div1 = $("#divDataFinEffet");
        div2 = $("#divDataDuree");
    }
    else {
        input1 = $("#FinEffet");
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
        //input1.clear();
        input2.attr('disabled', 'disabled');
        input2.addClass('readonly');
        //input2.clear();

        if (input3 != "") {
            input3.attr('disabled', 'disabled');
            input3.addClass('readonly');
            //input3.clear();
        }

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
        //inputReadOnly1.clear();
        inputReadOnly2.attr('disabled', 'disabled');
        inputReadOnly2.addClass('readonly');
        //inputReadOnly2.clear();
        if (inputReadOnly3 != "") {
            inputReadOnly3.attr('disabled', 'disabled');
            inputReadOnly3.addClass('readonly');
            //inputReadOnly3.clear();
        }
        if (inputReadOnly1.val() != '' && inputReadOnly2.val() != '') {
            if (input1.attr('disabled') == 'disabled' && currentCheck === false) {
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
        div1.hide();
    }
    if (init != true || (init == true && $("#ProchaineEcheance").val() == ""))
        ChangePreavisResil();
};
//---------------------Affecte les fonctions sur les controles heures-----------------------
function ChangeHour() {
    $("#HeureEffetHours").offOn("change", function () {
        AffecteHour($(this));
        if ($(this).hasVal() && $("#HeureEffetMinutes").val() == "")
            $("#HeureEffetMinutes").val("00");
    });
    $("#HeureEffetMinutes").offOn("change", function () {
        AffecteHour($(this));
        if ($(this).hasVal() && $("#HeureEffetHours").val() == "")
            $("#HeureEffetHours").val("00");
    });
    $("#HeureFinEffetHours").offOn("change", function () {
        $("#Duree").clear();
        $("#DureeString").clear();
        AffecteHour($(this));
        if ($(this).hasVal() && $("#HeureFinEffetMinutes").val() == "")
            $("#HeureFinEffetMinutes").val("00");
    });
    $("#HeureFinEffetMinutes").offOn("change", function () {
        $("#Duree").clear();
        $("#DureeString").clear();
        AffecteHour($(this));
        if ($(this).hasVal() && $("#HeureFinEffetHours").val() == "")
            $("#HeureFinEffetHours").val("00");
    });
}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "")

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").clear();
        $("#" + elemId + "Minutes").clear();
    }
}
//----------------------Appel de la fonction checkFieldIndice sur le changment de l'indice---------------------
$("#IndiceReference").offOn("change", function () {
    checkFieldIndice($(this).val(), $("#EffetGaranties").val());
    AffectTitleList($(this));
});
//----------------------Met à jour le controle de l'indice---------------------
function checkFieldIndice(currentIndice, currentDateEffet) {
    //todo indice
    //if (currentIndice != "") {
    if (currentDateEffet != "" && currentIndice != "") {
        loadIndiceValeur(currentIndice, currentDateEffet);
    }
    else {
        $("#Valeur").val("0");
    }
}
//----------------------Récupère la valeur de l'indice suivant la date d'effet en base---------------------
function loadIndiceValeur(currentIndice, currentDateEffet) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnInformationsGenerales/GetValeurIndiceByCode",
        data: { indiceString: currentIndice, dateEffet: currentDateEffet },
        success: function (json) {
            updateValeurIndice(json);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
};
//----------------------Met à jour le controle de l'indice---------------------
function updateValeurIndice(item) {
    //$("#Valeur").val(item.Indice.replace(",", "."));
    $("#Valeur").val(item.Indice);
};
//-----------------Redirection----------------------
function Redirection(cible, job) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AnInformationsGenerales/Redirection/",
        data: { cible: cible, job: job },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue(initNumeric) {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    //FormatPourcentDecimal();
    //FormatPourcentNumerique();
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'pourcentnumeric', '', null, null, '100', '0');
    //FormatNumerique('numeric', ' ', '9999999999999', '0');
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'numeric', ' ', null, 0, '9999999999999', '0');
    common.autonumeric.applyAll(initNumeric ? 'init' : 'update', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
    //FormatNumerique('numericDuree', ' ', '99', '0');
    common.autonumeric.apply($("#Duree"), initNumeric ? 'init' : 'update', 'numeric', ' ', null, null, '99', '0');
    //FormatNumerique('fraisAcc', ' ', '99999', '0');
    common.autonumeric.apply($("#inFraisAcc"), initNumeric ? 'init' : 'update', 'numeric', ' ', null, null, '99999', '0');
}
//-------Supprimer l'autoNumeric sur les input/span des valeurs----------
function DestroyAutoNumeric() {
    //$("input[albMask=pourcentdecimal]").autoNumeric('destroy');
    //$("input[albMask=pourcentnumeric]").autoNumeric('destroy');
}
function FormatTextAreaDatePeriodicite() {
    if (!window.isReadonly) {
        $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));
        $("#Description").html($("#Description").html().replace(/&lt;br&gt;/ig, "\n"));
        //FormatCLEditor($("#Observations"), 902, 80);
        formatDatePicker();
        ChangeAccesPeriodicite(false);
    }
}

//-----------Charge le préavis de résiliation--------
function ChangePreavisResil() {
    var fieldsTrue = chekValideDate();
    if (fieldsTrue) {
        $("#ProchaineEcheance").clear();
        var periodicite = $("#Periodicite").val();
        var echeancePrincipale = $("#EcheancePrincipale").val();
        var duree = $("#Duree").val();
        var dateEffet = $("#EffetGaranties").val();
        var dateFinEffet = $("#FinEffet").val();
        var dureeUnite = $("#DureeString").val();
        var splitHtmlChar = $("#SplitHtmlChar").val();
        var acteGestion = $("#ActeGestion").val();
        var modeNavig = $("#ModeNavig").val();
        var tabGuid = $("#tabGuid").val();
        var codeAvn = $("#NumAvenantPage").val();

        /* controle des dates */

        //if (periodicite != "" && echeancePrincipale != "") {// && dateEffet != "" && (dateFinEffet != "" || (duree != "" && dureeUnite != ""))) {
        if (periodicite != "" && echeancePrincipale != "" && dateEffet != "") {// ECM 2015-02-27 remise de la date d'effet dans le test sinon le pgm 400 plante

            if (!$("#EffetCheck").isChecked()) {
                dateFinEffet = "";
            }

            if (dateFinEffet == "" && $("#DureeCheck").isChecked()) {
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
                url: "/AnInformationsGenerales/ChangePreavisResil",
                data: {
                    codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), dateEffet: dateEffet, dateFinEffet: dateFinEffet,
                    dateAvenant: dateEffet, periodicite: periodicite, echeancePrincipale: echeancePrincipale, splitHtmlChar: splitHtmlChar, acteGestion: acteGestion,
                    modeNavig: modeNavig, tabGuid: tabGuid, codeAvn: codeAvn
                },
                success: function (data) {
                    if (data != "") {
                        if (data == "IMP") {
                            ShowCommonFancy("Error", "", "Calcul de la prochaine échéance impossible, voir la compta pour annuler la quittance.", 300, 80, true, true, true, false);
                        }
                        else {
                            var tData = AlbJsSplitArray(data, splitHtmlChar);
                            if (tData != "noData") {
                                $("#ProchaineEcheance").val(tData[2]);
                                $("#PeriodeDeb").val(tData[0]);
                                $("#PeriodeFin").val(tData[1]);
                            }
                            else {
                                $("#ProchaineEcheance").clear();
                                $("#PeriodeDeb").clear();
                                $("#PeriodeFin").clear();
                            }
                        }
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
            $("#ProchaineEcheance").clear();
            $("#PeriodeDeb").clear();
            $("#PeriodeFin").clear();
        }
    }
    else {
        ShowCommonFancy("Error", "", "Erreur de formatage de date.<br/>Formats acceptés : DD/MM/YYYY ; DDMMYYYY ; DD/MM/YY ; DDMMYY", 300, 150, true, true, true);
    }
}
function chekValideDate() {

    var trueBool = true;
    if ($("#EffetGaranties").hasVal()) {
        var dateDebValide = isDate($("#EffetGaranties").val());
        if (!dateDebValide) {
            $("#EffetGaranties").addClass('requiredField');
            trueBool = false;
        }
    }
    if ($("#FinEffet").hasVal() && $("#EffetCheck").isChecked()) {
        var dateFinValide = isDate($("#FinEffet").val());
        if (!dateFinValide) {
            $("#FinEffet").addClass('requiredField');
            trueBool = false;
        }
    }

    return trueBool;
}
//------------Controle l'échéance renseignée par l'utilisateur----------
function ControleEcheance() {
    var dateValide = isDate($("#UserEcheance").val());
    var dateDebValide = isDate($("#EffetGaranties").val());
    var dateFinValide = isDate($("#FinEffet").val());
    var dateDeb = $("#EffetGaranties").val();
    var dateFin = "";

    var modeNavig = $("#ModeNavig").val();
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();

    //2017-02-22 Modif suivant Spec Périodicité "R" Régularisable
    if (dateValide && dateDebValide) {
        if (getDate($("#UserEcheance").val()) < getDate(dateDeb)) {
            common.dialog.error("La date de prochaine échéance doit être supérieure ou égale à la date d'effet de garanties");
            return false;
        }

        if ($("#EffetCheck").isChecked())
            dateFin = $("#FinEffet").val();

        if ($("#DureeCheck").isChecked()) {
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
            if (dateDebValide && dateFinValide) {
                if (getDate(dateFin) < getDate($("#UserEcheance").val())) {
                    common.dialog.error("La date de prochaine échéance doit être inférieure ou égale à la date d'effet de garanties");
                    return false;
                }
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
                url: "/AnInformationsGenerales/ControleEcheance",
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
    else
        common.dialog.error("La date de prochaine échéance n'est pas une date valide");

}

function FillDataAperiteur() {
    if ($("#inCode").hasVal()) {
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
    let codeAperiteur = $("#inCode").val();
    let nomAperiteur = $("#inLibelle").val();
    let partAperiteur = $("#inPourcentPart").autoNumeric('get');
    let interloAperiteur = $("#inInterlocuteurCode").val();
    let interloLabel = $("#inInterlocuteur").val();
    let referenceAperiteur = $("#inReference").val();
    let fraisAccAperiteur = $("#inFraisAcc").autoNumeric('get');
    let commissionAperiteur = $("#inCommissionApe").autoNumeric('get');
    let partAlbingia = $("#PartAlbingia").autoNumeric('get');

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

    $("#PartAperiteur").val(partAperiteur.replace('.', ','));
    $("#IdInterlocuteurAperiteur").val(interloAperiteur);
    $("#libInterlocuteurAperiteur").val(interloLabel);
    $("#ReferenceAperiteur").val(referenceAperiteur);
    $("#FraisAccAperiteur").val(fraisAccAperiteur.replace('.', ','));
    $("#CommissionAperiteur").val(commissionAperiteur.replace('.', ','));

    $("#FraisApe").val(commissionAperiteur.replace('.', ','));
    $("#divDataEditPopIn").clearHtml();
    $("#divFullScreen").hide();

}
