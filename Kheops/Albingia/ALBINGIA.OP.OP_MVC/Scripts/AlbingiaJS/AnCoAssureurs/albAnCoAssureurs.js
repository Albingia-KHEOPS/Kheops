/// <reference path="../Common/common.js" />
/// <reference path="../albCommon.js" />

$(document).ready(function () {
    AlternanceLigne("CoAssureurs", "noInput", true, null);
    MapElementPage();
    //Affiche l'écran en lecture seule si l'offre est verrouillée
    //SLA (23.01.2013) : fonction désactivée pour le moment
    //Verrouillage(); 
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly) {

        if ($("#IsModifHorsAvn").val() !== 'True')
            $("img[name=btnEditer]").hide();

        $("img[name=btnSupprimer]").hide();
        $('#AjouterButton').hide();
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    if ($("#chkModificationAVN").attr("id") != undefined) {
        if (($("#IsAvenantModificationLocale").val() == "True" && $("#IsTraceAvnExist").val() == "True") || window.isReadonly) {
            $("#chkModificationAVN").attr("disabled", "disabled");
            $("#chkModificationAVN").die();
        }
        else {
            $("#chkModificationAVN").removeAttr("disabled");
            $("#chkModificationAVN").die().live("change", function () {
                Redirection("AnCoAssureurs", "Index", !$("#chkModificationAVN").is(':checked'));
            });
        }
    }

    $("img[name=btnEditer]").each(function () {
        $(this).click(function () {
            ModifierCoAssureur($(this).attr("id"));
        });
    });

    $("img[name=btnSupprimer]").each(function () {
        $(this).click(function () {
            SupprimerCoAssureur($(this).attr("id"));
        });
    });

    $('#AjouterButton').die();
    $('#AjouterButton').live('click', function (evt) {
        AjouterCoAssureur();
    });

    $("#btnSuivant").kclick(function (evt, data) {
        valider(data && data.returnHome);
    });

    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
          "Voulez-vous revenir à l'écran Informations Générales Contrat ?<br/>Toutes les modifications effectuées dans cet écran vont être annulées",
          350, 130, true, true);
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Annuler();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $('#btnEnregistrerEdition').die();
    $('#btnEnregistrerEdition').live('click', function (evt) {
        EnregistrerCoAssureur($('#hiddenIdEdit').val());
    });

    $('#btnAnnulerEdition').die();
    $('#btnAnnulerEdition').live('click', function (evt) {
        ReactivateShortCut();
        $('#divFullScreen').hide();
    });

    $("#inInterlocuteur").die().live('keyup', function (evt) {
        if ($("#inInterlocuteur").val() == "")
            $('#InterlocuteurInvalideDiv').empty();
    });
    FormatDecimalNumericValue();
}

