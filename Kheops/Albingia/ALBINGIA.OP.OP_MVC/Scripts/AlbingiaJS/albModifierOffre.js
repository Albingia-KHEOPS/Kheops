
$(document).ready(function () {
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();
    MapCommonAutoCompAperiteur();
    DescriptifFocus();
    CheckPeriodEffet();
    CheckPeriodDuree();
    Suivant();
    ChangeClass();
    ChangeHour();
    LoadCheckPeriod();
    MapElementPage();
    CheckBoxRelance();
});
var dureeStr = "";
var couvertureStr = "";
var fraisStr = "";
var partAlbingiaStr = "";
//----------------Map les éléments de la page------------------
function MapElementPage() {
    toggleDescription($("#Observations"));
    ConfigureNumericFields(true);

    if (!window.isReadonly) {     
        //Mettre une div pour simuler un textarea, pour prendre en compte le format HTML
        //FormatCLEditor($("#Observations"), 902, 80, 6);
        $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));
        //achref
        ChangeNatureContrat();
        formatDatePicker();
        ChangeAccesPeriodicite(); 
    }
    else {
        $("#FinEffet").attr("disabled", "disabled");
        $("#HeureFinEffetHours").attr("disabled", "disabled");
        $("#HeureFinEffetMinutes").attr("disabled", "disabled");
        $("#Duree").attr("disabled", "disabled");
        $("#DureeString").attr("disabled", "disabled");
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
    });
    $("#btnUpdate").live('click', function () {
        Redirection("CreationSaisie", "Index");
    });

    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Cancel();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });


    $("#NatureContrat").live('change', function () {
        AffectTitleList($(this));
        ChangeNatureContrat();
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
    AffectTitleList($("#DureeString"));
    AffectTitleList($("#FraisApe"));


    //Mise à jour sur changement de valeur
    $("#Devise").die().live('change', function () {
        AffectTitleList($(this));
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

    $("#DureeString").die().live('change', function () {
        $("#FinEffet").val('');
        $("#HeureFinEffetHours").val('');
        $("#HeureFinEffetMinutes").val('');
        AffectTitleList($(this));
    });

    $("#RegimeTaxe").die().live('change', function () {
        AffectTitleList($(this));
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
    });

    $("#LTA").live('change', function () {
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            $("#lnkLTA").addClass("navig");
        }
        else {
            $("#lnkLTA").removeClass("navig");
        }
    });

    $("#lnkLTA").live("click", function () {
        if ($(this).hasClass("navig"))
            OpenWindowLTA();
    });

}
function ConfigureNumericFields(init) {
    var mode = init ? "init" : "update";
    common.autonumeric.apply($("#Duree"), mode, 'numeric', '', null, null, '99', '0');
    common.autonumeric.applyAll(mode, 'pourcentnumeric', '', null, null, '100', '0');
    common.autonumeric.applyAll(mode, 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
}

//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    $.ajax({
        type: "POST",
        url: "/ModifierOffre/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.show(x);
        }
    });
}
//----------------------Met le focus sur le premier controle au démarrage---------------------
function DescriptifFocus() {
    $("#Descriptif").focus();
}

//------------- Annule la form ------------------------
function Cancel() {
    Redirection("RechercheSaisie", "Index");
}

//---------------------Appel de la méthode RemoveCSS------------------------
function ChangeClass() {
    $("#EffetGaranties").live("change", function () {
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
    });
    $("#FinEffet").live("change", function () {
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
    });
    $("#HeureEffet").live("change", function () {
        RemoveCSS();
    });
    $("#HeureFinEffet").live("change", function () {
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
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $("#EffetGaranties").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#FinEffet").datepicker({ dateFormat: 'dd/mm/yy' });
}
//----------------------Appel de la fonction ChangeAccesPeriodicite---------------------
$("#Periodicite").live('change', function () {
    AffectTitleList($(this));
    ChangeAccesPeriodicite();
});
//----------------------Active ou désactive le controle EcheancePrincipale---------------------
function ChangeAccesPeriodicite() {
    var periodicites = $("#Periodicite").val();
    var ctrlEcheancePrincipale = $("#EcheancePrincipale");
    var ctrlPreavis = $("#Preavis");
    $("#EcheancePrincipale").removeClass("requiredField");
    $("#Preavis").removeClass("requiredField");
    if (periodicites == "U" || periodicites == "E") {
        //--EcheancePrincipale
        ctrlEcheancePrincipale.attr('disabled', 'disabled');
        ctrlEcheancePrincipale.addClass('readonly');
        ctrlEcheancePrincipale.clear();
        //--Preavis
        ctrlPreavis.attr('disabled', 'disabled');
        ctrlPreavis.addClass('readonly');
        ctrlPreavis.clear();
    }
    else {
        //--EcheancePrincipale
        ctrlEcheancePrincipale.removeAttr('disabled');
        ctrlEcheancePrincipale.removeClass('readonly');
        //--Preavis
        ctrlPreavis.removeAttr('disabled');
        ctrlPreavis.removeClass('readonly');
    }
    if ((periodicites != "U" || periodicites != "E") && ($("#Preavis").val() == "0" || $("#Preavis").val() == "")) {
        var str = "Periodicite" + $("#Periodicite").val();
        var valTPCN1 = $("#" + str).attr('albDescriptif');
        $("#Preavis").val(valTPCN1);
    }
}
//----------------------Active ou désactive les différents controles des périodes---------------------
function ChangeNatureContrat() {
    $("#dvAperiteur").show();
    $("#dvFraisAperiteur").show();

    if ($("#NatureContrat").val() != "") {
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
            $("#AperiteurNom").val('');
            $("#AperiteurCode").val('');
            $("#AperiteurSelect").val('');
            $("#FraisApe").attr('readonly', 'readonly');
            $("#FraisApe").addClass('readonly');
            $("#FraisApe").val('0');
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
        //$("#PartAlbingia").autoNumeric('set', 100);
        $("#PartAlbingia").val('100');
        $("#Aperiteur").attr('disabled', 'disabled');
        $("#AperiteurNom").addClass('readonly');
        $("#AperiteurNom").val('');
        $("#AperiteurCode").val('');
        $("#AperiteurSelect").val('');
        $("#FraisApe").attr('readonly', 'readonly');
        $("#FraisApe").addClass('readonly');
        $("#FraisApe").val('');
        $("#Couverture").attr('readonly', 'readonly');
        $("#Couverture").addClass('readonly');
        $("#Couverture").val('100');
        $("#dvAperiteur").hide();
        $("#dvFraisAperiteur").hide();
    }
    ConfigureNumericFields(false);
}
//----------------------Coche le type de période au load de la page---------------------
function LoadCheckPeriod() {
    if ($("#Duree").val() != "" || $("#DureeString").val()) {
        checkPeriod("Duree");
    }
    else {
        if ($("#FinEffet").val() != "" || $("#HeureFinEffet").val()) {
            checkPeriod("Effet");
        }
    }
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Date fin---------------------
function CheckPeriodEffet() {
    $("#EffetCheck").live('change', function () {
        changeCheckBox($(this));
    });
}
//----------------------Appel de la fonction changeCheckBox avec pour la période Durée---------------------
function CheckPeriodDuree() {
    $("#DureeCheck").live('change', function () {
        changeCheckBox($(this));
    });
}
//----------------------Renseigne les champs cachés correspondant au checkbox de période---------------------
function checkPeriod(e) {
    var checkbox = e + "Check";
    var input = "input" + e;
    $("#" + checkbox).attr("checked", true);
    $("#" + input).val("True");
    changeCheckBox($("#" + checkbox));
}
//----------------------Active les controles correspondant au checkbox de période---------------------
function changeCheckBox(e) {
    var currentId = e.attr('id');
    var currentCheck = e.is(':checked');

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
    //$("#FinEffet").die();
    //$("#FinEffet").live('change', function () {
    //    $("#Duree").val('');
    //    $("#DureeString").val('');
    //});
    //$("#HeureFinEffetHours").die();
    //$("#HeureFinEffetHours").live('change', function () {
    //    $("#Duree").val('');
    //    $("#DureeString").val('');
    //});
    //$("#HeureFinEffetMinutes").die();
    //$("#HeureFinEffetMinutes").live('change', function () {
    //    $("#Duree").val('');
    //    $("#DureeString").val('');
    //});
    $("#Duree").die().live('change', function () {
        $("#FinEffet").val('');
        $("#HeureFinEffetHours").val('');
        $("#HeureFinEffetMinutes").val('');
    });
    //$("#DureeString").die();
    //$("#DureeString").live('change', function () {
    //    $("#FinEffet").val('');
    //    $("#HeureFinEffetHours").val('');
    //    $("#HeureFinEffetMinutes").val('');
    //});

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

        inputReadOnly1.removeAttr('disabled');
        inputReadOnly1.removeClass('readonly');
        inputReadOnly2.removeAttr('disabled');
        inputReadOnly2.removeClass('readonly');

        if (inputReadOnly3 != "") {
            inputReadOnly3.removeAttr('disabled');
            inputReadOnly3.removeClass('readonly');
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
};
//---------------------Affecte les fonctions sur les controles heures-----------------------
function ChangeHour() {
    $("#HeureEffetHours").live('change', function () {
        if ($(this).val() != "" && $("#HeureEffetMinutes").val() == "")
            $("#HeureEffetMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureEffetMinutes").live('change', function () {
        if ($(this).val() != "" && $("#HeureEffetHours").val() == "")
            $("#HeureEffetHours").val("00");
        AffecteHour($(this));
    });
    $("#HeureFinEffetHours").live('change', function () {
        $("#Duree").val('');
        $("#DureeString").val('');
        if ($(this).val() != "" && $("#HeureFinEffetMinutes").val() == "")
            $("#HeureFinEffetMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureFinEffetMinutes").live('change', function () {
        $("#Duree").val('');
        $("#DureeString").val('');
        if ($(this).val() != "" && $("#HeureFinEffetHours").val() == "")
            $("#HeureFinEffetHours").val("00");
        AffecteHour($(this));
    });
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
//----------------------Appel de la fonction checkFieldIndice sur le changment de l'indice---------------------
$("#IndiceReference").live('change', function () {
    AffectTitleList($(this));
    checkFieldIndice($(this).val(), $("#EffetGaranties").val());
});
//----------------------Appel de la fonction checkFieldIndice sur le changment de la date d'effet---------------------
$("#EffetGaranties").live('change', function () {
    checkFieldIndice($("#IndiceReference").val(), $(this).val());
});
//----------------------Met à jour le controle de l'indice---------------------
function checkFieldIndice(currentIndice, currentDateEffet) {
    if (currentDateEffet != "" && currentIndice != "") {
        //if (currentIndice != "") {
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
        url: "/ModifierOffre/GetValeurIndiceByCode",
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

//----------------------Validation du formulaire---------------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        if (!window.isReadonly) {

            $(".requiredField").removeClass("requiredField");

            var erreurBool = false;

            $("#Descriptif").val($.trim($("#Descriptif").val()));
            $("#SouscripteurNom").val($.trim($("#SouscripteurNom").val()));
            $("#GestionnaireNom").val($.trim($("#GestionnaireNom").val()));

            if ($("#Descriptif").val() == "") {
                $("#Descriptif").addClass('requiredField');
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

            if ($("#Periodicite").val() != "") {
                var periodicite = $("#Periodicite").val().split("-")[0];
                if ((periodicite == "A" || periodicite == "S" || periodicite == "T" || periodicite == "R") && ($("#Preavis").length == 0 || $("#Preavis").val() == 0)) {
                    $("#Preavis").addClass('requiredField');
                    erreurBool = true;
                }
            }

            if ($("#SouscripteurNom").val() == "") {
                $("#SouscripteurNom").addClass('requiredField');
                erreurBool = true;
            }

            if ($("#GestionnaireNom").val() == "") {
                $("#GestionnaireNom").addClass('requiredField');
                erreurBool = true;
            }

            if ($("#EcheancePrincipale").val() != "") {
                //Ajout de 2012 à la date pour avoir une année bisextile
                //if (Date.parse($("#EcheancePrincipale").val() + "/2012") == null) {
                if (!isDate($("#EcheancePrincipale").val() + "/2012")) {
                    $("#EcheancePrincipale").addClass('requiredField');
                    erreurBool = true;
                }
            }
            if ($("#EffetCheck").is(':checked') && $("#EffetGaranties").val() != "") {
                var checkDH = checkDateHeure($("#EffetGaranties"), $("#FinEffet"), $("#HeureEffetHours"), $("#HeureFinEffetHours"), $("#HeureEffetMinutes"), $("#HeureFinEffetMinutes"));
                if (!checkDH) {
                    erreurBool = true;
                }
            }

            if ($("#EffetCheck").is(':checked') && ($("#FinEffet").val() == "")) {

                $("#HeureFinEffetHours").addClass('requiredField');
                $("#HeureFinEffetMinutes").addClass('requiredField');
                $("#FinEffet").addClass('requiredField');
                erreurBool = true;
            }

            if (!isDate($("#FinEffet").val()) && $("#FinEffet").val() != "") {
                $("#FinEffet").addClass('requiredField');
                erreurBool = true;
            }

            if ($("#EffetCheck").is(':checked') && ($("#EffetGaranties").val() == "")) {
                $("#EffetGaranties").addClass('requiredField');
                $("#HeureEffetMinutes").addClass('requiredField');
                $("#HeureEffetHours").addClass('requiredField');
                //erreurBool = true;
            }
            if (!isDate($("#EffetGaranties").val()) && $("#EffetGaranties").val() != "") {
                $("#EffetGaranties").addClass('requiredField');
                erreurBool = true;
            }

            if ($("#DureeCheck").is(':checked') && ($("#Duree").val() == "" || $("#Duree").val() == "0" || $("#DureeString").val() == "")) {
                $("#Duree").addClass('requiredField');
                $("#DureeString").addClass('requiredField');
                if ($("#EffetGaranties").val() == "") {
                    $("#EffetGaranties").addClass('requiredField');
                    $("#HeureEffetHours").addClass('requiredField');
                    $("#HeureEffetMinutes").addClass('requiredField');
                }
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
            if (($("#NatureContrat").val() == "C" || $("#NatureContrat").val() == "D") && $("#AperiteurCode").val() == "") {
                $("#AperiteurNom").addClass("requiredField");
                erreurBool = true;
            }

            if (erreurBool) {
                $("#txtParamRedirect").val("");
                return false;
            }
        }
       UpdateInfoFin(evt, data);
   
  
    });
}
//----------------------Traitement suivant---------------------
function UpdateInfoFin(evt, data) {

        $(":input").removeClass("requiredField");

        $(".requiredField").removeClass("requiredField");
        var errDate = false;
        if ($("#DateProjet").val() != "" && !isDate($("#DateProjet").val())) {
            $("#DateProjet").addClass("requiredField");
            $("#ModifInfoFin").val("false");
            errDate = true;
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

            $('Div[name="zoneTxtAreaDesc"]').addClass('requiredField'); 
            $('Div[name="zoneTxtAreaDesc"]').attr("title", "La description ne peut être vide si l'antécédent est égal à 'A'"); 
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

      /*  var argModeleFinOffreInfos = JSON.stringify(ModeleFinOffreInfos);
        var argModeleFinOffreAnnotation = $("#AnnotationFin").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        common.page.isLoading = true;
       /* let saveCancel = data && data.returnHome ? 1 : 0;
      /*  $.ajax({
            type: "POST",
            url: "/FinOffre/Update",
            data: { codeOffre: codeOffre, version: version, type: type, argModeleFinOffreInfos: argModeleFinOffreInfos, argModeleFinOffreAnnotation: argModeleFinOffreAnnotation, tabGuid: tabGuid, saveCancel: saveCancel, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function () { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });*/
    if (errDate) {
        return false;
    } else {
        SubmitForm(data && data.returnHome);
    }
 //   });
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
    $("#NumAvenantPage").removeAttr("disabled");
    $("#AddParamType").removeAttr("disabled");
    $("#AddParamValue").removeAttr("disabled");
    var argModeleFinOffreAnnotation = $("#AnnotationFin").val();
    let obj = $('body :input').not("#div_popups :input").serializeObject();
    obj.txtSaveCancel = returnHome ? 1 : 0;
    let formData = JSON.stringify(obj);
    formData = formData.replace("Offre.AddParamType", "AddParamType").replace("Offre.AddParamValue", "AddParamValue");
    
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ModifierOffre/ModifierOffreEnregistrer",
        data: formData,
        contentType: "application/json",
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);

        }

    });
    $("#RegimeTaxe").attr("disabled", "disabled");
    $("#SoumisCatNat").attr("disabled", "disabled");
};