//---------------------fonction qui ouvre la popup du coassureur selectionné pour modification------------------------
function ModifierCoAssureur(guidId) {
    //Appel Ajax, récupération des informations complètes du co-assureur sélectionné
    guidId = guidId.split("_")[1];
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var typeContrat = $("#Offre_Type").val();

    $.ajax({
        type: "POST",
        url: "/AnCoAssureurs/GetCoAssureurDetail",
        data: {
            guidId: guidId, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat,
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataEditPopIn").html(data);
            //Numerique();
            //FormatPartCoAs();
            FormatDecimalNumericValue();
            formatDatePicker();
            AffectDateFormat();
            $("#inCode").attr("readonly", "readonly");
            $("#inLibelle").attr("readonly", "readonly");
            $("#inCode").addClass("readonly");
            $("#inLibelle").addClass("readonly");
            
            if ($('#IsModifHorsAvn').val() == 'True') {
                $('#inPourcentPart').attr('readonly','readonly');
            }
            
            AlbScrollTop();
            $("#divFullScreen").show();
            MapCommonAutoCompAperiteur();
            FormatDecimalNumericValue();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------------fonction qui ouvre une popup avec champs vides pour créer un coassureur---------------
function AjouterCoAssureur() {
    $.ajax({
        type: "POST",
        url: "/AnCoAssureurs/GetNouveauCoAssureur",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataEditPopIn").html(data);
            //Numerique();
            //FormatPartCoAs();
            FormatDecimalNumericValue();
            formatDatePicker();
            AffectDateFormat();
            $("#inCode").removeAttr('readonly');
            $("#inLibelle").removeAttr('readonly');
            $("#inCode").removeClass("readonly");
            $("#inLibelle").removeClass("readonly");
            AlbScrollTop();
            $("#divFullScreen").show();
            MapCommonAutoCompAperiteur();
            FormatDecimalNumericValue();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//---------------------fonction qui enregistre les modifications réalisées dans une popup (ajout et modification)
function EnregistrerCoAssureur(guidId) {

    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var typeContrat = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();
    var dateDebut = $("#inDateDeb").val();
    var dateFin = $("#inDateFin").val();

    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    if ((dateDebut != "" && !isDate(dateDebut)) || (dateFin != "" && !isDate(dateFin))) {
        $("#inDateDeb").addClass('requiredField');
        $("#inDateFin").addClass('requiredField');
        return false;
    }


    if ($("#inPourcentPart").val() == "" || $("#inPourcentPart").autoNumeric('get') == "0") {
        common.dialog.bigError("La part du co-assureur ne peut être nulle", true);
        return false;
    }
    if ($("#inCode").val() == "") {
        common.dialog.bigError("Le code du co-assureur ne peut être nul", true);
        return false;
    }

    if ($("#inLibelle").val() == "") {
        common.dialog.bigError("Le nom du co-assureur ne peut être nul", true);
        return false;
    }

    var data = SerialiserDataCoAssureur(guidId);



    if (dateDebut != "" && dateFin != "") {
        var dDateDeb = Date.parse(dateDebut);
        var dDateFin = Date.parse(dateFin);
        if (dDateFin < dDateDeb) {
            common.dialog.bigError("La date de fin ne peut être antérieure à la date de début", true);
            return false;
        }
    }


    if (guidId == "")//Mode nouvel élement
    {
        var typeOperation = "I";
        var total = $("#total").val();
        var partCouverte = $("#inPartCouverte").val();
        var partAssureur = $("#inPourcentPart").val();
        var partAlbingia = $("#inPartAlbingia").val();

        //SLA 03.03.2015 : désactivation du contrôle à l'enregistrement du coass, voir bug 1309
        //if (parseFloat(total) + parseFloat(partAssureur) + parseFloat(partAlbingia) > parseFloat(partCouverte)) {
        //    common.dialog.bigError("La somme des % de parts + la part Albingia ne peut pas être  supérieure à la part couverte", false);
        //    return false;
        //}

        $.ajax({
            type: "POST",
            url: "/AnCoAssureurs/EnregistrerCoAssureur",
            data: {
                guidId: guidId, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat, codeAvn: codeAvn,
                tabGuid: tabGuid, data: data, typeOperation: typeOperation, addParamType: addParamType, addParamValue: addParamValue
            },
            success: function (data) {
                ReactivateShortCut();
                $("#divFullScreen").hide();
                $("#divCoAssureurs").html(data);
                MapElementPage();
                AlternanceLigne("CoAssureurs", "noInput", true, null);
                FormatDecimalNumericValue();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        var typeOperation = "U";
        var total = $("#total").val();
        var partCouverte = $("#inPartCouverte").val();
        var partAssureur = $("#inPourcentPart").val();
        var partAlbingia = $("#inPartAlbingia").val();
        var anciennePartAssureur = $("#AncienPourcentPart").val();

        //SLA 03.03.2015 : désactivation du contrôle à l'enregistrement du coass, voir bug 1309
        //if (parseFloat(total) + parseFloat(partAssureur) + parseFloat(partAlbingia) - parseFloat(anciennePartAssureur) > parseFloat(partCouverte)) {
        //    common.dialog.bigError("La somme des % de parts + la part Albingia ne peut pas être  supérieure à la part couverte", false);
        //    return false;
        //}

        $.ajax({
            type: "POST",
            url: "/AnCoAssureurs/EnregistrerCoAssureur",
            data: {
                guidId: guidId, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat, codeAvn: codeAvn,
                tabGuid: tabGuid, data: data, typeOperation: typeOperation, addParamType: addParamType, addParamValue: addParamValue
            },
            success: function (data) {
                $("#divFullScreen").hide();
                $("#divCoAssureurs").html(data);
                MapElementPage();
                AlternanceLigne("CoAssureurs", "noInput", true, null);
                FormatDecimalNumericValue();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }


}

//---------------------fonction qui supprime le coassureur selectionné
function SupprimerCoAssureur(guidId) {
    DesactivateShortCut();
    ShowCommonFancy("Confirm", "Cancel",
        "Êtes-vous sûr(e) de vouloir supprimer ce co-assureur?<br/>",
        350, 100, true, true);
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        var codeContrat = $("#Offre_CodeOffre").val();
        var versionContrat = $("#Offre_Version").val();
        var typeContrat = $("#Offre_Type").val();
        var codeAvn = $("#NumAvenantPage").val();
        var tabGuid = $("#tabGuid").val();
        var typeOperation = "D";
        var total = $("#total").val();
        var partCouverte = $("#inPartCouverte").val();
        var partAssureur = $("#inPourcentPart").val();
        var partAlbingia = $("#inPartAlbingia").val();
        var anciennePartAssureur = $("#AncienPourcentPart").val();
        var data = "";
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        guidId = guidId.split("_")[1];
        $.ajax({
            type: "POST",
            url: "/AnCoAssureurs/EnregistrerCoAssureur",
            data: {
                guidId: guidId, codeContrat: codeContrat, versionContrat: versionContrat, typeContrat: typeContrat, codeAvn: codeAvn,
                tabGuid: tabGuid, data: data, typeOperation: typeOperation, addParamType: addParamType, addParamValue: addParamValue
            },
            success: function (data) {
                ReactivateShortCut();
                $("#divFullScreen").hide();
                $("#divCoAssureurs").html(data);
                MapElementPage();
                AlternanceLigne("CoAssureurs", "noInput", true, null);
                FormatDecimalNumericValue();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
        
    });
    $("#btnConfirmCancel").die().live('click', function () {
        ReactivateShortCut();
        CloseCommonFancy();
    });
}

//----------------------Formate le controle des parts co-assureur---------------------
function FormatPartCoAs() {
    var part = 0;
    $("#inPourcentPart").attr("maxlength", 5);
    $("#inPourcentPart").live("keyup", function (event) {

        var splitTab = $(this).val().split(",");
        if (splitTab[0] > 100 || splitTab[0] == -1 || (splitTab[0] == 100 && splitTab.length == 2 && splitTab[1] > 0)) {
            $(this).val("");
        }
        //if (!isNaN($(this).val()) && $(this).val() <= 100 && $(this).val() != -1) {
        //    part = $.trim($(this).val());
        //}
        //$(this).val(part);
    });
}

//--------------------fonction qui formate les datepicker--------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}

function SerialiserDataCoAssureur(id) {
    var libelle = "";
    if (id == "")
        libelle = $.trim($("#inLibelle").val().split(" - ")[1]);
    else
        libelle = $("#inLibelle").val();

    var serialize = "[";
    serialize += '{';
    serialize += 'GuidId:"' + $("#hiddenIdEdit").val() + '",';
    serialize += 'Code:"' + $("#inCode").val() + '",';
    serialize += 'Nom:"' + libelle + '",';
    serialize += 'CodePostal:"",';
    serialize += 'Ville:"",';
    serialize += 'PourcentPart:"' + $("#inPourcentPart").val() + '",';
    serialize += 'DateDebut:"' + $("#inDateDeb").val() + '",';
    serialize += 'DateFin:"' + $("#inDateFin").val() + '",';
    serialize += 'Interlocuteur:"' + $("#inInterlocuteur").val() + '",';
    serialize += 'IdInterlocuteur:"' + $("#inInterlocuteurCode").val() + '",';
    serialize += 'Reference:"' + $("#inReference").val() + '",';
    serialize += 'FraisAcc:"' + $("#inFraisAcc").val() + '",';
    serialize += 'FraisApeAlb:"' + $("#inFraisApe").val() + '"';
    serialize += '}';
    serialize += ']';

    return serialize;
}

//-------------------fonction qui desactive les boutons d'édition si l'offre/le contrat est verrouillé----------------
function Verrouillage() {
    if ($("#etatVerrouillage").val() == "True") {
        //desactivation des boutons d'ajout, de suppression, de modification        
        $('.Action').removeAttr('onclick');
        $('.Action').addClass("disabled");
    }
}

//-----------------Retourne à l'écran information générale du contrat
function Annuler() {
    Redirection("AnInformationsGenerales", "Index");
}

//--------------------fonction exécutée lors du clic sur le bouton valider de l'écran------------------------------
function valider(returnHome) {
    var erreurBool = false;
    var total = 0;
    if ($("#total").attr("id") == undefined)
        total = 0;
    else
        total = $("#total").autoNumeric("get");

    var partCouverte = $("#inPartCouverte").autoNumeric("get");
    var partAlbingia = $("#inPartAlbingia").autoNumeric("get");
    if (parseFloat(total) + parseFloat(partAlbingia) != parseFloat(partCouverte))
        erreurBool = true;
    if (erreurBool && (!window.isReadonly || $('#IsModifHorsAvn').val() === 'True')) {
        common.dialog.bigError("La somme des % de parts + la part Albingia ne peut pas être différente à la part couverte", true);
        return false;
    }
    else {
        //validation
        Redirection(returnHome ? "RechercheSaisie" : "AnCourtier", "Index");
        //$.fn.submitForm();
    }
}

//----------------Redirection------------------
function Redirection(cible, job, readonlyDisplay) {
    ShowLoading();
    var codeContrat = $("#Offre_CodeOffre").val();
    var versionContrat = $("#Offre_Version").val();
    var typeContrat = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var dateAVNModificationLocale = $("#DateEffetAvenantModificationLocale").val();
    var partCouverte = $("#inPartCouverte").autoNumeric("get");
    var partAlbingia = $("#inPartAlbingia").autoNumeric("get");
    $.ajax({
        type: "POST",
        url: "/AnCoAssureurs/Redirection/",
        data: { cible: cible, job: job, partCouverte: partCouverte, partAlbingia: partAlbingia, codeContrat: codeContrat, version: versionContrat, type: typeContrat, dateAVNModificationLocale: dateAVNModificationLocale, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, readonlyDisplay: readonlyDisplay, isModeConsultationEcran: $("#IsModeConsultationEcran").val() },
        success: function (data) { },
        error: function (request) {
            let result = null;
            try {
                result = JSON.parse(request.responseText);
            } catch (e) { result = null; }

            if (!kheops.alerts.blacklist.displayAll(result)) {
                common.error.showXhr(request);
            }
        }
    });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatPourcentNumerique();
    common.autonumeric.applyAll('init', 'pourcentnumeric', '', null, null, '100', '0');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    //FormatPourcentDecimal();

    //FormatNumerique('fraisAcc', ' ', '99999', '0');
    common.autonumeric.apply($("#inFraisAcc"), 'init', 'numeric', ' ', null, null, '99999', '0');
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '999.99', '0.00');
}
